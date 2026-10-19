
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace QuizMaster
{
    public class QuizTimer : IDisposable
    {
        public const double GameDurationSeconds = 60;
        public const double QuestionDurationSeconds = 5;
        public const double FeedbackDurationSeconds = 1;
        public const double StartCountdownSeconds = 3;
        public const double EndCountdownSeconds = 5;

        private readonly System.Windows.Forms.Timer clock = new();
        private readonly Stopwatch stopwatch = new();

        private TimeSpan lastElapsedTime;
        private bool disposed;

        public double GameTimeRemaining { get; private set; } = 60;
        public double QuestionTimeRemaining { get; private set; } = 5;
        public double StartTimeRemaining { get; private set; } = 3;
        public double EndTimeRemaining { get; private set; } = 5;

        public bool IsPaused { get; private set; }
        public bool IsRunning => clock.Enabled;

        private enum TimerPhase
        {
            Starting,
            Playing,
            Feedback,
            Ending,
            Stopped
        }

        private TimerPhase phase = TimerPhase.Starting;
        private double phaseTimeRemaining = StartCountdownSeconds;

        public event Action? CountdownUpdated;
        public event Action? StartingFinished;
        public event Action? QuestionExpired;
        public event Action? FeedbackFinished;
        public event Action? GameExpired;
        public event Action? EndingFinished;

        public QuizTimer()
        {
            clock.Interval = 50;
            clock.Tick += Clock_Tick;
        }

        public void Start()
        {
            if (disposed)
                return;

            stopwatch.Start();
            lastElapsedTime = stopwatch.Elapsed;

            IsPaused = false;
            clock.Start();
        }

        public void StartQuestion()
        {
            QuestionTimeRemaining = QuestionDurationSeconds;
            phaseTimeRemaining = QuestionDurationSeconds;
            phase = TimerPhase.Playing;
        }

        public void StartFeedback()
        {
            phaseTimeRemaining = FeedbackDurationSeconds;
            phase = TimerPhase.Feedback;
        }

        public void StartEndingCountdown()
        {
            if (disposed)
                return;

            EndTimeRemaining = EndCountdownSeconds;
            phaseTimeRemaining = EndCountdownSeconds;
            phase = TimerPhase.Ending;

            lastElapsedTime = stopwatch.Elapsed;

            if (!stopwatch.IsRunning)
                stopwatch.Start();

            IsPaused = false;
            clock.Start();
        }

        public void Pause()
        {
            if (disposed || IsPaused)
                return;

            ProcessElapsed();

            if (disposed)
                return;

            // If the game expired while processing elapsed time,
            // allow the results countdown to continue.
            if (phase == TimerPhase.Ending ||
                phase == TimerPhase.Stopped)
                return;

            clock.Stop();
            stopwatch.Stop();
            IsPaused = true;
        }

        public void Resume()
        {
            if (disposed || !IsPaused)
                return;

            lastElapsedTime = stopwatch.Elapsed;
            stopwatch.Start();

            IsPaused = false;
            clock.Start();
        }

        private void Clock_Tick(object? sender, EventArgs e)
        {
            ProcessElapsed();
        }

        private void ProcessElapsed()
        {
            if (disposed || IsPaused ||
                phase == TimerPhase.Stopped)
                return;

            TimeSpan now = stopwatch.Elapsed;

            double elapsed =
                Math.Max(0, (now - lastElapsedTime).TotalSeconds);

            lastElapsedTime = now;

            if (phase == TimerPhase.Starting)
            {
                phaseTimeRemaining -= elapsed;
                StartTimeRemaining =
                    Math.Max(0, phaseTimeRemaining);

                CountdownUpdated?.Invoke();

                if (phaseTimeRemaining <= 0)
                {
                    StartQuestion();
                    StartingFinished?.Invoke();
                }

                return;
            }

            if (phase == TimerPhase.Ending)
            {
                phaseTimeRemaining -= elapsed;
                EndTimeRemaining =
                    Math.Max(0, phaseTimeRemaining);

                CountdownUpdated?.Invoke();

                if (phaseTimeRemaining <= 0)
                {
                    phase = TimerPhase.Stopped;
                    clock.Stop();
                    stopwatch.Stop();
                    EndingFinished?.Invoke();
                }

                return;
            }

            GameTimeRemaining =
                Math.Max(0, GameTimeRemaining - elapsed);

            if (GameTimeRemaining <= 0)
            {
                phase = TimerPhase.Stopped;
                clock.Stop();
                stopwatch.Stop();
                CountdownUpdated?.Invoke();
                GameExpired?.Invoke();
                return;
            }

            phaseTimeRemaining -= elapsed;

            if (phase == TimerPhase.Playing)
            {
                QuestionTimeRemaining =
                    Math.Max(0, phaseTimeRemaining);

                CountdownUpdated?.Invoke();

                if (phaseTimeRemaining <= 0)
                {
                    phase = TimerPhase.Stopped;
                    QuestionExpired?.Invoke();
                }
            }
            else if (phase == TimerPhase.Feedback)
            {
                CountdownUpdated?.Invoke();

                if (phaseTimeRemaining <= 0)
                {
                    phase = TimerPhase.Stopped;
                    FeedbackFinished?.Invoke();
                }
            }
        }

        public void Dispose()
        {
            if (disposed)
                return;

            clock.Stop();
            stopwatch.Stop();

            clock.Tick -= Clock_Tick;
            clock.Dispose();

            disposed = true;
        }
    }
}
