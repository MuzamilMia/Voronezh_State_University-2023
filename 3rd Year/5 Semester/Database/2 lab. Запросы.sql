
--1. Выбрать все данные о курсах. Результат отсортировать по названию курса в лексикографическом порядке. 
SELECT* FROM course order by name Asc;

SELECT c.name, c.description, c.startingdate, c.endingdate
FROM Course c
ORDER BY c.name ASC;
--------------------------------------------------------------------------------------
		--2. Выбрать фамилии и инициалы учащихся в одном столбце. Результат отсортировать по длине фамилии.
SELECT 
		S.secondname ||' '|| S.firstname||' '|| S.nikename As Stundents_Full_Info
FROM Student S ORDER BY LENGTH(S.secondname);

		--сортировкa по фамилии при одинаковой длине
SELECT 
     S.secondname|| ' ' || S.firstname || ' ' || S.nikename AS Students_Full_Info
FROM Student S
ORDER BY LENGTH(S.secondname), S.secondname;
--------------------------------------------------------------------------------------
		--3. Выбрать все уникальные годы приема учащихся. Результат отсортировать по годам в порядке убывания. 
		--The whole date will be
SELECT DISTINCT S.AdmissionDate
FROM Student S;
	--Here I am exracting the years and providing to the screen.
	
SELECT DISTINCT EXTRACT(YEAR FROM AdmissionDate) AS admission_year
FROM Student 
ORDER BY admission_year DESC;
--------------------------------------------------------------------------------------
		--4.Выбрать id_преподавателя, для которого не указан e-mail.
SELECT 
	T.teacherid, T.lastname
FROM Teacher T 
	WHERE T.email is null OR T.email='';
	
SELECT T.*
FROM Teacher T WHERE T.email is null OR T.email='';
--------------------------------------------------------------------------------------
		-- 5.Выбрать все данные о преподавателях с двойной фамилией.Результат отсортировать по фамилии в лексикографическом порядке,
		-- по имени в порядке обратном лексикографическому и по отчеству в лексикографическом порядке. 
SELECT *
FROM Teacher T
	WHERE T.lastname LIKE('%-%') 
	OR T.lastname LIKE('% %')
	OR T.lastname LIKE('%''%')
	ORDER BY lastname ASC, firstname DESC, middlename ASC;

SELECT*
FROM Teacher 
WHERE lastname LIKE '%-%' 
   OR lastname LIKE '% %'
   OR lastname LIKE '%''%'
ORDER BY lastname ASC, firstname DESC, middlename ASC;
--------------------------------------------------------------------------------------
		-- 6. Выбрать id и фамилии, имена, отчества учащихся, примечание которых содержит mail, gmail, _, или +.
SELECT S.studentid, S.firstname, S.Secondname, S.nikename, S.email
FROM STUDENT S
WHERE S.email LIKE('%mail%') 
	OR S.email LIKE('%gmail%') 
	OR S.email LIKE('%!+%') ESCAPE '!' 
	OR S.email LIKE('%!_%') ESCAPE '!';

	--If I write only the gmail, mail is also containing to the category and there is no need for the mail. 
SELECT S.studentid, S.firstname, S.secondname, S.nikename, S.email
FROM student S
WHERE S.email LIKE '%gmail%' 
   OR S.email LIKE '%!+%' ESCAPE '!'
   OR S.email LIKE '%!_%' ESCAPE '!';

--------------------------------------------------------------------------------------
		-- 7. Выбрать фамилии, имена, отчества учащихся, которые приняты на курсы в текущем месяце. 
		-- variant 1
SELECT S.secondname, S.firstname, S.nikename, S.admissiondate
FROM STUDENT S
WHERE TO_CHAR( S.admissiondate, 'YYYY-MM')=TO_CHAR(CURRENT_DATE, 'YYYY-MM');
		--variant2
SELECT S.secondname, S.firstname, S.nikename, S.admissiondate
FROM Student S 
WHERE EXTRACT(MONTH FROM S.admissiondate)=EXTRACT(MONTH FROM CURRENT_DATE) 
	AND EXTRACT(YEAR FROM S.admissiondate)=EXTRACT(YEAR FROM CURRENT_DATE);

