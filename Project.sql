

IF db_id('Project') IS NULL CREATE DATABASE Project;
GO

-- if object_id('object_name', 'U') is null -- for table

USE Project;

CREATE TABLE Programs( ProgId VARCHAR(5)  NOT NULL, 
 ProgName VARCHAR(50) NOT NULL,
 PRIMARY KEY (ProgId));

CREATE TABLE Courses(CId VARCHAR(7) NOT NULL,
CName VARCHAR(50) NOT NULL,
ProgId VARCHAR(5) NOT NULL,
PRIMARY KEY (CId));

CREATE TABLE Students(StId VARCHAR(10) NOT NULL,
StName VARCHAR(50) NOT NULL,
ProgId VARCHAR(5) NOT NULL,
PRIMARY KEY (StId));


CREATE TABLE Enrollments(StId VARCHAR(10) NOT NULL, 
 CId VARCHAR(7) NOT NULL,
 FinalGrade INT,
 PRIMARY KEY (StId, CId),
 FOREIGN KEY (StId) REFERENCES 
 Students(StId)
 ON DELETE CASCADE
 ON UPDATE CASCADE,
 FOREIGN KEY (CId) REFERENCES
 Courses(CId)
 ON DELETE NO ACTION
 ON UPDATE NO ACTION);
 
ALTER TABLE Courses
ADD CONSTRAINT FK_Courses_ProgId FOREIGN KEY (ProgId) 
REFERENCES Programs(ProgId)
ON DELETE CASCADE
ON UPDATE CASCADE;

ALTER TABLE Students
ADD CONSTRAINT FK_Student_ProgId FOREIGN KEY (ProgId) 
REFERENCES Programs(ProgId)
ON DELETE NO ACTION
ON UPDATE CASCADE;
 
GO

-- finished the creation of tables--

-- populate the tables with some information--

INSERT INTO Programs (ProgId, ProgName) 
VALUES ( 'P0001', 'IT'),
('P0002', 'Finances');

INSERT INTO Courses (CId, CName, ProgId)
VALUES ('C000100','C#', 'P0001'),
('C000200', 'Phyton', 'P0001'),
('C000300', 'Economy', 'P0002'),
('C000400', 'Taxes', 'P0002');

INSERT INTO Students (StId, StName, ProgId)
VALUES ('S000010000','Student1','P0001'),
('S000020000','Student2','P0001'),
('S000030000','Student3','P0002'),
('S000040000','Student4','P0002');

INSERT INTO Enrollments (StId, CId, FinalGrade)
VALUES ('S000010000','C000200',80),
('S000020000','C000100',72),
('S000030000','C000400',85),
('S000040000','C000300',100);


GO









