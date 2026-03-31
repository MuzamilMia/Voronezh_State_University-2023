DROP TABLE IF EXISTS Lesson_Attendance CASCADE;
DROP TABLE IF EXISTS Student_Plan CASCADE;
DROP TABLE IF EXISTS StudentPerformance CASCADE;
DROP TABLE IF EXISTS Course_Dependenc CASCADE;
DROP TABLE IF EXISTS Lesson_Replacment CASCADE;
DROP TABLE IF EXISTS WorkStation CASCADE;
DROP TABLE IF EXISTS Equipment CASCADE;
DROP TABLE IF EXISTS Type_equipment CASCADE;
DROP TABLE IF EXISTS Classroom CASCADE;
DROP TABLE IF EXISTS Building CASCADE;
DROP TABLE IF EXISTS Lesson_Reason CASCADE;
DROP TABLE IF EXISTS CourseTheme CASCADE;
DROP TABLE IF EXISTS Student CASCADE;
DROP TABLE IF EXISTS Course CASCADE;
DROP TABLE IF EXISTS Category CASCADE;
DROP TABLE IF EXISTS Lesson CASCADE;
DROP TABLE IF EXISTS Teacher_Direction CASCADE;
DROP TABLE IF EXISTS Direction CASCADE;
DROP TABLE IF EXISTS Teacher CASCADE;
DROP TABLE IF EXISTS Country CASCADE;
DROP TABLE IF EXISTS Status CASCADE;
--1
CREATE TABLE Country (
    CountryID SERIAL,
    Name VARCHAR(100),
    ShortName VARCHAR(30),
    CodeNumber VARCHAR(20),
    Description TEXT
);

--2
CREATE TABLE Teacher (
    TeacherID SERIAL,
    LastName VARCHAR(100),
    FirstName VARCHAR(100),
    MiddleName VARCHAR(100),
    BirthDate DATE,
    Phone VARCHAR(20),
    Address TEXT,
    Email VARCHAR(100),
    CountryID INT
);

--3
CREATE TABLE Direction (
    DirectionID SERIAL,
    Name VARCHAR(100)
);

--4
CREATE TABLE Teacher_Direction (

    DirectionID INT,
    TeacherID INT
);

--5
CREATE TABLE Lesson (
    LessonID SERIAL,
    Date DATE,
    Name VARCHAR(100),
    Status VARCHAR(50),
    onlinclassname VARCHAR(100),
    LinktoClass VARCHAR(200),
    Descrip_Online TEXT,
	TeacherID INT,
    ThemeID INT,
    ReasonID INT,
    ClassroomID INT,
	ReplacementID INT
);

--6
CREATE TABLE Course (
    CourseID SERIAL,
    Name VARCHAR(100),
    Description TEXT,
    StartingDate DATE,
    EndingDate DATE,
    CategoryID INT
);

--7
CREATE TABLE CourseTheme (
    ThemeID SERIAL,
    Name VARCHAR(100),
    Description TEXT,
    CourseID INT
);

--8
CREATE TABLE Category (
    CategoryID SERIAL,
    Name VARCHAR(100),
    Description TEXT
);

--9
CREATE TABLE Student (
    StudentID SERIAL,
    FirstName VARCHAR(100),
    SecondName VARCHAR(100),
    NikeName VARCHAR(100),
    Phone VARCHAR(20),
    Address VARCHAR(200),
    Email VARCHAR(100),
    AdmissionDate DATE,
    Description TEXT,
    CountryID INT
);

--10
CREATE TABLE StudentPerformance (
    StudentCourse SERIAL,
    Marks INTEGER,
    status_id INT,
    CourseID INT,
    StudentID INT
);

--11
CREATE TABLE Student_Plan (
    Stud_PlanID SERIAL,
    Planned_Start DATE,
    Planned_Finish DATE,
    CourseID INT,
    StudentID INT
);

--12
CREATE TABLE Lesson_Reason (
    ReasonID SERIAL,
    Description TEXT
);

--13
CREATE TABLE Building (
    BuildingID SERIAL,
    Name VARCHAR(100),
    Address VARCHAR(200),
    Description TEXT
);

--14
CREATE TABLE Classroom (
    ClassroomID SERIAL,
    Name VARCHAR(100),
    Number VARCHAR(20),
    Description TEXT,
    BuildingID INT
);

--15
CREATE TABLE Equipment (
    EquipmentID SERIAL,
    ClassroomID INT,
    Name VARCHAR(100),
    InventoryNumber VARCHAR(50),
    InstallationDate DATE,
    Description TEXT,
    typeID INT
);

--16
CREATE TABLE WorkStation (
    WorkStationID SERIAL,
    ClassroomID INT,
    Name VARCHAR(100),
    Number VARCHAR(50),
    Address VARCHAR(200),
    Description TEXT
);

--17
-- CREATE TABLE Course_Dependenc (
--     DependencID SERIAL,
--     Description TEXT,
--     CourseID INT
-- );

CREATE TABLE Course_Dependenc (
    CourseID INT NOT NULL,
    DependsOnCourseID INT NOT NULL,
    Description TEXT    
);


--18
CREATE TABLE Lesson_Attendance (
    AttendanceID SERIAL,
    status VARCHAR(50),
    Comments TEXT,
    Description TEXT,
	LessonID INT,
    StudentID INT
);

--19
CREATE TABLE Type_equipment (
    typeID SERIAL,
    Name VARCHAR(100),
    registerNumber VARCHAR(50),
    ShortName VARCHAR(50)
);

--20
CREATE TABLE Status (
    status_id SERIAL,
    Name VARCHAR(50),
    Discription TEXT
);
--21 
CREATE TABLE Lesson_Replacment (
    ReplacementID SERIAL,
    Reason VARCHAR(200),
    RescedualDate DATE
);

--------------------------------------------------Now the limitation will be added---------------------------------------------

