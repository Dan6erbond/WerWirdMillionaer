using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class QuizQuestionMySqlRepository : IRepository<QuizQuestion>
    {
        private readonly IDbConnection _connection;
        public IEnumerable<QuizQuestion> List { get; private set; }

        public QuizQuestionMySqlRepository(IConfiguration config)
        {
            _connection = new MySqlConnection(config.GetConnectionString("Default"));

            List = _connection.Query<QuizQuestion>("SELECT * FROM `questions`").ToList();
        }

        public void Create(QuizQuestion item)
        {
            var sql = "INSERT INTO `questions` (`QuestionId`, `CategoryId`, `Question`) VALUES (NULL, @CategoryId, @Question);";
            _connection.Execute(sql, item);
            
            List = _connection.Query<QuizQuestion>("SELECT * FROM `questions`").ToList();
        }

        public void Update(QuizQuestion item)
        {
            var sql = "UPDATE `questions` SET `CategoryId` = @CategoryId, `Question` = @Question WHERE `questions`.`QuestionId` = @QuestionId";
            _connection.Execute(sql, item);
            
            List = _connection.Query<QuizQuestion>("SELECT * FROM `questions`").ToList();
        }

        public void Delete(QuizQuestion item)
        {
            var sql = "DELETE FROM `questions` WHERE `questions`.`QuestionId` = @QuestionId";
            _connection.Execute(sql, item);
            
            List = _connection.Query<QuizQuestion>("SELECT * FROM `questions`").ToList();
        }

        public List<QuizQuestion> Query(ISpecification<QuizQuestion> specification)
        {
            var questions = List.Where(specification.Specificied).ToList();
            return questions;
        }
    }
}