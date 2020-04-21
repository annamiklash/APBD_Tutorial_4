CREATE TABLE s18458.apbd_student.Enrollment
(
    IdEnrollment int  NOT NULL,
    Semester     int  NOT NULL,
    IdStudy      int  NOT NULL,
    StartDate    date NOT NULL,
    CONSTRAINT Enrollment_pk PRIMARY KEY (IdEnrollment)
);

-- Table: Student
CREATE TABLE s18458.apbd_student.Student
(
    IndexNumber  nvarchar(100) NOT NULL,
    FirstName    nvarchar(100) NOT NULL,
    LastName     nvarchar(100) NOT NULL,
    Password     nvarchar(100) NOT NULL,
    BirthDate    date          NOT NULL,
    IdEnrollment int           NOT NULL,
    CONSTRAINT Student_pk PRIMARY KEY (IndexNumber)
);

-- Table: Studies
CREATE TABLE s18458.apbd_student.Studies
(
    IdStudy int           NOT NULL,
    Name    nvarchar(100) NOT NULL,
    CONSTRAINT Studies_pk PRIMARY KEY (IdStudy)
);

-- foreign keys
-- Reference: Enrollment_Studies (table: Enrollment)
ALTER TABLE s18458.apbd_student.Enrollment
    ADD CONSTRAINT Enrollment_Studies
        FOREIGN KEY (IdStudy)
            REFERENCES s18458.apbd_student.Studies (IdStudy);

-- Reference: Student_Enrollment (table: Student)
ALTER TABLE s18458.apbd_student.Student
    ADD CONSTRAINT Student_Enrollment
        FOREIGN KEY (IdEnrollment)
            REFERENCES s18458.apbd_student.Enrollment (IdEnrollment);

-- End of file.
