using System.Collections.Generic;
using System.Text.RegularExpressions;
using APBD_Tutorial_4.Model;
using Microsoft.AspNetCore.Http;

namespace APBD_Tutorial_4.Services
{
    public interface IStudentsDb
    {
        IEnumerable<Student> GetStudents();
        Enrollment GetSemesterByIndex(string indexNumber);
        Response EnrollNewStudent(Request request);
        bool StudiesExist(Request request);
        bool EnrollmentExists(Request request);

        bool IndexExists(Request request);

        int FindEnrollmentId(Request request);
    }
}