namespace ATEMUSBToWebInterface
{
	public class SwitcherErrorException : Exception
    {
        public SwitcherErrorException(string message) : base(message) { }
    }

    public class UnexpectedSwitcherDisconnectionException : SwitcherErrorException
    {
        public UnexpectedSwitcherDisconnectionException() : base("Switcher was unexpectedly disconnected.") { }
    }
}
