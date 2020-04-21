using System;
using System.Collections.Generic;

namespace APBD_Tutorial_4.Model
{
    public class PromotionResponse
    {
        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public DateTime StartDate { get; set; }
        public  string Index { get; set; }

        public PromotionResponse(int idEnrollment, int semester, DateTime startDate, string index)
        {
            IdEnrollment = idEnrollment;
            Semester = semester;
            StartDate = startDate;
            Index = index;
        }

        public PromotionResponse()
        {
            
        }

        public override string ToString()
        {
            return $"IdEnrollment : {IdEnrollment}, Semester {Semester}, StartDate {StartDate}, Index {Index}, ";
        }
    }
}