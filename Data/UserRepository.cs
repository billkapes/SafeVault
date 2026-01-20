using MySql.Data.MySqlClient;
using SafeVault.Models;
using Microsoft.Extensions.Options;

namespace SafeVault.Data
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IOptions<DatabaseConfig> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        public void InsertUserWithPassword(User user)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            const string sql = @"
                INSERT INTO Users (Username, Email, PasswordHash, PasswordSalt, Role)
                VALUES (@username, @email, @hash, @salt, @role);
            ";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@hash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@salt", user.PasswordSalt);
            cmd.Parameters.AddWithValue("@role", user.Role);
            cmd.ExecuteNonQuery();
        }

        public User? GetUserByUsername(string username)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            const string sql = @"
                SELECT UserID, Username, Email, PasswordHash, PasswordSalt, Role
                FROM Users
                WHERE Username = @username
                LIMIT 1;
            ";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", username);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    UserID = reader.GetInt32("UserID"),
                    Username = reader.GetString("Username"),
                    Email = reader.GetString("Email"),
                    PasswordHash = (byte[])reader["PasswordHash"],
                    PasswordSalt = (byte[])reader["PasswordSalt"],
                    Role = reader.GetString("Role"),
                };
            }

            return null;
        }
    }
}
