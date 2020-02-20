using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class QuizAnswerMySqlRepository : IRepository<QuizQuestion>
    {
        private readonly IDbConnection _connection;
        public IEnumerable<QuizQuestion> List { get; private set; }

        public QuizAnswerMySqlRepository(IConfiguration config)
        {
            _connection = new MySqlConnection(config.GetConnectionString("Default"));

            List = _connection.Query<QuizQuestion>("SELECT * FROM `answers`").ToList();
        }

        public void Create(QuizQuestion item)
        {
            var sql = "INSERT INTO `answers` (`AnswerId`, `QuestionId`, `Answer`, `Correct`) VALUES (NULL, @QuestionId, @Answer, @Correct);";
            _connection.Execute(sql, item);
            
            List = _connection.Query<QuizQuestion>("SELECT * FROM `answers`").ToList();
        }

        public void Update(QuizQuestion item)
        {
            var sql = "UPDATE `answers` SET `QuestionId` = @QuestionId, `Answer` = @Answer, `Correct` = @Correct WHERE `answers`.`AnswerId` = @AnswerId";
            _connection.Execute(sql, item);
            
            List = _connection.Query<QuizQuestion>("SELECT * FROM `answers`").ToList();
        }

        public void Delete(QuizQuestion item)
        {
            var sql = "DELETE FROM `answers` WHERE `answers`.`AnswerId` = @AnswerId";
            _connection.Execute(sql, item);
            
            List = _connection.Query<QuizQuestion>("SELECT * FROM `answers`").ToList();
        }

        public List<QuizQuestion> Query(ISpecification<QuizQuestion> specification)
        {
            var categories = List.Where(specification.Specificied).ToList();
            return categories;
        }
    }
}