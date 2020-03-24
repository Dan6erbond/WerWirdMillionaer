using System.Linq;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Games;
using WhoWantsToBeAMillionaire.Tests.Models.Data;
using Xunit;

namespace WhoWantsToBeAMillionaire.Tests.UnitTests
{
    public class GameQuestionTests
    {
        private readonly IRepository<QuizQuestion> _questionRepository;
        private readonly IRepository<QuizAnswer> _answerRepository;

        public GameQuestionTests()
        {
            _questionRepository = new QuizQuestionMockRepository();
            _answerRepository = new QuizAnswerMockRepository();
        }
        
        [Fact]
        public void TestUseJoker()
        {
            var quizQuestion = _questionRepository.List.First();
            
            var answerSpecification = new QuizAnswerSpecification(questionId: quizQuestion.QuestionId);
            var quizAnswers = _answerRepository.Query(answerSpecification);
            quizQuestion.Answers = quizAnswers;
            
            // Would be much better:
            /*
             * var question = new GameQuestion(quizQuestion, quizAnswers, 0, 0);
             */
                
            var question = new GameQuestion(quizQuestion, 0, 0);
            question.UseJoker();
            
            var hiddenAnswers = question.Answers.Where(a => a.HiddenAnswer);
            Assert.Equal(2, hiddenAnswers.Count());
        }
    }
}