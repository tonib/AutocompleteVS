using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs.Logging
{
    /// <summary>
    /// Logs a execution time
    /// </summary>
    class ExecutionTime : IDisposable
    {
        private readonly string Message;
        private readonly bool WriteTimeOnDispose;
        private Stopwatch Stopwatch = new Stopwatch();

        public ExecutionTime(string message, bool writeTimeOnDispose = true)
        {
            Stopwatch.Start();
            Message = message;
            WriteTimeOnDispose = writeTimeOnDispose;
        }

        /// <summary>
        /// Logs the execution time
        /// </summary>
        public void Dispose()
        {
            if(Stopwatch.IsRunning)
                Stopwatch.Stop();

            if(WriteTimeOnDispose)
                WriteElapsedTime();
        }

        /// <summary>
        /// Logs the execution time. This can be called only in UI thread
        /// </summary>
        public void WriteElapsedTime()
        {
            if (Stopwatch.IsRunning)
                Stopwatch.Stop();
            OutputPaneHandler.Instance.Log($"{Message}: {Stopwatch.ElapsedMilliseconds} ms");
        }

        /// <summary>
        /// Logs a execution time asynchronously
        /// </summary>
        async public Task WriteElapsedTimeAsync()
        {
            if (Stopwatch.IsRunning)
                Stopwatch.Stop();

            await OutputPaneHandler.Instance.LogAsync($"{Message}: {Stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
