using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using APBD_Tutorial_4.Generator;
using APBD_Tutorial_4.Mapper;
using APBD_Tutorial_4.Model;

namespace APBD_Tutorial_4.Services
{
    public class SqlServerDb : IStudentsDb
    {
        private const string CONNECTION_DATA_STRING =
            "Data Source=db-mssql;Initial Catalog=s18458;Integrated Security=True";

        private const string FIND_ALL_STUDENTS_QUERY =
            "select IndexNumber, FirstName, LastName, BirthDate from apbd_student_pass.Student";
            
        private const string FIND_SEMESTER_AND_LASTNAME_FOR_INDEX =
            "select e.Semester, s.LastName from apbd_student_pass.Enrollment e JOIN " +
            "apbd_student_pass.Student s on s.IdEnrollment=e.IdEnrollment where s.IndexNumber=@index";

        private const string FIND_SEMESTER_BY_INDEX_QUERY =
            "select e.Semester, e.StartDate from apbd_student_pass.Student s " +
            "JOIN apbd_student_pass.Enrollment e on e.IdEnrollment=s.IdEnrollment where s.IndexNumber=@index";

        private const string FIND_STUDIES =
            "SELECT Name FROM  apbd_student_pass.Studies where Name=@studiesName";

        private const string ENROLLMENT_EXISTS_ON_FIRST_SEMESTER =
            "Select COUNT(*) as enrollment_count FROM  apbd_student_pass.Enrollment e"+
            " JOIN apbd_student_pass.Studies s on e.idStudy=s.IdStudy WHERE e.Semester = 1" +
            " AND s.Name in (SELECT Name FROM  apbd_student_pass.Studies where Name=@studiesName)";

        private const string ENROLLMENT_EXISTS_FOR_SEMESTER_AND_STUDIES =
            "select count(*) as enrollment_count from apbd_student.Studies s"+
            " JOIN apbd_student_pass.Enrollment e" +
            " ON s.IdStudy = e.IdStudy WHERE s.Name=@studiesName AND e.Semester=@semester";

        private const string INDEX_EXISTS =
            "SELECT COUNT(*) as index_count"+ 
            " FROM apbd_student_pass.Student WHERE IndexNumber=@index";

        private const string ENROLL_STUDENT = 
                "INSERT INTO apbd_student_pass.Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment, Password)"+
                " values (@index, @firstName, @lastName," +
                " (SELECT CONVERT(datetime, @birthDate, 104)), @idEnrollment, @encryptedPassword)";

        private const string FIND_ENROLLMENT_ID_FIRST_SEMESTER =
            "Select e.IdEnrollment from apbd_student_pass.Enrollment e"+
            " JOIN apbd_student_pass.Studies s on e.idStudy=s.IdStudy WHERE s.Name = @name and e.Semester=1";

        private const string GET_PROMOTION_RESPONSE_FOR_ENROLLMENT_ID = 
            "select e.IdEnrollment, e.Semester, s.Name, e.StartDate, st.IndexNumber, st.FirstName, st.LastName, st.BirthDate " + 
            "from apbd_student_pass.Enrollment e " +
            "JOIN apbd_student_pass.Student st on e.IdEnrollment = st.IdEnrollment " + 
            "JOIN apbd_student_pass.Studies s on e.IdStudy = s.IdStudy " + 
            "where e.IdEnrollment = @enrollmentId;";

        private const string GET_STUDENT_BY_INDEX =
            "Select IndexNumber, FirstName, LastName, BirthDate" +
            " from apbd_student_pass.Student where IndexNumber=@index";
        
        private const string CREDENTIALS_EXIST = 
            "Select COUNT(*) as valid_count from apbd_student_pass.Student"+
            " where IndexNumber=@username AND Password=@password";

        private const string GET_ROLES =
            "SELECT r.Name as role_name" +
            " FROM apbd_student_pass.Student s" +
            " JOIN apbd_student_pass.Student_Role sr ON s.IndexNumber = sr.IndexNumber" +
            " JOIN apbd_student_pass.Role r on sr.IdRole = r.IdRole" +
            " WHERE s.IndexNumber = @username" +
            " AND s.Password = @password";

        private const string GET_PASS =
            "SELECT Password as password from apbd_student_pass.Student" +
            " WHERE IndexNumber=@index";

        private const string GET_SALT =
            "SELECT p.Salt from apbd_student_pass.Password p JOIN apbd_student_pass.Student s " +
            "on s.Password = p.Password WHERE s.IndexNumber=@index";

        private const string SAVE_PASSWORD =
            "INSERT INTO apbd_student_pass.Password (Password, Salt)" +
            " VALUES (@password, @salt)";


