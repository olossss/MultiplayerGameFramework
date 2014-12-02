using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiplayerFramework.Common
{
    class AsyncWorker
    {
        public Thread _workerThread { get; set; }
        protected volatile bool _isRunning;

        public AsyncWorker() {
        }
        
        public void Start() {
            _isRunning = true;
            _workerThread.Start();
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Join(int timeout = 0)
        {
            if (timeout > 0)
                _workerThread.Join(timeout);
            else
                _workerThread.Join();
        }

        public bool isAlive() {
            return _workerThread.IsAlive;
        }

        public bool isRunning()
        {
            return _isRunning;
        }
    }
}
