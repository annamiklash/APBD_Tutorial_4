using System.Collections.Generic;
using APBD_Tutorial_4.Model;

namespace APBD_Tutorial_4.Services
{
    public interface IStudentsDb
    {
        IEnumerable<Student> GetStudents();
        Enrollment GetSemesterByIndex(string indexNumber);
    }
}