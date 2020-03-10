﻿using System;
 using System.Collections.Generic;
 using System.Linq;
 using WhoWantsToBeAMillionaire.Models.Data.Quiz;

 namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class Question
    {
        public QuizQuestion QuizQuestion { get; }
        public Answer AnsweredAnswer { get; set; }
        public bool JokerUsed { get; private set; } = false;
        public DateTime TimeAsked { get; set; }
        public DateTime TimeAnswered { get; set; }

        public Question(QuizQuestion quizQuestion)
        {
            QuizQuestion = quizQuestion;
        }

        public QuizQuestion UseJoker()
        {
            JokerUsed = true;

            var random = new Random();
            int unhiddenIndex;
            do
            {
                unhiddenIndex = random.Next(QuizQuestion.Answers.Count);
            } while (QuizQuestion.Answers[unhiddenIndex].Correct);
            
            for (var i = 0; i < QuizQuestion.Answers.Count; i++)
            {
                var answer = QuizQuestion.Answers[i];
                if (!answer.Correct && i != unhiddenIndex)
                {
                    QuizQuestion.Answers[i].HiddenAnswer = true;
                }
            }

            return QuizQuestion;
        }
    }
}