-------------- This section is only for the Primary Keys -------------------------
--Country 1
ALTER TABLE Country
    ADD CONSTRAINT pk_country PRIMARY KEY (CountryID);
--Teacher 2
ALTER TABLE Teacher
    ADD CONSTRAINT pk_teacher PRIMARY KEY (TeacherID);
--Direction 3
ALTER TABLE Direction
    ADD CONSTRAINT pk_direction PRIMARY KEY (DirectionID);
--Teacher_Direction 4
ALTER TABLE Teacher_Direction
    ADD CONSTRAINT pk_teacher_direction PRIMARY KEY (DirectionID, TeacherID);
--Lesson 5
ALTER TABLE Lesson
    ADD CONSTRAINT pk_lesson PRIMARY KEY (LessonID);	
-- Course 6
ALTER TABLE Course
    ADD CONSTRAINT pk_course PRIMARY KEY (CourseID);
--CourseTheme 7
ALTER TABLE CourseTheme
    ADD CONSTRAINT pk_course_theme PRIMARY KEY (ThemeID);
--Category 8
ALTER TABLE Category
    ADD CONSTRAINT pk_category PRIMARY KEY (CategoryID);
--Student 9
ALTER TABLE Student
    ADD CONSTRAINT pk_student PRIMARY KEY (StudentID);
--StudentPerformance 10
ALTER TABLE StudentPerformance
    ADD CONSTRAINT pk_student_course PRIMARY KEY (StudentCourse);
--Student_Plan 11
ALTER TABLE Student_Plan
    ADD CONSTRAINT pk_student_plan PRIMARY KEY (Stud_PlanID);
-- Lesson_Reason    12
ALTER TABLE Lesson_Reason
    ADD CONSTRAINT pk_lesson_reason PRIMARY KEY (ReasonID);
--Building 13
ALTER TABLE Building
    ADD CONSTRAINT pk_building PRIMARY KEY (BuildingID);
--Classroom 14
ALTER TABLE Classroom
    ADD CONSTRAINT pk_classroom PRIMARY KEY (ClassroomID);
--Equipment 15
ALTER TABLE Equipment
    ADD CONSTRAINT pk_equipment PRIMARY KEY (EquipmentID);
--WorkStation 16
ALTER TABLE WorkStation
    ADD CONSTRAINT pk_workstation PRIMARY KEY (WorkStationID);
--Course_Dependenc 17

ALTER TABLE Course_Dependenc
	ADD CONSTRAINT pk_course_dependenc PRIMARY KEY (CourseID, DependsOnCourseID);

--ALTER TABLE Course_Dependenc
 --   ADD CONSTRAINT pk_course_dependenc PRIMARY KEY (DependencID);
	
--Lesson_Attendance 18
ALTER TABLE Lesson_Attendance
    ADD CONSTRAINT pk_lesson_attendance PRIMARY KEY (AttendanceID);
--Type_equipment 19
ALTER TABLE Type_equipment
    ADD CONSTRAINT pk_type_equipment PRIMARY KEY (typeID);
-- Status 20
ALTER TABLE Status
    ADD CONSTRAINT pk_status PRIMARY KEY (status_id);
-- Lesson_Replacment 21 
ALTER TABLE Lesson_Replacment
    ADD CONSTRAINT pk_lesson_replacment PRIMARY KEY (ReplacementID);
-----------------------------------------------------------------------------------------

----------------------------------------- This Section is for the Foreign Key --------------------------------
--Country 1
ALTER TABLE Country
    ALTER COLUMN Name SET NOT NULL,
    ALTER COLUMN ShortName SET NOT NULL,
    ALTER COLUMN CodeNumber SET NOT NULL;
	
--Teacher 2
ALTER TABLE Teacher
    ALTER COLUMN LastName SET NOT NULL,
    ALTER COLUMN FirstName SET NOT NULL,
    ALTER COLUMN BirthDate SET NOT NULL,
    ALTER COLUMN Phone SET NOT NULL,
    ADD CONSTRAINT fk_teacher_country FOREIGN KEY (CountryID) REFERENCES Country(CountryID);
	
--Direction 3
ALTER TABLE Direction
    ALTER COLUMN Name SET NOT NULL;

-- Teacher_Direction 4
ALTER TABLE Teacher_Direction
    ADD CONSTRAINT fk_teacher_direction_direction FOREIGN KEY (DirectionID) REFERENCES Direction(DirectionID),
    ADD CONSTRAINT fk_teacher_direction_teacher FOREIGN KEY (TeacherID) REFERENCES Teacher(TeacherID); 

-- Lesson 5
ALTER TABLE Lesson
    ALTER COLUMN Date SET NOT NULL,
    ALTER COLUMN Name SET NOT NULL,
    ADD CONSTRAINT fk_lesson_teacher FOREIGN KEY (TeacherID) REFERENCES Teacher(TeacherID),
    ADD CONSTRAINT fk_lesson_theme FOREIGN KEY (ThemeID) REFERENCES CourseTheme(ThemeID),
    ADD CONSTRAINT fk_lesson_reason FOREIGN KEY (ReasonID) REFERENCES Lesson_Reason(ReasonID),
    ADD CONSTRAINT fk_lesson_classroom FOREIGN KEY (ClassroomID) REFERENCES Classroom(ClassroomID),
    ADD CONSTRAINT fk_lesson_replacement FOREIGN KEY (ReplacementID) REFERENCES Lesson_Replacment(ReplacementID);

-- Course 6
ALTER TABLE Course
    ALTER COLUMN Name SET NOT NULL,
    ALTER COLUMN EndingDate SET NOT NULL,
    ADD CONSTRAINT fk_course_category FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID);

-- CourseTheme 7
ALTER TABLE CourseTheme
    ALTER COLUMN Name SET NOT NULL,
    ADD CONSTRAINT fk_course_theme_course FOREIGN KEY (CourseID) REFERENCES Course(CourseID);

-- Category 8
ALTER TABLE Category
    ALTER COLUMN Name SET NOT NULL;
	
