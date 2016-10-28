using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using beltReviewer.Models;
using Microsoft.Extensions.Options;

namespace beltReviewer.Factory
{
    public class GroupFactory : IFactory<User>
    {
        // private string connectionString;

        // public UserFactory()
        // {
        //     connectionString = "server=localhost;UserId=root;password=root;port=8889;database=login;SslMode=None";
        // }

         private readonly IOptions<MySqlOptions> mysqlConfig;

        public GroupFactory(IOptions<MySqlOptions> conf) {
            mysqlConfig = conf;
        }

        internal IDbConnection Connection
        {
            get {
                return new MySqlConnection(mysqlConfig.Value.ConnectionString);
            }
        }
         public int Add(string name, string description,int user_id){
            using (IDbConnection dbConnection = Connection) {
                string query = "INSERT INTO groups (name, description, user_id) VALUES ('"+name +"', '"+ description + "', '"+ user_id + "')";
                dbConnection.Open();
               var new_group= dbConnection.Execute(query);
               return new_group;
                // return dbConnection.Query<User>("SELECT * FROM users WHERE email = @Email", new { Email = email }).FirstOrDefault();
            }
        }

         public void AddUserGroup(int user_id, int group_id){
            using (IDbConnection dbConnection = Connection) {
                string query = "INSERT INTO user_groups (user_id, group_id) VALUES ('"+user_id +"', '"+ group_id + "')";
                dbConnection.Open();
                dbConnection.Execute(query);
                
            }
        }

        public IEnumerable<Group> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Group>("SELECT * FROM groups");
            }
        }

        //  public Group FindById(int id)
        // {
        //     using (IDbConnection dbConnection = Connection)
        //     {

        //         dbConnection.Open();
        //         return dbConnection.Query<Group>("SELECT * FROM groups where id = @id", new {id = id}).FirstOrDefault();
        //     }
        // }

          public Group FindById(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var query = $"select * from groups left join users on groups.user_id = users.id where groups.id = 4";
                dbConnection.Open();
                var my_group = dbConnection.Query<Group, User, Group>(query, (group, user) => { group.user = user; return group; }).FirstOrDefault();
                
                return my_group;
            }
        }

         public Group FindByName(string name){
            using(IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Group>("SELECT * FROM groups WHERE name = @Name", new { Name = name }).FirstOrDefault();
            }
        }

          public UserGroup FindUserGroups(int Id)
        {
            using (IDbConnection dbConnection = Connection){
                dbConnection.Open();
                var query = 
                @"
                select * from groups where groups.id = '"+Id +"'; select * from user_groups join users on user_groups.user_id = users.id where user_groups.group_id = '"+Id +"';";
                
                using (var multi = dbConnection.QueryMultiple(query)){
                    var userGroup = multi.Read<UserGroup>().SingleOrDefault();
                    userGroup.groups = multi.Read<Group>().ToList();
                    return userGroup;
                    
                }
            }
        } 

        public void Delete(int group_id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "delete from groups where id = '"+group_id +"'";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }   

        public void LeaveGroup(int user_id, int group_id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "delete from user_groups where user_id = '"+user_id +"' and group_id = '"+group_id +"'";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }       
    }
}       
        