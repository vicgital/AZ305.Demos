using sqlapp.Models;
using Microsoft.Data.SqlClient;

namespace sqlapp.Services
{

    // This service will interact with our Product data in the SQL database
    public class CourseService
    {
        private SqlConnection GetConnection()
        {
            var _builder = new SqlConnectionStringBuilder
            {
                DataSource = Environment.GetEnvironmentVariable("SQL_SERVERNAME") ?? throw new Exception("SQL_SERVERNAME NOT FOUND IN ENVIRONMENT VARIABLE"),
                UserID = Environment.GetEnvironmentVariable("SQL_USERNAME") ?? throw new Exception("SQL_USERNAME NOT FOUND IN ENVIRONMENT VARIABLE"),
                Password = Environment.GetEnvironmentVariable("SQL_PASSWORD") ?? throw new Exception("SQL_PASSWORD NOT FOUND IN ENVIRONMENT VARIABLE"),
                InitialCatalog = Environment.GetEnvironmentVariable("SQL_DATABASE") ?? throw new Exception("SQL_DATABASE NOT FOUND IN ENVIRONMENT VARIABLE")
            };

            return new SqlConnection(_builder.ConnectionString);

        }
        public List<Course> GetCourses()
        {
            List<Course> _course_lst = [];
            string _statement = "SELECT CourseID,ExamImage,CourseName,rating from Course";
            SqlConnection _connection = GetConnection();
            
            _connection.Open();
            
            SqlCommand _sqlcommand = new(_statement, _connection);

            var storageAccountBaseUrl = Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_BASE_URL") ?? throw new Exception("STORAGE_ACCOUNT_BASE_URL NOT FOUND IN ENVIRONMENT VARIABLE");

            using (SqlDataReader _reader = _sqlcommand.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Course _course = new()
                    {
                        CourseID = _reader.GetInt32(0),
                        ExamImage = storageAccountBaseUrl + _reader.GetString(1),
                        CourseName = _reader.GetString(2),
                        Rating = _reader.GetDecimal(3)
                    };

                    _course_lst.Add(_course);
                }
            }
            _connection.Close();
            return _course_lst;
        }

    }
}

