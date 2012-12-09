using FC3Editor.Nomad;
using FC3Editor.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
namespace FC3Editor
{
	public class SplashForm : Form
	{
		private IContainer components;
		private Timer pollTimer;
		private bool m_aboutMode;
		private static SplashForm s_form;
		private static bool s_isDone;
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 524288;
				return createParams;
			}
		}
		public bool AboutMode
		{
			get
			{
				return this.m_aboutMode;
			}
			set
			{
				this.m_aboutMode = value;
			}
		}
		public bool IsDoneLoading
		{
			get
			{
				return SplashForm.s_isDone;
			}
			set
			{
				SplashForm.s_isDone = value;
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.components = new Container();
			this.pollTimer = new Timer(this.components);
			base.SuspendLayout();
			this.pollTimer.Enabled = true;
			this.pollTimer.Tick += new EventHandler(this.pollTimer_Tick);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(705, 459);
			base.FormBorderStyle = FormBorderStyle.None;
			base.Name = "SplashForm";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Loading Far Cry 2 Editor...";
			base.Click += new EventHandler(this.SplashForm_Click);
			base.KeyUp += new KeyEventHandler(this.SplashForm_KeyUp);
			base.Load += new EventHandler(this.SplashForm_Load);
			base.ResumeLayout(false);
		}
		public SplashForm()
		{
			this.InitializeComponent();
			base.Icon = Resources.appIcon;
			Bitmap splash = Resources.splash;
			base.Width = splash.Width;
			base.Height = splash.Height;
		}
		private void SplashForm_Load(object sender, EventArgs e)
		{
			if (this.AboutMode)
			{
				this.Text = Localizer.Localize("EDITOR_ABOUT");
			}
			Bitmap splash = Resources.splash;
			string s = "";
			string s2 = "";
			string text = "";
			string text2 = Path.GetDirectoryName(Application.ExecutablePath);
			text2 += "\\FC2Init.ini";
			Win32.GetPrivateProfileStringW("FC2_INIT", "language", "english", out text, text2);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(Resources.SplashLocalization);
			XmlElement documentElement = xmlDocument.DocumentElement;
			XmlNodeList elementsByTagName = documentElement.GetElementsByTagName(text);
			foreach (XmlNode xmlNode in elementsByTagName)
			{
				if (xmlNode.Name == text)
				{
					XmlElement xmlElement = (XmlElement)xmlNode;
					foreach (XmlNode xmlNode2 in xmlElement.ChildNodes)
					{
						string value = xmlNode2.Attributes["enum"].Value;
						string value2 = xmlNode2.Attributes["value"].Value;
						string a;
						if ((a = value) != null)
						{
							if (!(a == "LOADING_GAMEFILE"))
							{
								if (!(a == "EDITOR_NAME"))
								{
									if (!(a == "TEXT_LEGAL"))
									{
										if (a == "LOADING_TITLE")
										{
											this.Text = value2;
										}
									}
									else
									{
										s2 = value2;
									}
								}
							}
							else
							{
								s = value2;
							}
						}
					}
				}
			}
			string s3 = "MAP EDITOR";
			using (Graphics graphics = Graphics.FromImage(splash))
			{
				graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
				Font font = new Font("Verdana", 12f, FontStyle.Bold);
				Font font2 = new Font("Tahoma", 10f, FontStyle.Bold);
				Font font3 = new Font("Tahoma", 8f);
				SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, 225, 234, 239));
				SolidBrush solidBrush2 = new SolidBrush(Color.FromArgb(255, 181, 59, 59));
				StringFormat stringFormat = new StringFormat();
				stringFormat.Alignment = StringAlignment.Center;
				graphics.DrawString(s3, font, solidBrush, new RectangleF(200f, 383f, 625f, 0f), stringFormat);
				graphics.DrawString(s2, font3, solidBrush, new RectangleF(200f, 405f, 625f, 0f));
				if (!this.AboutMode)
				{
					graphics.DrawString(s, font2, solidBrush2, new RectangleF(200f, 435f, 625f, 0f));
				}
				solidBrush.Dispose();
				solidBrush2.Dispose();
				font.Dispose();
				font2.Dispose();
				font3.Dispose();
			}
			Win32.UpdateLayeredWindowHelper(this, splash);
		}
		private void SplashForm_Click(object sender, EventArgs e)
		{
			if (this.AboutMode)
			{
				base.Close();
			}
		}
		private void SplashForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.AboutMode)
			{
				base.Close();
			}
		}
		private void pollTimer_Tick(object sender, EventArgs e)
		{
		}
		private static void _Start()
		{
			SplashForm.s_form = new SplashForm();
			SplashForm.s_form.Show();
		}
		public static void Start()
		{
			SplashForm._Start();
		}
		public static void Stop()
		{
			SplashForm.s_form.Dispose();
		}
	}
}
