using System;
using System.Data.SqlClient;
using APBD_Tutorial_4.Model;

namespace APBD_Tutorial_4.Mapper
{
    public class EnrollmentMapper
    {
        public static Enrollment MapToSemester(SqlDataReader dataReader)
        {
            return new Enrollment()
            {
                Semester = (int) dataReader["Semester"],
                StartDate = Convert.ToDateTime(dataReader["StartDate"].ToString())
            };
        }
    }
}