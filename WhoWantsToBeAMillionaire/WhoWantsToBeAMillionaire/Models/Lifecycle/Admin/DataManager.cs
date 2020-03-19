using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models.Data.Games;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Admin
{
    public class DataManager
    {
        private readonly IRepository<QuizQuestion> _questionRepository;
        private readonly IRepository<QuizAnswer> _answerRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<Round> _roundRepository;
        private readonly IRepository<CategoryGame> _categoryGameRepository;

        public DataManager(IRepository<Game> gameRepository, IRepository<QuizQuestion> questionRepository,
            IRepository<Category> categoryRepository, IRepository<CategoryGame> categoryGameRepository,
            IRepository<Round> roundRepository, IRepository<QuizAnswer> answerRepository)
        {
            _categoryRepository = categoryRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _gameRepository = gameRepository;
            _roundRepository = roundRepository;
            _categoryGameRepository = categoryGameRepository;
        }

        public List<QuizQuestion> GetQuestions(int categoryId)
        {
            var quizQuestionSpecification = new QuizQuestionSpecification(categoryId: categoryId);
            var quizQuestions = _questionRepository.Query(quizQuestionSpecification);

            foreach (var question in quizQuestions)
            {
                var quizAnswerSpecification = new QuizAnswerSpecification(questionId: question.QuestionId);
                var quizAnswers = _answerRepository.Query(quizAnswerSpecification);
                question.Answers = quizAnswers;
            }

            return quizQuestions;
        }

        public void EditQuestion(QuizQuestion question)
        {
            _questionRepository.Update(question);

            foreach (var answer in question.Answers)
            {
                _answerRepository.Update(answer);
            }
        }

        public QuizQuestion AddQuestion(QuizQuestion question)
        {
            var questionId = _questionRepository.Create(question);
            question.QuestionId = questionId;

            foreach (var answer in question.Answers)
            {
                answer.QuestionId = question.QuestionId;
                var answerId = _answerRepository.Create(answer);
                answer.AnswerId = answerId;
            }

            return question;
        }

        public void DeleteQuestion(int id)
        {
            var quizAnswerSpecification = new QuizAnswerSpecification(questionId: id);
            var quizAnswers = _answerRepository.Query(quizAnswerSpecification);

            foreach (var answer in quizAnswers)
            {
                _answerRepository.Delete(answer);
            }

            var quizQuestionSpecification = new QuizQuestionSpecification(id);
            var quizQuestion = _questionRepository.Query(quizQuestionSpecification).First();
            _questionRepository.Delete(quizQuestion);
        }

        public Category AddCategory(Category category)
        {
            var categoryId = _categoryRepository.Create(category);
            category.CategoryId = categoryId;
            return category;
        }

        public void DeleteCategory(int id)
        {
            var categorySpecification = new CategorySpecification(id);
            var category = _categoryRepository.Query(categorySpecification).First();
            _categoryRepository.Delete(category);
        }

        public void DeleteGame(int id)
        {
            var gameSpecification = new GameSpecification(gameId: id);
            var game = _gameRepository.Query(gameSpecification).First();
            game.Hidden = true;
            _gameRepository.Update(game);
        }
    }
}