-- Student 9
ALTER TABLE Student
    ALTER COLUMN FirstName SET NOT NULL,
    ADD CONSTRAINT fk_student_country FOREIGN KEY (CountryID) REFERENCES Country(CountryID);

--StudentCourse or StudentPerformance 10
ALTER TABLE StudentPerformance
    ADD CONSTRAINT fk_student_course_status FOREIGN KEY (status_id) REFERENCES Status(status_id),
    ADD CONSTRAINT fk_student_course_course FOREIGN KEY (CourseID) REFERENCES Course(CourseID),
    ADD CONSTRAINT fk_student_course_student FOREIGN KEY (StudentID) REFERENCES Student(StudentID);

-- Student_Plan 11
ALTER TABLE Student_Plan
    ALTER COLUMN Planned_Start SET NOT NULL,
    ALTER COLUMN Planned_Finish SET NOT NULL,
    ADD CONSTRAINT fk_student_plan_course FOREIGN KEY (CourseID) REFERENCES Course(CourseID),
    ADD CONSTRAINT fk_student_plan_student FOREIGN KEY (StudentID) REFERENCES Student(StudentID);

-- Building 13
ALTER TABLE Building
    ALTER COLUMN Name SET NOT NULL,
    ALTER COLUMN Address SET NOT NULL;

-- Classroom 14
ALTER TABLE Classroom
    ALTER COLUMN Number SET NOT NULL,
    ADD CONSTRAINT fk_classroom_building FOREIGN KEY (BuildingID) REFERENCES Building(BuildingID);

-- Equipment 15
ALTER TABLE Equipment
    ALTER COLUMN Name SET NOT NULL,
    ALTER COLUMN InventoryNumber SET NOT NULL,
    ADD CONSTRAINT fk_equipment_classroom FOREIGN KEY (ClassroomID) REFERENCES Classroom(ClassroomID),  
    ADD CONSTRAINT fk_equipment_type FOREIGN KEY (typeID) REFERENCES Type_equipment(typeID); 

-- WorkStation 16
ALTER TABLE WorkStation
    ALTER COLUMN Name SET NOT NULL,
    ALTER COLUMN Number SET NOT NULL,
    ADD CONSTRAINT fk_workstation_classroom FOREIGN KEY (ClassroomID) REFERENCES Classroom(ClassroomID); 
	
--ALTER TABLE Course_Dependenc
  --  ADD CONSTRAINT fk_course_dependenc_course FOREIGN KEY (CourseID) REFERENCES Course(CourseID); 
 ALTER TABLE Course_Dependenc
 	ADD CONSTRAINT fk_course_id FOREIGN KEY (CourseID) REFERENCES Course(CourseID),
    ADD CONSTRAINT fk_course_dependen_course FOREIGN KEY (DependsOnCourseID) REFERENCES Course(CourseID);

-- Lesson_Attendance 18
ALTER TABLE Lesson_Attendance
    ALTER COLUMN status SET NOT NULL,
    ADD CONSTRAINT fk_lesson_attendance_lesson FOREIGN KEY (LessonID) REFERENCES Lesson(LessonID), 
    ADD CONSTRAINT fk_lesson_attendance_student FOREIGN KEY (StudentID) REFERENCES Student(StudentID); 

-- Type_equipment 19 
ALTER TABLE Type_equipment
    ALTER COLUMN Name SET NOT NULL,
    ALTER COLUMN registerNumber SET NOT NULL;

-- Status 20
ALTER TABLE Status
    ALTER COLUMN Name SET NOT NULL;
	
-- Lesson_Replacment 21 
ALTER TABLE Lesson_Replacment
    ALTER COLUMN Reason SET NOT NULL,
    ALTER COLUMN RescedualDate SET NOT NULL;

--------------------------------Insertion into Country ----------------------------
INSERT INTO Country 
(Name, ShortName, CodeNumber, Description) VALUES
('Afghanistan', 'AFG', '093', 'My native birth land');

INSERT INTO Country
(Name, ShortName, CodeNumber, Description) VALUES
('Russia', 'RU', '079', 'The beautiful country');

INSERT INTO Country 
(Name, ShortName, CodeNumber, Description) VALUES
('United Kingdom', 'UK', 0089,'One of the beautiful land');

INSERT INTO Country 
(Name, ShortName, CodeNumber, Description) VALUES
('Canada', 'CA', 001,'One of the coldest land in world'),
('United States', 'USA', '002', 'North American country'),
('Germany', 'DE', '049', 'European country'),
('France', 'FR', '033', 'Western European country');



-----------------------------------Insertion into Teacher-------------------------------
INSERT INTO Teacher
(LastName, FirstName, MiddleName, BirthDate, Phone, Address,Email, CountryID) VALUES
('Mia','Muzamil','','2000-2-23','+79539342355','Voronezh,Russia','daf@gmail.com',
	(SELECT CountryID FROM Country WHERE ShortName='AFG'));

INSERT INTO Teacher
(LastName, FirstName, MiddleName, BirthDate, Phone, Address, Email, CountryID) VALUES 
('Oleg','Alekcy','Alekcyevich', '1990-1-15','+72343242366', 'Moscow', 'aksdf@gmail.com',
	(SELECT CountryID FROM Country WHERE ShortName='RU'));

INSERT INTO Teacher
(LastName, FirstName, MiddleName, BirthDate, Phone, Address, Email, CountryID) VALUES
('Liam','Donald','Jan','1934-4-23','+42341313411','New York', 'myna@mail.com',
	(SELECT CountryID FROM Country WHERE ShortName='US'));

INSERT INTO Teacher
(LastName, FirstName, MiddleName, BirthDate, Phone, Address, Email, CountryID)VALUES
('Khan', 'Mahmmod','','2003-2-23','+931245533522','Kandahar', 'khan@gmail.com',
	(SELECT CountryID FROM Country WHERE ShortName='FR'));

