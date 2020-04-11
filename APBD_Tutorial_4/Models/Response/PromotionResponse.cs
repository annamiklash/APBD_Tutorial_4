using System;
using System.Collections.Generic;

namespace APBD_Tutorial_4.Model
{
    public class PromotionResponse
    {
        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public DateTime StartDate { get; set; }
        public  Student Student { get; set; }

        public PromotionResponse(int idEnrollment, int semester, DateTime startDate, Student student)
        {
            IdEnrollment = idEnrollment;
            Semester = semester;
            StartDate = startDate;
            Student = student;
        }

        public PromotionResponse()
        {
            
        }

        public override string ToString()
        {
            return $"IdEnrollment : {IdEnrollment}, Semester {Semester}, StartDate {StartDate}, Student {Student}, ";
        }
    }
}