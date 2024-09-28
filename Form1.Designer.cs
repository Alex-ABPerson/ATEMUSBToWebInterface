namespace ATEMUSBToWebInterface
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			label1 = new Label();
			udpBox = new TextBox();
			label2 = new Label();
			ipBox = new TextBox();
			label3 = new Label();
			connectBtn = new Button();
			lblProgram = new Label();
			lblPreview = new Label();
			SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
			label1.ForeColor = Color.White;
			label1.Location = new Point(12, 9);
			label1.Name = "label1";
			label1.Size = new Size(271, 30);
			label1.TabIndex = 0;
			label1.Text = "ATEM USB to Web Interface";
			// 
			// udpBox
			// 
			udpBox.Enabled = false;
			udpBox.Location = new Point(232, 70);
			udpBox.Name = "udpBox";
			udpBox.Size = new Size(41, 23);
			udpBox.TabIndex = 1;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
			label2.ForeColor = Color.White;
			label2.Location = new Point(232, 46);
			label2.Name = "label2";
			label2.Size = new Size(41, 21);
			label2.TabIndex = 2;
			label2.Text = "UDP";
			// 
			// ipBox
			// 
			ipBox.Enabled = false;
			ipBox.Location = new Point(14, 70);
			ipBox.Name = "ipBox";
			ipBox.Size = new Size(212, 23);
			ipBox.TabIndex = 3;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
			label3.ForeColor = Color.White;
			label3.Location = new Point(13, 46);
			label3.Name = "label3";
			label3.Size = new Size(83, 21);
			label3.TabIndex = 4;
			label3.Text = "IP Address";
			// 
			// connectBtn
			// 
			connectBtn.Location = new Point(14, 99);
			connectBtn.Name = "connectBtn";
			connectBtn.Size = new Size(259, 23);
			connectBtn.TabIndex = 5;
			connectBtn.Text = "Connect to ATEM";
			connectBtn.UseVisualStyleBackColor = true;
			connectBtn.Click += connectBtn_Click;
			// 
			// lblProgram
			// 
			lblProgram.AutoSize = true;
			lblProgram.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
			lblProgram.ForeColor = Color.White;
			lblProgram.Location = new Point(12, 135);
			lblProgram.Name = "lblProgram";
			lblProgram.Size = new Size(78, 21);
			lblProgram.TabIndex = 6;
			lblProgram.Text = "Program: ";
			// 
			// lblPreview
			// 
			lblPreview.AutoSize = true;
			lblPreview.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
			lblPreview.ForeColor = Color.White;
			lblPreview.Location = new Point(131, 135);
			lblPreview.Name = "lblPreview";
			lblPreview.Size = new Size(72, 21);
			lblPreview.TabIndex = 7;
			lblPreview.Text = "Preview: ";
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.FromArgb(32, 32, 32);
			ClientSize = new Size(301, 165);
			Controls.Add(lblPreview);
			Controls.Add(lblProgram);
			Controls.Add(connectBtn);
			Controls.Add(label3);
			Controls.Add(ipBox);
			Controls.Add(label2);
			Controls.Add(udpBox);
			Controls.Add(label1);
			Name = "Form1";
			Text = "ATEM USB to Web Interface";
			Load += Form1_Load;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label label1;
		private TextBox udpBox;
		private Label label2;
		private TextBox ipBox;
		private Label label3;
		private Button connectBtn;
		private Label lblProgram;
		private Label lblPreview;
	}
}