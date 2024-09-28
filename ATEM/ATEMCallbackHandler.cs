using ATEMUSBToWebInterface.ATEM.Native;
using BMDSwitcherAPI;
using System.Diagnostics;

namespace ATEMUSBToWebInterface.ATEM
{
	public interface IATEMCallbackHandler
    {
        void AttachToSwitcher(INativeATEMSwitcher switcher);
        void DetachFromSwitcher(INativeATEMSwitcher switcher);
        void AttachMixBlocks(INativeATEMMixBlock[] mixBlocks);
        void DetachMixBlocks(INativeATEMMixBlock[] mixBlocks);
    }

    internal class ATEMCallbackHandler : IATEMCallbackHandler, INativeATEMSwitcherCallbackHandler
    {
        readonly IATEMSwitcher _topSwitcher;
        MixEffectBlockHandler[] _handlers = Array.Empty<MixEffectBlockHandler>();

        public ATEMCallbackHandler(IATEMSwitcher switcher) => _topSwitcher = switcher;

        public void Notify(_BMDSwitcherEventType type, _BMDSwitcherVideoMode videoMode)
        {
            if (type == _BMDSwitcherEventType.bmdSwitcherEventTypeDisconnected)
                _topSwitcher.ProcessError(new UnexpectedSwitcherDisconnectionException());
        }

        public void AttachToSwitcher(INativeATEMSwitcher switcher) => switcher.AddCallback(this);
        public void DetachFromSwitcher(INativeATEMSwitcher switcher) => switcher.ClearCallback();

        public void DetachMixBlocks(INativeATEMMixBlock[] mixBlocks)
        {
            for (int i = 0; i < mixBlocks.Length; i++) mixBlocks[i].ClearCallback();
            _handlers = Array.Empty<MixEffectBlockHandler>();
        }

        public void AttachMixBlocks(INativeATEMMixBlock[] mixBlocks)
        {
            Debug.Assert(_handlers.Length == 0); // Old mix-blocks should have been detached

            _handlers = new MixEffectBlockHandler[mixBlocks.Length];
            for (int i = 0; i < mixBlocks.Length; i++)
            {
                _handlers[i] = new MixEffectBlockHandler(_topSwitcher, i);
                mixBlocks[i].AddCallback(_handlers[i]);
            }
        }

        class MixEffectBlockHandler : INativeATEMBlockCallbackHandler
        {
            readonly IRawSwitcher _switcher;
            readonly int _index;

            public MixEffectBlockHandler(IRawSwitcher handler, int index) => (_switcher, _index) = (handler, index);

            public void Notify(_BMDSwitcherMixEffectBlockEventType eventType)
            {
                Form1.Dispatcher.Queue(() =>
                {
                    if (eventType == _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypeProgramInputChanged)
                        _switcher.RefreshProgram(_index);
                    else if (eventType == _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypePreviewInputChanged)
                        _switcher.RefreshPreview(_index);
                });
            }
        }
    }
}
