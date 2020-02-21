using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class CategoryMySqlRepository : IRepository<Category>
    {
        private readonly IDbConnection _connection;
        public IEnumerable<Category> List { get; private set; }

        public CategoryMySqlRepository(IConfiguration config)
        {
            _connection = new MySqlConnection(config.GetConnectionString("Default"));

            List = _connection.Query<Category>("SELECT * FROM `categories`").ToList();
        }

        public int Create(Category item)
        {
            var sql = @"INSERT INTO `categories` (`CategoryId`, `Name`) VALUES (NULL, @Name);
            SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = _connection.Query<int>(sql, item).Single();

            List = _connection.Query<Category>("SELECT * FROM `categories`").ToList();

            return id;
        }

        public void Update(Category item)
        {
            var sql = "UPDATE `categories` SET `Name` = @Name WHERE `categories`.`CategoryId` = @CategoryId";
            _connection.Execute(sql, item);

            List = _connection.Query<Category>("SELECT * FROM `categories`").ToList();
        }

        public void Delete(Category item)
        {
            var sql = "DELETE FROM `categories` WHERE `categories`.`CategoryId` = @CategoryId";
            _connection.Execute(sql, item);

            List = _connection.Query<Category>("SELECT * FROM `categories`").ToList();
        }

        public List<Category> Query(ISpecification<Category> specification)
        {
            var categories = List.Where(specification.Specificied).ToList();
            return categories;
        }
    }
}