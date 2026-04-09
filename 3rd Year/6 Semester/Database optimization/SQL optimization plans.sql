	-- 41. Выбрать курс, который сдало наибольшее количество человек. (we have to check the condition as well)
EXPLAIN (ANALYZE, BUFFERS, VERBOSE)
SELECT 
    C.CourseID,
    C.Name,
    COUNT(DISTINCT SP.StudentID) AS Pass_student
FROM Course C
JOIN StudentPerformance SP ON C.CourseID = SP.CourseID
WHERE SP.Marks >= 3
GROUP BY C.CourseID, C.Name
ORDER BY Pass_student DESC;
------

EXPLAIN (ANALYZE, BUFFERS, VERBOSE)
WITH CoursePassCounts AS (
    SELECT 
        C.CourseID,
        C.Name,
        COUNT(DISTINCT SP.StudentID) AS PassedStudents
    FROM Course C
    JOIN StudentPerformance SP ON C.CourseID = SP.CourseID
    WHERE SP.Marks >= 3
    GROUP BY C.CourseID, C.Name
)
SELECT * FROM CoursePassCounts
	WHERE PassedStudents = (SELECT MAX(PassedStudents) FROM CoursePassCounts);
	
--========================================================================================================
EXPLAIN (ANALYZE, BUFFERS, VERBOSE)
SELECT DISTINCT TE.Name
FROM Type_equipment TE
WHERE NOT EXISTS (
    SELECT 1 FROM Equipment E
    WHERE E.typeID = TE.typeID AND E.InstallationDate >= CURRENT_DATE - INTERVAL '5 years')
ORDER BY TE.Name;
--------

EXPLAIN (ANALYZE, BUFFERS, VERBOSE)
SELECT DISTINCT TE.Name
FROM Type_equipment TE
WHERE TE.typeID NOT IN (
	SELECT E.typeID FROM equipment E
	WHERE E.InstallationDate >= CURRENT_DATE - INTERVAL '5 years'
	)
ORDER BY TE.Name;
--------

EXPLAIN (ANALYZE, BUFFERS, VERBOSE)
SELECT TE.Name
FROM Type_equipment TE
EXCEPT
SELECT TE2.Name
FROM Type_equipment TE2
	JOIN Equipment E ON E.typeID = TE2.typeID
WHERE E.InstallationDate >= CURRENT_DATE - INTERVAL '5 years'
ORDER BY Name;
--==========================================================================
