using ATEMUSBToWebInterface.ATEM;
using ATEMUSBToWebInterface.Server;
using System.Net;

namespace ATEMUSBToWebInterface
{
	public interface IThreadDispatcher
	{
		void Queue(Action act);
		Task Yield();
	}

	public interface IATEMEventHandler
	{
		void OnPreviewRefresh(int val);
		void OnProgramRefresh(int val);
	}

	public partial class Form1 : Form, IServerEventHandler, IATEMEventHandler
	{
		public static Form1 Instance = null!;
		public static IThreadDispatcher Dispatcher = null!;
		public ATEMSwitcher Switcher = null!;
		public UDPServer Server;

		public Form1()
		{
			InitializeComponent();
			Instance = this;
			Dispatcher = new FormDispatcher(this);

			Switcher = new(new ATEMSwitcherConfig(null), this, Dispatcher);
			Server = new(new IPEndPoint(IPAddress.Loopback, 907), this);
			ipBox.Text = IPAddress.Loopback.ToString();
			udpBox.Text = "907";
		}

		public static void ErrorOccurred(string? msg)
		{
			MessageBox.Show(msg);
		}

		class FormDispatcher : IThreadDispatcher
		{
			readonly Form _form;
			public FormDispatcher(Form form) => _form = form;
			public void Queue(Action act) => _form.Invoke(act);
			public async Task Yield()
			{
				Application.DoEvents();
				await Task.Yield();
			}
		}

		private void connectBtn_Click(object sender, EventArgs e)
		{
			if (Switcher.IsConnected)
				Switcher.Disconnect();
			else
				Switcher.Connect();

			connectBtn.Text = Switcher.IsConnected ? "Disconnect" : "Connect";
		}

		public void SetProgram(int val) => Switcher.SendProgramValue(0, val);
		public void SetPreview(int val) => Switcher.SendPreviewValue(0, val);
		public void SetAuxOutput(int val) => Switcher.SendAuxOut(val);
		public void Cut() => Switcher.Cut(0);
		public void Auto() => Switcher.Auto(0);

		private async void Form1_Load(object sender, EventArgs e)
		{
			try
			{
				await Server.Run();
			}
			catch (Exception ex)
			{
				Dispatcher.Queue(() => throw ex);
			}
		}

		public void OnPreviewRefresh(int val)
		{
			lblPreview.Text = "Preview: " + val;
		}

		public void OnProgramRefresh(int val)
		{
			lblProgram.Text = "Program: " + val;
		}
	}
}