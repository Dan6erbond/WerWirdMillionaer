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

        private List<RunningGame> runningGames = new List<RunningGame>();

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
            // Remove any already running user games
            runningGames = runningGames.Where(g => g.UserId != user.UserId).ToList();
            runningGames.Add(new RunningGame(user.UserId, categories));
        }

        public void EndGame(User user)
        {
            var gameIndex = runningGames.FindIndex(g => g.UserId == user.UserId);
            
            // TODO: Throw error if no game has been found (gameIndex = -1)
            
            
        }

        public bool AnswerQuestion(User user, int questionId, int answerId)
        {
            var gameIndex = runningGames.FindIndex(g => g.UserId == user.UserId);
            
            // TODO: Throw error if no game has been found (gameIndex = -1)
            
            var quizQuestionSpecification = new QuizQuestionSpecification(questionId);
            var quizQuestion = _quizQuestionMySqlRepository.Query(quizQuestionSpecification).FirstOrDefault();

            // TODO: Check if question doesn't exist
            // TODO: Check if question is the currently asked question

            var quizAnswerSpecification = new QuizAnswerSpecification(answerId, questionId);
            var quizAnswer = _quizAnswerMySqlRepository.Query(quizAnswerSpecification).FirstOrDefault();

            // TODO: Check if answerId is valid

            var answer = new Answer(quizAnswer.AnswerId, quizAnswer.Correct);

            runningGames[gameIndex].AnswerQuestion(answer);

            return answer.Correct;
        }

        public QuizQuestion GetQuestion(User user)
        {
            var gameIndex = runningGames.FindIndex(g => g.UserId == user.UserId);
            var game = runningGames[gameIndex];

            // TODO: Throw error if no game has been found
            // TODO: Check if current question is unanswered

            var quizQuestionSpecification =
                new QuizQuestionSpecification(categories: game.Categories, excludeQuestions: game.QuestionIds);
            var quizQuestions = _quizQuestionMySqlRepository.Query(quizQuestionSpecification);

            var random = new Random();
            int index = 0;
            QuizQuestion quizQuestion = null; 

            if (quizQuestions.Count != 0)
            {
                index = random.Next(quizQuestions.Count);
                quizQuestion = quizQuestions[index];
            }
            else
            {
                var genericQuestions = _quizQuestionMySqlRepository.List.ToList();
                index = random.Next(genericQuestions.Count);
                quizQuestion = genericQuestions[index];
            }

            var quizAnswerSpecification = new QuizAnswerSpecification(questionId: quizQuestion.QuestionId);
            var quizAnswers = _quizAnswerMySqlRepository.Query(quizAnswerSpecification);
            quizAnswers.Shuffle();

            quizQuestion.Answers = quizAnswers;

            // TODO: Move logic to model class
            var question = new Question(quizQuestion.QuestionId);
            runningGames[gameIndex].AskQuestion(question);

            return quizQuestion;
        }
    }
}