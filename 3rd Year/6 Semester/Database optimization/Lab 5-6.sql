CREATE VIEW v_teacher_course_stats AS
SELECT
    T.TEACHERID, T.LASTNAME, T.FIRSTNAME,
    COUNT(DISTINCT CT.COURSEID) AS AMOUNT_COURSES_TEACHER
FROM TEACHER T
LEFT JOIN LESSON L ON T.TEACHERID = L.TEACHERID
LEFT JOIN COURSETHEME CT ON L.THEMEID = CT.THEMEID
GROUP BY T.TEACHERID, T.LASTNAME, T.FIRSTNAME;

EXPLAIN (ANALYZE, BUFFERS, VERBOSE)
SELECT * FROM v_teacher_course_stats WHERE TEACHERID = 10;
------------============================
--33.but the materizlize is not getting. 
CREATE VIEW v_students_failed_one_course AS
SELECT
    S.STUDENTID,
    S.FIRSTNAME,
    S.SECONDNAME,
    S.NIKENAME
FROM
    STUDENT S
    JOIN (
        SELECT
            SP.STUDENTID
        FROM
            STUDENTPERFORMANCE SP
            RIGHT JOIN COURSE C ON C.COURSEID = SP.COURSEID
        GROUP BY
            SP.STUDENTID
        HAVING
            COUNT(C.COURSEID) - COUNT(
                CASE
                    WHEN SP.MARKS >= 3 THEN 1
                END
            ) = 1
    ) AS T ON S.STUDENTID = T.STUDENTID;

EXPLAIN (ANALYZE, BUFFERS, VERBOSE)
SELECT * FROM v_students_failed_one_course WHERE SECONDNAME LIKE 'A%';
--=======================
--no materialize.
CREATE VIEW v_teacher_stats AS
SELECT
    T.TEACHERID,
    T.LASTNAME,
    T.FIRSTNAME,
    COUNT(DISTINCT CT.COURSEID) AS AMOUNT_COURSES_TEACHER,
    (SELECT COUNT(*) FROM COURSE) AS TOTAL_COURSES
FROM TEACHER T
LEFT JOIN LESSON L ON T.TEACHERID = L.TEACHERID
LEFT JOIN COURSETHEME CT ON L.THEMEID = CT.THEMEID
GROUP BY T.TEACHERID, T.LASTNAME, T.FIRSTNAME;

EXPLAIN (ANALYZE, BUFFERS, VERBOSE)
SELECT * FROM v_teacher_stats 
WHERE AMOUNT_COURSES_TEACHER > (SELECT AVG(AMOUNT_COURSES_TEACHER) FROM v_teacher_stats);
--==============================
--no materizalize
CREATE VIEW v_course_dependency_issue AS
SELECT DISTINCT
    S.StudentID,
    S.SecondName,
    S.FirstName,
    S.NikeName,
    C.Name AS CourseName,
    PC.Name AS PrerequisiteCourseName
FROM Student S
    JOIN StudentPerformance SP ON S.StudentID = SP.StudentID
    JOIN Course C ON SP.CourseID = C.CourseID
    JOIN Course_Dependenc CD ON C.CourseID = CD.CourseID
    JOIN Course PC ON CD.DependsOnCourseID = PC.CourseID
WHERE 
    (SP.Marks >= 3 OR SP.status_id IN (SELECT status_id FROM Status WHERE Name IN ('Completed')))
    AND NOT EXISTS (
        SELECT 1
        FROM StudentPerformance SP2
        WHERE SP2.StudentID = S.StudentID
            AND SP2.CourseID = CD.DependsOnCourseID
            AND (SP2.Marks >= 3 OR SP2.status_id IN (SELECT status_id FROM Status WHERE Name IN ('Completed')))
    );

EXPLAIN (ANALYZE, BUFFERS, VERBOSE)
SELECT * FROM v_course_dependency_issue WHERE SecondName LIKE 'A%';

--==============================
CREATE VIEW v_yearly_equipment_counts AS
WITH YEARLY_COUNTS AS (
    SELECT 
        TE.NAME AS TYPE_EQUIPMENT, 
        EXTRACT(YEAR FROM E.INSTALLATIONDATE) AS YEAR,
        COUNT(E.EQUIPMENTID) AS EQUIPMENT_COUNT
    FROM TYPE_EQUIPMENT TE 
    LEFT JOIN EQUIPMENT E ON E.TYPEID = TE.TYPEID
    WHERE EXTRACT(YEAR FROM E.INSTALLATIONDATE) BETWEEN 2015 AND 2019  
       OR E.EQUIPMENTID IS NULL
    GROUP BY TE.NAME, EXTRACT(YEAR FROM E.INSTALLATIONDATE)
)
SELECT
    TYPE_EQUIPMENT,
    COALESCE(SUM(CASE WHEN YEAR = 2019 THEN EQUIPMENT_COUNT END), 0) AS "2019",
    COALESCE(SUM(CASE WHEN YEAR = 2018 THEN EQUIPMENT_COUNT END), 0) AS "2018",
    COALESCE(SUM(CASE WHEN YEAR = 2017 THEN EQUIPMENT_COUNT END), 0) AS "2017",
    COALESCE(SUM(CASE WHEN YEAR = 2016 THEN EQUIPMENT_COUNT END), 0) AS "2016",
    COALESCE(SUM(CASE WHEN YEAR = 2015 THEN EQUIPMENT_COUNT END), 0) AS "2015" 
