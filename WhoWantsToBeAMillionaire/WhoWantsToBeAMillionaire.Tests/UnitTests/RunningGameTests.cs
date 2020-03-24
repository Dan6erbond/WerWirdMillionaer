using System.Linq;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Api.ApiResults;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Games;
using WhoWantsToBeAMillionaire.Tests.Models.Data;
using Xunit;
using Xunit.Abstractions;

namespace WhoWantsToBeAMillionaire.Tests.UnitTests
{
    public class RunningGameTests
    {
        private readonly ITestOutputHelper _outputHelper;
        
        private readonly IRepository<QuizQuestion> _questionRepository;
        private readonly IRepository<QuizAnswer> _answerRepository;

        public RunningGameTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            
            _questionRepository = new QuizQuestionMockRepository();
            _answerRepository = new QuizAnswerMockRepository();
        }

        [Fact]
        public void TestAskQuestion()
        {
            var quizQuestion = _questionRepository.List.First();

            var answerSpecification = new QuizAnswerSpecification(questionId: quizQuestion.QuestionId);
            var quizAnswers = _answerRepository.Query(answerSpecification);
            quizQuestion.Answers = quizAnswers;

            var question = new GameQuestion(quizQuestion, 0, 0);

            var game = new RunningGame(0, new[] {0});
            game.AskQuestion(question);

            Assert.Equal(question, game.CurrentQuestion);
        }

        [Fact]
        public void TestUseJoker()
        {
            var quizQuestion = _questionRepository.List.First();

            var answerSpecification = new QuizAnswerSpecification(questionId: quizQuestion.QuestionId);
            var quizAnswers = _answerRepository.Query(answerSpecification);
            quizQuestion.Answers = quizAnswers;

            var question = new GameQuestion(quizQuestion, 0, 0);

            var game = new RunningGame(0, new[] {0});
            game.AskQuestion(question);

            game.UseJoker();

            Assert.True(game.JokerUsed);
        }

        [Fact]
        public void TestAnswerQuestionCorrect()
        {
            var quizQuestion = _questionRepository.List.First();

            var answerSpecification = new QuizAnswerSpecification(questionId: quizQuestion.QuestionId);
            var quizAnswers = _answerRepository.Query(answerSpecification);
            quizQuestion.Answers = quizAnswers;

            var question = new GameQuestion(quizQuestion, 0, 0);

            var game = new RunningGame(0, new[] {0});
            game.AskQuestion(question);

            var answer = quizAnswers.First(a => a.Correct);
            
            var result = game.AnswerQuestion(answer);

            Assert.IsType<AnswerResult>(result);
        }

        [Fact]
        public void TestAnswerQuestionIncorrect()
        {
            var quizQuestion = _questionRepository.List.First();

            var answerSpecification = new QuizAnswerSpecification(questionId: quizQuestion.QuestionId);
            var quizAnswers = _answerRepository.Query(answerSpecification);
            quizQuestion.Answers = quizAnswers;

            var question = new GameQuestion(quizQuestion, 0, 0);

            var game = new RunningGame(0, new[] {0});
            game.AskQuestion(question);

            var answer = quizAnswers.First(a => !a.Correct);
            
            var result = game.AnswerQuestion(answer);

            Assert.True(!result.Correct);
        }

        [Fact]
        public void TestEndGame()
        {
            var quizQuestion = _questionRepository.List.First();

            var answerSpecification = new QuizAnswerSpecification(questionId: quizQuestion.QuestionId);
            var quizAnswers = _answerRepository.Query(answerSpecification);
            
            quizQuestion.Answers = quizAnswers;

            var question = new GameQuestion(quizQuestion, 0, 0);

            var game = new RunningGame(0, new[] {0});
            game.AskQuestion(question);

            var answer = quizAnswers.First(a => a.Correct);
            var answerResult = game.AnswerQuestion(answer);

            var result = game.End(answerResult.Correct && !answerResult.TimeOver, answerResult.TimeOver);

            Assert.Equal(30, result.Points);
        }
    }
}