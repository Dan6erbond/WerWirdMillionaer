﻿using System;
using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models.Data.Games;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;
using WhoWantsToBeAMillionaire.Models.Data.Users;
using WhoWantsToBeAMillionaire.Models.Exceptions;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Users;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class GameManager
    {
        private readonly IRepository<QuizQuestion> _questionRepository;
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<QuizAnswer> _answerRepository;
        private readonly IRepository<Round> _roundRepository;
        private readonly IRepository<CategoryGame> _categoryGameRepository;

        private List<RunningGame> _runningGames = new List<RunningGame>();

        public GameManager(IRepository<Game> gameRepository, IRepository<QuizQuestion> questionRepository,IRepository<Round> roundRepository,
            IRepository<User> userRepository, IRepository<QuizAnswer> answerRepository, IRepository<CategoryGame> categoryGameRepository)
        {
            _questionRepository = questionRepository;
            _gameRepository = gameRepository;
            _roundRepository = roundRepository;
            _userRepository = userRepository;
            _answerRepository = answerRepository;
            _categoryGameRepository = categoryGameRepository;
        }

        public void StartGame(User user, IEnumerable<int> categories)
        {
            // Remove any already running user games
            _runningGames = _runningGames.Where(g => g.UserId != user.UserId).ToList();
            _runningGames.Add(new RunningGame(user.UserId, categories));
        }

        public QuizResult EndGame(User user, bool won = true)
        {
            var gameIndex = _runningGames.FindIndex(g => g.UserId == user.UserId);

            // TODO: Throw error if no game has been found (gameIndex = -1)

            var runningGame = _runningGames[gameIndex];
            _runningGames.RemoveAt(gameIndex);

            var game = new Game
            {
                Start = runningGame.TimeStarted,
                UserId = user.UserId
            };
            var gameId = _gameRepository.Create(game);

            if (runningGame.CurrentQuestion != null) runningGame.AskedQuestions.Add(runningGame.CurrentQuestion);
            foreach (var question in runningGame.AskedQuestions)
            {
                var round = new Round
                {
                    Duration = (question.TimeAnswered - question.TimeAsked).Seconds,
                    AnswerId = question.AnsweredAnswer?.AnswerId,
                    GameId = gameId,
                    QuestionId = question.QuestionId,
                    UsedJoker = question.JokerUsed
                };
                _roundRepository.Create(round);
            }

            foreach (var category in runningGame.Categories)
            {
                var categoryGame = new CategoryGame(category, gameId);
                _categoryGameRepository.Create(categoryGame);
            }

            return runningGame.End(won);
        }

        public dynamic AnswerQuestion(User user, int questionId, int answerId)
        {
            var gameIndex = _runningGames.FindIndex(g => g.UserId == user.UserId);

            // TODO: Throw error if no game has been found (gameIndex = -1)

            var quizQuestionSpecification = new QuizQuestionSpecification(questionId);
            var quizQuestion = _questionRepository.Query(quizQuestionSpecification).FirstOrDefault();

            // TODO: Check if question doesn't exist
            // TODO: Check if question is the currently asked question

            var quizAnswerSpecification = new QuizAnswerSpecification(answerId, questionId);
            var quizAnswer = _answerRepository.Query(quizAnswerSpecification).FirstOrDefault();

            // TODO: Check if answerId is valid

            var result = _runningGames[gameIndex].AnswerQuestion(quizAnswer);
            if (result.Correct) return result;

            return EndGame(user, false);
        }

        public IQuestion<GameAnswer> GetQuestion(User user)
        {
            var gameIndex = _runningGames.FindIndex(g => g.UserId == user.UserId);
            var game = _runningGames[gameIndex];

            // TODO: Throw error if no game has been found
            // TODO: Check if current question is unanswered

            var quizQuestionSpecification =
                new QuizQuestionSpecification(categories: game.Categories, excludeQuestions: game.QuestionIds);
            var quizQuestions = _questionRepository.Query(quizQuestionSpecification);

            var random = new Random();
            QuizQuestion quizQuestion;

            if (quizQuestions.Count != 0)
            {
                var index = random.Next(quizQuestions.Count);
                quizQuestion = quizQuestions[index];
            }
            else
            {
                throw new NoMoreQuestionsException(
                    "There are no more questions available in the selected categories for this quiz!");
            }

            var quizAnswerSpecification = new QuizAnswerSpecification(questionId: quizQuestion.QuestionId);
            var quizAnswers = _answerRepository.Query(quizAnswerSpecification);
            quizQuestion.Answers = quizAnswers;

            var correctAnswer = quizAnswers.First(a => a.Correct);

            var roundSpecification = new RoundSpecification(questionId: quizQuestion.QuestionId);
            var rounds = _roundRepository.Query(roundSpecification);
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

        public List<Game> GetGames(User user = null)
        {
            var games = _gameRepository.List.ToList(); // Grab all the games to calculate the rank

            foreach (var game in games)
            {
                var roundSpecification = new RoundSpecification(game.GameId);
                var rounds = _roundRepository.Query(roundSpecification);
                game.Rounds = rounds;

                var lastAnswerSpecification = new QuizAnswerSpecification(rounds.Last().AnswerId);
                var lastAnswer = _answerRepository.Query(lastAnswerSpecification).First();

                foreach (var round in rounds)
                {
                    game.Duration += round.Duration;

                    var answerSpecification = new QuizAnswerSpecification(round.AnswerId);
                    var answer = _answerRepository.Query(answerSpecification).FirstOrDefault();
                    if (answer != null && answer.Correct && lastAnswer.Correct)
                    {
                        game.Points += 30;
                    }
                }
                
                if (user != null && game.UserId != user.UserId)
                {
                    continue; // Saves time by not grabbing information for unneeded games
                }
                
                game.WeightedPoints = game.Points / Math.Max(game.Duration, 1);

                var userSpecification = new UserSpecification(game.UserId);
                var u = _userRepository.Query(userSpecification).First();
                game.Username = u.Username;
                
                var categoryGameSpecification = new CategoryGameSpecification(gameId: game.GameId);
                var categoryGames = _categoryGameRepository.Query(categoryGameSpecification);
                foreach (var categoryGame in categoryGames)
                {
                    game.Categories.Add(categoryGame.CategoryId);
                }
            }

            //TODO: Use IComparer<T>
            games.Sort((game, game1) => game.CompareTo(game1));

            //TODO: Figure out more efficient way to complete this task
            for (int i = 0; i < games.Count; i++)
            {
                games[i].Rank = i + 1;
            }
            

            if (user != null)
            {
                games = games.Where(g => g.UserId == user.UserId).ToList();
                // TODO: Use IComparer<T> & sort games by date
            }

            return games;
        }
    }
}