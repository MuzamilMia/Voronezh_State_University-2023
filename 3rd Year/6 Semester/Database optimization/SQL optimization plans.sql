-- 33. Выбрать все данные об обучающихся, которые не сдали только один курс из имеющихся в базе данных. 
EXPLAIN (
	ANALYZE,
	BUFFERS,
	VERBOSE
)
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
	) AS T ON S.STUDENTID = T.STUDENTID
ORDER BY
	S.SECONDNAME,
	S.FIRSTNAME;

------------------------------------------------------------------------------------
EXPLAIN (
	ANALYZE,
	BUFFERS,
	VERBOSE
)
SELECT
	S.STUDENTID,
	S.FIRSTNAME,
	S.SECONDNAME,
	S.NIKENAME
FROM
	STUDENT S
	LEFT JOIN STUDENTPERFORMANCE SP ON S.STUDENTID = SP.STUDENTID
	LEFT JOIN COURSE C ON C.COURSEID = SP.COURSEID
GROUP BY
	S.STUDENTID,
	S.FIRSTNAME,
	S.SECONDNAME,
	S.NIKENAME
HAVING
	COUNT(C.COURSEID) - COUNT(
		CASE
			WHEN SP.MARKS >= 3 THEN 1
		END
	) = 1
ORDER BY
	S.SECONDNAME,
	S.FIRSTNAME;

--==========================================================================
--44. Выбрать для каждого преподавателя количество курсов, которые он преподает,и общее количество курсов, которые есть в БД.
EXPLAIN (
	ANALYZE,
	BUFFERS,
	VERBOSE
)
SELECT
	T.TEACHERID,
	T.LASTNAME,
	T.FIRSTNAME,
	T.MIDDLENAME,
	COUNT(DISTINCT CT.COURSEID) AS AMOUNT_COURSES_TEACHER,
	(
		SELECT
			COUNT(*)
		FROM
			COURSE
	) AS WHOLE_AMOUNT_COURSES_DATABASE
FROM
	TEACHER T
	LEFT JOIN LESSON L ON T.TEACHERID = L.TEACHERID
	LEFT JOIN COURSETHEME CT ON L.THEMEID = CT.THEMEID
GROUP BY
	T.TEACHERID,
	T.LASTNAME,
	T.FIRSTNAME,
	T.MIDDLENAME
ORDER BY
	AMOUNT_COURSES_TEACHER DESC,
	T.LASTNAME;

----------------
EXPLAIN (
	ANALYZE,
	BUFFERS,
	VERBOSE
)
SELECT
	T.TEACHERID,
	T.LASTNAME,
	T.FIRSTNAME,
	T.MIDDLENAME,
	COUNT(DISTINCT CT.COURSEID) AS AMOUNT_COURSES_TEACHER,
	C.TOTAL_COURSES AS WHOLE_AMOUNT_COURSES_DATABASE
FROM
	TEACHER T
	LEFT JOIN LESSON L ON T.TEACHERID = L.TEACHERID
	LEFT JOIN COURSETHEME CT ON L.THEMEID = CT.THEMEID
	CROSS JOIN (
		SELECT
			COUNT(*) AS TOTAL_COURSES
		FROM
			COURSE
	) C
GROUP BY
	T.TEACHERID,
	T.LASTNAME,
	T.FIRSTNAME,
	T.MIDDLENAME,
	C.TOTAL_COURSES
ORDER BY
	AMOUNT_COURSES_TEACHER DESC,
	T.LASTNAME;

--==========================================================================
-- 26. Выбрать для каждого типа оборудования количество установленных единиц для пяти последних лет. В результирующей 
-- таблице должно быть шесть столбцов: название типа оборудования, 2019, 2018, 2017, 2016, 2015. Исключить из результирующей 
-- таблицы тип оборудования, который не устанавливался последние пять лет.
EXPLAIN (
	ANALYZE,
	BUFFERS,
	VERBOSE
)
SELECT 
    TE.Name AS Type_equipment,
    SUM(CASE WHEN EXTRACT(YEAR FROM E.InstallationDate) = 2019 THEN 1 ELSE 0 END) AS "2019",
    SUM(CASE WHEN EXTRACT(YEAR FROM E.InstallationDate) = 2018 THEN 1 ELSE 0 END) AS "2018",
    SUM(CASE WHEN EXTRACT(YEAR FROM E.InstallationDate) = 2017 THEN 1 ELSE 0 END) AS "2017",
    SUM(CASE WHEN EXTRACT(YEAR FROM E.InstallationDate) = 2016 THEN 1 ELSE 0 END) AS "2016",
    SUM(CASE WHEN EXTRACT(YEAR FROM E.InstallationDate) = 2015 THEN 1 ELSE 0 END) AS "2015"
FROM Type_equipment TE
LEFT JOIN Equipment E ON E.typeID = TE.typeID
    AND EXTRACT(YEAR FROM E.InstallationDate) BETWEEN 2015 AND 2019
GROUP BY TE.Name
HAVING(
        SUM(CASE WHEN EXTRACT(YEAR FROM E.InstallationDate) = 2019 THEN 1 ELSE 0 END) +
        SUM(CASE WHEN EXTRACT(YEAR FROM E.InstallationDate) = 2018 THEN 1 ELSE 0 END) +
        SUM(CASE WHEN EXTRACT(YEAR FROM E.InstallationDate) = 2017 THEN 1 ELSE 0 END) +
        SUM(CASE WHEN EXTRACT(YEAR FROM E.InstallationDate) = 2016 THEN 1 ELSE 0 END) +
        SUM(CASE WHEN EXTRACT(YEAR FROM E.InstallationDate) = 2015 THEN 1 ELSE 0 END)) > 0 
