using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using beltReviewer.Models;
using Microsoft.Extensions.Options;

namespace beltReviewer.Factory
{
    public class UserFactory : IFactory<User>
    {
        // private string connectionString;

        // public UserFactory()
        // {
        //     connectionString = "server=localhost;UserId=root;password=root;port=8889;database=login;SslMode=None";
        // }

         private readonly IOptions<MySqlOptions> mysqlConfig;

        public UserFactory(IOptions<MySqlOptions> conf) {
            mysqlConfig = conf;
        }

        internal IDbConnection Connection
        {
            get {
                return new MySqlConnection(mysqlConfig.Value.ConnectionString);
            }
        }
         public void Add(User item){
            using (IDbConnection dbConnection = Connection) {
                string query = "INSERT INTO users (first_name, last_name, email, password, created_at, updated_at) VALUES (@first_name, @last_name, @email, @password, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, item);
                
            }
        }
        public User FindbyEmail(string email){
            using(IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>("SELECT * FROM users WHERE email = @Email", new { Email = email }).FirstOrDefault();
            }
        }
    }
}       
        