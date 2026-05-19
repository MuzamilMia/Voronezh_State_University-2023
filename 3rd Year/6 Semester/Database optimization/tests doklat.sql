
DROP TABLE IF EXISTS Type_equipment CASCADE;
DROP TABLE IF EXISTS Equipment CASCADE;



CREATE TABLE Equipment (
    EquipmentID SERIAL,
    ClassroomID INT,
    Name VARCHAR(100),
    InventoryNumber VARCHAR(50),
    InstallationDate DATE,
    Description TEXT,
    typeID INT
);


CREATE TABLE Type_equipment (
    typeID SERIAL,
    Name VARCHAR(100),
    registerNumber VARCHAR(50),
    ShortName VARCHAR(50)
);


ALTER TABLE Equipment
    ADD CONSTRAINT pk_equipment PRIMARY KEY (EquipmentID);
ALTER TABLE Type_equipment
    ADD CONSTRAINT pk_type_equipment PRIMARY KEY (typeID);

ALTER TABLE Equipment
    ALTER COLUMN Name SET NOT NULL,
    ALTER COLUMN InventoryNumber SET NOT NULL,
    ADD CONSTRAINT fk_equipment_type FOREIGN KEY (typeID) REFERENCES Type_equipment(typeID); 

ALTER TABLE Type_equipment
    ALTER COLUMN Name SET NOT NULL,
    ALTER COLUMN registerNumber SET NOT NULL;

--=============================

SELECT COUNT(*) as total_type_equipment FROM Type_equipment;

SELECT COUNT(*) as total_equipment FROM Equipment;
--==========================================================
CREATE INDEX idx_equipment_typeid_date ON Equipment (typeID, InstallationDate);
Drop INDEX idx_type_equipment_name; ON Type_equipment(Name);

DROP INDEX idx_equipment_typeid_date;
-- Оставить только простой индекс на typeID
CREATE INDEX idx_equipment_typeid ON Equipment(typeID);
ANALYZE Type_equipment;
ANALYZE Equipment;
--==============================================================
-- Check distribution
SELECT 
    t.ShortName,
    COUNT(*) as equipment_count
FROM Equipment e
JOIN Type_equipment t ON e.typeID = t.typeID
GROUP BY t.ShortName
ORDER BY equipment_count DESC
LIMIT 10;

DISCARD ALL;
--=============================

EXPLAIN ( ANALYZE, BUFFERS, VERBOSE)
SELECT DISTINCT TE.Name
FROM Type_equipment TE
WHERE NOT EXISTS (
    SELECT 1 FROM Equipment E
    WHERE E.typeID = TE.typeID AND E.InstallationDate >= CURRENT_DATE - INTERVAL '5 years')
ORDER BY TE.Name;
--------

EXPLAIN ( ANALYZE, BUFFERS, VERBOSE)
SELECT DISTINCT TE.Name
FROM Type_equipment TE
WHERE TE.typeID NOT IN (
	SELECT E.typeID FROM equipment E
	WHERE E.InstallationDate >= CURRENT_DATE - INTERVAL '5 years' )
ORDER BY TE.Name;
--------

EXPLAIN ( ANALYZE, BUFFERS, VERBOSE)
SELECT TE.Name FROM Type_equipment TE
EXCEPT 
SELECT TE2.Name FROM Type_equipment TE2 JOIN Equipment E ON E.typeID = TE2.typeID
WHERE E.InstallationDate >= CURRENT_DATE - INTERVAL '5 years'
ORDER BY Name;



