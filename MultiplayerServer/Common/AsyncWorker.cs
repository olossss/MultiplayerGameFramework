using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiplayerServer.Common
{
    class AsyncWorker
    {
        public Thread _workerThread { get; set; }

        public AsyncWorker() { 
            // Some Delagate
        }
        
        public void Start() {
            _workerThread.Start();
        }

        public void Stop() {
            _workerThread.Abort();
            _workerThread.Join();
        }
    }
}