INSERT INTO Teacher
(LastName, FirstName, MiddleName, BirthDate, Phone, Address, Email, CountryID	) VALUES
('Nasrat','Rasool','Sahib', '1987-6-23','+12345235322','Ontario','rasool@mail.com',
	(SELECT CountryID FROM Country WHERE ShortName='DE'));
	
INSERT INTO Teacher
(LastName, FirstName, MiddleName, BirthDate, Phone, Address, Email, CountryID	) VALUES
('Babi-Tarin','Khan','Sahibi', '1917-6-23','+12345235322','Ottawa','',
	(SELECT CountryID FROM Country WHERE ShortName='CA'));

INSERT INTO Teacher
(LastName, FirstName, MiddleName, BirthDate, Phone, Address, Email, CountryID	) VALUES
('Edris-Mia','Kandahari','Alivi', '1912-9-01','+12345235322','Kabul','',
	(SELECT CountryID FROM Country WHERE ShortName='Afg'));

INSERT INTO Teacher
(LastName, FirstName, MiddleName, BirthDate, Phone, Address, Email, CountryID) VALUES
('Laura','Josep','Josee','1942-9-14','+43452436642','Las Vegas', 'joshe@mail.com',
	(SELECT CountryID FROM Country WHERE ShortName='US'));

--SELECT*FROM Teacher INNER JOIN Country ON Teacher.CountryID=Country.CountryID ORDER BY TeacherID ASC;

-----------------------------------Insertion into Direction -------------------------------
INSERT INTO Direction (Name) VALUES ('Artificial Intelligence');
INSERT INTO Direction (Name) VALUES ('Software Engineering');
INSERT INTO Direction (Name) VALUES ('Computer Systems');
INSERT INTO Direction (Name) VALUES ('Network and Security');
INSERT INTO Direction (Name) VALUES ('Data Science');
INSERT INTO Direction (Name) VALUES ('Cyber Security');

--SELECT 'Names', Direction.Name  FROM Direction;
-- -----------------------------------Insertion into Teacher_Direction -----------------------

INSERT INTO Teacher_Direction (DirectionID,TeacherID) VALUES 
((SELECT DirectionID FROM Direction WHERE Name='Computer Systems'),(SELECT TeacherID FROM Teacher WHERE LastName='Mia' AND FirstName='Muzamil'));

INSERT INTO Teacher_Direction (DirectionID, TeacherID) VALUES
((SELECT DirectionID FROM Direction WHERE Name='Cyber Security'),(SELECT TeacherID FROM Teacher WHERE LastName='Laura' AND FirstName='Josep'));

INSERT INTO Teacher_Direction (DirectionID, TeacherID) VALUES
((SELECT DirectionID FROM Direction WHERE Name='Cyber Security'),(SELECT TeacherID FROM Teacher WHERE LastName='Nasrat' AND FirstName='Rasool'));

INSERT INTO Teacher_Direction (DirectionID, TeacherID) VALUES
((SELECT DirectionID FROM Direction WHERE Name='Software Engineering'), (SELECT TeacherID FROM Teacher WHERE LastName='Khan' AND FirstName='Mahmmod'));

--SELECT*FROM Teacher_Direction;

-- SELECT*FROM Teacher INNER JOIN Teacher_Direction ON Teacher.TeacherID=Teacher_Direction.TeacherID 
-- INNER JOIN Direction ON Direction.DirectionID=Teacher_Direction.DirectionID ;
--INNER JOIN Country ON Country.CountryID=Teacher.TeacherID;

-----------------------------------Insertion into Lesson_Reason -------------------------------
INSERT INTO Lesson_Reason (Description) VALUES ('Regular scheduled class');
INSERT INTO Lesson_Reason (Description) VALUES ('Extra practice session');
INSERT INTO Lesson_Reason (Description) VALUES ('Exam preparation');
INSERT INTO Lesson_Reason (Description) VALUES ('Guest lecture');
INSERT INTO Lesson_Reason (Description) VALUES ('Introduction lecture');

-----------------------------------Insertion into Lesson_Replacment ---------------------------
INSERT INTO Lesson_Replacment (Reason, RescedualDate) VALUES 
	('Class not avaliable', '2025-09-30'),
	('Professor illness','2025-09-22'),
	('Technical issues','2024-03-13'),
	('Other','2020-05-23');

-----------------------------------Insertion into Category ------------------------------------
INSERT INTO Category (Name, Description) VALUES 
	('Programming', 'Software development courses'),
	('Design','Graphic and UI/UX design'),
	('Business', 'Business and management'),
	('Language', 'Language learning courses'),
	('Mathematics', 'Math and statistics courses');

-----------------------------------Insertion into Course --------------------------------------
--ALTER TABLE Course ALTER COLUMN DependenceID DROP NOT NULL;

INSERT INTO Course (Name, Description, startingDate, EndingDate, CategoryID) VALUES
('Python Programming', 'Introduction to Python programming language', '2024-01-15', '2024-03-15',
	(SELECT CategoryID FROM Category WHERE Name='Programming'));

INSERT INTO Course (Name, Description, startingDate, EndingDate, CategoryID) VALUES
('Web Development', 'Full-stack web development course', '2024-02-01', '2024-05-01', 
	(SELECT CategoryID FROM Category WHERE Name='Programming'));

INSERT INTO Course (Name, Description, startingDate, EndingDate, CategoryID) VALUES
('Graphic Design Fundamentals', 'Basic principles of graphic design', '2024-01-20', '2024-04-20', 
	(SELECT CategoryID FROM Category WHERE Name='Design'));

INSERT INTO Course (Name, Description, startingDate, EndingDate, CategoryID) VALUES
('Business Management', 'Introduction to business administration', '2024-03-01', '2024-06-01', 
	(SELECT CategoryID FROM Category WHERE NAME='Business'));

