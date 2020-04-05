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
        void EnrollNewStudent(EnrollmentRequest enrollmentRequest);
        string FindStudies(EnrollmentRequest enrollmentRequest);
        bool EnrollmentExistsOnFirstSemester(EnrollmentRequest enrollmentRequest);
        bool IndexExists(EnrollmentRequest enrollmentRequest);
       // int FindEnrollmentId(EnrollmentRequest enrollmentRequest);
        bool EnrollmentExistsWithSemesterAndStudies(PromotionRequest promotionRequest);
        List<PromotionResponse> PromoteStudents(PromotionRequest promotionRequest);
        
        
    }
}