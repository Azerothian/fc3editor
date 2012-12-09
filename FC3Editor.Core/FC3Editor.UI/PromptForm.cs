using FC3Editor.Nomad;
using FC3Editor.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class PromptForm : Form
	{
		public delegate bool ValidationDelegate(string input, out string message);
		private PromptForm.ValidationDelegate m_validation;
		private IContainer components;
		private TableLayoutPanel tableLayoutPanel1;
		private Panel panel1;
		private NomadButton okButton;
		private NomadButton cancelButton;
		private TextBox promptTextBox;
		private Label promptLabel;
		public string Prompt
		{
			get
			{
				return this.promptLabel.Text;
			}
			set
			{
				this.promptLabel.Text = value;
			}
		}
		public string Input
		{
			get
			{
				return this.promptTextBox.Text;
			}
			set
			{
				if (value.Length > this.promptTextBox.MaxLength)
				{
					value = value.Substring(0, this.promptTextBox.MaxLength);
				}
				this.promptTextBox.Text = value;
			}
		}
		public int MaxLength
		{
			get
			{
				return this.promptTextBox.MaxLength;
			}
			set
			{
				this.promptTextBox.MaxLength = value;
				if (this.promptTextBox.Text.Length > value)
				{
					this.promptTextBox.Text = this.promptTextBox.Text.Substring(0, value);
				}
			}
		}
		public PromptForm.ValidationDelegate Validation
		{
			get
			{
				return this.m_validation;
			}
			set
			{
				this.m_validation = value;
			}
		}
		public PromptForm()
		{
			this.InitializeComponent();
			base.Icon = Resources.appIcon;
		}
		public PromptForm(string prompt) : this()
		{
			this.Prompt = prompt;
		}
		public PromptForm(string prompt, string title) : this()
		{
			this.Prompt = prompt;
			this.Text = title;
		}
		private void okButton_Click(object sender, EventArgs e)
		{
			string message;
			if (this.m_validation != null && !this.m_validation(this.promptTextBox.Text, out message))
			{
				LocalizedMessageBox.Show(this, message, "", Localizer.Localize("Generic", "GENERIC_OK"), null, null, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
				base.DialogResult = DialogResult.None;
				return;
			}
			base.DialogResult = DialogResult.OK;
		}
		public static PromptForm.ValidationDelegate GetIntegerValidation(int min, int max)
		{
			return delegate(string input, out string message)
			{
				message = null;
				int num;
				if (!int.TryParse(input, out num))
				{
					message = Localizer.Localize("PROMPT_NOT_A_NUMBER");
					return false;
				}
				if (num < min || num > max)
				{
					message = string.Format(Localizer.Localize("PROMPT_NUMBER_NOT_IN_RANGE"), min, max);
					return false;
				}
				return true;
			}
			;
		}
		public static PromptForm.ValidationDelegate GetFloatValidation(float min, float max)
		{
			return delegate(string input, out string message)
			{
				message = null;
				float num;
				if (!float.TryParse(input, out num))
				{
					message = Localizer.Localize("PROMPT_NOT_A_NUMBER");
					return false;
				}
				if (num < min || num > max)
				{
					message = string.Format(Localizer.Localize("PROMPT_NUMBER_NOT_IN_RANGE"), min, max);
					return false;
				}
				return true;
			}
			;
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
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.panel1 = new Panel();
			this.promptTextBox = new TextBox();
			this.promptLabel = new Label();
			this.okButton = new NomadButton();
			this.cancelButton = new NomadButton();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.promptTextBox, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.promptLabel, 0, 0);
			this.tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
			this.tableLayoutPanel1.Location = new Point(1, 2);
			this.tableLayoutPanel1.MaximumSize = new Size(375, 0);
			this.tableLayoutPanel1.MinimumSize = new Size(375, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
			this.tableLayoutPanel1.Size = new Size(375, 80);
			this.tableLayoutPanel1.TabIndex = 3;
			this.panel1.Controls.Add(this.okButton);
			this.panel1.Controls.Add(this.cancelButton);
			this.panel1.Dock = DockStyle.Fill;
			this.panel1.Location = new Point(3, 48);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new Padding(0, 3, 0, 3);
			this.panel1.Size = new Size(369, 29);
			this.panel1.TabIndex = 3;
			this.promptTextBox.Dock = DockStyle.Fill;
			this.promptTextBox.Location = new Point(3, 22);
			this.promptTextBox.Name = "promptTextBox";
			this.promptTextBox.Size = new Size(369, 20);
			this.promptTextBox.TabIndex = 2;
			this.promptLabel.AutoSize = true;
			this.promptLabel.Location = new Point(3, 0);
			this.promptLabel.Name = "promptLabel";
			this.promptLabel.Padding = new Padding(0, 3, 0, 3);
			this.promptLabel.Size = new Size(40, 19);
			this.promptLabel.TabIndex = 1;
			this.promptLabel.Text = "Prompt";
			this.okButton.DialogResult = DialogResult.OK;
			this.okButton.Dock = DockStyle.Right;
			this.okButton.Location = new Point(219, 3);
			this.okButton.Name = "okButton";
			this.okButton.Size = new Size(75, 23);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "&OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new EventHandler(this.okButton_Click);
			this.cancelButton.DialogResult = DialogResult.Cancel;
			this.cancelButton.Dock = DockStyle.Right;
			this.cancelButton.Location = new Point(294, 3);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new Size(75, 23);
			this.cancelButton.TabIndex = 0;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			base.AcceptButton = this.okButton;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoSize = true;
			base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			base.CancelButton = this.cancelButton;
			base.ClientSize = new Size(416, 141);
			base.Controls.Add(this.tableLayoutPanel1);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "PromptForm";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Prompt";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