ORDER BY TE.Name;

--================================
EXPLAIN (
	ANALYZE,
	BUFFERS,
	VERBOSE
)
WITH
	YEARLY_COUNTS AS (
		SELECT TE.NAME AS TYPE_EQUIPMENT, 
		EXTRACT( YEAR FROM E.INSTALLATIONDATE ) AS YEAR,
		COUNT(E.EQUIPMENTID) AS EQUIPMENT_COUNT
		FROM TYPE_EQUIPMENT TE LEFT JOIN EQUIPMENT E ON E.TYPEID = TE.TYPEID
		WHERE EXTRACT( YEAR FROM E.INSTALLATIONDATE ) BETWEEN 2015 AND 2019  OR E.EQUIPMENTID IS NULL
		GROUP BY TE.NAME, EXTRACT( YEAR FROM E.INSTALLATIONDATE )
	)
SELECT
	TYPE_EQUIPMENT,
	COALESCE( SUM( CASE WHEN YEAR = 2019 THEN EQUIPMENT_COUNT END ), 0 ) AS "2019",
	COALESCE( SUM( CASE WHEN YEAR = 2018 THEN EQUIPMENT_COUNT END ), 0 ) AS "2018",
	COALESCE( SUM( CASE WHEN YEAR = 2017 THEN EQUIPMENT_COUNT END ), 0 ) AS "2017",
	COALESCE( SUM( CASE WHEN YEAR = 2016 THEN EQUIPMENT_COUNT END ), 0 ) AS "2016",
	COALESCE( SUM( CASE WHEN YEAR = 2015 THEN EQUIPMENT_COUNT END ), 0 ) AS "2015" 
	FROM YEARLY_COUNTS 
	GROUP BY TYPE_EQUIPMENT
	HAVING SUM(EQUIPMENT_COUNT) > 0 
	ORDER BY TYPE_EQUIPMENT;




---====================================================================================================
--CTE (оправданное и неоправданное использование)
--47. Выбрать обучающихся, которые нарушили цепочку зависимостей курсов, т. е. сдали 
			--	  успешно один курс, но не сдали предшествующий им курс.
EXPLAIN (
	ANALYZE,
	BUFFERS,
	VERBOSE
)	
SELECT DISTINCT
    S.StudentID,
    S.SecondName,
    S.FirstName,
    S.NikeName,
    C.Name,
    PC.Name
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
    )
ORDER BY S.secondname, S.FirstName;
------------------------------------------------------------------------------------
EXPLAIN (
	ANALYZE,
	BUFFERS,
	VERBOSE
)
WITH SuccessfulCourses AS (
    SELECT 
        SP.StudentID,
        SP.CourseID,
        SP.Marks,
        ST.Name AS status_name
    FROM StudentPerformance SP
        LEFT JOIN Status ST ON SP.status_id = ST.status_id
    WHERE SP.Marks >= 3 OR ST.Name IN ('Completed')
)
SELECT 
    S.StudentID,
    S.Secondname,
    S.FirstName,
    S.NikeName,
    C.Name,
    PC.Name
FROM SuccessfulCourses SC
    JOIN Student S ON SC.StudentID = S.StudentID
    JOIN Course C ON SC.CourseID = C.CourseID
    JOIN Course_Dependenc CD ON C.CourseID = CD.CourseID
    JOIN Course PC ON CD.DependsOnCourseID = PC.CourseID
WHERE NOT EXISTS (
    SELECT 1
    FROM SuccessfulCourses SC2
    WHERE SC2.StudentID = SC.StudentID
        AND SC2.CourseID = CD.DependsOnCourseID
)
ORDER BY S.secondname, S.FirstName, C.Name;

---====================================================================================================

--======================== Window Fucntion========================
EXPLAIN (
	ANALYZE,
	BUFFERS,
	VERBOSE
)
SELECT 
    T.teacherid,
    T.lastname,
    T.firstname,
    T.middlename,
    COUNT(DISTINCT CT.courseid) AS Amount_courses_teacher,
    (SELECT COUNT(*) FROM Course) AS Whole_amount_courses_DataBase
FROM Teacher T
    LEFT JOIN Lesson L ON T.teacherid = L.teacherid
    LEFT JOIN CourseTheme CT ON L.themeid = CT.themeid
GROUP BY T.teacherid, T.lastname, T.firstname, T.middlename
ORDER BY Amount_courses_teacher DESC, T.lastname;

--===============================================================

EXPLAIN (
	ANALYZE,
	BUFFERS,
	VERBOSE
)
SELECT DISTINCT
    teacherid,
    lastname,
    firstname,
    middlename,
    COUNT(courseid) OVER (PARTITION BY teacherid) AS Amount_courses_teacher,
    (SELECT COUNT(*) FROM Course) AS Whole_amount_courses_DataBase
FROM (
    SELECT DISTINCT
        T.teacherid,
        T.lastname,
        T.firstname,
        T.middlename,
        CT.courseid
    FROM Teacher T
    LEFT JOIN Lesson L ON T.teacherid = L.teacherid
    LEFT JOIN CourseTheme CT ON L.themeid = CT.themeid
)
ORDER BY Amount_courses_teacher DESC, lastname;