--------------------------------------------------------------------------------------
		-- 8. Выбрать темы курсов с id из диапазона от 2 до 10.
SELECT  CT.name
FROM CourseTheme CT 
	WHERE CT.themeid BETWEEN 2 AND 10;
	
SELECT  *
FROM CourseTheme CT 
	WHERE CT.themeid BETWEEN 2 AND 10;
--------------------------------------------------------------------------------------
		-- 9. Выбрать id единиц оборудования, которые были установлены в январе, феврале, июне или августе прошлого года.
SELECT E.equipmentid, E.name, E.installationdate
FROM Equipment E
	WHERE EXTRACT(YEAR FROM E.installationdate)=EXTRACT(YEAR FROM CURRENT_DATE)-1
	AND EXTRACT(MONTH FROM E.installationdate) IN(1,2,6,8);
	
SELECT *
FROM Equipment E
	WHERE EXTRACT(YEAR FROM E.installationdate)=EXTRACT(YEAR FROM CURRENT_DATE)-1
	AND EXTRACT(MONTH FROM E.installationdate) IN(1,2,6,8);
--------------------------------------------------------------------------------------
 		-- 10. Выбрать все данные об учащихся; если телефон не известен, то вывести в соответствующем столбец «нет»; если не
		-- определен адрес, то вывести «не известно». 
SELECT 
	S.firstname,
	S.secondname,
	S.nikename, 
	CASE
		WHEN S.phone IS NULL OR S.phone='' THEN 'нет'
		ELSE S.phone
	END AS phone, --When I write the S.phone here the posgres is giving me the error which is say that this is a syntax error. 
	CASE 
		WHEN S.address IS NULL OR S.address='' THEN 'не известно'
		ELSE S.address
	END AS address, 
	S.email, 
	S.admissiondate, 
	S.description
	FROM student S;
--------------------------------------------------------------------------------------
 		-- 11.Выбрать дату приема первого обучающегося.
 --variant1.
 SELECT MIN(S.admissiondate)
 FROM Student S;
 --variant2
 SELECT S.firstname, S.secondname, S.admissiondate
 FROM Student S
 WHERE S.admissiondate=(SELECT MIN(St.admissiondate) FROM Student st);
--------------------------------------------------------------------------------------
		-- 12. Выбрать последную и первую даты установки оборудования.
SELECT 
	MIN(E.installationdate) AS FIRST_INSTALLED, 
	MAX(E.installationdate) AS LAST_INSTALLED
FROM Equipment E;

SELECT 
	E.name, 
	E.InventoryNumber, 
	E.installationdate
FROM Equipment E
	WHERE E.installationdate=(SELECT MIN(EQ.installationdate) FROM Equipment EQ) 
	OR E.installationdate=(SELECT MAX(eqp.installationdate) FROM Equipment eqp);
--Select*from Equipment;
--------------------------------------------------------------------------------------
	-- 13.Выбрать номер аудитории, номер учебного места, имя и IP-адрес рабочей станции. Результат отсортировать по номеру аудитории 
	-- в убывающем порядке и по номеру учебного места в убывающем порядке.
SELECT 
	C.number AS Class_Number, 
	b.name AS Building_name, 
	ws.number AS WorkStation_Number, 
	ws.name AS WorkStation_Name
FROM building b 
	join classroom C ON b.buildingid=C.buildingid 
	join workstation ws ON ws.workstationid=ws.classroomid
	ORDER BY C.number DESC, ws.number DESC ;
--------------------------------------------------------------------------------------
	-- 14.Выбрать фамилию, имя, отчество учащегося, название курса, оценку, фамилию, имя, отчество преподавателя. Результат 
	-- отсортировать по фамилии, имени, отчеству учащегося в порядке обратном лексикографическому, по названию курса в лексикографическом порядке. 
SELECT
	S.secondname, 
	S.firstname, 
	S.Nikename, 
	C.Name,
	SP.marks, 
	T.LastName, 
	T.firstname, 
	T.Middlename
