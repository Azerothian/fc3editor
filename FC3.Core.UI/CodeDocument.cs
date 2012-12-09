using FC3Editor.Nomad;
using FC3Editor.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TD.SandDock;
namespace FC3Editor.UI
{
	internal class CodeDocument : UserTabbedDocument
	{
		private class ImageIcon : NomadCodeBox.IIcon, IDisposable
		{
			private ImageMap m_image;
			public ImageMap Image
			{
				get
				{
					return this.m_image;
				}
			}
			public ImageIcon(ImageMap image)
			{
				this.m_image = image;
			}
			public void Draw(Graphics g, Rectangle rect)
			{
				g.DrawImage(Resources.InsertPictureHS, rect);
			}
			public void Dispose()
			{
				this.m_image.Dispose();
			}
		}
		private IContainer components;
		private NomadCodeBox textBox1;
		private SaveFileDialog saveFileDialog;
		private string m_fileName;
		public string FileName
		{
			get
			{
				return this.m_fileName;
			}
			set
			{
				this.m_fileName = value;
				this.Text = Path.GetFileName(value);
			}
		}
		public NomadCodeBox Content
		{
			get
			{
				return this.textBox1;
			}
		}
		protected override void Dispose(bool disposing)
		{
			this.DisposeInternal();
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.saveFileDialog = new SaveFileDialog();
			this.textBox1 = new NomadCodeBox();
			base.SuspendLayout();
			this.saveFileDialog.DefaultExt = "lua";
			this.saveFileDialog.Filter = "Far Cry 2 script files (*.lua)|*.lua|All files (*.*)|*.*";
			this.saveFileDialog.Title = "Save script file";
			this.textBox1.BackColor = SystemColors.Window;
			this.textBox1.Cursor = Cursors.IBeam;
			this.textBox1.Dock = DockStyle.Fill;
			this.textBox1.Font = new Font("Lucida Console", 10f);
			this.textBox1.Location = new Point(0, 0);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(550, 400);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "\r\n";
			this.textBox1.CaretPositionChanged += new EventHandler(this.textBox1_CaretPositionChanged);
			base.Controls.Add(this.textBox1);
			base.Name = "CodeDocument";
			this.Text = "Untitled";
			base.ResumeLayout(false);
		}
		public CodeDocument()
		{
			this.InitializeComponent();
		}
		private void DisposeInternal()
		{
			this.ClearImages();
		}
		public void LoadFile(string fileName)
		{
			this.FileName = fileName;
			this.Content.Text = File.ReadAllText(fileName);
		}
		public void SaveFile()
		{
			if (this.FileName == null)
			{
				if (this.saveFileDialog.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				this.FileName = this.saveFileDialog.FileName;
			}
			File.WriteAllText(this.FileName, this.textBox1.Text);
		}
		public void Run()
		{
			this.ClearImages();
			Wilderness.RunScriptBuffer(this.Content.Text, new Wilderness.MapCallback(this.mapCallback), new Wilderness.ErrorCallback(this.errorCallback));
		}
		private void mapCallback(int line, IntPtr pMap)
		{
			ImageMapEngine imageMapEngine = new ImageMapEngine(pMap);
			ImageMap image = imageMapEngine.GetImage();
			this.Content.SetIcon(line - 1, new CodeDocument.ImageIcon(image));
		}
		private void errorCallback(int line, string errorMessage)
		{
			MessageBox.Show(this, "An error has occurred while executing the wilderness script.\n\n" + errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			if (line > 0)
			{
				this.textBox1.CaretPosition = new NomadTextBox.Position(line, 1);
			}
		}
		public ImageMap GetLineImage(int line)
		{
			CodeDocument.ImageIcon imageIcon = (CodeDocument.ImageIcon)this.Content.GetIcon(line);
			if (imageIcon == null)
			{
				return null;
			}
			return imageIcon.Image;
		}
		private void ClearImages()
		{
			this.Content.ClearIcons();
		}
		private void textBox1_CaretPositionChanged(object sender, EventArgs e)
		{
			CodeEditor.Instance.UpdateCaretPosition(this);
		}
	}
}
