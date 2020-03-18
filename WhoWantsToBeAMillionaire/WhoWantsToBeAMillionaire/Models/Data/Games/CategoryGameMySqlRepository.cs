using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;

namespace WhoWantsToBeAMillionaire.Models.Data.Games
{
    public class CategoryGameMySqlRepository : IRepository<CategoryGame>
    {
        private readonly IDbConnection _connection;
        public IEnumerable<CategoryGame> List { get; private set; }

        public CategoryGameMySqlRepository(IConfiguration config)
        {
            _connection = new MySqlConnection(config.GetConnectionString("Default"));

            List = _connection.Query<CategoryGame>("SELECT * FROM `categoriesgames`").ToList();
        }

        public int Create(CategoryGame item)
        {
            var sql =
                @"INSERT INTO `categoriesgames` (`CategoryGameId`, `CategoryId`, `GameId`) 
                VALUES (NULL, @CategoryId, @GameId);
                SELECT CAST(LAST_INSERT_ID() as int);";
            var id = _connection.Query<int>(sql, item).Single();

            List = _connection.Query<CategoryGame>("SELECT * FROM `categoriesgames`").ToList();

            return id;
        }

        public void Update(CategoryGame item)
        {
            var sql =
                "UPDATE `categoriesgames` SET `CategoryId` = @CategoryId, `GameId` = @GameId WHERE `categoriesgames`.`CategoryGameId` = @CategoryGameId";
            _connection.Execute(sql, item);

            List = _connection.Query<CategoryGame>("SELECT * FROM `categoriesgames`").ToList();
        }

        public void Delete(CategoryGame item)
        {
            var sql = "DELETE FROM `categoriesgames` WHERE `categoriesgames`.`CategoryGameId` = @CategoryGameId";
            _connection.Execute(sql, item);

            List = _connection.Query<CategoryGame>("SELECT * FROM `categoriesgames`").ToList();
        }

        public List<CategoryGame> Query(ISpecification<CategoryGame> specification)
        {
            var categoryGames = List.Where(specification.Specificied).ToList();
            return categoryGames;
        }
    }
}