FROM StudentPerformance SP 
	JOIN course C ON C.courseid=SP.courseid 
	JOIN student S ON SP.studentid=S.studentid 
	JOIN country Co ON Co.countryid=S.countryid 
	JOIN Teacher T ON T.countryid=Co.countryid 
	ORDER BY S.secondname, S.firstname, S.Nikename DESC, C.Name ASC;
	
			--- Here I am finding the teacher and his lesson which he has given the marks of the lesson to the student in his subject.
SELECT
    S.secondname, 
    S.firstname, 
    S.nikename, 
    C.name AS course_name, 
    SP.marks, 
    T.lastname AS teacher_lastname, 
    T.firstname AS teacher_firstname, 
    T.middlename AS teacher_middlename
FROM StudentPerformance SP 
    JOIN student S ON SP.studentid = S.studentid 
    JOIN course C ON SP.courseid = C.courseid 
    JOIN Lesson L ON C.courseid = L.themeid  
    LEFT JOIN CourseTheme CT ON L.themeid = CT.themeid
    LEFT JOIN Teacher T ON L.teacherid = T.teacherid
ORDER BY S.secondname DESC, S.firstname DESC, S.nikename DESC, C.name ASC;
--------------------------------------------------------------------------------------
		-- 15.Выбрать название курса, название темы, номер планового занятия, дату, фамилию, имя, отчество обучающегося, название 
		-- причины отмены занятия. В результат должны войти только отмененные занятия. 
SELECT 
	C.name, 
	CT.name, 
	SP.Stud_PlanID,
	SP.Planned_Start, 
	S.secondname, 
	S.firstname, 
	S.nikename
FROM Lesson Le 
	JOIN CourseTheme CT ON CT.themeid=Le.themeid
	JOIN Course C ON C.courseid=CT.courseid 
	JOIN Student_Plan SP ON SP.courseid=C.courseid 
	JOIN Student S ON SP.studentid=S.studentid;
----------------------------------------------------------------------------------------
		-- 16. Выбрать среднюю оценку по курсу N (значение подставьте сами).
SELECT 
    C.name AS course_name,
    AVG(SP.marks) AS average_grade
FROM StudentPerformance SP
    JOIN Course C ON SP.courseid = C.courseid
WHERE C.name='Python Programming'         --SP.courseid  IN (1, 2, 3) orrr --Sp.courseid=1;
GROUP BY C.name;
-- select*from course;
----------------------------------------------------------------------------------------
				---Here was the comment. ---
		-- 17. Выбрать общее количество курсов, которые не зависят от других. 
-- SELECT
-- 	C.name, COUNT(*) AS Independent_course_course
-- FROM Course C
-- 	WHERE dependenceid IS NULL OR dependenceid = 4
-- 	GROUP BY C.name;

SELECT COUNT(*) AS Independent_Course_Count
FROM Course C
LEFT JOIN Course_Dependenc D
    ON C.CourseID = D.CourseID
WHERE D.CourseID IS NULL;
-------------
SELECT C.CourseID, C.Name
FROM Course C
LEFT JOIN Course_Dependenc D
    ON C.CourseID = D.CourseID
WHERE D.CourseID IS NULL;
-------------
SELECT COUNT(*) AS Independent_Course_Count
FROM Course C
WHERE C.CourseID NOT IN (
    SELECT CourseID FROM Course_Dependenc
);
---not EXISTS
----------------------------------------------------------------------------------------
		-- 18. Выбрать фамилию и инициалы обучающегося в одном столбце, во втором столбце указать «новый набор», если обучающийся 
		-- принят в текущем учебном году. Результат отсортировать следующим образом: в первую очередь обучающиеся, принятые в 
		-- текущем году, а затем обучающиеся с четным id и в последнюю очередь – с нечетным. 
SELECT 
    secondname || ' ' || firstname || ' ' || nikename || ' ' AS student_Full_Name,
    CASE 
        WHEN EXTRACT(YEAR FROM admissiondate) = EXTRACT(YEAR FROM CURRENT_DATE)
        THEN 'новый набор'
        ELSE ''
    END AS New_admission,
    studentid,
    admissiondate
