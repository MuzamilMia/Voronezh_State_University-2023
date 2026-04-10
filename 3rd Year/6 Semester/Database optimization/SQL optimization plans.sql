--47. Выбрать обучающихся, которые нарушили цепочку зависимостей курсов, т. е. сдали успешно один курс, но не сдали предшествующий им курс.
EXPLAIN (ANALYZE, BUFFERS, VERBOSE)
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
    (SP.Marks >= 3 OR SP.status_id IN (SELECT status_id FROM Status WHERE Name IN ('Completed', 'зачет')))
    AND NOT EXISTS (
        SELECT 1
        FROM StudentPerformance SP2
        WHERE SP2.StudentID = S.StudentID
            AND SP2.CourseID = CD.DependsOnCourseID
            AND (SP2.Marks >= 3 OR SP2.status_id IN (SELECT status_id FROM Status WHERE Name IN ('Completed', 'зачет')))
    )
ORDER BY S.secondname, S.FirstName;
------------------------------------------------------------------------------------
EXPLAIN (ANALYZE, BUFFERS, VERBOSE)
WITH SuccessfulCourses AS (
    SELECT 
        SP.StudentID,
        SP.CourseID,
        SP.Marks,
        ST.Name AS status_name
    FROM StudentPerformance SP
        LEFT JOIN Status ST ON SP.status_id = ST.status_id
    WHERE SP.Marks >= 3 OR ST.Name IN ('Completed', 'зачет')
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

--==========================================================================
--44. Выбрать для каждого преподавателя количество курсов, которые он преподает,и общее количество курсов, которые есть в БД.
EXPLAIN (ANALYZE, BUFFERS, VERBOSE)
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
----------------
EXPLAIN (ANALYZE, BUFFERS, VERBOSE)
SELECT 
    T.teacherid,
    T.lastname,
    T.firstname,
    T.middlename,
    COUNT(DISTINCT CT.courseid) AS Amount_courses_teacher,
    C.total_courses AS Whole_amount_courses_DataBase
FROM Teacher T
LEFT JOIN Lesson L ON T.teacherid = L.teacherid
LEFT JOIN CourseTheme CT ON L.themeid = CT.themeid
CROSS JOIN (SELECT COUNT(*) AS total_courses FROM Course) C
GROUP BY T.teacherid, T.lastname, T.firstname, T.middlename, C.total_courses
ORDER BY Amount_courses_teacher DESC, T.lastname;

--==========================================================================
	-- 41. Выбрать курс, который сдало наибольшее количество человек.
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
---------------------------------------------------------------
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



