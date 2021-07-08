using System;
using System.Diagnostics;

namespace Notifications.Wpf.Classes
{
    /// <summary>
    /// Timer for calculate long operation
    /// </summary>
    public class OperationTimer : IDisposable
    {
        /// <summary> Base waiting message, will show when calculation </summary>
        public string BaseWaitingMessage { get; set; } = "Calculation time";
        /// <summary> timer </summary>
        private Stopwatch Watch = new ();

        /// <summary> Last calculate value </summary>
        private double LastSpan;
        /// <summary> Start timer </summary>
        public void Start()
        {
            Watch.Start();
            LastSpan = 0;
            IsRunning = true;
        }
        /// <summary> Stop timer </summary>
        public void Stop()
        {
            Watch.Stop();
            LastSpan = 0;
            IsRunning = false;
        }
        /// <summary> Reset timer </summary>
        public void Reset()
        {
            Watch.Reset();
            LastSpan = 0;
            IsRunning = true;
        }
        /// <summary> Restart timer </summary>
        public void Restart()
        {
            Watch.Restart();
            LastSpan = 0;
            IsRunning = true;
        }
        /// <summary> Timer status </summary>
        private bool IsRunning { get; set; }

        public OperationTimer(string Waiting_Message) => BaseWaitingMessage = Waiting_Message; 
        public OperationTimer() { }
        /// <summary> Calculate time </summary>
        /// <param name="current_index">current index</param>
        /// <param name="total_index">total index</param>
        /// <returns></returns>
        public TimeSpan? CalculateOperationTime(int current_index, int total_index) => CalculateOperationTime((double)current_index, (double)total_index);
        /// <summary> Get time in string format </summary>
        /// <param name="current_index">current index</param>
        /// <param name="total_index">total index</param>
        /// <returns></returns>
        public string GetStringTime(int current_index, int total_index) => GetStringTime((double)current_index, (double)total_index);
        /// <summary> Calculate time </summary>
        /// <param name="current_index">current index</param>
        /// <param name="total_index">total index</param>
        /// <returns></returns>
        public TimeSpan? CalculateOperationTime(double current_index, double total_index)
        {
            if(!IsRunning)
                Start();

            if (current_index == 0)
                return null;

            var mid_operation_time = Watch.Elapsed.TotalSeconds / current_index;

            if (LastSpan == 0)
            {
                var remained = (total_index - current_index) * mid_operation_time;
                LastSpan = remained;
                return TimeSpan.FromSeconds(remained);

            }
            else
            {
                var remained = (total_index - current_index) * mid_operation_time;
                var mid_value = (remained + LastSpan) / 2;
                LastSpan = mid_value;
                return TimeSpan.FromSeconds(mid_value);

            }
        }


        /// <summary>
        /// Get time in string format
        /// </summary>
        /// <param name="current_index">current index</param>
        /// <param name="total_index">total index</param>
        /// <returns></returns>
        public string GetStringTime(double current_index, double total_index)
        {
            var time = CalculateOperationTime(current_index, total_index);
            return time is null ? BaseWaitingMessage ?? "" :
                time.Value.Days > 0 ? time.Value.ToString(@"d\.hh\:mm\:ss") :
                time.Value.Hours > 0 ? time.Value.ToString(@"hh\:mm\:ss") :
                time.Value.Minutes > 0 ? time.Value.ToString(@"mm\:ss") : $"{Math.Round(time.Value.TotalSeconds, 0)} c";
        }
        #region IDisposable

        /// <inheritdoc />
        public void Dispose() => Watch = null;

        #endregion
    }

}