FROM student
ORDER BY 
    CASE WHEN EXTRACT(YEAR FROM admissiondate) = EXTRACT(YEAR FROM CURRENT_DATE) THEN 1 ELSE 2 END,
    CASE WHEN studentid % 2 = 0 THEN 1 ELSE 2 END,
    studentid;
----------------------------------------------------------------------------------------
		-- 19. Выбрать названия курсов и количество тем в курсе. Результат отсортировать по названию курсов.
SELECT 
    C.name AS Course,
    COUNT(CT.themeid) AS theme_count
FROM Course C
    LEFT JOIN CourseTheme CT ON C.courseid = CT.courseid
GROUP BY C.courseid, C.name
ORDER BY C.name;
----------------------------------------------------------------------------------------
		-- 20. Выбрать фамилию, имя, отчество преподавателя и количество составленных им индивидуальных 
		-- планов в этом учебном году. Результат отсортировать по убыванию количества.
SELECT 
    T.lastname,
    T.firstname,
    T.middlename,
    COUNT(SP.stud_planid) AS Total_Plan
FROM Teacher T
    JOIN Lesson L ON T.teacherid = L.teacherid
    JOIN CourseTheme CT ON L.themeid = CT.themeid
    JOIN Course C ON CT.courseid = C.courseid
    JOIN Student_Plan SP ON C.courseid = SP.courseid
WHERE
    EXTRACT (YEAR FROM SP.planned_start) = EXTRACT(YEAR FROM CURRENT_DATE)
GROUP BY T.teacherid, T.lastname, T.firstname, T.middlename
ORDER BY Total_Plan DESC;
----------------------------------------------------------------------------------------
		-- 21. Выбрать все данные об обучающихся, успешно сдавших более двух курсов.  
SELECT S.*
FROM Student S
WHERE S.studentid IN (
    SELECT SP.studentid
    FROM StudentPerformance SP
    WHERE SP.marks IN(3,4,5)
    GROUP BY SP.studentid
    HAVING COUNT(*) >2
);

--THORUGH JOIN
SELECT S.*
FROM Student S
    JOIN (
        SELECT SP.studentid
        FROM StudentPerformance SP
        WHERE SP.marks IN (3, 4, 5)
        GROUP BY SP.studentid
        HAVING COUNT(*) > 2
    ) SP ON S.studentid = SP.studentid;

SELECT S.*
FROM Student S
    JOIN StudentPerformance SP ON S.studentid = SP.studentid
WHERE SP.marks IN (3, 4, 5)
GROUP BY S.studentid, S.firstname, S.secondname
HAVING COUNT(*) > 2;
----------------------------------------------------------------------------------------
		-- 22. Выбрать для каждого преподавателя год составления его первого индивидуального плана и количество различных курсов, 
		-- которые он преподавал и преподает.
SELECT 
    T.teacherid,
    T.lastname,
    T.firstname ,
    T.middlename,
    MIN(EXTRACT(YEAR FROM SP.planned_start)) AS First_Plan_Year,
    COUNT(DISTINCT CT.courseid) AS Course_Count
FROM Teacher T
    LEFT JOIN Lesson L ON T.teacherid = L.teacherid
    LEFT JOIN CourseTheme CT ON L.themeid = CT.themeid
    LEFT JOIN Student_Plan SP ON CT.courseid = SP.courseid
GROUP BY T.teacherid, T.lastname, T.firstname, T.middlename
ORDER BY First_Plan_Year, Course_Count DESC;
----------------------------------------------------------------------------------------
		-- 23. Выбрать курс, который преподавало несколько преподавателей в прошлом учебном году. Результат отсортировать по 
		-- названию в лексикографическом порядке.
SELECT 
    C.courseid,
    C.name AS course_name,
    COUNT(DISTINCT T.teacherid) AS teacher_count
FROM Course C
    JOIN CourseTheme CT ON C.courseid = CT.courseid
    JOIN Lesson L ON CT.themeid = L.themeid
    JOIN Teacher T ON L.teacherid = T.teacherid
