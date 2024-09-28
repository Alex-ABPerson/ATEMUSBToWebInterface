using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATEMUSBToWebInterface
{
    public class MainThreadExecutionBuffer
    {
        readonly Action<Exception> _onError;
        readonly IThreadDispatcher _dispatcher;

        public MainThreadExecutionBuffer(Action<Exception> onError, IThreadDispatcher dispatcher) => (_onError, _dispatcher) = (onError, dispatcher);

        public void StartExecution() { }
        public void QueueFinish() { }
        public void QueueFinish(Action finishAct) => QueueTask(finishAct);

        public void QueueTask(Action act)
        {
            _dispatcher.Queue(() => Run(act));
        }

        public void Run(Action act)
        {
            try
            {
                act();
            }
            catch (Exception ex) { _onError(ex); }
        }
    }
}