INSERT INTO Course (Name, Description, startingDate, EndingDate, CategoryID) VALUES
('Data Analysis', 'Data analysis with Python and SQL', '2024-02-15', '2024-05-15', 
	(SELECT CategoryID FROM Category WHERE NAME='Mathematics'));

INSERT INTO Course (Name, Description, startingDate, EndingDate, CategoryID) VALUES
('Advanced Databases', 'Optimization, indexing', '2025-03-05', '2025-04-05',
	(SELECT CategoryID FROM Category WHERE Name='Programming'));

INSERT INTO Course (Name, Description, startingDate, EndingDate, CategoryID) VALUES
('Programming Basics', 'Learn basics of programming','2025-01-12', '2025-02-25',
	(SELECT CategoryID FROM Category WHERE Name='Programming'));

INSERT INTO Course (Name, Description, startingDate, EndingDate, CategoryID) VALUES
('Data Structures', 'Learn common data structures', '2025-04-05', '2025-05-05',
	(SELECT CategoryID FROM Category WHERE Name='Programming'));

INSERT INTO Course (Name, Description, startingDate, EndingDate, CategoryID) VALUES
('Mathematics II', 'Advanced mathematics', '2025-02-15', '2025-03-20',
	(SELECT CategoryID FROM Category WHERE Name='Programming'));

INSERT INTO Course (Name, Description, startingDate, EndingDate, CategoryID) VALUES
('Mathematics I', 'Basic math course', '2025-01-10', '2025-02-10',
	(SELECT CategoryID FROM Category WHERE Name='Programming'));

INSERT INTO Course (Name, Description, startingDate, EndingDate, CategoryID) VALUES
('Database Fundamentals', 'Basic relational DB concepts', '2025-02-01', '2025-03-01',
	(SELECT CategoryID FROM Category WHERE Name='Programming'));
-----------------------------------Insertion into Course_Dependenc ----------------------------

INSERT INTO Course_Dependenc (CourseID, DependsOnCourseID, Description)
VALUES (
    (SELECT CourseID FROM Course WHERE Name='Web Development'),
    (SELECT CourseID FROM Course WHERE Name='Python Programming'),
    'Basic programming knowledge required'
);

INSERT INTO Course_Dependenc (CourseID, DependsOnCourseID, Description)
VALUES (
    (SELECT CourseID FROM Course WHERE Name='Data Analysis'),
    (SELECT CourseID FROM Course WHERE Name='Python Programming'),
    'Course requires Python basics'
);

INSERT INTO Course_Dependenc (CourseID, DependsOnCourseID, Description)
VALUES (
    (SELECT CourseID FROM Course WHERE Name='Data Analysis'),
    (SELECT CourseID FROM Course WHERE Name='Web Development'),
    'Some web fundamentals recommended'
);

INSERT INTO Course_Dependenc (CourseID, DependsOnCourseID, Description)
VALUES (
    (SELECT CourseID FROM Course WHERE Name='Mathematics II'),
    (SELECT CourseID FROM Course WHERE Name='Mathematics I'),
    'Requires understanding of basic mathematics'
);

INSERT INTO Course_Dependenc (CourseID, DependsOnCourseID, Description)
VALUES (
    (SELECT CourseID FROM Course WHERE Name='Data Structures'),
    (SELECT CourseID FROM Course WHERE Name='Programming Basics'),
    'Important to know variables, loops, and functions'
);

INSERT INTO Course_Dependenc (CourseID, DependsOnCourseID, Description)
VALUES (
    (SELECT CourseID FROM Course WHERE Name='Advanced Databases'),
    (SELECT CourseID FROM Course WHERE Name='Database Fundamentals'),
    'Must understand relational database basics'
);


-----------------------------------Insertion into UPDATING Course TABLE ------------------------------------
--======================
-- First Method which it works: 
-- UPDATE Course SET DependenceID = 1 WHERE CourseID = 1;
-- UPDATE Course SET DependenceID = 2 WHERE CourseID = 2;
-- UPDATE Course SET DependenceID = 3 WHERE CourseID = 3;
-- UPDATE Course SET DependenceID = 4 WHERE CourseID = 4;
-- UPDATE Course SET DependenceID = 5 WHERE CourseID = 5;

--ALTER TABLE Course ALTER COLUMN DependenceID SET NOT NULL;
--=============================
							--Second Methosd it is not giving the reference to the rows
-- UPDATE Course 
-- SET DependenceID = (
--     SELECT DependenceID 
--     FROM Course_Dependenc 
--     WHERE Course_Dependenc.CourseID = Course.CourseID
-- )
-- WHERE DependenceID IS NULL;
							--Third Method, it is not giving the reference to the rows as well.
-- UPDATE Course
-- SET DependenceID= (SELECT DependenceID FROM Course_Dependenc WHERE CourseID=(SELECT CourseID FROM Course WHERE Name='Python Programming'))
-- WHERE Name='Python Programming';

-- UPDATE Course
-- SET DependenceID = ( SELECT DependenceID FROM Course_Dependenc 
--     WHERE CourseID = (SELECT CourseID FROM Course WHERE Name = 'Web Development') )
-- WHERE Name = 'Web Development';

-- UPDATE Course
-- SET DependenceID = ( SELECT DependenceID FROM Course_Dependenc 
--     WHERE CourseID = (SELECT CourseID FROM Course WHERE Name = 'Graphic Design Fundamentals') )
-- WHERE Name = 'Graphic Design Fundamentals';

-- UPDATE Course
-- SET DependenceID = ( SELECT DependenceID FROM Course_Dependenc 
--     WHERE CourseID = (SELECT CourseID FROM Course WHERE Name = 'Business Management'))
-- WHERE Name = 'Business Management';

-- UPDATE Course
-- SET DependenceID = ( SELECT DependenceID FROM Course_Dependenc 
--     WHERE CourseID = (SELECT CourseID FROM Course WHERE Name = 'Data Analysis') )
-- WHERE Name = 'Data Analysis';

