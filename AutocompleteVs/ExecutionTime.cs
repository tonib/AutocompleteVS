using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs
{
    /// <summary>
    /// Logs a execution time
    /// </summary>
    class ExecutionTime : IDisposable
    {
        private readonly string Message;
        private Stopwatch Stopwatch = new Stopwatch();


        public ExecutionTime(string message)
        {
            Stopwatch.Start();
            Message = message;
        }

        public void Dispose()
        {
            Stopwatch.Stop();
            Debug.WriteLine($"{Message}: {Stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
