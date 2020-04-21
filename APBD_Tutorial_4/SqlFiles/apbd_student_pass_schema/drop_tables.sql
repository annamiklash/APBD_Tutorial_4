-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2020-04-20 19:21:47.064

-- foreign keys
ALTER TABLE apbd_student_pass.Enrollment DROP CONSTRAINT Enrollment_Studies;

ALTER TABLE apbd_student_pass.Student DROP CONSTRAINT Student_Enrollment;

ALTER TABLE apbd_student_pass.Student DROP CONSTRAINT Student_Password;

ALTER TABLE apbd_student_pass.Student_Role DROP CONSTRAINT Student_Role_Role;

ALTER TABLE apbd_student_pass.Student_Role DROP CONSTRAINT Student_Role_Student;

-- tables
DROP TABLE apbd_student_pass.Enrollment;

DROP TABLE apbd_student_pass.Password;

DROP TABLE apbd_student_pass.Role;

DROP TABLE apbd_student_pass.Student;

DROP TABLE apbd_student_pass.Student_Role;

DROP TABLE apbd_student_pass.Studies;

-- End of file.