-----------------------------------Insertion into CourseTheme  --------------------------------------

INSERT INTO CourseTheme (Name, Description, CourseID) VALUES
('Python Basics', 'Variables, data types, and basic syntax', (SELECT CourseID FROM Course WHERE Name='Python Programming'));

INSERT INTO CourseTheme(Name, Description, CourseID) VALUES 
('Functions and Modules', 'Creating and using functions and modules', (SELECT CourseID FROM Course WHERE Name='Python Programming'));

INSERT INTO CourseTheme(Name, Description, CourseID) VALUES
	('HTML/CSS', 'Front-end web development basics', (SELECT CourseID FROM Course WHERE Name='Web Development')),
	('JavaScript', 'Client-side programming', (SELECT CourseID FROM Course WHERE Name='Web Development')),
	('Color Theory', 'Understanding color in design', (SELECT CourseID FROM Course WHERE Name='Graphic Design Fundamentals')),
	('Typography', 'Fonts and text design', (SELECT CourseID FROM Course WHERE Name='Graphic Design Fundamentals')),
	('Business Strategy', 'Strategic planning and execution', (SELECT CourseID FROM Course WHERE Name='Business Management')),
	('Data Visualization', 'Creating charts and graphs', (SELECT CourseID FROM Course WHERE Name='Data Analysis'));

-----------------------------------Insertion into Building ------------------------------------
INSERT INTO Building (Name, Address, Description) VALUES
('Main Campus', '123 University Ave', 'Primary academic building'),
('Tech Center', '456 Innovation St', 'Technology and computer labs'),
('Arts Building', '789 Creative Lane', 'Design and arts department'),
('Business Hall', '321 Commerce Rd', 'Business studies center');

-----------------------------------Insertion into Classroom -----------------------------------
INSERT INTO Classroom (Name, Number, Description, BuildingID) VALUES
('Programming Lab', 'LAB-101', 'Computer programming laboratory', (SELECT BuildingID FROM Building WHERE Name='Tech Center')),
('Lecture Hall A', 'LHA-201', 'Large lecture hall', (SELECT BuildingID FROM Building WHERE Name='Main Campus')),
('Design Studio', 'DS-301', 'Creative design workspace', (SELECT BuildingID FROM Building WHERE Name='Main Campus')),
('Conference Room', 'CR-102', 'Small group meeting room', (SELECT BuildingID FROM Building WHERE Name='Arts Building')),
('Computer Lab B', 'LAB-202', 'General computer lab', (SELECT BuildingID FROM Building WHERE Name='Tech Center'));

-----------------------------------Insertion into WorkStation ---------------------------------
INSERT INTO WorkStation (ClassroomID, Name, Number, Address, Description) VALUES
((SELECT ClassroomID FROM Classroom WHERE Number='LAB-101'), 'Developer Station 1', 'WS-101', '192.168.1.101', 'Primary development workstation'),
((SELECT ClassroomID FROM Classroom WHERE Number='LAB-202'), 'Developer Station 2', 'WS-102', '192.168.1.102', 'Secondary development workstation'),
((SELECT ClassroomID FROM Classroom WHERE Number='LHA-201'), 'Instructor Station', 'WS-201', '192.168.1.201', 'Lecturer workstation'),
((SELECT ClassroomID FROM Classroom WHERE Number='DS-301'), 'Design Station 1', 'WS-301', '192.168.1.301', 'Graphic design workstation');

-----------------------------------Insertion into Type_equipment ------------------------------
INSERT INTO Type_equipment (Name, registerNumber, ShortName) VALUES
('Desktop Computer', 'COMP-001', 'PC'),
('Projector', 'PROJ-001', 'PROJ'),
('Projector', 'PROJ-002', 'PROJ'),
('Whiteboard', 'WB-001', 'WB'),
('Printer', 'PRINT-001', 'PRINT'),
('Network Switch', 'NET-001', 'SWITCH');

-----------------------------------Insertion into Equipment -----------------------------------
INSERT INTO Equipment (ClassroomID, Name,InventoryNumber,InstallationDate, Description, typeID) VALUES
((SELECT ClassroomID From Classroom WHERE Number='LAB-101'), 'Dell Workstation', 'INV-COMP-001', '2025-01-15', 
		'High-performance computer', (SELECT typeID FROM Type_equipment WHERE registerNumber='PROJ-001' ));

INSERT INTO Equipment (ClassroomID, Name,InventoryNumber,InstallationDate, Description, typeID) VALUES
((SELECT ClassroomID From Classroom WHERE Number='LHA-201'), 'Epson Projector', 'INV-PROJ-001', '2023-08-20', 
		'HD Projector', (SELECT typeID FROM Type_equipment WHERE registerNumber='PROJ-002'));

INSERT INTO Equipment (ClassroomID, Name,InventoryNumber,InstallationDate, Description, typeID) VALUES
((SELECT ClassroomID From Classroom WHERE Number='DS-301'), 'Interactive Whiteboard', 'INV-WB-001', '2024-03-10', 
		'Smart whiteboard', (SELECT typeID FROM Type_equipment WHERE registerNumber='WB-001')),
		
((SELECT ClassroomID From Classroom WHERE Number='DS-301'), 'HP Laser Printer', 'INV-PRINT-001', '2024-01-25', 
		'Network printer', (SELECT typeID FROM Type_equipment WHERE registerNumber='PRINT-001'));

-----------------------------------Insertion into Lesson --------------------------------------
ALTER TABLE lesson ALTER COLUMN ReplacementID DROP NOT NULL;
--I want to add a condtion like this, when the classroomID is not Null it means that the lesson is provided and no need for replacement

