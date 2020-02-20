using System;
using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;
using WhoWantsToBeAMillionaire.Models.Data.Users;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Users;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class GameManager
    {
        private readonly CategoryMySqlRepository _categoryMySqlRepository;
        private readonly QuizQuestionMySqlRepository _quizQuestionMySqlRepository;
        private readonly QuizAnswerMySqlRepository _quizAnswerMySqlRepository;

        private readonly List<RunningGame> _runningGames = new List<RunningGame>();

        public GameManager(CategoryMySqlRepository categoryMySqlRepository, QuizQuestionMySqlRepository quizQuestionMySqlRepository, QuizAnswerMySqlRepository quizAnswerMySqlRepository)
        {
            _categoryMySqlRepository = categoryMySqlRepository;
            _quizQuestionMySqlRepository = quizQuestionMySqlRepository;
            _quizAnswerMySqlRepository = quizAnswerMySqlRepository;
        }

        public string StartGame(User user, IEnumerable<int> categories)
        {
            var gameId = Guid.NewGuid().ToString();
            _runningGames.Add(new RunningGame(user.UserId, gameId, categories));
            return gameId;
        }

        public QuizQuestion GetQuestion(string gameId)
        {
            var game = _runningGames.First(g => g.GameId == gameId);
            
            // TODO: throw error if game doesn't belong to user

            var specification = new QuizQuestionSpecification(game.Categories, game.QuestionIds);
            var questions = _quizQuestionMySqlRepository.Query(specification);
            
            // TODO: Check if no eligible questions exist
            
            var random = new Random();
            var index = random.Next(questions.Count);
            
            return questions[index];
        }
    }
}