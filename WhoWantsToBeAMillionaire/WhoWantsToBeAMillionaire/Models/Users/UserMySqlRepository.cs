using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace WhoWantsToBeAMillionaire.Models.Users
{
    public class UserMySqlRepository : IRepository<User>
    {
        private readonly IDbConnection _connection;
        public IEnumerable<User> List { get; private set; }

        public UserMySqlRepository(IConfiguration config)
        {
            _connection = new MySqlConnection(config.GetConnectionString("Default"));

            List = _connection.Query<User>("SELECT * FROM `users`").ToList();
        }

        public void Create(User item)
        {
            var sql =
                "INSERT INTO `users` (`UserId`, `Username`, `IsAdmin`, `Salt`, `Password`) VALUES (NULL, @Username, @IsAdmin, @Salt, @Password);";
            _connection.Execute(sql, item);
            List = _connection.Query<User>("SELECT * FROM users").ToList();
        }

        public void Update(User item)
        {
            var sql =
                "UPDATE `users` SET `Username` = @Username, `IsAdmin` = @IsAdmin, `Salt` = @Salt, `Password` = @Password WHERE `users`.`UserId` = @UserId";
            _connection.Execute(sql, item);
            List = _connection.Query<User>("SELECT * FROM users").ToList();
        }

        public void Delete(User item)
        {
            var sql = "DELETE FROM `users` WHERE `users`.`UserId` = @UserId";
            _connection.Execute(sql, item);
            List = _connection.Query<User>("SELECT * FROM users").ToList();
        }

        public List<User> Query(ISpecification<User> specification)
        {
            var users = List.Where(specification.Specificied).ToList();
            return users;
        }
    }
}