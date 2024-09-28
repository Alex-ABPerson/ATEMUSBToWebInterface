using BMDSwitcherAPI;

namespace ATEMUSBToWebInterface.ATEM.Native
{
	public interface INativeATEMSwitcherDiscovery
    {
        INativeATEMSwitcher Connect(string address);
    }

    public interface INativeATEMSwitcher : IDisposable
    {
        INativeATEMBlockIterator CreateMixBlockIterator();
        INativeATEMInputIterator CreateInputIterator();
        void AddCallback(INativeATEMSwitcherCallbackHandler callback);
        void ClearCallback();
    }

    public interface INativeATEMBlockIterator : IDisposable
    {
        bool MoveNext(out INativeATEMMixBlock item);
    }

    public interface INativeATEMInputIterator : IDisposable
    {
        bool MoveNext(out INativeATEMInput item);
    }

    public interface INativeATEMMixBlock : IDisposable
    {
        void AddCallback(INativeATEMBlockCallbackHandler handler);
        void ClearCallback();
        long GetProgramInput();
        long GetPreviewInput();
        void SetProgramInput(long val);
        void SetPreviewInput(long val);
        void Cut();
        void Auto();
    }

    public interface INativeATEMInput : IDisposable
    {
        long GetID();
        string GetShortName();
        INativeATEMAuxOut? GetAsAux();
        _BMDSwitcherInputAvailability GetAvailability();
    }

	public interface INativeATEMAuxOut : IDisposable
    {
        void SetInputSource(long val);
    }


	public interface INativeATEMSwitcherCallbackHandler
    {
        void Notify(_BMDSwitcherEventType type, _BMDSwitcherVideoMode videoMode);
    }

    public interface INativeATEMBlockCallbackHandler
    {
        void Notify(_BMDSwitcherMixEffectBlockEventType eventType);
    }
}