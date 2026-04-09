DROP TABLE IF EXISTS Lesson_Attendance;
DROP TABLE IF EXISTS Student_Plan;
DROP TABLE IF EXISTS StudentPerformance;
DROP TABLE IF EXISTS Course_Dependenc;
DROP TABLE IF EXISTS Lesson_Replacment;
DROP TABLE IF EXISTS WorkStation;
DROP TABLE IF EXISTS Equipment;
DROP TABLE IF EXISTS Type_equipment;
DROP TABLE IF EXISTS Classroom;
DROP TABLE IF EXISTS Building;
DROP TABLE IF EXISTS Lesson_Reason;
DROP TABLE IF EXISTS CourseTheme;
DROP TABLE IF EXISTS Student;
DROP TABLE IF EXISTS Course;
DROP TABLE IF EXISTS Category;
DROP TABLE IF EXISTS Lesson;
DROP TABLE IF EXISTS Teacher_Direction;
DROP TABLE IF EXISTS Direction;
DROP TABLE IF EXISTS Teacher;
DROP TABLE IF EXISTS Country;
DROP TABLE IF EXISTS Status;

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
	CountryID SERIAL
);

--3
CREATE TABLE Direction (
    DirectionID SERIAL,
    Name VARCHAR(100)
);

--4
CREATE TABLE Teacher_Direction (
    DirectionID SERIAL,
    TeacherID SERIAL
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
	TeacherID SERIAL,
    ThemeID SERIAL,
    ReasonID SERIAL,
    ClassroomID SERIAL,
	ReplacementID SERIAL
);

--6
CREATE TABLE Course (
    CourseID SERIAL,
    Name VARCHAR(100),
    Description TEXT,
    StartingDate DATE,
    EndingDate DATE,
    CategoryID SERIAL,
    DependenceID SERIAL
);

--7
CREATE TABLE CourseTheme (
    ThemeID SERIAL,
    Name VARCHAR(100),
    Description TEXT,
    CourseID SERIAL
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
    CountryID SERIAL
);

--10
CREATE TABLE StudentPerformance (
    StudentCourse SERIAL,
    Marks INTEGER,
    status_id SERIAL,
    CourseID SERIAL,
    StudentID SERIAL
);

--11
CREATE TABLE Student_Plan (
    Stud_PlanID SERIAL,
    Planned_Start DATE,
    Planned_Finish DATE,
    CourseID SERIAL,
    StudentID SERIAL
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
    BuildingID SERIAL
);

--15
CREATE TABLE Equipment (
    EquipmentID SERIAL,
    ClassroomID SERIAL,
    Name VARCHAR(100),
    InventoryNumber VARCHAR(50),
    InstallationDate DATE,
    Description TEXT,
    typeID SERIAL
);

--16
CREATE TABLE WorkStation (
    WorkStationID SERIAL,
    ClassroomID SERIAL,
    Name VARCHAR(100),
    Number VARCHAR(50),
    Address VARCHAR(200),
    Description TEXT
);

--17
CREATE TABLE Course_Dependenc (
    DependencID SERIAL,
    Description TEXT,
    CourseID SERIAL
);

--18
CREATE TABLE Lesson_Attendance (
    AttendanceID SERIAL,
    status VARCHAR(50),
    Comments TEXT,
    Description TEXT,
	LessonID SERIAL,
    StudentID SERIAL
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
    ADD CONSTRAINT pk_course_dependenc PRIMARY KEY (DependencID);
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
    ADD CONSTRAINT fk_course_category FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID),
    ADD CONSTRAINT fk_course_dependence FOREIGN KEY (DependenceID) REFERENCES Course_Dependenc(DependencID);

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

-- Course_Dependenc 17
--Here is the question that how we are giveing the foreign key here, Now
--we have selected the foreigen key for the table as the 
--CourseID (this is located in the Course table, however we have the foreign key 
--of this table in the course table already). 
ALTER TABLE Course_Dependenc
    ADD CONSTRAINT fk_course_dependenc_course FOREIGN KEY (CourseID) REFERENCES Course(CourseID); 

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

