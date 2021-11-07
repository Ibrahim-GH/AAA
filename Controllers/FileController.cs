using BF1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace BF1.Controllers
{
    public class FileController : ApiController
    {

        private static string cs = @"Server=DESKTOP-SS05IUF\MYSQL;database=MyDB; Integrated Security = SSPI;";

        [HttpGet]
        [ActionName("GetAllFiles")]
        public IEnumerable<Models.File> GetAllFiles(int Id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = "SELECT * FROM Files WHERE StudentId=@Id";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Id", Id);


                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Models.File> files = new List<Models.File>();
                    while (reader.Read())
                    {
                        Models.File file = new Models.File();

                        file.Id = Convert.ToInt32(reader.GetValue(0));
                        file.Name = reader.GetValue(1).ToString();
                        file.Path = reader.GetValue(2).ToString();
                        file.StudentId = Convert.ToInt32(reader.GetValue(3));

                        files.Add(file);
                    }
                    return files;
                }
            }

        }




        [HttpGet]
        [ActionName("GetFileById")]
        public Models.File GetFileById(int Id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = "SELECT * FROM Files WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Id", Id);

                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    Models.File s = new Models.File();

                    while (reader.Read())
                    {
                        s.Id = Convert.ToInt32(reader.GetValue(0));
                        s.Name = reader.GetValue(1).ToString();
                        s.Path = reader.GetValue(2).ToString();
                        s.StudentId = Convert.ToInt32(reader.GetValue(3));
                    }

                    return s;
                }
            }
        }




        [HttpGet]
        [ActionName("UploadFile")]
        public void UploadFile(int Id)
        {

            var file = HttpContext.Current.Request.Files[0];
            if (file != null && file.ContentLength > 0) { 
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = "INSERT INTO Files (Name,path,StudentId) values (@NAme,@Path,@StudentId) WHERE StudentrId=@Id";
                    using (SqlCommand cmd = new SqlCommand(q, con))
                    {
                        cmd.Connection = con;

                        string filename = Path.GetFileName(file.FileName);

                        string pathdb = "~/Upload" + filename;

                        string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload"),filename);
                        file.SaveAs(path);


                        cmd.Parameters.AddWithValue("@Name", file.FileName);
                        cmd.Parameters.AddWithValue("@Path", pathdb);
                        cmd.Parameters.AddWithValue("@StudentId", Id);

                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                }
            }

        }



        [HttpGet]
        [ActionName("DownloadFile")]
        public HttpResponseMessage DownloadFile(int Id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = "SELECT * FROM Files WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Connection = con;
                    con.Open();

                    cmd.Parameters.AddWithValue("@Id",Id);

                    SqlDataReader reader = cmd.ExecuteReader();


                    Models.File f = new Models.File();

                    while (reader.Read())
                    {
                        f.Id = Convert.ToInt32(reader.GetValue(0));
                        f.Name = reader.GetValue(1).ToString();
                        f.Path = reader.GetValue(2).ToString();
                        f.StudentId = Convert.ToInt32(reader.GetValue(3));
                    }
                  

                    string path = Path.Combine( HttpContext.Current.Server.MapPath("~/Upload") , f.Name);


                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

                    byte[] bytes = System.IO.File.ReadAllBytes(path);

                    response.Content = new ByteArrayContent(bytes);

                    response.Content.Headers.ContentLength = bytes.Length;

                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("Attachment");

                    response.Content.Headers.ContentDisposition.FileName = f.Name;

                    response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(f.Name));

                    return response;
                }
            }
        }


        [HttpGet]
        [ActionName("DeleteFile")]
        public void DeleteFile(int Id)
        {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string q = "SELECT Path FROM Files WHERE Id=@Id";
                    using (SqlCommand cmd = new SqlCommand(q, con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Id",Id);

                    con.Open();
                    string oldpath = (string)cmd.ExecuteScalar();
                    string oldfile = HttpContext.Current.Server.MapPath("~/Upload");
                    System.IO.File.Delete(oldfile);
                    con.Close();


                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM Files WHERE Id="+Id+"";

                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                }
            }

        



    }
}
