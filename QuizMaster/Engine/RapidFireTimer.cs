
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace QuizMaster
{
    public class RapidFireTimer : IDisposable
    {
        // ==========================================
        // TIMER DURATIONS
        // ==========================================

        public const double QuestionDurationSeconds = 5;
        public const double FeedbackDurationSeconds = 1;
        public const double StartCountdownSeconds = 3;
        public const double EndCountdownSeconds = 5;

        // ==========================================
        // TIMER COMPONENTS
        // ==========================================

        private readonly System.Windows.Forms.Timer clock = new();
        private readonly Stopwatch stopwatch = new();

        private TimeSpan lastElapsedTime;
        private bool disposed;

        // ==========================================
        // TIMER VALUES
        // ==========================================

        public double StopwatchSeconds { get; private set; }

        public double QuestionTimeRemaining { get; private set; } = 5;
        public double StartTimeRemaining { get; private set; } = 3;
        public double EndTimeRemaining { get; private set; } = 5;

        public bool IsPaused { get; private set; }
        public bool IsRunning => clock.Enabled;

        // ==========================================
        // TIMER PHASES
        // ==========================================

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

        // ==========================================
        // EVENTS
        // ==========================================

        public event Action? CountdownUpdated;
        public event Action? StartingFinished;
        public event Action? QuestionExpired;
        public event Action? FeedbackFinished;
        public event Action? EndingFinished;

        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public RapidFireTimer()
        {
            clock.Interval = 50;
            clock.Tick += Clock_Tick;
        }

        // ==========================================
        // START TIMER
        // ==========================================

        public void Start()
        {
            if (disposed)
                return;

            stopwatch.Start();
            lastElapsedTime = stopwatch.Elapsed;

            IsPaused = false;
            clock.Start();
        }

        // ==========================================
        // START QUESTION
        // ==========================================

        public void StartQuestion()
        {
            QuestionTimeRemaining = QuestionDurationSeconds;
            phaseTimeRemaining = QuestionDurationSeconds;
            phase = TimerPhase.Playing;
        }

        // ==========================================
        // START FEEDBACK
        // ==========================================

        public void StartFeedback()
        {
            phaseTimeRemaining = FeedbackDurationSeconds;
            phase = TimerPhase.Feedback;
        }

        // ==========================================
        // START ENDING COUNTDOWN
        // ==========================================

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

        // ==========================================
        // PAUSE
        // ==========================================

        public void Pause()
        {
            if (disposed || IsPaused)
                return;

            ProcessElapsed();

            if (disposed)
                return;

            if (phase == TimerPhase.Ending ||
                phase == TimerPhase.Stopped)
                return;

            clock.Stop();
            stopwatch.Stop();
            IsPaused = true;
        }

        // ==========================================
        // RESUME
        // ==========================================

        public void Resume()
        {
            if (disposed || !IsPaused)
                return;

            lastElapsedTime = stopwatch.Elapsed;
            stopwatch.Start();

            IsPaused = false;
            clock.Start();
        }

        // ==========================================
        // CLOCK TICK
        // ==========================================

        private void Clock_Tick(object? sender, EventArgs e)
        {
            ProcessElapsed();
        }

        // ==========================================
        // PROCESS ELAPSED TIME
        // ==========================================

        private void ProcessElapsed()
        {
            if (disposed || IsPaused ||
                phase == TimerPhase.Stopped)
                return;

            TimeSpan now = stopwatch.Elapsed;

            double elapsed = Math.Max(
                0,
                (now - lastElapsedTime).TotalSeconds);

            lastElapsedTime = now;

            // Starting countdown
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

            // Ending countdown
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

            // Stopwatch runs during gameplay and feedback.
            StopwatchSeconds += elapsed;

            phaseTimeRemaining -= elapsed;

            // Question countdown
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
            // Answer feedback
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

        // ==========================================
        // CLEANUP
        // ==========================================

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
