using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class QuizAnswerMySqlRepository : IRepository<QuizAnswer>
    {
        private readonly IDbConnection _connection;
        public IEnumerable<QuizAnswer> List { get; private set; }

        public QuizAnswerMySqlRepository(IConfiguration config)
        {
            _connection = new MySqlConnection(config.GetConnectionString("Default"));

            List = _connection.Query<QuizAnswer>("SELECT * FROM `answers`").ToList();
        }

        public void Create(QuizAnswer item)
        {
            var sql = "INSERT INTO `answers` (`AnswerId`, `QuestionId`, `Answer`, `Correct`) VALUES (NULL, @QuestionId, @Answer, @Correct);";
            _connection.Execute(sql, item);
            
            List = _connection.Query<QuizAnswer>("SELECT * FROM `answers`").ToList();
        }

        public void Update(QuizAnswer item)
        {
            var sql = "UPDATE `answers` SET `QuestionId` = @QuestionId, `Answer` = @Answer, `Correct` = @Correct WHERE `answers`.`AnswerId` = @AnswerId";
            _connection.Execute(sql, item);
            
            List = _connection.Query<QuizAnswer>("SELECT * FROM `answers`").ToList();
        }

        public void Delete(QuizAnswer item)
        {
            var sql = "DELETE FROM `answers` WHERE `answers`.`AnswerId` = @AnswerId";
            _connection.Execute(sql, item);
            
            List = _connection.Query<QuizAnswer>("SELECT * FROM `answers`").ToList();
        }

        public List<QuizAnswer> Query(ISpecification<QuizAnswer> specification)
        {
            var answers = List.Where(specification.Specificied).ToList();
            return answers;
        }
    }
}