WHERE 
    EXTRACT(YEAR FROM L.date) = EXTRACT(YEAR FROM CURRENT_DATE)-1
GROUP BY C.courseid, C.name
HAVING COUNT(DISTINCT T.teacherid) > 1
ORDER BY C.name;
----------------------------------------------------------------------------------------
		--24.Выбрать классы определенного корпуса (значения подставьте сами), которые имеют более 7 рабочих мест.  
SELECT
	C.Name, C.Number, C.Capacity
FROM Classroom C JOIN Building B on B.BuildingID =C.BuildingID 
WHERE 
	B.Name='Tech Center' AND C.Capacity>30;
----------------------------------------------------------------------------------------
		--25. Выбрать фамилии, имена, отчества преподавателей, которые преподают несколько лет и только один курс. 
		-- Результат отсортировать по количеству лет работы в убывающем порядке.
SELECT
    T.teacherid,
    T.lastname,
    T.firstname,
    T.middlename,
    (MAX(EXTRACT(YEAR FROM CURRENT_DATE)) - MIN(EXTRACT(YEAR FROM L.date))) AS years_teaching,
    COUNT(DISTINCT C.courseid) AS course_count
FROM Teacher T
    JOIN Lesson L ON T.teacherid = L.teacherid
    JOIN CourseTheme CT ON L.themeid = CT.themeid
    JOIN Course C ON CT.courseid = C.courseid
GROUP BY 
    T.teacherid, T.lastname, T.firstname, T.middlename
HAVING 
    COUNT(DISTINCT C.courseid) = 1 ---Only one Course
    AND (MAX(EXTRACT(YEAR FROM L.date)) - MIN(EXTRACT(YEAR FROM L.date))) >= 2
ORDER BY 
    years_teaching DESC;  

----------------------------------------------------------------------------------------
		-- 26. Выбрать для каждого типа оборудования количество установленных единиц для пяти последних лет. В результирующей 
		-- таблице должно быть шесть столбцов: название типа оборудования, 2019, 2018, 2017, 2016, 2015. Исключить из результирующей 
		-- таблицы тип оборудования, который не устанавливался последние пять лет.

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

----------------------------------------------------------------------------------------
		-- 27. Выбрать названия всех курсов и, если есть темы в курсе, то название тем. Результат отсортировать 
		-- по названию курса и названию темы. 
SELECT C.Name, CT.Name
FROM Course C LEFT JOIN CourseTheme CT ON CT.CourseID=C.CourseID
ORDER BY C.Name, CT.Name;
----------------------------------------------------------------------------------------
		-- 28.  Выбрать названия всех курсов и, если курс сдавался обучающимися, то указать количество сдавших. 
		-- Результат отсортировать по количеству в возрастающем порядке; те курсы, которые не сдавались,
		-- должны быть первыми в результирующей таблице.
		
SELECT 
    C.Name,
    COUNT(SP.StudentID) AS PassedCount
FROM Course C
LEFT JOIN StudentPerformance SP ON C.CourseID = SP.CourseID
    AND SP.Marks IN(3,4,5) 
GROUP BY C.Name, C.CourseID
ORDER BY PassedCount ASC;
----------------------------------------------------------------------------------------
		--29. Выбрать для фамилии, имени, отчества каждого обучающегося названия всех курсов. Результат отсортировать 
		-- по фамилии, имени, отчеству и названию курса. 
SELECT DISTINCT ON(S.Secondname, S.FirstName, S.NikeName)
	S.Secondname, S.FirstName, S.NikeName, C.Name AS CourseName
FROM Student S
JOIN StudentPerformance SP ON S.StudentID = SP.StudentID
JOIN Course C ON SP.CourseID = C.CourseID
ORDER BY S.Secondname, S.FirstName, S.Nikename, C.Name;

----------------------------------------------------------------------------------------
		-- 30.Выбрать для фамилии, имени, отчества каждого обучающегося названия всех курсов и,
		-- если обучающийся успешно сдал курс, то его оценку.
