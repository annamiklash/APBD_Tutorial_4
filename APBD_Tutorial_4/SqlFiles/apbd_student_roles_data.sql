
insert into apbd_student_roles.Studies (IdStudy, Name)
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

insert into apbd_student_roles.Enrollment (IdEnrollment, Semester, IdStudy, StartDate)
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

Insert Into apbd_student_roles.Role (IdRole, Name) values
(1, 'student'),
(2, 'admin');

insert into apbd_student_roles.Student (IndexNumber, FirstName, LastName, Password, BirthDate, IdEnrollment)
values ('s1111', 'Armaan', 'Pham', 'aaa', '1981-01-05', 1),
       ('s2355', 'Esha', 'Valencia', 'fQq9VtUtXGBHYBee', '1993-09-15', 2),
       ('s35253', 'Lana', 'Abbott', 'Q9HH5fWc2THyxjvK', '1989-10-18', 3),
       ('s3452', 'Eugene', 'Golden', '5Smv8egSWDmanvHG', '1996-06-18', 4),
       ('s12345', 'Hisham', 'Wagner', '123344', '1990-09-25', 5),
       ('s86533', 'Elena', 'Britton', 'hello', '1994-09-01', 6),
       ('s7443', 'Bobbie', 'Clements', '654322', '1993-09-15', 7),
       ('s18955', 'Herman', 'Ellison', 'random', '1995-08-04', 8),
       ('s8745', 'Nida', 'Simpson', 'password', '1988-06-14', 9),
       ('s67834', 'Erin', 'ONeill', 'outside', '1995-08-04', 10),
       ('s74900', 'Luka', 'Wilkinson', 'hook', '1993-02-09', 1),
       ('s28659', 'Rhydian', 'Browning', 'privet', '1989-05-18', 2),
       ('s9345', 'Dylan', 'Yoder', 'PcXBcLPU9E', '1995-10-23', 3),
       ('s54321', 'Jimmy', 'Frey', 'agXHxH63Sf', '1992-09-03', 4),
       ('s2674', 'Saxon ', 'Beattie', 'AxMzZMuL8Y', '1991-06-21', 5),
       ('s7463', 'Fredrick', 'Kumar', 'zt268AxysF', '1995-12-18', 6),
       ('s12478', 'Brogan', 'Power', 'encode', '1986-12-10', 7),
       ('s5699', 'Jace', 'Rees', 'sharp', '1983-03-25', 8),
       ('s34689', 'Gia', 'Forrest', 'good morning', '1985-07-02', 9),
       ('s73468', 'Flynn', 'Harding', 'bye', '1996-04-24', 10);


Insert Into apbd_student_roles.Student_Role(IndexNumber, IdRole)
values ('s1111', 1),
       ('s1111', 2),
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
       ('s73468', 1)