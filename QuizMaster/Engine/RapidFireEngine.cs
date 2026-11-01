
using System;
using System.Collections.Generic;

namespace QuizMaster
{
    public class RapidFireEngine
    {
        // ==========================================
        // QUIZ DATA
        // ==========================================

        public List<QuizQuestion> Questions { get; }

        public List<QuestionResult> Results { get; } = new();

        public int CurrentQuestionIndex { get; private set; }

        public int CorrectAnswers { get; private set; }
        public int WrongAnswers { get; private set; }
        public int UnansweredQuestions { get; private set; }

        public bool SkipUsed { get; private set; }

        public const int RequiredCorrectAnswers = 10;

        public bool HasMoreQuestions =>
            CurrentQuestionIndex < Questions.Count;

        public bool IsCompleted =>
            CorrectAnswers >= RequiredCorrectAnswers;

        public QuizQuestion CurrentQuestion =>
            Questions[CurrentQuestionIndex];

        // ==========================================
        // GOLDEN QUESTION
        // ==========================================

        public int GoldenQuestionIndex { get; }

        public bool IsGoldenQuestion =>
            HasMoreQuestions &&
            GoldenQuestion.IsGolden(
                CurrentQuestionIndex,
                GoldenQuestionIndex);

        // ==========================================
        // TIME ADJUSTMENTS
        // ==========================================

        public int TimeAdjustmentSeconds { get; private set; }

        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public RapidFireEngine(List<QuizQuestion> questions)
        {
            Questions = questions;

            GoldenQuestionIndex = GoldenQuestion.SelectIndex(
                Questions.Count,
                isSpeedQuiz: true);
        }

        // ==========================================
        // SHUFFLE ANSWERS
        // ==========================================

        public string[] ShuffleAnswers()
        {
            string[] answers =
            {
                CurrentQuestion.CorrectAnswer,
                CurrentQuestion.WrongAnswer1,
                CurrentQuestion.WrongAnswer2,
                CurrentQuestion.WrongAnswer3
            };

            Random.Shared.Shuffle(answers);

            return answers;
        }

        // ==========================================
        // RECORD QUESTION RESULT
        // ==========================================

        private void RecordResult(
            string? userAnswer,
            string status)
        {
            Results.Add(new QuestionResult
            {
                Question = CurrentQuestion,
                UserAnswer = userAnswer,
                Status = status
            });
        }

        // ==========================================
        // SUBMIT ANSWER
        // ==========================================

        public bool SubmitAnswer(string answer)
        {
            bool correct =
                answer == CurrentQuestion.CorrectAnswer;

            if (correct)
            {
                CorrectAnswers++;

                if (IsGoldenQuestion)
                    TimeAdjustmentSeconds -= 5;
            }
            else
            {
                WrongAnswers++;

                TimeAdjustmentSeconds +=
                    IsGoldenQuestion ? 2 : 5;
            }

            RecordResult(
                answer,
                correct ? "Correct" : "Wrong");

            return correct;
        }

        // ==========================================
        // QUESTION TIMEOUT
        // ==========================================

        public void TimeoutQuestion()
        {
            UnansweredQuestions++;

            TimeAdjustmentSeconds +=
                IsGoldenQuestion ? 1 : 2;

            RecordResult(null, "Timed out");
        }

        // ==========================================
        // SKIP QUESTION
        // ==========================================

        public bool SkipQuestion()
        {
            if (SkipUsed)
                return false;

            SkipUsed = true;
            UnansweredQuestions++;

            RecordResult(null, "Skipped");

            return true;
        }

        // ==========================================
        // NEXT QUESTION
        // ==========================================

        public void AdvanceQuestion()
        {
            if (HasMoreQuestions)
                CurrentQuestionIndex++;
        }

        // ==========================================
        // FINAL TIME
        // ==========================================

        public double CalculateFinalTime(
            double stopwatchSeconds)
        {
            return Math.Max(
                0,
                stopwatchSeconds + TimeAdjustmentSeconds);
        }
    }
}
