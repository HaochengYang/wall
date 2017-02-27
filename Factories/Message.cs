using System;
using System.Collections.Generic;
using System.Data;
using System.Ling;
using Dapper;
using wall.Models;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using static Dapper.SqlMapper;

namespace wall.Factory{
    public class MessageFactory : IFactory<Message>{
        private readonly IOptions<MySqlOptions>MySqlConfig;

        public MessageFactory(IOptions<MySqlOptions>config){
            MySqlConfig = config;
      }

      internal IDbConnection Connection{
        get {return new MySqlConnection(MySqlConfig.Value.ConnectionString);}
      }
      
      public void Add(Message Item){
          using(IDbConnection dbConnection = Connection){
          string Query = "INSERT INTO messages (message, posterid, CreateAt, UpdateAt) VALUES (@message , @posterid, NOW(),NOW())";
          dbConnection.Open();
          dbConnection.Execute(Query, Item);
          }
      }

      public List<Message> GetAllMessages(){
          using(IDbConnection dbConnection = Connection){
          string Query = @"SELECT * from messages JOIN users ON messages.posterid WhERE messages.posterid = users.Userid ORDER BY messages.CreateAt DESC; SELECT * FROM comments JOIN users comments.commenterid WhERE comments.commenterid = users.Userid";
          dbConnection.Open();
          using(GridReader multi = dbConnection.QueryMultiple(Query, null)){
            List<Message> Messages = multi.Read<Message, User, Message>((message, user) => { message.poster = user; return message; }, splitOn: "Userid").ToList();
            List<Comment> Comments = multi.Read<Comment, User, Comment>((comment, user) => { comment.Commenter = user; return comment; }, splitOn: "Userid").ToList();
            List<Message> combo = Messages.GroupJoin(
                                Comments,
                                message => message.Messageid,
                                comment => comment.Messageid,
                                (message, comments) =>
                                {
                                    message.Comments = comments.ToList();
                                    return message;
                                }).ToList();

                    return combo;
          }
        }
      }

        public Message GetMessageById(int id)
        {
            using(IDbConnection dbConnection = Connection)
            {
                string Query = $"SELECT * FROM messages WHERE MessageId = {id} LIMIT 1";

                dbConnection.Open();
                dbConnection.Query<Message, User, Message>(Query,());
                return dbConnection.QuerySingleOrDefault<Message>(Query);
            }
        }
     
        public void DeleteMessage(int id)
        {
            using(IDbConnection dbConnection = Connection)
            {
                string Query = $"DELETE FROM messages WHERE Messageid = {id}";
                dbConnection.Open();
                dbConnection.Execute(Query);
            }
        }
      }

    }