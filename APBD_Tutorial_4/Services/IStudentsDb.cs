using System;
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
        EnrollmentResponse EnrollNewStudent(EnrollmentRequest enrollmentRequest);
        string FindStudies(EnrollmentRequest enrollmentRequest);
        bool EnrollmentExistsOnFirstSemester(EnrollmentRequest enrollmentRequest);
        bool IndexExists(EnrollmentRequest enrollmentRequest);
        bool EnrollmentExistsWithSemesterAndStudies(PromotionRequest promotionRequest);
        IEnumerable<PromotionResponse> PromoteStudents(PromotionRequest promotionRequest);
        Student GetStudentByIndex(string index);

        bool CredentialsValid(string username, string password);

        IEnumerable<string> GetStudentRole(string username, string password);
        string GetPassword(string requestIndex);
    }
}