SELECT 
    S.Secondname, S.FirstName, S.NikeName, C.Name, SP.Marks,
    CASE 
        WHEN SP.Marks IN(3,4,5) THEN 'passed'
        ELSE 'Not Passed'
    END
FROM Student S
CROSS JOIN Course C
LEFT JOIN StudentPerformance SP ON S.StudentID = SP.StudentID AND C.CourseID = SP.CourseID
ORDER BY 
    S.Secondname, S.FirstName,S.NikeName,C.Name;

----------------------------------------------------------------------------------------
		-- 31. Выбрать фамилии обучающихся, являющихся однофамильцами преподавателям. 
		-- Результат отсортировать в порядке обратном лексикографическому. 
SELECT DISTINCT
    S.Secondname
FROM Student S
JOIN Teacher T 
    ON S.Secondname = T.LastName
ORDER BY S.Secondname DESC;

SELECT DISTINCT 
	S.Secondname
FROM Student S
WHERE EXISTS( SELECT 1 FROM Teacher T WHERE T.Lastname=S.Secondname )
ORDER BY S.Secondname DESC;
--why we are using distanct and the


----------------------------------------------------------------------------------------
		-- 32. Выбрать дни, в которые были отменены все занятия. 
SELECT 
    L.Name,
    L.Date,
    LR.Reason,
    LR.RescedualDate
FROM Lesson L
JOIN Lesson_Replacment LR ON LR.LessonID = L.LessonID
ORDER BY L.Date;
----------------------------------------------------------------------------------------
		-- 33. Выбрать все данные об обучающихся, которые не сдали только один курс из имеющихся в базе данных. 
SELECT 
    S.StudentID,
    S.FirstName,
    S.SecondName,
    S.NikeName
FROM Student S
JOIN (
    SELECT SP.StudentID
    FROM StudentPerformance SP
    RIGHT JOIN Course C ON C.CourseID = SP.CourseID
    GROUP BY SP.StudentID
    HAVING COUNT(C.CourseID) - COUNT(CASE WHEN SP.Marks >= 3 THEN 1 END) = 1
) AS T ON S.StudentID = T.StudentID
ORDER BY S.SecondName, S.FirstName;
----------------------------------------------------------------------------------------
		--34. Выбрать номер корпуса, адрес, номер аудитории, для которой нет учебных мест в БД.
SELECT 
    B.Name, B.Address,C.Number
FROM Classroom C
JOIN Building B ON C.BuildingID = B.BuildingID
LEFT JOIN WorkStation W ON C.ClassroomID = W.ClassroomID
WHERE W.WorkStationID IS NULL;
--------------------
SELECT  B.Name, B.Address,C.Number,C.Name 
FROM Building B
JOIN Classroom C ON B.BuildingID = C.BuildingID
WHERE NOT EXISTS (
    SELECT 1 
    FROM WorkStation WS 
    WHERE WS.ClassroomID = C.ClassroomID
)
ORDER BY B.Name, C.Number;
----------------------------------------------------------------------------------------
		--35. Выбрать названия типов оборудования, которое не устанавливалось последние пять лет.
SELECT TE.Name
FROM Type_equipment TE
WHERE NOT EXISTS (
    SELECT 1 FROM Equipment E
    WHERE E.typeID = TE.typeID AND E.InstallationDate >= CURRENT_DATE - INTERVAL '5 years')
ORDER BY TE.Name;
----------------------------------------------------------------------------------------
		--36. Вывести сообщение «Есть обучающиеся без индивидуального плана», если есть обучающиеся,
		-- для которых не составлен индивидуальный план.
		
SELECT 'Есть обучающиеся без индивидуального плана' AS MyMessage
WHERE EXISTS (
    SELECT 1
    FROM Student S
    LEFT JOIN Student_Plan SP ON S.StudentID = SP.StudentID
    WHERE SP.Stud_PlanID IS NULL
);
--inside NOT EXISTS
SELECT 'Есть обучающиеся без индивидуального плана' AS MyMessage
WHERE EXISTS ( SELECT 1 FROM Student S
    WHERE NOT EXISTS (
        SELECT 1 FROM Student_Plan SP
        WHERE SP.StudentID = S.StudentID)
);

