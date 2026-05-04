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
WITH yearly_counts AS (
    SELECT 
        TE.Name AS type_equipment,
        EXTRACT(YEAR FROM E.InstallationDate) AS year,
        COUNT(E.EquipmentID) AS equipment_count
    FROM Type_equipment TE
    LEFT JOIN Equipment E ON E.typeID = TE.typeID
    WHERE EXTRACT(YEAR FROM E.InstallationDate) BETWEEN 2015 AND 2019
        OR E.EquipmentID IS NULL
    GROUP BY TE.Name, EXTRACT(YEAR FROM E.InstallationDate)
)
SELECT 
    type_equipment,
    COALESCE(SUM(CASE WHEN year = 2019 THEN equipment_count END), 0) AS "2019",
    COALESCE(SUM(CASE WHEN year = 2018 THEN equipment_count END), 0) AS "2018",
    COALESCE(SUM(CASE WHEN year = 2017 THEN equipment_count END), 0) AS "2017",
    COALESCE(SUM(CASE WHEN year = 2016 THEN equipment_count END), 0) AS "2016",
    COALESCE(SUM(CASE WHEN year = 2015 THEN equipment_count END), 0) AS "2015"
FROM yearly_counts
GROUP BY type_equipment
HAVING SUM(equipment_count) > 0
ORDER BY type_equipment;