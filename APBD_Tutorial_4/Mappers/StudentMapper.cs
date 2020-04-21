using System;
using System.Data.SqlClient;
using APBD_Tutorial_4.Model;

namespace APBD_Tutorial_4.Mapper
{
    public class StudentMapper
    {
        public static Student MapToStudent(SqlDataReader dataReader, string password)
        {
           return new Student
            {
                FirstName = dataReader["FirstName"].ToString(),
                LastName = dataReader["LastName"].ToString(),
                IndexNumber = dataReader["IndexNumber"].ToString(),
                BirthDate = dataReader["BirthDate"].ToString(),
                HashedPassword = password
                    
            };
        }
        
        public static Student MapToStudent(SqlDataReader dataReader)
        {
            return new Student
            {
                FirstName = dataReader["FirstName"].ToString(),
                LastName = dataReader["LastName"].ToString(),
                IndexNumber = dataReader["IndexNumber"].ToString(),
                BirthDate = dataReader["BirthDate"].ToString()
            };
        }
    }
}