using System.Data;
using Dapper;
using wall.Models;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;


namespace wall.Factory{
    public class UserFactory:IFactory<User>
    {
        private readonly IOptions<MySqlOptions> MySqlConfig;
        public UserFactory(IOptions<MySqlOptions> config)
        {
           MySqlConfig = config;
        }
        internal IDbConnection Connection
        {
            get {
                return new MySqlConnection(MySqlConfig.Value.ConnectionString);
            }
        }
        public void Add(User item)
        {
            using (IDbConnection dbConnection = Connection) {
                string query =  "INSERT INTO users (firstname, lastname, email, username, password, CreateAt, UpdateAt) VALUES (@firstname,@lastname ,@email,@username,@Password, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, item);
            }
        }

     public User GetLatestUser(){
         using(IDbConnection dbConnection = Connection)
         {
             string Query = "SELECT * FROM users ORDER BY UserId DESC LIMIT 1";
             dbConnection.Open();
             return dbConnection.QuerySingleOrDefault<User>(Query);
         }
     }

     public User GetUserById(int id){
         using(IDbConnection dbConnection = Connection)
         {
             string Query = $"SELECT * FROM users WHERE UserId ={id}";
             dbConnection.Open();
             return dbConnection.QuerySingleOrDefault<User>(Query);
         }
     }


     public User GetUserByUsername(string username){
         using(IDbConnection dbConnection = Connection)
         {
          string Query = $"SELECT * FROM users WHERE username = '{username}' LIMIT 1";
          dbConnection.Open();
          return dbConnection.QuerySingleOrDefault<User>(Query);
         }
     }

    }

    public interface IFactory<T>
    {
    }
}
