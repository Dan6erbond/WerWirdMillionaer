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

        public QuizQuestion StartGame(User user, IEnumerable<int> categories)
        {
            _runningGames.Add(new RunningGame(user.UserId, categories));
            return GetQuestion(user);
        }

        public QuizQuestion GetQuestion(User user)
        {
            var gameIndex = _runningGames.FindIndex(g => g.UserId == user.UserId);
            var game = _runningGames[gameIndex];

            // TODO: Throw error if no game has been found

            var quizQuestionSpecification = new QuizQuestionSpecification(game.Categories, game.QuestionIds);
            var quizQuestions = _quizQuestionMySqlRepository.Query(quizQuestionSpecification);

            // TODO: Check if no eligible questions exist

            var random = new Random();
            var index = random.Next(quizQuestions.Count);
            var quizQuestion = quizQuestions[index];

            var quizAnswerSpecification = new QuizAnswerSpecification(quizQuestion.QuestionId);
            var quizAnswers = _quizAnswerMySqlRepository.Query(quizAnswerSpecification);

            quizQuestion.Answers = quizAnswers;
            
            // TODO: Move logic to model class
            var question = new Question(quizQuestion.QuestionId);

            if (_runningGames[gameIndex].CurrentQuestion != null)
            {
                _runningGames[gameIndex].AskedQuestions.Add(_runningGames[gameIndex].CurrentQuestion);
            }
            
            _runningGames[gameIndex].CurrentQuestion = question;
            
            return quizQuestion;
        }
    }
}