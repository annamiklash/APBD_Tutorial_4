using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using APBD_Tutorial_4.Model;

namespace APBD_Tutorial_4.Mapper
{
    public class PromotionResponseMapper
    {
        public static PromotionResponse MapToResponse(SqlDataReader dataReader)
        {
            return new PromotionResponse
            {
                IdEnrollment = Convert.ToInt32(dataReader["IdEnrollment"].ToString()),
                Semester = Convert.ToInt32(dataReader["Semester"].ToString()),
                StartDate = Convert.ToDateTime(dataReader["StartDate"].ToString()), 
                Student = StudentMapper.MapToStudent(dataReader)
            };
        }
    }
}