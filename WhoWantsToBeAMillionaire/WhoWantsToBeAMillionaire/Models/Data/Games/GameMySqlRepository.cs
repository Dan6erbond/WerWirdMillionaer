using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;

namespace WhoWantsToBeAMillionaire.Models.Data.Games
{
    public class GameMySqlRepository : IRepository<Game>
    {
        private readonly IDbConnection _connection;
        public IEnumerable<Game> List { get; private set; }

        public GameMySqlRepository(IConfiguration config)
        {
            _connection = new MySqlConnection(config.GetConnectionString("Default"));

            List = _connection.Query<Game>("SELECT * FROM `games`").ToList();
        }

        public int Create(Game item)
        {
            var sql =
                @"INSERT INTO `games` (`GameId`, `UserId`, `Start`) 
                VALUES (NULL, @UserId, @Start);
                SELECT CAST(LAST_INSERT_ID() as int);";
            var id = _connection.Query<int>(sql, item).Single();

            List = _connection.Query<Game>("SELECT * FROM `games`").ToList();

            return id;
        }

        public void Update(Game item)
        {
            var sql =
                "UPDATE `games` SET `UserId` = @UserId, `Start` = @Start WHERE `games`.`GameId` = @GameId";
            _connection.Execute(sql, item);

            List = _connection.Query<Game>("SELECT * FROM `games`").ToList();
        }

        public void Delete(Game item)
        {
            var sql = "DELETE FROM `games` WHERE `games`.`GameId` = @GameId";
            _connection.Execute(sql, item);

            List = _connection.Query<Game>("SELECT * FROM `games`").ToList();
        }

        public List<Game> Query(ISpecification<Game> specification)
        {
            var games = List.Where(specification.Specificied).ToList();
            return games;
        }
    }
}