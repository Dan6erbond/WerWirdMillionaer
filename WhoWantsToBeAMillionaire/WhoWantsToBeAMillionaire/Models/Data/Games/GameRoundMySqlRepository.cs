using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;

namespace WhoWantsToBeAMillionaire.Models.Data.Games
{
    public class GameRoundMySqlRepository : IRepository<GameRound>
    {
        private readonly IDbConnection _connection;
        public IEnumerable<GameRound> List { get; private set; }

        public GameRoundMySqlRepository(IConfiguration config)
        {
            _connection = new MySqlConnection(config.GetConnectionString("Default"));

            List = _connection.Query<GameRound>("SELECT * FROM `rounds`").ToList();
        }

        public int Create(GameRound item)
        {
            var sql =
                @"INSERT INTO `rounds` (`RoundId`, `GameId`, `QuestionId`, `AnswerId`, `Duration`, `UsedJoker`) 
                VALUES (NULL, @GameId, @QuestionId, @AnswerId, @Duration, @UsedJoker);
                SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = _connection.Query<int>(sql, item).Single();

            List = _connection.Query<GameRound>("SELECT * FROM `rounds`").ToList();

            return id;
        }

        public void Update(GameRound item)
        {
            var sql =
                "UPDATE `rounds` SET `GameId` = @GameId, `QuestionId` = @QuestionId, `AnswerId` = @AnswerId, `Duration` = @Duration, `UsedJoker` = @UsedJoker WHERE `rounds`.`RoundId` = @RoundId";
            _connection.Execute(sql, item);

            List = _connection.Query<GameRound>("SELECT * FROM `rounds`").ToList();
        }

        public void Delete(GameRound item)
        {
            var sql = "DELETE FROM `rounds` WHERE `rounds`.`RoundId` = @RoundId";
            _connection.Execute(sql, item);

            List = _connection.Query<GameRound>("SELECT * FROM `rounds`").ToList();
        }

        public List<GameRound> Query(ISpecification<GameRound> specification)
        {
            var rounds = List.Where(specification.Specificied).ToList();
            return rounds;
        }
    }
}