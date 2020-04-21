insert into apbd_student_pass.Studies (IdStudy, Name)
values (1, 'Engineering'),
       (2, 'New Media'),
       (3, 'Culture Of Japan'),
       (4, 'Journalism'),
       (5, 'Sociology'),
       (6, 'Economics'),
       (7, 'Business'),
       (8, 'Big Data'),
       (9, 'Accounting'),
       (10, 'Management');

insert into apbd_student_pass.Enrollment (IdEnrollment, Semester, IdStudy, StartDate)
values (1, 1, 1, '2019-10-01'),
       (2, 1, 2, '2019-10-01'),
       (3, 1, 3, '2019-10-01'),
       (4, 1, 4, '2019-10-01'),
       (5, 1, 5, '2019-10-01'),
       (6, 1, 6, '2019-10-01'),
       (7, 1, 7, '2019-10-01'),
       (8, 1, 8, '2019-10-01'),
       (9, 1, 9, '2019-10-01'),
       (10, 1, 10, '2019-10-01');

Insert Into apbd_student_pass.Role (IdRole, Name)
values (1, 'student'),
       (2, 'admin');

------ENROLL STUDENTS FROM STUDENTS.JSON--------------


Insert Into apbd_student_pass.Student_Role(IndexNumber, IdRole)
values ('s1111', 1),
       ('s1111', 2),
       ('s2355', 1),
       ('s35253', 1),
       ('s3452', 1),
       ('s12345', 1),
       ('s86533', 1),
       ('s7443', 1),
       ('s18955', 1),
       ('s8745', 1),
       ('s67834', 1),
       ('s74900', 1),
       ('s74900', 2),
       ('s28659', 1),
       ('s9345', 1),
       ('s54321', 1),
       ('s2674', 1),
       ('s7463', 1),
       ('s7463', 2),
       ('s12478', 1),
       ('s5699', 1),
       ('s34689', 1),
       ('s73468', 1),
       ('s95220', 1),
       ('s95220', 2)