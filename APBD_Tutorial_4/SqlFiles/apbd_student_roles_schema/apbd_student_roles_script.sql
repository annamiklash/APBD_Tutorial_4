CREATE TABLE apbd_student_roles.Enrollment
(
    IdEnrollment int  NOT NULL,
    Semester     int  NOT NULL,
    StartDate    date NOT NULL,
    IdStudy      int  NOT NULL,
    CONSTRAINT Enrollment_pk PRIMARY KEY (IdEnrollment)
);

-- Table: Role
CREATE TABLE apbd_student_roles.Role
(
    IdRole int          NOT NULL,
    Name   varchar(100) NOT NULL,
    CONSTRAINT Role_pk PRIMARY KEY (IdRole)
);

-- Table: Student
CREATE TABLE apbd_student_roles.Student
(
    IndexNumber  varchar(20)  NOT NULL,
    FirstName    varchar(100) NOT NULL,
    LastName     varchar(100) NOT NULL,
    Password     varchar(100) NOT NULL,
    BirthDate    date         NOT NULL,
    IdEnrollment int          NOT NULL,
    CONSTRAINT Student_pk PRIMARY KEY (IndexNumber)
);

-- Table: Student_Role
CREATE TABLE apbd_student_roles.Student_Role
(
    IndexNumber varchar(20) NOT NULL,
    IdRole      int NOT NULL
);

-- Table: Studies
CREATE TABLE apbd_student_roles.Studies
(
    IdStudy int          NOT NULL,
    Name    varchar(100) NOT NULL,
    CONSTRAINT Studies_pk PRIMARY KEY (IdStudy)
);

-- foreign keys
-- Reference: Enrollment_Studies (table: Enrollment)
ALTER TABLE apbd_student_roles.Enrollment
    ADD CONSTRAINT Enrollment_Studies
        FOREIGN KEY (IdStudy)
            REFERENCES apbd_student_roles.Studies (IdStudy);

-- Reference: Student_Enrollment (table: Student)
ALTER TABLE apbd_student_roles.Student
    ADD CONSTRAINT Student_Enrollment
        FOREIGN KEY (IdEnrollment)
            REFERENCES apbd_student_roles.Enrollment (IdEnrollment);

-- Reference: Student_Role_Role (table: Student_Role)
ALTER TABLE apbd_student_roles.Student_Role
    ADD CONSTRAINT Student_Role_Role
        FOREIGN KEY (IdRole)
            REFERENCES apbd_student_roles.Role (IdRole);

-- Reference: Student_Role_Student (table: Student_Role)
ALTER TABLE apbd_student_roles.Student_Role
    ADD CONSTRAINT Student_Role_Student
        FOREIGN KEY (IndexNumber)
            REFERENCES apbd_student_roles.Student (IndexNumber);