FROM YEARLY_COUNTS 
GROUP BY TYPE_EQUIPMENT
HAVING SUM(EQUIPMENT_COUNT) > 0;

EXPLAIN (ANALYZE, BUFFERS, VERBOSE)
SELECT * FROM v_yearly_equipment_counts 
WHERE "2019" > 0;  -- Фильтр повычисленному столбцу
---============================



--==========================================================================================================
--  Seocnd part of the lab
--Плюсы – ускорение запросов (сравнить план запроса к таблицам vs к материализованному представлению)
--Минусы – устаревание данных (изменить таблицы, показать, что в представлении старые данные)
-- REFRESH – обновить представление и показать актуальные данные

--we are making it. 
-- Полностью пересоздаём MV (если REFRESH не помогает)

CREATE MATERIALIZED VIEW mv_equipment_yearly_stats AS
SELECT 
    TE.NAME AS TYPE_EQUIPMENT,
    COALESCE(SUM(CASE WHEN EXTRACT(YEAR FROM E.INSTALLATIONDATE) = 2019 THEN 1 ELSE 0 END), 0) AS "2019",
    COALESCE(SUM(CASE WHEN EXTRACT(YEAR FROM E.INSTALLATIONDATE) = 2018 THEN 1 ELSE 0 END), 0) AS "2018",
    COALESCE(SUM(CASE WHEN EXTRACT(YEAR FROM E.INSTALLATIONDATE) = 2017 THEN 1 ELSE 0 END), 0) AS "2017",
    COALESCE(SUM(CASE WHEN EXTRACT(YEAR FROM E.INSTALLATIONDATE) = 2016 THEN 1 ELSE 0 END), 0) AS "2016",
    COALESCE(SUM(CASE WHEN EXTRACT(YEAR FROM E.INSTALLATIONDATE) = 2015 THEN 1 ELSE 0 END), 0) AS "2015"
FROM TYPE_EQUIPMENT TE
LEFT JOIN EQUIPMENT E ON E.TYPEID = TE.TYPEID
GROUP BY TE.NAME;
--==
EXPLAIN (ANALYZE, BUFFERS, TIMING)
SELECT * FROM mv_equipment_yearly_stats;

-- simply the query without the view.
EXPLAIN (ANALYZE, BUFFERS, TIMING)
SELECT 
    TE.NAME AS TYPE_EQUIPMENT,
    COALESCE(SUM(CASE WHEN EXTRACT(YEAR FROM E.INSTALLATIONDATE) = 2019 THEN 1 ELSE 0 END), 0) AS "2019",
    COALESCE(SUM(CASE WHEN EXTRACT(YEAR FROM E.INSTALLATIONDATE) = 2018 THEN 1 ELSE 0 END), 0) AS "2018",
    COALESCE(SUM(CASE WHEN EXTRACT(YEAR FROM E.INSTALLATIONDATE) = 2017 THEN 1 ELSE 0 END), 0) AS "2017",
    COALESCE(SUM(CASE WHEN EXTRACT(YEAR FROM E.INSTALLATIONDATE) = 2016 THEN 1 ELSE 0 END), 0) AS "2016",
    COALESCE(SUM(CASE WHEN EXTRACT(YEAR FROM E.INSTALLATIONDATE) = 2015 THEN 1 ELSE 0 END), 0) AS "2015"
FROM TYPE_EQUIPMENT TE
LEFT JOIN EQUIPMENT E ON E.TYPEID = TE.TYPEID
GROUP BY TE.NAME

--МИНУСЫ 
SELECT * FROM mv_equipment_yearly_stats WHERE type_equipment = 'Camera';

-- 2. Добавляем оборудование с датой ВНУТРИ диапазона (например, 2019)
INSERT INTO Equipment (ClassroomID, Name, InventoryNumber, InstallationDate, Description, typeID) VALUES
((SELECT ClassroomID FROM Classroom WHERE Number='CR-100'), 'Old Desktop 2019','INV-DESK-888', '2019-12-01', 'Old desktop from 2019',
 (SELECT typeID FROM Type_equipment WHERE Name = 'Camera'));
 
SELECT * FROM Type_equipment;
-- 3. Check the MV, all will be old data.
SELECT * FROM mv_equipment_yearly_stats WHERE type_equipment = 'Camera';

--== Check the maing tables
SELECT TE.NAME, COUNT(E.EQUIPMENTID), EXTRACT(YEAR FROM E.INSTALLATIONDATE)
FROM TYPE_EQUIPMENT TE
LEFT JOIN EQUIPMENT E ON E.TYPEID = TE.TYPEID
WHERE TE.NAME = 'Camera'
GROUP BY TE.NAME, EXTRACT(YEAR FROM E.INSTALLATIONDATE);

-- we are refreshing it. 
REFRESH MATERIALIZED VIEW mv_equipment_yearly_stats;

-- 5. Now the data will be refreshed, and shows the correct data. 
SELECT * FROM mv_equipment_yearly_stats WHERE type_equipment = 'Camera';