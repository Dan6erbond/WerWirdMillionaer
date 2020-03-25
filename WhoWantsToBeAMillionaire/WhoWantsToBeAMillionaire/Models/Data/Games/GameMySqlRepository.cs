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
                @"INSERT INTO `games` (`GameId`, `UserId`, `Start`, `Hidden`) 
                VALUES (NULL, @UserId, @Start, @Hidden);
                SELECT CAST(LAST_INSERT_ID() as int);";
            var id = _connection.Query<int>(sql, item).Single();

            Read();

            return id;
        }

        private void Read()
        {
            List = _connection
                .Query<Game>(@"SELECT * FROM `games` INNER JOIN `users` ON `games`.UserId = `users`.`UserID`;")
                .ToList();
        }

        public void Update(Game item)
        {
            var sql =
                "UPDATE `games` SET `UserId` = @UserId, `Start` = @Start, `Hidden` = @Hidden WHERE `games`.`GameId` = @GameId";
            _connection.Execute(sql, item);

            Read();
        }

        public void Delete(Game item)
        {
            var sql = "DELETE FROM `games` WHERE `games`.`GameId` = @GameId";
            _connection.Execute(sql, item);

            Read();
        }

        public List<Game> Query(ISpecification<Game> specification)
        {
            var games = List.Where(specification.Specificied).ToList();
            return games;
        }
    }
}