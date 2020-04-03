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

        private const string FIND_STUDIES =
            "SELECT COUNT(*) as studies_count FROM  apbd_student.Studies where Name=@studiesName";

        private const string ENROLLMENT_EXISTS =
            "Select COUNT(*) as enrollment_count FROM  apbd_student.Enrollment e JOIN apbd_student.Studies s on e.idStudy=s.IdStudy WHERE e.Semester = 1" +
            " AND s.Name in (" + FIND_STUDIES + ")";

        private const string INDEX_EXISTS =
            "Select COUNT(*) as index_count from apbd_student.Student where IndexNumber=@index";

        private const string ENROLL_STUDENT = "INSERT INTO apbd_student.Student (IndexNumber, FirsName, LastName, BirthDate, IdEnrollment) values (@index, @firstName, @lastName, @birthDate, @idEnrollment)";

        private const string FIND_ENROLLMENT_ID =
            "Select e.IdEnrollment from apbd_student.Enrollment e JOIN apbd_student.Studies s on e.idStudy=s.IdStudy WHERE s.Name = @name";

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

        public Response EnrollNewStudent(Request request)
        {
            int enrollmentId = FindEnrollmentId(request);
            
            using (SqlConnection sqlConnection = new SqlConnection(ConnString))
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = ENROLL_STUDENT;
                
                sqlCommand.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                sqlCommand.Parameters.AddWithValue("FistName", request.FirstName);
                sqlCommand.Parameters.AddWithValue("LastName", request.LastName);
                sqlCommand.Parameters.AddWithValue("BirthDate", request.BirthDate);
                sqlCommand.Parameters.AddWithValue("IdEnrollment", enrollmentId);
                
                sqlConnection.Open();
                
                sqlCommand.ExecuteNonQuery();
                
                return new Response(1.ToString(), request.LastName);
            }
            
        }

        public bool EnrollmentExists(Request request)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnString))
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = ENROLLMENT_EXISTS;

                sqlCommand.Parameters.Add("@studiesName", System.Data.SqlDbType.NVarChar, 4);
                sqlCommand.Parameters["@studiesName"].Value = request.Studies;

                sqlConnection.Open();

                SqlDataReader dataReader = sqlCommand.ExecuteReader(); //expected feedback
                while (dataReader.Read())
                {
                    var exists = dataReader["enrollment_count"].ToString();
                    if (exists.Equals("1"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IndexExists(Request request)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnString))
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = INDEX_EXISTS;

                sqlCommand.Parameters.Add("@studiesName", System.Data.SqlDbType.NVarChar, 4);
                sqlCommand.Parameters["@studiesName"].Value = request.Studies;

                sqlConnection.Open();

                SqlDataReader dataReader = sqlCommand.ExecuteReader(); //expected feedback
                while (dataReader.Read())
                {
                    var exists = dataReader["enrollment_count"].ToString();
                    if (exists.Equals("1"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public int FindEnrollmentId(Request request)
        {
            int enrollmentId = 0;
            using (SqlConnection sqlConnection = new SqlConnection(ConnString))
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = FIND_ENROLLMENT_ID;

                sqlConnection.Open();

                SqlDataReader dataReader = sqlCommand.ExecuteReader(); //expected feedback
                while (dataReader.Read())
                {
                    enrollmentId = Convert.ToInt32(dataReader["IdEnrollment"].ToString());
                }
            }

            return enrollmentId;
        }


        public bool StudiesExist(Request request)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnString))
            using (SqlCommand sqlCommand = new SqlCommand())

            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = FIND_STUDIES;

                sqlCommand.Parameters.Add("@studiesName", System.Data.SqlDbType.NVarChar, 4);
                sqlCommand.Parameters["@studiesName"].Value = request.Studies;

                sqlConnection.Open();

                SqlDataReader dataReader = sqlCommand.ExecuteReader(); //expected feedback
                while (dataReader.Read())
                {
                    var exists = dataReader["index_count"].ToString();
                    if (exists.Equals("1"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}