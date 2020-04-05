drop procedure apbd_student.promoteStudents;

go
create procedure apbd_student.promoteStudents @semester INT,
                                              @studiesName VARCHAR(20),
                                              @newIdEnrollment INT OUTPUT
AS
DECLARE
    @currentEnrollmentId INT,
    @targetEnrollmentId  INT

BEGIN

    set @targetEnrollmentId = (select e.IdEnrollment
                               from apbd_student.Enrollment e
                                        join apbd_student.Studies s on e.IdStudy = s.IdStudy
                               where e.Semester = (@semester + 1)
                                 and s.Name = @studiesName);

    set @currentEnrollmentId = (select e.IdEnrollment
                                from apbd_student.Enrollment e
                                         join apbd_student.Studies s on e.IdStudy = s.IdStudy
                                where e.Semester = @semester
                                  and s.Name = @studiesName);

    if @targetEnrollmentId is null
        begin
            set @targetEnrollmentId = (select max(IdEnrollment) + 1 from apbd_student.Enrollment);
            insert into apbd_student.Enrollment(IdEnrollment, Semester, IdStudy, StartDate)
            VALUES (@targetEnrollmentId,
                    @semester + 1,
                    (select IdStudy from apbd_student.Studies where Name = @studiesName),
                    GETDATE());
        end
    update apbd_student.Student
    set IdEnrollment = @targetEnrollmentId
    where IndexNumber in (select IndexNumber from Student where IdEnrollment = @currentEnrollmentId);

    set @newIdEnrollment = @targetEnrollmentId;
    return @newIdEnrollment
end;

go
