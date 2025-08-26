using Azure;
using Azure.Data.Tables;

namespace cloud_poe_sem_2_part_1.Models
{
    public class UserEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = "Users";
        public string RowKey { get; set; } // will be the username or email
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        // Azure fields
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }

    // For login/register forms
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