        public string SavePassword(string password)
        {
            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                var hashSalt = HashSaltGenerator.GenerateSaltedHash(password);
                var salt = hashSalt.Salt;
                var hashedPassword = hashSalt.Hash;

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = SAVE_PASSWORD;

                sqlCommand.Parameters.Add("@password", System.Data.SqlDbType.NVarChar, 500);
                sqlCommand.Parameters["@password"].Value = hashedPassword;

                sqlCommand.Parameters.Add("@salt", System.Data.SqlDbType.NVarChar, 200);
                sqlCommand.Parameters["@salt"].Value = salt;
                
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();

                return hashedPassword;
            }
        }
        

        public string GetHashedPassword(string index)
        {
            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
            using (SqlCommand sqlCommand = new SqlCommand())
            {

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = GET_PASS;

                sqlCommand.Parameters.Add("@index", System.Data.SqlDbType.NVarChar, 100);
                sqlCommand.Parameters["@index"].Value = index;

                sqlConnection.Open();
                
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.Read())
                {
                    var password = dataReader["password"].ToString();
                    return password;
                }
            }

            return null;
        }
        
        public IEnumerable<string> GetStudentRole(string username, string password)
        {
            List<string> roles = new List<string>();
            var hashedPassword = GetHashedPassword(username);
            var salt = GetSalt(username);

            var isPasswordValid = IsPasswordValid(password, hashedPassword, salt);
            
            if (!isPasswordValid)
            {
                Console.WriteLine("Passwords not same");
                return roles;
            }

            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = GET_ROLES;
                
                sqlCommand.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 6);
                sqlCommand.Parameters["@username"].Value = username;
                
                sqlCommand.Parameters.Add("@password", System.Data.SqlDbType.NVarChar, 500);
                sqlCommand.Parameters["@password"].Value = hashedPassword;

                sqlConnection.Open();
               
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    var role = dataReader["role_name"].ToString();
                    roles.Add(role);

                }
            }
            return roles;
        }

        private bool IsPasswordValid(string enteredPassword, string hashedPassword, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000);
            var base64String = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            return base64String.Equals(hashedPassword);
        }

        private string GetSalt(string index)
        {
            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
            using (SqlCommand sqlCommand = new SqlCommand())
            {

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = GET_SALT;

                sqlCommand.Parameters.Add("@index", System.Data.SqlDbType.NVarChar, 100);
                sqlCommand.Parameters["@index"].Value = index;

                sqlConnection.Open();
                
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.Read())
                {
                    var salt = dataReader["Salt"].ToString();
                    return salt;
                }
            }

            return null;
        }

        public bool CredentialsValid(string username, string password)
        {
            var encodedPassword = GetHashedPassword(username);
            if (!encodedPassword.Equals(password))
            {
                return false;
            }
            
            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = CREDENTIALS_EXIST;
                
                sqlCommand.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 6);
                sqlCommand.Parameters["@username"].Value = username;
                
                sqlCommand.Parameters.Add("@password", System.Data.SqlDbType.NVarChar, 500);
                sqlCommand.Parameters["@password"].Value = encodedPassword;

                sqlConnection.Open();

                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    var exists = dataReader["valid_count"].ToString();
                    if (exists.Equals("1"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

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

                SqlDataReader dataReader = sqlCommand.ExecuteReader(); 
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
                    sqlCommand.Parameters.Add("@index", System.Data.SqlDbType.NVarChar, 6);
                    sqlCommand.Parameters["@index"].Value = indexNumber;
                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (!dataReader.Read()) return new Enrollment();
                    var semester = EnrollmentMapper.MapToSemester(dataReader);
                    dataReader.Close();
                    return semester;
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
            var savePassword = SavePassword(enrollmentRequest.Password);
            using (SqlConnection sqlConnection = new SqlConnection(CONNECTION_DATA_STRING))
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "apbd_student_pass.enroll_student";
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("@index_number", enrollmentRequest.IndexNumber);
                    sqlCommand.Parameters.AddWithValue("@first_name", enrollmentRequest.FirstName);
                    sqlCommand.Parameters.AddWithValue("@last_name", enrollmentRequest.LastName);
                    sqlCommand.Parameters.AddWithValue("@birth_date", enrollmentRequest.BirthDate);
                    sqlCommand.Parameters.AddWithValue("@studies", enrollmentRequest.Studies);
                    sqlCommand.Parameters.AddWithValue("@password", savePassword);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();

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

                    SqlDataReader dataReader = sqlCommand.ExecuteReader(); 
                    if (dataReader.Read())
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

        public IEnumerable<PromotionResponse> PromoteStudents(PromotionRequest promotionRequest)
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
                    IEnumerable<PromotionResponse> list = GeneratePromotionResponseList(enrollmentId);

                    return list;

                }
            }
        }

        private IEnumerable<PromotionResponse> GeneratePromotionResponseList(int enrollmentId)
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