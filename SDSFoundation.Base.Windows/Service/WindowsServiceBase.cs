using log4net;
using System;
using System.Threading;
using System.Threading.Tasks;
using Topshelf.Runtime;

namespace SDSFoundation.Base.Windows.NetStandard.Service
{
    public abstract class WindowsServiceBase<TService> where TService : class
    {
        public static readonly ILog Log = log4net.LogManager.GetLogger(typeof(TService));
        public Action BeforeInitializeWorkCallback { get; set; }

        public HostSettings TopShelfSettings { get; set; }
        private SemaphoreSlim _semaphoreToRequestStop;
        private Thread _thread;

        public WindowsServiceBase(Action beforeInitializeWorkCallback)
        {
            this.BeforeInitializeWorkCallback = beforeInitializeWorkCallback;
        }

        public abstract void DoWork();

        public bool Start()
        {
            Task.Delay(0).ContinueWith(x => {
                _semaphoreToRequestStop = new SemaphoreSlim(0);
                _thread = new Thread(InitializeWork);
                _thread.IsBackground = true;  // This line will prevent thread from working after service stop

                _thread.Start();
            });

            return true;
        }

        public bool Stop()
        {
            _semaphoreToRequestStop.Release();
            _thread.Join();

            return true;
        }

        private void InitializeWork()
        {

            if (BeforeInitializeWorkCallback != null)
            {
                BeforeInitializeWorkCallback?.Invoke();
            }
            while (true)
            {
                Log.Info("Doing work...");
                DoWork();
                if (_semaphoreToRequestStop != null && _semaphoreToRequestStop.Wait(500))
                {
                    Log.Info("Stopped");
                    break;
                }
            }
        }



    }
}
