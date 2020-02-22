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

        public void StartGame(User user, IEnumerable<int> categories)
        {
            // TODO: Make sure categories were selected
            _runningGames.Add(new RunningGame(user.UserId, categories));
        }

        public bool AnswerQuestion(User user, int questionId, int answerId)
        {
            var gameIndex = _runningGames.FindIndex(g => g.UserId == user.UserId);
            var game = _runningGames[gameIndex];
            
            // TODO: Throw error if no game has been found
            
            var quizQuestionSpecification = new QuizQuestionSpecification(questionId);
            var quizQuestion = _quizQuestionMySqlRepository.Query(quizQuestionSpecification).FirstOrDefault();

            // TODO: Check if question doesn't exist
            // TODO: Check if question is the currently asked question

            var quizAnswerSpecification = new QuizAnswerSpecification(answerId, questionId);
            var quizAnswer = _quizAnswerMySqlRepository.Query(quizAnswerSpecification).FirstOrDefault();

            // TODO: Check if answerId is valid

            var answer = new Answer(quizAnswer.AnswerId, quizAnswer.Correct);

            _runningGames[gameIndex].AnswerQuestion(answer);

            return answer.Correct;
        }

        public QuizQuestion GetQuestion(User user)
        {
            var gameIndex = _runningGames.FindIndex(g => g.UserId == user.UserId);
            var game = _runningGames[gameIndex];

            // TODO: Throw error if no game has been found
            // TODO: Check if current question is unanswered

            var quizQuestionSpecification =
                new QuizQuestionSpecification(categories: game.Categories, excludeQuestions: game.QuestionIds);
            var quizQuestions = _quizQuestionMySqlRepository.Query(quizQuestionSpecification);

            // TODO: Check if no eligible questions exist

            var random = new Random();
            var index = random.Next(quizQuestions.Count);
            var quizQuestion = quizQuestions[index];

            var quizAnswerSpecification = new QuizAnswerSpecification(questionId: quizQuestion.QuestionId);
            var quizAnswers = _quizAnswerMySqlRepository.Query(quizAnswerSpecification);

            quizQuestion.Answers = quizAnswers;

            // TODO: Move logic to model class
            var question = new Question(quizQuestion.QuestionId);
            _runningGames[gameIndex].AskQuestions(question);

            return quizQuestion;
        }
    }
}