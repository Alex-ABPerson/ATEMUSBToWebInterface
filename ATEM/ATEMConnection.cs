using ATEMUSBToWebInterface.ATEM.Native;
using BMDSwitcherAPI;

namespace ATEMUSBToWebInterface.ATEM
{
	public interface IATEMConnection : IDisposable
    {
        //SwitcherSpecs InvalidateCurrentSpecs();
        long GetProgram(int mixBlock);
        long GetPreview(int mixBlock);
        void SendProgram(int mixBlock, long val);
        void SendPreview(int mixBlock, long val);
        void SendAuxOut(long val);

		void Cut(int mixBlock);
        void Auto(int mixBlock);
    }

    public class ATEMConnection : IATEMConnection
    {
        readonly IATEMCallbackHandler _callbackHandler;
        readonly INativeATEMSwitcher _nativeSwitcher;
        INativeATEMAuxOut? _auxOut = null!;
        INativeATEMMixBlock[] _nativeBlocks = Array.Empty<INativeATEMMixBlock>();

        public ATEMConnection(ATEMSwitcherConfig config, IATEMSwitcher eventHandler)
        {
            _nativeSwitcher = new WindowsNativeATEMSwitcherDiscovery().Connect(config.IP ?? "");
            _callbackHandler = new ATEMCallbackHandler(eventHandler);
            _callbackHandler.AttachToSwitcher(_nativeSwitcher);

            InvalidateCurrentSpecs(); // ++
		}

        public long GetProgram(int mixBlock) => _nativeBlocks[mixBlock].GetProgramInput();
        public long GetPreview(int mixBlock) => _nativeBlocks[mixBlock].GetPreviewInput();
        public void SendProgram(int mixBlock, long val) => _nativeBlocks[mixBlock].SetProgramInput(val);
        public void SendAuxOut(long val) => _auxOut?.SetInputSource(val);
        public void SendPreview(int mixBlock, long val) => _nativeBlocks[mixBlock].SetPreviewInput(val);
        public void Cut(int mixBlock) => _nativeBlocks[mixBlock].Cut();
		public void Auto(int mixBlock) => _nativeBlocks[mixBlock].Auto();

		public void InvalidateCurrentSpecs()
        {
			// Get raw data
			var rawMixBlocks = GetRawMixBlocks();

            // Get aux out
            using var iter = _nativeSwitcher.CreateInputIterator();
            while (iter.MoveNext(out var input))
            {
                var asAux = input.GetAsAux();
				_auxOut ??= asAux;
                if (asAux == null) input.Dispose();
            }

			// Update internal structure
			DisposeNativeBlocks();
            _nativeBlocks = rawMixBlocks.ToArray();
            _callbackHandler.AttachMixBlocks(_nativeBlocks);
        }

        //IList<RawInputData> GetRawInputData()
        //{
        //    var res = new List<RawInputData>();

        //    using var iter = _nativeSwitcher.CreateInputIterator();
        //    while (iter.MoveNext(out var input))
        //    {
        //        // Only count inputs that are actually assigned to a mix block
        //        var availability = input.GetAvailability();
        //        if ((availability & _BMDSwitcherInputAvailability.bmdSwitcherInputAvailabilityInputCut) == 0) continue;

        //        res.Add(new RawInputData(
        //            input.GetID(),
        //            input.GetShortName(),
        //            (byte)(availability & (
        //                _BMDSwitcherInputAvailability.bmdSwitcherInputAvailabilityMixEffectBlock0 |
        //                _BMDSwitcherInputAvailability.bmdSwitcherInputAvailabilityMixEffectBlock1 |
        //                _BMDSwitcherInputAvailability.bmdSwitcherInputAvailabilityMixEffectBlock2 |
        //                _BMDSwitcherInputAvailability.bmdSwitcherInputAvailabilityMixEffectBlock3
        //            ))
        //        ));

        //        input.Dispose();
        //    }

        //    return res;
        //}

        IList<INativeATEMMixBlock> GetRawMixBlocks()
        {
            var res = new List<INativeATEMMixBlock>();

            using var iter = _nativeSwitcher.CreateMixBlockIterator();
            while (iter.MoveNext(out var input))
                res.Add(input);

            return res;
        }

        //static SwitcherMixBlockFeatures GetFeatures() => new()
        //{
        //    SupportsDirectProgramModification = true,
        //    SupportsDirectPreviewAccess = true,
        //    SupportsCutAction = true,
        //    SupportsAutoAction = false,
        //    SupportsCutBusCutMode = false,
        //    SupportsCutBusAutoMode = false,
        //    SupportsCutBusModeChanging = false,
        //    SupportsCutBusSwitching = false
        //};

        void DisposeNativeBlocks()
        {
            _callbackHandler.DetachMixBlocks(_nativeBlocks);
            for (int i = 0; i < _nativeBlocks.Length; i++)
                _nativeBlocks[i].Dispose();
        }

        public void Dispose()
        {
            DisposeNativeBlocks();
            _callbackHandler.DetachFromSwitcher(_nativeSwitcher);
            _nativeSwitcher.Dispose();
            _auxOut?.Dispose();
        }

        public record struct RawInputData(long Id, string Name, byte MixBlockMask);
    }
}
