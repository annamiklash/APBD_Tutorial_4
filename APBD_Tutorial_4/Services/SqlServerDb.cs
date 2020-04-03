using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using APBD_Tutorial_4.Mapper;
using APBD_Tutorial_4.Model;

namespace APBD_Tutorial_4.Services
{
    public class SqlServerDb : IStudentsDb
    {
        private string ConnString =
            "Data Source=db-mssql;Initial Catalog=s18458;Integrated Security=True";

        private const string FIND_ALL_STUDENTS_QUERY =
            "select IndexNumber, FirstName, LastName, BirthDate from apbd_student.Student";

        private const string FIND_SEMESTER_BY_INDEX_QUERY =
            "select e.Semester, e.StartDate from apbd_student.Student s " +
            "JOIN apbd_student.Enrollment e on e.IdEnrollment=s.IdEnrollment where s.IndexNumber=@index";

        public IEnumerable<Student> GetStudents()
        {
            List<Student> studentList = new List<Student>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnString))
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = FIND_ALL_STUDENTS_QUERY;

                    sqlConnection.Open();

                    SqlDataReader dataReader = sqlCommand.ExecuteReader(); //expected feedback
                    while (dataReader.Read())
                    {
                        Student student = StudentMapper.MapToStudent(dataReader);
                        studentList.Add(student);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            return studentList;
        }

        public Enrollment GetSemesterByIndex(string indexNumber)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnString))
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = FIND_SEMESTER_BY_INDEX_QUERY;
                
                try
                {
                    if (Regex.IsMatch(indexNumber, "^s[0-9]+$") && indexNumber.Length <= 6)
                    {
                        sqlCommand.Parameters.Add("@index", System.Data.SqlDbType.NVarChar, 6);
                        sqlCommand.Parameters["@index"].Value = indexNumber;
                        
                        sqlConnection.Open();
                        SqlDataReader dataReader = sqlCommand.ExecuteReader();

                        if (!dataReader.Read()) return new Enrollment();
                        var semester = EnrollmentMapper.MapToSemester(dataReader);
                        return semester;
                    }
                }
                catch (SqlException exception)
                {
                    Console.WriteLine("invalid input" + exception.Message);
                }
                return null;
            }
        }
    }
}