----------------------------------------------------------------------------------------
		--37. Выбрать обучающихся, которые на данный момент изучают курсы, сданные двумя или более обучающимися.
SELECT DISTINCT
    S.StudentID,
    S.FirstName,
    S.SecondName,
    S.NikeName
FROM Student S
JOIN Student_Plan SP ON S.StudentID = SP.StudentID
WHERE SP.Planned_Finish >= CURRENT_DATE
  AND SP.CourseID IN (
      SELECT SP2.CourseID
      FROM StudentPerformance SP2
      WHERE SP2.marks>= 3
      GROUP BY SP2.CourseID
      HAVING COUNT(DISTINCT SP2.StudentID) >= 2)
ORDER BY S.SecondName, S.FirstName;
----------------------------------------------------------------------------------------
		--38. Выбрать пары курсы, которые включают как минимум две общие темы.
SELECT 
    C1.CourseID AS Курс1_ID,
    C1.Name AS Курс1_Название,
    C2.CourseID AS Курс2_ID,
    C2.Name AS Курс2_Название,
    COUNT(DISTINCT CT1.ThemeID) AS ОбщихТем
FROM Course C1
JOIN Course C2 ON C1.CourseID < C2.CourseID  -- Чтобы не было дубликатов (A-B и B-A)
JOIN CourseTheme CT1 ON C1.CourseID = CT1.CourseID
JOIN CourseTheme CT2 ON C2.CourseID = CT2.CourseID AND CT1.ThemeID = CT2.ThemeID
GROUP BY C1.CourseID, C1.Name, C2.CourseID, C2.Name
HAVING COUNT(DISTINCT CT1.ThemeID) >= 2
ORDER BY ОбщихТем DESC, C1.Name, C2.Name;

SELECT 
    LEAST(CT1.CourseID, CT2.CourseID) AS Course1,
    GREATEST(CT1.CourseID, CT2.CourseID) AS Course2,
    COUNT(*) AS CommonThemes
FROM CourseTheme CT1
JOIN CourseTheme CT2 
    ON CT1.Name = CT2.Name  -- или другое условие совпадения темы
    AND CT1.CourseID <> CT2.CourseID
GROUP BY LEAST(CT1.CourseID, CT2.CourseID), GREATEST(CT1.CourseID, CT2.CourseID)
HAVING COUNT(*) >= 2
ORDER BY Course1, Course2;
----------------------------------------------------------------------------------------
		-- 39. Выбрать тройки однофамильцев по всей БД.
WITH Surnames AS (
    SELECT Secondname
    FROM Student
    GROUP BY Secondname
    HAVING COUNT(*) >= 3
)
SELECT S.StudentID,
       S.FirstName,
       S.SecondName,
       S.NikeName,
       S.secondname
FROM Student S
JOIN Surnames SN ON S.Secondname = SN.secondname
ORDER BY S.secondname, S.FirstName, S.SecondName;
----------------------------------------------------------------------------------------
		-- 40. Выбрать среднее количество учебных мест в аудитории.
SELECT AVG(Capacity)
FROM Classroom;
----------------------------------------------------------------------------------------
		-- 41. Выбрать курс, который сдало наибольшее количество человек.
SELECT 
    C.CourseID,
    C.Name,
    COUNT(DISTINCT SP.StudentID) AS Pass_student
FROM Course C
JOIN StudentPerformance SP ON C.CourseID = SP.CourseID
WHERE SP.Marks >= 3
GROUP BY C.CourseID, C.Name
ORDER BY Pass_student DESC
LIMIT 1;
----------------------------------------------------------------------------------------
		-- 42. Выбрать курс, который сдают успешнее всего, т. е. с наивысшей средней оценкой. 
SELECT 
    C.CourseID,
    C.Name,
    AVG(SP.Marks) AS AvgMark
FROM Course C
JOIN StudentPerformance SP ON C.CourseID = SP.CourseID
GROUP BY C.CourseID, C.Name
ORDER BY AvgMark DESC
LIMIT 1;

