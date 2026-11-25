using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Project_11.Models;

namespace Project_11.DAL
{
    public class ServiceRequestDAL
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["CampusConnection"].ConnectionString;

        public List<ServiceRequest> GetAllRequests(string category = null)
        {
            var list = new List<ServiceRequest>();
            using (var con = new SqlConnection(_connectionString))
            {
                string query = string.IsNullOrEmpty(category)
                    ? "SELECT * FROM ServiceRequests ORDER BY CreatedDate DESC"
                    : "SELECT * FROM ServiceRequests WHERE Category = @Category ORDER BY CreatedDate DESC";

                using (var cmd = new SqlCommand(query, con))
                {
                    if (!string.IsNullOrEmpty(category))
                        cmd.Parameters.AddWithValue("@Category", category);

                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ServiceRequest
                            {
                                RequestId = Convert.ToInt32(reader["RequestId"]),
                                StudentEnrollment = reader["StudentEnrollment"].ToString(),
                                StudentName = reader["StudentName"].ToString(),
                                Category = reader["Category"].ToString(),
                                Description = reader["Description"].ToString(),
                                Status = reader["Status"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        public ServiceRequest GetRequestById(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT * FROM ServiceRequests WHERE RequestId = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ServiceRequest
                        {
                            RequestId = Convert.ToInt32(reader["RequestId"]),
                            StudentEnrollment = reader["StudentEnrollment"].ToString(),
                            StudentName = reader["StudentName"].ToString(),
                            Category = reader["Category"].ToString(),
                            Description = reader["Description"].ToString(),
                            Status = reader["Status"].ToString(),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                        };
                    }
                }
            }
            return null;
        }

        public void AddRequest(ServiceRequest req)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO ServiceRequests (StudentEnrollment, StudentName, Category, Description, Status, CreatedDate)
                                 VALUES (@Enrollment, @Name, @Category, @Description, @Status, @CreatedDate)";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Enrollment", req.StudentEnrollment);
                    cmd.Parameters.AddWithValue("@Name", req.StudentName);
                    cmd.Parameters.AddWithValue("@Category", req.Category);
                    cmd.Parameters.AddWithValue("@Description", req.Description);
                    cmd.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(req.Status) ? "Pending" : req.Status);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool UpdateStatus(int id, string status)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("UPDATE ServiceRequests SET Status = @Status WHERE RequestId = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteRequest(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("DELETE FROM ServiceRequests WHERE RequestId = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}