INSERT INTO lesson (DATE,Name, Status, onlinclassname, LinktoClass, Descrip_Online,TeacherID, ThemeID, ReasonID, ClassroomID, ReplacementID)
VALUES ('2025-09-24','Database Fundementals','Face-to-Face', null, null, null, 
		(SELECT TeacherID FROM Teacher WHERE LastName='Liam' AND FirstName='Donald' AND MiddleName='Jan'),
		(SELECT ThemeID FROM CourseTheme WHERE Name='Data Visualization'),
		(SELECT ReasonID FROM Lesson_Reason WHERE Description='Introduction lecture'),
		(SELECT ClassroomID FROM Classroom WHERE Number='LAB-101'),
		NULL
		);

INSERT INTO lesson (DATE,Name, Status, onlinclassname, LinktoClass, Descrip_Online,TeacherID, ThemeID, ReasonID, ClassroomID, ReplacementID)
VALUES ('2025-04-01','Python Introduction','Face-to-Face', null, null, null, 
		(SELECT TeacherID FROM Teacher WHERE LastName='Mia' AND FirstName='Muzamil' AND MiddleName=''),
		(SELECT ThemeID FROM CourseTheme WHERE Name='Python Basics'),
		(SELECT ReasonID FROM Lesson_Reason WHERE Description='Extra practice session'),
		(SELECT ClassroomID FROM Classroom WHERE Number='CR-102'),
		NULL
		);
		
INSERT INTO lesson (DATE,Name, Status, onlinclassname, LinktoClass, Descrip_Online,TeacherID, ThemeID, ReasonID, ClassroomID, ReplacementID)
VALUES ('2025-01-16', 'HTML Basics', 'Online', 'HTML-Basics', 'https://meet.google.com/abc-123', 'Introduction to HTML programming',
	(SELECT TeacherID FROM Teacher WHERE LastName='Khan' AND FirstName='Mahmmod'), 
	(SELECT ThemeID FROM CourseTheme WHERE Name='HTML/CSS'), 
	(SELECT ReasonID FROM Lesson_Reason WHERE Description='Regular scheduled class'), 
	(SELECT ClassroomID FROM Classroom WHERE Number='LAB-202'), 
	(SELECT ReplacementID FROM Lesson_Replacment WHERE Reason='Technical issues' ));
	

INSERT INTO lesson (DATE,Name, Status, onlinclassname, LinktoClass, Descrip_Online,TeacherID, ThemeID, ReasonID, ClassroomID, ReplacementID)
VALUES ('2025-01-25', 'Color Theory Fundamentals', 'Face-to-Face', NULL, NULL, NULL,
	(SELECT TeacherID FROM Teacher WHERE LastName='Laura' AND FirstName='Josep' And MiddleName='Josee'),
	(SELECT ThemeID FROM CourseTheme WHERE Name='Typography'), 
	(SELECT ReasonID FROM Lesson_Reason WHERE Description='Introduction lecture'), 
	(SELECT ClassroomID FROM Classroom WHERE Number='DS-301'),
	NULL);
-----------------------------------Insertion into Status --------------------------------------
INSERT INTO Status (Name, Discription) VALUES
		('Active', 'Currently active'),
		('Completed', 'Successfully finished'),
		('Dropped', 'Left the course'),
		('On Hold', 'Temporarily paused'),
		('Excellent', 'Outstanding performance'),
		('Good', 'Satisfactory performance'),
		('Poor', 'Needs improvement');
SELECT*FROM country;
-----------------------------------Insertion into Student --------------------------------------
INSERT INTO Student (FirstName, SecondName, NikeName, Phone, Address, Email, AdmissionDate, Description, CountryID) VALUES
('Mike', 'Johnson', 'MikeJ', '+1-555-0201', '123 Student St, london', 'mike.johnson@student.edu', '2012-01-10', 'Computer Science major', 
	(SELECT CountryID FROM Country WHERE CodeNumber='89'));

INSERT INTO Student (FirstName, SecondName, NikeName, Phone, Address, Email, AdmissionDate, Description, CountryID) VALUES
('Hans', 'Schmidt', 'HansS', '+49-555-0203', '79 Student Str Berlin', 'hans.schmidt@student.edu', '2024-01-10', 'Business student', 
	(SELECT CountryID FROM Country WHERE CodeNumber='049'));

INSERT INTO Student (FirstName, SecondName, NikeName, Phone, Address, Email, AdmissionDate, Description, CountryID) VALUES
('Khan', 'Bacha', 'KhanBacha', '+49-555-0203', '39 main Str, Kabul', 'Khan.Bacha@gmail.com', '2020-10-23', 'Analytic', 
	(SELECT CountryID FROM Country WHERE CodeNumber='093')),
('Fatima', 'Ahmadi', 'FatimaA', '+93-555-0204', '321 Student Rd, Paris', 'fatima.ahmadi@student.edu', '2024-01-10', 'Programming enthusiast', 
	(SELECT CountryID FROM Country WHERE CodeNumber='033')),
('Rahman', 'Omari', 'Omerzai', '', '', 'Khan.Bacha@gmail.com', '2020-10-23', 'Analytic', 
	(SELECT CountryID FROM Country WHERE CodeNumber='093')),
('Gulanar', 'Ahmadzai', 'GulAzai', '', '321 Student Rd, Paris', 'fatima.ahmadi@student.edu', '2024-01-10', 'Programming enthusiast', 
	(SELECT CountryID FROM Country WHERE CodeNumber='033')),
('David', 'Brown', 'DavidB', '+1-555-0205', '654 Student Lane, Town', 'david.brown@student.edu', '2024-01-10', 'Data science student',
	(SELECT CountryID FROM Country WHERE CodeNumber='002')),
('Smith', 'John', 'Jack', '+1-885-0205', '6524 Student Lane1, Town', 'John.brown@student.edu', '2022-01-10', 'Data science student',
	(SELECT CountryID FROM Country WHERE CodeNumber='002')),
('Smith', 'Michael', 'Mike', '+1-642-0205', '6002 Main Lane1, Town', 'Mike.brown@student.edu', '2025-11-26', 'Data science student',
	(SELECT CountryID FROM Country WHERE CodeNumber='002'));

