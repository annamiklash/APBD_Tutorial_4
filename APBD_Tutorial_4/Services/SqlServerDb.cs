using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using APBD_Tutorial_4.Mapper;
using APBD_Tutorial_4.Model;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Tutorial_4.Services
{
    public class SqlServerDb : IStudentsDb
    {
        private const string CONNECTION_DATA_STRING =
            "Data Source=db-mssql;Initial Catalog=s18458;Integrated Security=True";

        private const string FIND_ALL_STUDENTS_QUERY =
            "select IndexNumber, FirstName, LastName, BirthDate from apbd_student.Student";
            
        private const string FIND_SEMESTER_AND_LASTNAME_FOR_INDEX =
            "select e.Semester, s.LastName from apbd_student.Enrollment e JOIN " +
            "apbd_student.Student s on s.IdEnrollment=e.IdEnrollment where s.IndexNumber=@index";

        private const string FIND_SEMESTER_BY_INDEX_QUERY =
            "select e.Semester, e.StartDate from apbd_student.Student s " +
            "JOIN apbd_student.Enrollment e on e.IdEnrollment=s.IdEnrollment where s.IndexNumber=@index";

        private const string FIND_STUDIES =
            "SELECT Name FROM  apbd_student.Studies where Name=@studiesName";

        private const string ENROLLMENT_EXISTS_ON_FIRST_SEMESTER =
            "Select COUNT(*) as enrollment_count FROM  apbd_student.Enrollment e JOIN apbd_student.Studies s on e.idStudy=s.IdStudy WHERE e.Semester = 1" +
            " AND s.Name in (SELECT Name FROM  apbd_student.Studies where Name=@studiesName)";

        private const string ENROLLMENT_EXISTS_FOR_SEMESTER_AND_STUDIES =
            "select count(*) as enrollment_count from apbd_student.Studies s JOIN apbd_student.Enrollment e" +
            " ON s.IdStudy = e.IdStudy Where s.Name=@studiesName AND e.Semester=@semester";

        private const string INDEX_EXISTS =
            "Select COUNT(*) as index_count from apbd_student.Student where IndexNumber=@index";

        private const string ENROLL_STUDENT = 
            "INSERT INTO apbd_student.Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) values (@index, @firstName, @lastName, (SELECT CONVERT(datetime, @birthDate, 104)), @idEnrollment)";

        private const string FIND_ENROLLMENT_ID_FIRST_SEMESTER =
            "Select e.IdEnrollment from apbd_student.Enrollment e JOIN apbd_student.Studies s on e.idStudy=s.IdStudy WHERE s.Name = @name and e.Semester=1";

        private const string GET_PROMOTION_RESPONSE_FOR_ENROLLMENT_ID = 
            "select e.IdEnrollment, e.Semester, s.Name, e.StartDate, st.IndexNumber, st.FirstName, st.LastName, st.BirthDate " + 
            "from apbd_student.Enrollment e " +
            "JOIN apbd_student.Student st on e.IdEnrollment = st.IdEnrollment " + 
            "JOIN apbd_student.Studies s on e.IdStudy = s.IdStudy " + 
            "where e.IdEnrollment = @enrollmentId;";

        private const string GET_STUDENT_BY_INDEX =
            "Select IndexNumber, FirstName, LastName, BirthDate from apbd_student.Student where IndexNumber=@index";

        public Student GetStudentByIndex(string index)
        {
            bool exists = IndexExists(index);
            if (!exists)
            {
                return new Student();
            }
            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = GET_STUDENT_BY_INDEX;
                
                sqlCommand.Parameters.Add("@index", SqlDbType.NVarChar, 6);
                sqlCommand.Parameters["@index"].Value = index;

                sqlConnection.Open();

                SqlDataReader dataReader = sqlCommand.ExecuteReader(); //expected feedback
                while (dataReader.Read())
                {
                    Student student = StudentMapper.MapToStudent(dataReader);
                    dataReader.Close();
                    return student;
                }
            }
            return new Student();
        }
        
        private bool IndexExists(string index)
        {
            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = INDEX_EXISTS;
                
                sqlCommand.Parameters.Add("@index", System.Data.SqlDbType.NVarChar, 6);
                sqlCommand.Parameters["@index"].Value = index;

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
        

        public IEnumerable<Student> GetStudents()
        {
            List<Student> studentList = new List<Student>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = FIND_ALL_STUDENTS_QUERY;

                    sqlConnection.Open();

                    SqlDataReader dataReader = sqlCommand.ExecuteReader(); 
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
            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
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
                        dataReader.Close();
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

        public EnrollmentResponse EnrollNewStudent(EnrollmentRequest enrollmentRequest)
        {
         
            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = FIND_ENROLLMENT_ID_FIRST_SEMESTER;

                    sqlCommand.Parameters.Add("@name", System.Data.SqlDbType.NVarChar, 20);
                    sqlCommand.Parameters["@name"].Value = enrollmentRequest.Studies;
                    sqlCommand.Connection = sqlConnection;

                    sqlConnection.Open();
                    var transaction = sqlConnection.BeginTransaction();

                    sqlCommand.Transaction = transaction;

                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    int enrollmentId = 0;
                    if (dataReader.Read())
                    {
                        if (!dataReader.HasRows)
                        {
                            dataReader.Close();
                            transaction.Rollback();
                            throw new Exception();
                        }

                        enrollmentId = Convert.ToInt32(dataReader["IdEnrollment"].ToString());
                        
                    }
                    dataReader.Close();
                    sqlCommand.CommandText = ENROLL_STUDENT;

                    sqlCommand.Parameters.Add("@index", System.Data.SqlDbType.NVarChar, 6);
                    sqlCommand.Parameters["@index"].Value = enrollmentRequest.IndexNumber;

                    sqlCommand.Parameters.Add("@firstName", System.Data.SqlDbType.NVarChar, 20);
                    sqlCommand.Parameters["@firstName"].Value = enrollmentRequest.FirstName;

                    sqlCommand.Parameters.Add("@lastName", System.Data.SqlDbType.NVarChar, 40);
                    sqlCommand.Parameters["@lastName"].Value = enrollmentRequest.LastName;

                    sqlCommand.Parameters.Add("@birthDate", System.Data.SqlDbType.NVarChar, 10);
                    sqlCommand.Parameters["@birthDate"].Value = enrollmentRequest.BirthDate;

                    sqlCommand.Parameters.Add("@idEnrollment", System.Data.SqlDbType.NVarChar, 2);
                    sqlCommand.Parameters["@idEnrollment"].Value = enrollmentId;

                    sqlCommand.ExecuteNonQuery();
                    transaction.Commit();
                    
                    EnrollmentResponse response = GenerateEnrollmentResponse(enrollmentRequest.IndexNumber);
                    return response;

                }
            }
        }

        private EnrollmentResponse GenerateEnrollmentResponse(string enrollmentRequestIndexNumber)
        {
            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = FIND_SEMESTER_AND_LASTNAME_FOR_INDEX;

                    sqlCommand.Parameters.Add("@index", System.Data.SqlDbType.NVarChar, 20);
                    sqlCommand.Parameters["@index"].Value = enrollmentRequestIndexNumber;

                    sqlConnection.Open();

                    SqlDataReader dataReader = sqlCommand.ExecuteReader(); //expected feedback
                    while (dataReader.Read())
                    {
                        int semester = Convert.ToInt32(dataReader["Semester"].ToString());
                        string lastName = dataReader["LastName"].ToString();
                           
                        return new EnrollmentResponse(semester, lastName);
                      
                    }
                }
            }

            return null;
        }

        public bool EnrollmentExistsOnFirstSemester(EnrollmentRequest enrollmentRequest)
        {
            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
            {

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = ENROLLMENT_EXISTS_ON_FIRST_SEMESTER;

                    sqlCommand.Parameters.Add("@studiesName", System.Data.SqlDbType.NVarChar, 20);
                    sqlCommand.Parameters["@studiesName"].Value = enrollmentRequest.Studies;

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
            }

            return false;
        }

        public bool IndexExists(EnrollmentRequest enrollmentRequest)
        {
            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
            {

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = INDEX_EXISTS;

                    sqlCommand.Parameters.Add("@index", System.Data.SqlDbType.NVarChar, 6);
                    sqlCommand.Parameters["@index"].Value = enrollmentRequest.IndexNumber;

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
            }

            return false;
        }
        
        public bool EnrollmentExistsWithSemesterAndStudies(PromotionRequest promotionRequest)
        {
            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = ENROLLMENT_EXISTS_FOR_SEMESTER_AND_STUDIES;

                    sqlCommand.Parameters.Add("@semester", SqlDbType.Int, 2);
                    sqlCommand.Parameters["@semester"].Value = promotionRequest.Semester;

                    sqlCommand.Parameters.Add("@studiesName", SqlDbType.VarChar, 20);
                    sqlCommand.Parameters["@studiesName"].Value = promotionRequest.Studies;

                    sqlConnection.Open();

                    SqlDataReader dataReader = sqlCommand.ExecuteReader(); //expected feedback
                    while (dataReader.Read())
                    {
                        var value = dataReader["enrollment_count"].ToString();
                        if (value.Equals("1"))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public  List<PromotionResponse> PromoteStudents(PromotionRequest promotionRequest)
        {
            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
            {

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {

                    sqlCommand.CommandText = "apbd_student.promoteStudents";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("@semester", promotionRequest.Semester);
                    sqlCommand.Parameters.AddWithValue("@studiesName", promotionRequest.Studies);


                    IDbDataParameter outputParameter = sqlCommand.CreateParameter();
                    outputParameter.ParameterName = "@newIdEnrollment";
                    outputParameter.Direction = System.Data.ParameterDirection.Output;
                    outputParameter.DbType = System.Data.DbType.Int32;
                    outputParameter.Size = 2;
                    sqlCommand.Parameters.Add(outputParameter);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();

                    int enrollmentId = (int) sqlCommand.Parameters["@newIdEnrollment"].Value;
                   List<PromotionResponse> list = GeneratePromotionResponseList(enrollmentId);

                    return list;

                }
            }
        }

        private List<PromotionResponse> GeneratePromotionResponseList(int enrollmentId)
        {
            List<PromotionResponse> promotionResponses = new List<PromotionResponse>();
            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = GET_PROMOTION_RESPONSE_FOR_ENROLLMENT_ID;

                    sqlCommand.Parameters.Add("@enrollmentId", SqlDbType.Int, 2);
                    sqlCommand.Parameters["@enrollmentId"].Value = enrollmentId;

                    sqlConnection.Open();

                    SqlDataReader dataReader = sqlCommand.ExecuteReader(); //expected feedback

                    while (dataReader.Read())
                    {
                        PromotionResponse response = PromotionResponseMapper.MapToResponse(dataReader);
                        if (promotionResponses != null)
                        {
                            promotionResponses.Add(response);
                        } 
                    }
                }

                return promotionResponses;
            }
        }

        public string FindStudies(EnrollmentRequest enrollmentRequest)
        {
            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
            {
                using (SqlCommand sqlCommand = new SqlCommand())

                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = FIND_STUDIES;

                    sqlCommand.Parameters.Add("@studiesName", System.Data.SqlDbType.NVarChar, 20);
                    sqlCommand.Parameters["@studiesName"].Value = enrollmentRequest.Studies;

                    sqlConnection.Open();

                    SqlDataReader dataReader = sqlCommand.ExecuteReader(); //expected feedback
                    while (dataReader.Read())
                    {
                        var exists = dataReader["Name"].ToString();
                        return exists;
                    }
                }
            }

            return null;
        }

    }
}