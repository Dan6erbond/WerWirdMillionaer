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
        private readonly IRepository<Category> _categoryMySqlRepository;
        private readonly IRepository<QuizQuestion> _quizQuestionMySqlRepository;
        private readonly IRepository<QuizAnswer> _quizAnswerMySqlRepository;

        private readonly List<RunningGame> _runningGames = new List<RunningGame>();

        public GameManager(IRepository<Category> categoryMySqlRepository,
            IRepository<QuizQuestion> quizQuestionMySqlRepository,
            IRepository<QuizAnswer> quizAnswerMySqlRepository)
        {
            _categoryMySqlRepository = categoryMySqlRepository;
            _quizQuestionMySqlRepository = quizQuestionMySqlRepository;
            _quizAnswerMySqlRepository = quizAnswerMySqlRepository;
        }

        public string StartGame(User user, IEnumerable<int> categories)
        {
            var gameId = Guid.NewGuid().ToString();
            _runningGames.Add(new RunningGame(user.UserId, categories));
            return gameId;
        }

        public QuizQuestion GetQuestion(User user)
        {
            var game = _runningGames.First(g => g.UserId == user.UserId);

            // TODO: throw error if no game has been found

            var specification = new QuizQuestionSpecification(game.Categories, game.QuestionIds);
            var questions = _quizQuestionMySqlRepository.Query(specification);

            // TODO: Check if no eligible questions exist

            var random = new Random();
            var index = random.Next(questions.Count);

            return questions[index];
        }
    }
}