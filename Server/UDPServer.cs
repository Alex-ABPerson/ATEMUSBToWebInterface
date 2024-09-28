using ATEMUSBToWebInterface.ATEM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ATEMUSBToWebInterface.Server
{
	public interface IServerEventHandler
	{
		void SetProgram(int val);
		void SetPreview(int val);
		void SetAuxOutput(int val);
		void Cut();
		void Auto();
	}

	public class UDPServer
	{
		readonly UdpClient _listener;
		readonly IServerEventHandler _eventHandler;
		readonly CancellationTokenSource _cancel = new();

		public UDPServer(IPEndPoint ip, IServerEventHandler eventHandler)
		{
			_eventHandler = eventHandler;
			_listener = new UdpClient(ip);
		}

		public async Task Run()
		{
			while (true)
			{
				var res = await _listener.ReceiveAsync(_cancel.Token);
				if (_cancel.Token.IsCancellationRequested) break;

				RunRequest(res.Buffer);
			}
		}

		public void RunRequest(byte[] res)
		{
			int i = 0;
			while (i < res.Length)
			{
				switch ((char)Read())
				{
					case 'P':
						DoDigit(_eventHandler.SetProgram);
						break;
					case 'E':
						DoDigit(_eventHandler.SetPreview);
						break;
					case 'O':
						DoDigit(_eventHandler.SetAuxOutput);
						break;
					case 'C':
						_eventHandler.Cut();
						break;
					case 'A':
						_eventHandler.Auto();
						break;
				}
			}

			byte Read() => i >= res.Length ? (byte)0 : res[i++];
			byte Peek() => i >= res.Length ? (byte)0 : res[i];

			void DoDigit(Action<int> act)
			{
				// Get the length of the number
				int startPos = i;
				while (Peek() >= '0' && Peek() <= '9') i++;

				int len = i - startPos;
				if (len == 0) return;

				act(int.Parse(new ReadOnlySpan<byte>(res, startPos, len)));
			}
		}

		public void Dispose()
		{
			_cancel.Cancel();
		}
	}
}
