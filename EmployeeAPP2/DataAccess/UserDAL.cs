using EmployeeAPP2.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Helpers;
using System.Web.Script.Serialization;

public class UserDAL
{
    private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    public void Register(User user)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("AddUserJson", con);
            var jsonData = new
            {
                Username = user.Username,
                Email = user.Email,
                PasswordHash = user.PasswordHash
            };
            Console.WriteLine(jsonData);
            cmd.CommandType = CommandType.StoredProcedure;
            string jsonString = JsonConvert.SerializeObject(jsonData);
            cmd.Parameters.AddWithValue("@json", jsonString);

            con.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public User GetUserByUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) return null;

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand("GetUserByUsernameJ", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            // Create JSON input
            var jsonInput = new { Username = username };
            string jsonString = JsonConvert.SerializeObject(jsonInput);
            cmd.Parameters.AddWithValue("@json", jsonString);

            con.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new User
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Username = reader["Username"].ToString(),
                        Email = reader["Email"].ToString(),
                        PasswordHash = reader["PasswordHash"].ToString()
                    };
                }
            }
        }

        return null;
    }

    //public bool ChangePasswordByEmail(string email, string currentPassword, string newPassword)
    //{
    //    // Step 1: Get the user by email
    //    var user = GetUserByEmail(email);
    //    if (user == null)
    //        return false; // No user found

    //    // Step 2: Verify the current password using BCrypt
    //    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash);
    //    if (!isPasswordValid)
    //        return false; // Current password is incorrect

    //    // Step 3: Hash the new password
    //    string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

    //    //Step 4: Update the password using the stored procedure
    //    using (SqlConnection con = new SqlConnection(connectionString))
    //    {
    //        SqlCommand cmd = new SqlCommand("ChangePasswordByEmail", con);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.Parameters.AddWithValue("@Email", email);
    //        cmd.Parameters.AddWithValue("@NewPassword", hashedNewPassword);
    //        //cmd.Parameters.AddWithValue("@NewPassword", newPassword);


    //        con.Open();
    //        int result = cmd.ExecuteNonQuery();

    //        return true;
    //    }
    //}

    public bool ChangePasswordByEmail(string email, string currentPassword, string newPassword)
    {
        var user = GetUserByEmail(email);
        if (user == null)
            return false;

        if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
            return false;

        string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand("ChangePasswordByEmail", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            var jsonInput = new
            {
                Email = email,
                NewPassword = hashedNewPassword
            };
            string jsonString = JsonConvert.SerializeObject(jsonInput);
            cmd.Parameters.AddWithValue("@json", jsonString);

            // Output parameter
            SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(resultParam);

            con.Open();
            cmd.ExecuteNonQuery();

            int result = (int)resultParam.Value;
            return result == 1;
        }
    }



    public User GetUserByEmail(string email)
    {
        User user = null;

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("GetUserByEmail", con); // JSON-based SP name
            cmd.CommandType = CommandType.StoredProcedure;

            // Prepare JSON input
            var jsonInput = new { Email = email };
            string jsonString = JsonConvert.SerializeObject(jsonInput);
            cmd.Parameters.AddWithValue("@json", jsonString);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user = new User
                {
                    UserId = Convert.ToInt32(reader["UserId"]),
                    Username = reader["Username"].ToString(),
                    Email = reader["Email"].ToString(),
                    PasswordHash = reader["PasswordHash"].ToString()
                };
            }
        }

        return user;
    }



}



