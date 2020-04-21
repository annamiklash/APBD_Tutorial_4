create procedure apbd_student_pass.enroll_student @index_number VARCHAR(6),
                                                  @first_name VARCHAR(100),
                                                  @last_name VARCHAR(100),
                                                  @birth_date VARCHAR(20),
                                                  @studies VARCHAR(20),
                                                  @password varchar(500)

AS
DECLARE
    @enrollment_id INT
BEGIN
    set @enrollment_id = (Select e.IdEnrollment
                          from apbd_student_pass.Enrollment e
                                   JOIN apbd_student_pass.Studies s on e.idStudy = s.IdStudy
                          WHERE s.Name = @studies
                            and e.Semester = 1);

    INSERT INTO apbd_student_pass.Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment, Password)
    values (@index_number, @first_name, @last_name,
            (SELECT CONVERT(datetime, @birth_date, 104)), @enrollment_id, @password);
end
go

