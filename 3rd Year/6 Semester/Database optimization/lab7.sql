CREATE TABLE MyCountry (
    CountryID SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    ShortName VARCHAR(30) NOT NULL,
    CodeNumber VARCHAR(20) NOT NULL UNIQUE,
    Description TEXT
);

CREATE TABLE MyTeacher (
    TeacherID SERIAL PRIMARY KEY,
    LastName VARCHAR(100) NOT NULL,
    FirstName VARCHAR(100) NOT NULL,
    MiddleName VARCHAR(100),
    BirthDate DATE NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    Address TEXT,
    Email VARCHAR(100) NOT NULL,  
    CountryID INT NOT NULL
);
ALTER TABLE MyTeacher
ADD CONSTRAINT fk_teacher_country
FOREIGN KEY (CountryID) REFERENCES MyCountry(CountryID);
--==============================================================
-- ========= copy of the table which is without index.
CREATE TABLE Teacher_no_index AS TABLE MyTeacher;
CREATE TABLE Country_no_index AS TABLE MyCountry;

-- Copy of the the table which will have the index. 
CREATE TABLE Teacher_with_index AS TABLE MyTeacher;
CREATE TABLE Country_with_index AS TABLE MyCountry;
--=============================================================
-- 1. Индекс по внешнему ключу (foreign key)
CREATE INDEX idx_teacher_countryid ON Teacher_with_index(CountryID);

-- 2. Индекс для поиска преподавателей по фамилии
CREATE INDEX idx_teacher_lastname ON Teacher_with_index(LastName);

-- 3. Составной индекс для поиска по фамилии и имени
CREATE INDEX idx_teacher_name ON Teacher_with_index(LastName, FirstName);

-- 4. Индекс для поиска стран по коду
CREATE INDEX idx_country_codenumber ON Country_with_index(CodeNumber);
--=============================================================
-- Giving the information to the country table.
INSERT INTO Country_with_index (Name, ShortName, CodeNumber, Description)
VALUES
('Россия', 'РФ', '643', 'Российская Федерация'),
('США', 'США', '840', 'Соединённые Штаты Америки'),
('Германия', 'ФРГ', '276', 'Федеративная Республика Германия');
-- Copy of the table.
INSERT INTO Country_no_index SELECT * FROM Country_with_index;

-- Generation of the data. 
INSERT INTO Teacher_with_index
    (LastName, FirstName, MiddleName, BirthDate, Phone, Address, Email, CountryID)
SELECT
    'Фамилия' || gs,
    'Имя' || gs,
    CASE WHEN random() > 0.3 THEN 'Отчество' || gs ELSE NULL END,
    CURRENT_DATE - (random() * 15000)::integer,
    '+7' || (random() * 999999999)::bigint,
    'Адрес ' || gs,
    'email' || gs || '@example.com',
    (random() * 2 + 1)::integer
FROM generate_series(1, 1000000) gs;
INSERT INTO Teacher_no_index SELECT * FROM Teacher_with_index;

--================================================
EXPLAIN ANALYZE
SELECT t.TeacherID, t.LastName, c.Name
FROM Teacher_no_index t
JOIN Country_no_index c ON t.CountryID = c.CountryID
WHERE t.LastName = 'Фамилия1234';

EXPLAIN ANALYZE
SELECT t.TeacherID, t.LastName, c.Name
FROM Teacher_with_index t
JOIN Country_with_index c ON t.CountryID = c.CountryID
WHERE t.LastName = 'Фамилия1234';

\timing on

-- Без индексов
SELECT COUNT(*) FROM Teacher_no_index WHERE LastName LIKE 'Фамилия1%';

-- С индексами
SELECT COUNT(*) FROM Teacher_with_index WHERE LastName LIKE 'Фамилия1%';

--==================== Insertion of the data, and how it affects on the plan. ================
EXPLAIN ANALYZE
INSERT INTO Teacher_no_index (LastName, FirstName, BirthDate, Phone, Email, CountryID)
VALUES ('Тест', 'Тест', CURRENT_DATE, '+71234567890', 'test@example.com', 1);

EXPLAIN ANALYZE
INSERT INTO Teacher_with_index (LastName, FirstName, BirthDate, Phone, Email, CountryID)
VALUES ('Тест', 'Тест', CURRENT_DATE, '+71234567890', 'test@example.com', 1);


EXPLAIN ANALYZE
UPDATE Teacher_no_index SET Phone = '+70000000000' WHERE TeacherID = 50000;

EXPLAIN ANALYZE
UPDATE Teacher_with_index SET Phone = '+70000000000' WHERE TeacherID = 50000;


