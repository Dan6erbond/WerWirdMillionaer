using System;
using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models.Data.Games;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;
using WhoWantsToBeAMillionaire.Models.Data.Users;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Users;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class GameManager
    {
        private readonly IRepository<QuizQuestion> _quizQuestionMySqlRepository;
        private readonly IRepository<QuizAnswer> _quizAnswerMySqlRepository;
        private readonly IRepository<Game> _gameMySqlRepository;
        private readonly IRepository<Round> _roundMySqlRepository;

        private List<RunningGame> _runningGames = new List<RunningGame>();

        public GameManager(IRepository<Game> gameMySqlRepository,
            IRepository<QuizQuestion> quizQuestionMySqlRepository,
            IRepository<QuizAnswer> quizAnswerMySqlRepository,
            IRepository<Round> roundMySqlRepository)
        {
            _quizQuestionMySqlRepository = quizQuestionMySqlRepository;
            _quizAnswerMySqlRepository = quizAnswerMySqlRepository;
            _gameMySqlRepository = gameMySqlRepository;
            _roundMySqlRepository = roundMySqlRepository;
        }

        public void StartGame(User user, IEnumerable<int> categories)
        {
            // Remove any already running user games
            _runningGames = _runningGames.Where(g => g.UserId != user.UserId).ToList();
            _runningGames.Add(new RunningGame(user.UserId, categories));
        }

        public QuizResult EndGame(User user)
        {
            var gameIndex = _runningGames.FindIndex(g => g.UserId == user.UserId);

            // TODO: Throw error if no game has been found (gameIndex = -1)

            var runningGame = _runningGames[gameIndex];
            _runningGames.RemoveAt(gameIndex);

            var game = new Game
            {
                Start = runningGame.AskedQuestions[0].TimeAsked,
                UserId = user.UserId
            };
            var gameId = _gameMySqlRepository.Create(game);

            foreach (var question in runningGame.AskedQuestions)
            {
                var round = new Round
                {
                    Duration = (question.TimeAnswered - question.TimeAsked).Seconds,
                    AnswerId = question.AnsweredAnswer,
                    GameId = gameId,
                    QuestionId = question.QuestionId,
                    UsedJoker = question.JokerUsed
                };
                _roundMySqlRepository.Create(round);
            }

            return runningGame.End();
        }

        public dynamic AnswerQuestion(User user, int questionId, int answerId)
        {
            var gameIndex = _runningGames.FindIndex(g => g.UserId == user.UserId);

            // TODO: Throw error if no game has been found (gameIndex = -1)

            var quizQuestionSpecification = new QuizQuestionSpecification(questionId);
            var quizQuestion = _quizQuestionMySqlRepository.Query(quizQuestionSpecification).FirstOrDefault();

            // TODO: Check if question doesn't exist
            // TODO: Check if question is the currently asked question

            var quizAnswerSpecification = new QuizAnswerSpecification(answerId, questionId);
            var quizAnswer = _quizAnswerMySqlRepository.Query(quizAnswerSpecification).FirstOrDefault();

            // TODO: Check if answerId is valid

            var result = _runningGames[gameIndex].AnswerQuestion(quizAnswer);
            if (result.Correct) return result;

            return EndGame(user);
        }

        public IQuestion<GameAnswer> GetQuestion(User user)
        {
            var gameIndex = _runningGames.FindIndex(g => g.UserId == user.UserId);
            var game = _runningGames[gameIndex];

            // TODO: Throw error if no game has been found
            // TODO: Check if current question is unanswered

            var quizQuestionSpecification =
                new QuizQuestionSpecification(categories: game.Categories, excludeQuestions: game.QuestionIds);
            var quizQuestions = _quizQuestionMySqlRepository.Query(quizQuestionSpecification);

            var random = new Random();
            int index;
            QuizQuestion quizQuestion;

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
            quizQuestion.Answers = quizAnswers;

            var correctAnswer = quizAnswers.First(a => a.Correct);

            var roundSpecification = new RoundSpecification(questionId: quizQuestion.QuestionId);
            var rounds = _roundMySqlRepository.Query(roundSpecification);
            var correctlyAnswered = rounds.Count(r => r.AnswerId == correctAnswer.AnswerId);

            var question = new GameQuestion(quizQuestion, rounds.Count, correctlyAnswered);
            _runningGames[gameIndex].AskQuestion(question);

            return question;
        }

        public IQuestion<GameAnswer> UseJoker(User user)
        {
            var gameIndex = _runningGames.FindIndex(g => g.UserId == user.UserId);
            return _runningGames[gameIndex].UseJoker();
        }
    }
}