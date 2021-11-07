using BF1.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BF1.Controllers
{
    public class StudentController : ApiController
    {

        private static string cs = @"Server=DESKTOP-SS05IUF\MYSQL;database=MyDB; Integrated Security = SSPI;";



        [HttpGet]
        [ActionName("GetAllStudents")]
        public IEnumerable<Student> GetAllStudents()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = "SELECT * FROM Students";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Connection = con;

                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Student> students = new List<Student>();
                    while (reader.Read())
                    {
                        Student s = new Student();

                        s.Id = Convert.ToInt32(reader.GetValue(0));
                        s.name = reader.GetValue(1).ToString();
                        s.Age = Convert.ToInt32(reader.GetValue(2));

                        students.Add(s);
                    }
                    return students;
                }
            }

        }




        [HttpGet]
        [ActionName("GetStudentById")]
        public Student GetStudentById(int Id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = "SELECT * FROM Students WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Id", Id);

                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    Student s = new Student();

                    while (reader.Read())
                    {
                        s.Id = Convert.ToInt32(reader.GetValue(0));
                        s.name = reader.GetValue(1).ToString();
                        s.Age = Convert.ToInt32(reader.GetValue(2));
                    }

                    return s;
                }
            }
        }




        [HttpPost]
        [ActionName("CreateStudent")]
        public void CreateStudent(Student s)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = "INSERT INTO Students (Name,Age) values (@Name,@Age)";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("@Name", s.name);
                    cmd.Parameters.AddWithValue("Age", s.Age);

                    con.Open();
                    cmd.ExecuteNonQuery();

                }
            }
        }



        [HttpPut]
        [ActionName("UpdateStudent")]
        public void UpdateStudent(int Id, Student ss)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = "UPDATE Students SET Name=@NAme , Age=@Age WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Connection = con;
                    cmd.Parameters.Add(new SqlParameter("@Id", Id));
                    cmd.Parameters.Add(new SqlParameter("@Name", ss.name));
                    cmd.Parameters.Add(new SqlParameter("@Age", ss.Age));

                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }



        [HttpDelete]
        [ActionName("DeleteStudent")]
        public IHttpActionResult DeleteStudent(int Id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = "DELETE FROM Students WHERE Id="+Id+"";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Id", Id);

                    con.Open();

                    cmd.ExecuteNonQuery();

                }
            }
            return Ok();
        }


    }
}