--SELECT*FROM Student;
-----------------------------------Insertion into Student_Course/StudentPerformance -------------------------------------
Insert into StudentPerformance(Marks, status_id, CourseID, StudentID) Values
(5,(SELECT status_id FROM Status WHERE Name='Excellent'),(SELECT CourseID FROM Course WHERE Name='Web Development'), 
	(SELECT StudentID FROM Student WHERE FirstName='Khan' AND SecondName='Bacha') );

Insert into StudentPerformance(Marks, status_id, CourseID, StudentID) Values
(5,(SELECT status_id FROM Status WHERE Name='Active'),(SELECT CourseID FROM Course WHERE Name='Python Programming'), 
	(SELECT StudentID FROM Student WHERE FirstName='David' AND SecondName='Brown')),
(5,(SELECT status_id FROM Status WHERE Name='Active'),(SELECT CourseID FROM Course WHERE Name='Web Development'), 
	(SELECT StudentID FROM Student WHERE FirstName='David' AND SecondName='Brown')),
(3,(SELECT status_id FROM Status WHERE Name='Poor'),(SELECT CourseID FROM Course WHERE Name='Data Analysis'), 
	(SELECT StudentID FROM Student WHERE FirstName='David' AND SecondName='Brown'));

Insert into StudentPerformance(Marks, status_id, CourseID, StudentID) Values
(3,(SELECT status_id FROM Status WHERE Name='Poor'),(SELECT CourseID FROM Course WHERE Name='Python Programming'), 
	(SELECT StudentID FROM Student WHERE FirstName='Hans' AND SecondName='Schmidt')),
(4,(SELECT status_id FROM Status WHERE Name='Good'),(SELECT CourseID FROM Course WHERE Name='Data Analysis'), 
	(SELECT StudentID FROM Student WHERE FirstName='Hans' AND SecondName='Schmidt')),
(4,(SELECT status_id FROM Status WHERE Name='Good'),(SELECT CourseID FROM Course WHERE Name='Web Development'), 
	(SELECT StudentID FROM Student WHERE FirstName='Hans' AND SecondName='Schmidt')),
(4,(SELECT status_id FROM Status WHERE Name='Good'),(SELECT CourseID FROM Course WHERE Name='Data Analysis'), 
	(SELECT StudentID FROM Student WHERE FirstName='Fatima' AND SecondName='Ahmadi') ),
(3,(SELECT status_id FROM Status WHERE Name='Poor'),(SELECT CourseID FROM Course WHERE Name='Python Programming'), 
	(SELECT StudentID FROM Student WHERE FirstName='Fatima' AND SecondName='Ahmadi') );
	
-- SELECT*FROM StudentPerformance;

-- SELECT* FROM Course C full join StudentPerformance SC ON C.CourseID=SC.CourseID
-- FULL JOIN Student S on SC.StudentID=S.StudentID;


-----------------------------------Insertion into Student_Plan ----------------------------------------------
INSERT INTO Student_Plan (Planned_Start, Planned_Finish, CourseID,StudentID) VALUES
	('2025-01-15', '2026-03-15', 
		(SELECT CourseID FROM Course WHERE Name='Data Analysis'), (SELECT StudentID FROM Student WHERE FirstName='Fatima')),
	('2024-02-01', '2024-05-01',
		(SELECT CourseID FROM Course WHERE Name='Python Programming'), (SELECT StudentID FROM Student WHERE FirstName='Mike')),
	('2025-01-20', '2027-04-20', 
		(SELECT CourseID FROM Course WHERE Name='Business Management'), (SELECT StudentID FROM Student WHERE FirstName='David')),
	('2025-03-01', '2025-06-01',
		(SELECT CourseID FROM Course WHERE Name='Web Development'), (SELECT StudentID FROM Student WHERE FirstName='Hans')),
	('2025-11-4', '2027-06-01',
		(SELECT CourseID FROM Course WHERE Name='Business Management'), (SELECT StudentID FROM Student WHERE FirstName='Rahman')),
	('2025-11-4', '2026-03-22',
		(SELECT CourseID FROM Course WHERE Name='Business Management'), (SELECT StudentID FROM Student WHERE FirstName='Gulanar'))
	;
		
--SELECT*FROM Student_Plan;

-----------------------------------Insertion into Student_Plan ----------------------------------------------
INSERT INTO Lesson_Attendance (status, Comments, Description, LessonID, StudentID) VALUES
	('Present', 'Active participant', 'Attended full session', 
		(SELECT LessonID From Lesson WHERE Name='Database Fundementals'), (SELECT StudentID FROM Student WHERE FirstName='Fatima')),
	('Present', 'Good engagement', 'Participated in exercises', 
		(SELECT LessonID From Lesson WHERE Name='HTML Basics'), (SELECT StudentID FROM Student WHERE FirstName='Mike')),
	('Absent', 'Sick leave', 'Medical absence',
		(SELECT LessonID From Lesson WHERE Name='Color Theory Fundamentals'), (SELECT StudentID FROM Student WHERE FirstName='Hans')),
	('Late', '15 minutes late', 'Traffic delay', 
		(SELECT LessonID From Lesson WHERE Name='Python Introduction'), (SELECT StudentID FROM Student WHERE FirstName='David'));

----------------------------------------------------------------------------------------------------------------------------------------------
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
	S.nikename, 
	Lr.reason AS "Why lesson Canceled"
FROM Lesson Le 
	RIGHT JOIN Lesson_Replacment Lr ON Le.ReplacementID = Lr.ReplacementID 
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
    EXTRACT(YEAR FROM L.date) = EXTRACT(YEAR FROM CURRENT_DATE) - 1
GROUP BY C.courseid, C.name
HAVING COUNT(DISTINCT T.teacherid) > 1
ORDER BY C.name;
----------------------------------------------------------------------------------------
SELECT*FROM COURSE;
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------

----------------------------------------------------------------------------------------

----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------

----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------


