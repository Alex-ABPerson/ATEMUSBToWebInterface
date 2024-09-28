

namespace ATEMUSBToWebInterface.ATEM
{
    public interface IATEMSwitcher : IRawSwitcher, IErrorHandlingTarget
    {
    }

    public class ATEMSwitcher : RawSwitcher, IATEMSwitcher
    {
        readonly ATEMSwitcherConfig _config;
        readonly IThreadDispatcher _dispatcher;
        readonly IATEMEventHandler _eventHandler;
        readonly SameThreadExecutionBuffer _buffer;

        public bool IsConnected => _connection != null;

        IATEMConnection? _connection; // Only access with the background thread!

        public ATEMSwitcher(ATEMSwitcherConfig config, IATEMEventHandler eventHandler, IThreadDispatcher dispatcher)
        {
            _config = config;
            _eventHandler = eventHandler;
            _dispatcher = dispatcher;
            _buffer = new(ProcessError);
        }

        public override void Connect()
        {
            _buffer.QueueTask(() =>
            {
                _connection = new ATEMConnection(_config, this);
                //_dispatcher.Queue(() => _eventHandler?.OnConnectionStateChange(true));
            });
		}

        public override void Disconnect()
        {
            _buffer.QueueTask(() =>
            {
                if (_connection == null) throw new UnexpectedSwitcherDisconnectionException();

                _connection.Dispose();
                _connection = null;
                //_dispatcher.Queue(() => _eventHandler?.OnConnectionStateChange(false));
            });
        }

        //public override void RefreshConnectionStatus() => _eventHandler?.OnConnectionStateChange(_connection != null);

        //public override void RefreshSpecs()
        //{
        //    _buffer.QueueTask(() =>
        //    {
        //        if (_connection == null) throw new UnexpectedSwitcherDisconnectionException();

        //        var newSpecs = _connection.InvalidateCurrentSpecs();
        //        _dispatcher.Queue(() => _eventHandler?.OnSpecsChange(newSpecs));
        //    });
        //}

        public override void RefreshProgram(int mixBlock)
        {
            _buffer.QueueTask(() =>
            {
                if (_connection == null) throw new UnexpectedSwitcherDisconnectionException();

                long val = _connection.GetProgram(mixBlock);
                _dispatcher.Queue(() => _eventHandler?.OnProgramRefresh((int)val));
            });
        }

        public override void RefreshPreview(int mixBlock)
        {
            _buffer.QueueTask(() =>
            {
                if (_connection == null) throw new UnexpectedSwitcherDisconnectionException();

                long val = _connection.GetPreview(mixBlock);
                _dispatcher.Queue(() => _eventHandler?.OnPreviewRefresh((int)val));
            });
        }

        public override void SendProgramValue(int mixBlock, int id)
        {
            _buffer.QueueTask(() =>
            {
                if (_connection == null) throw new UnexpectedSwitcherDisconnectionException();
                _connection.SendProgram(mixBlock, id);
            });
        }

		public void SendAuxOut(int id)
		{
			_buffer.QueueTask(() =>
			{
				if (_connection == null) throw new UnexpectedSwitcherDisconnectionException();
				_connection.SendAuxOut(id);
			});
		}

		public override void SendPreviewValue(int mixBlock, int id)
        {
            _buffer.QueueTask(() =>
            {
                if (_connection == null) throw new UnexpectedSwitcherDisconnectionException();
                _connection.SendPreview(mixBlock, id);
            });
        }

        public override void Cut(int mixBlock)
        {
            _buffer.QueueTask(() =>
            {
                if (_connection == null) throw new UnexpectedSwitcherDisconnectionException();
                _connection.Cut(mixBlock);
            });
        }

		public void Auto(int mixBlock)
		{
			_buffer.QueueTask(() =>
			{
				if (_connection == null) throw new UnexpectedSwitcherDisconnectionException();
				_connection.Auto(mixBlock);
			});
		}

		public void ProcessError(Exception ex) => _dispatcher.Queue(() => Form1.ErrorOccurred(ex.Message));

        public override void Dispose()
        {
            if (_connection != null) Disconnect();
        }
    }
}