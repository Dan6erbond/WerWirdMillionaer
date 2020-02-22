﻿using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class RunningGame
    {
        public int UserId { get; }
        public List<int> Categories { get; }
        public List<Question> AskedQuestions { get; } = new List<Question>();
        public Question CurrentQuestion { get; set; }

        public List<int> QuestionIds
        {
            get
            {
                var questionIds = new List<int>(CurrentQuestion.QuestionId);
                AskedQuestions.ForEach(q => questionIds.Add(q.QuestionId));
                return questionIds;
            }
        }

        public RunningGame(int userId, IEnumerable<int> categories)
        {
            UserId = userId;
            Categories = categories.ToList();
        }
    }
}