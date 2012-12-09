using FC3Editor.Nomad;
using FC3Editor.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	public class InventoryList : ListView
	{
		private const int smallSize = 64;
		private const int mediumSize = 96;
		private const int largeSize = 128;
		private Inventory.Entry[] m_entries;
		private Dictionary<Inventory.Entry, ListViewItem> m_listItems = new Dictionary<Inventory.Entry, ListViewItem>();
		private Dictionary<Inventory.Entry, Image> m_listImages = new Dictionary<Inventory.Entry, Image>();
		private Thread m_imageThread;
		private List<KeyValuePair<Inventory.Entry, MemoryStream>> m_pendingImages = new List<KeyValuePair<Inventory.Entry, MemoryStream>>();
		private List<KeyValuePair<Inventory.Entry, Image>> m_readyImages = new List<KeyValuePair<Inventory.Entry, Image>>();
		private int m_pendingIndex;
		private int m_pendingIndexBegin;
		private int m_pendingIndexMid;
		private int m_pendingIndexEnd;
		private int m_pendingPass;
		private bool m_slotMode;
		private int m_imageSize;
		private IContainer components;
		private System.Windows.Forms.Timer flushTimer;
		private ContextMenuStrip contextMenu;
		private ToolStripMenuItem smallImagesToolStripMenuItem;
		private ToolStripMenuItem mediumImagesToolStripMenuItem;
		private ToolStripMenuItem largeImagesToolStripMenuItem;
		public Inventory.Entry[] Entries
		{
			get
			{
				return this.m_entries;
			}
			set
			{
				this.m_entries = value;
				this.RefreshList();
			}
		}
		public bool SlotMode
		{
			get
			{
				return this.m_slotMode;
			}
			set
			{
				this.m_slotMode = value;
			}
		}
		public int FirstVisibleIndex
		{
			get
			{
				ListViewItem itemAt = base.GetItemAt(base.ClientRectangle.X + 20, base.ClientRectangle.Bottom - 20);
				return base.Items.IndexOf(itemAt);
			}
			set
			{
				if (value > 0 && value < base.Items.Count)
				{
					base.EnsureVisible(value);
				}
			}
		}
		public InventoryList()
		{
			this.InitializeComponent();
		}
		public InventoryList(IContainer container)
		{
			container.Add(this);
			this.InitializeComponent();
		}
		private void DoInitialize()
		{
			base.View = View.Tile;
			base.OwnerDraw = true;
			if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
			{
				return;
			}
			this.smallImagesToolStripMenuItem.Text = Localizer.Localize(this.smallImagesToolStripMenuItem.Text);
			this.mediumImagesToolStripMenuItem.Text = Localizer.Localize(this.mediumImagesToolStripMenuItem.Text);
			this.largeImagesToolStripMenuItem.Text = Localizer.Localize(this.largeImagesToolStripMenuItem.Text);
			this.UpdateImageSize(Editor.GetRegistryInt("ObjectThumbnailSize", 96));
			this.StartImageThread();
		}
		private void DoDispose()
		{
			if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
			{
				return;
			}
			this.StopImageThread();
			this.ClearImages();
		}
		private void ClearImages()
		{
			foreach (Image current in this.m_listImages.Values)
			{
				current.Dispose();
			}
			this.m_listImages.Clear();
			List<KeyValuePair<Inventory.Entry, MemoryStream>> pendingImages;
			Monitor.Enter(pendingImages = this.m_pendingImages);
			try
			{
				for (int i = 0; i < this.m_pendingImages.Count; i++)
				{
					MemoryStream value = this.m_pendingImages[i].Value;
					if (value != null)
					{
						value.Dispose();
					}
				}
				this.m_pendingImages.Clear();
			}
			finally
			{
				Monitor.Exit(pendingImages);
			}
			List<KeyValuePair<Inventory.Entry, Image>> readyImages;
			Monitor.Enter(readyImages = this.m_readyImages);
			try
			{
				for (int j = 0; j < this.m_readyImages.Count; j++)
				{
					Image value2 = this.m_readyImages[j].Value;
					if (value2 != null)
					{
						value2.Dispose();
					}
				}
				this.m_readyImages.Clear();
			}
			finally
			{
				Monitor.Exit(readyImages);
			}
		}
		private void CompactImages()
		{
			foreach (Inventory.Entry current in new List<Inventory.Entry>(this.m_listImages.Keys))
			{
				bool flag = false;
				ListViewItem listViewItem;
				if (this.m_listItems.TryGetValue(current, out listViewItem) && listViewItem.Index >= this.m_pendingIndexBegin && listViewItem.Index < this.m_pendingIndexEnd)
				{
					flag = true;
				}
				if (!flag)
				{
					this.m_listImages[current].Dispose();
					this.m_listImages.Remove(current);
				}
			}
		}
		private ListViewItem CreateItem(Inventory.Entry entry)
		{
			if (entry.Deleted)
			{
				return null;
			}
			ListViewItem listViewItem = new ListViewItem(entry.DisplayName);
			this.m_listItems[entry] = listViewItem;
			listViewItem.Tag = entry;
			base.Items.Add(listViewItem);
			return listViewItem;
		}
		private void RefreshList()
		{
			base.BeginUpdate();
			base.Items.Clear();
			this.m_listItems.Clear();
			this.ClearImages();
			this.m_pendingIndex = 0;
			this.m_pendingIndexBegin = 0;
			this.m_pendingIndexMid = 0;
			this.m_pendingIndexEnd = 0;
			this.m_pendingPass = 0;
			List<KeyValuePair<Inventory.Entry, MemoryStream>> pendingImages;
			Monitor.Enter(pendingImages = this.m_pendingImages);
			try
			{
				if (this.m_entries != null)
				{
					Inventory.Entry[] entries = this.m_entries;
					for (int i = 0; i < entries.Length; i++)
					{
						Inventory.Entry entry = entries[i];
						this.CreateItem(entry);
					}
				}
			}
			finally
			{
				Monitor.Exit(pendingImages);
			}
			this.OnScroll();
			base.EndUpdate();
		}
		protected override void OnDrawItem(DrawListViewItemEventArgs e)
		{
			if (base.View != View.Tile)
			{
				e.DrawDefault = true;
				return;
			}
			SizeF sizeF = new SizeF((float)this.m_imageSize, (float)this.m_imageSize);
			BufferedGraphics bufferedGraphics = BufferedGraphicsManager.Current.Allocate(e.Graphics, e.Bounds);
			Graphics graphics = bufferedGraphics.Graphics;
			graphics.Clear(this.BackColor);
			RectangleF rectangleF = e.Bounds;
			rectangleF.Inflate(-1f, -1f);
			if (e.Item.Selected)
			{
				using (SolidBrush solidBrush = new SolidBrush(SystemColors.Highlight))
				{
					graphics.FillRectangle(solidBrush, rectangleF);
				}
			}
			if (e.Item.Focused)
			{
				using (Pen pen = new Pen(SystemColors.ActiveBorder, 1f))
				{
					graphics.DrawRectangle(pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
				}
			}
			rectangleF.Inflate(-1f, -1f);
			string text = e.Item.Text;
			Font font = this.Font;
			//SizeF sizeF2;
			//graphics.MeasureString(text, font).Height = sizeF2.Height + 2f;
			//graphics.MeasureString(text, font).Height = sizeF2.Height + 2f;
			
			RectangleF layoutRectangle = rectangleF;
			layoutRectangle.Height = rectangleF.Height - (float)this.m_imageSize - 8f;
			layoutRectangle.Y = rectangleF.Bottom - layoutRectangle.Height;
			if (this.m_imageSize == 64)
			{
				layoutRectangle.Inflate(1.6f, 0f);
			}
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Center;
			stringFormat.LineAlignment = StringAlignment.Near;
			stringFormat.Trimming = StringTrimming.EllipsisCharacter;
			stringFormat.FormatFlags = StringFormatFlags.LineLimit;
			Color color = e.Item.Selected ? SystemColors.HighlightText : this.ForeColor;
			using (SolidBrush solidBrush2 = new SolidBrush(color))
			{
				graphics.DrawString(text, font, solidBrush2, layoutRectangle, stringFormat);
			}
			Inventory.Entry key = (Inventory.Entry)e.Item.Tag;
			Image thumbwait;
			this.m_listImages.TryGetValue(key, out thumbwait);
			if (thumbwait == null)
			{
				thumbwait = Resources.thumbwait;
			}
			if (thumbwait != null)
			{
				RectangleF rectangleF2 = new RectangleF(rectangleF.X + (rectangleF.Width - (float)thumbwait.Width) / 2f, rectangleF.Y + 4f + (sizeF.Height - (float)thumbwait.Height) / 2f, sizeF.Width, sizeF.Height);
				using (new SolidBrush(Color.Orange))
				{
					graphics.DrawImage(thumbwait, rectangleF2.Location);
				}
			}
			bufferedGraphics.Render();
			bufferedGraphics.Dispose();
		}
		private void UpdateImageSize(int size)
		{
			this.m_imageSize = size;
			Editor.SetRegistryInt("ObjectThumbnailSize", size);
			base.TileSize = new Size(size + 12, size + 44);
			this.RefreshList();
		}
		private void contextMenu_Opening(object sender, CancelEventArgs e)
		{
			this.smallImagesToolStripMenuItem.Checked = false;
			this.mediumImagesToolStripMenuItem.Checked = false;
			this.largeImagesToolStripMenuItem.Checked = false;
			int imageSize = this.m_imageSize;
			if (imageSize == 64)
			{
				this.smallImagesToolStripMenuItem.Checked = true;
				return;
			}
			if (imageSize == 96)
			{
				this.mediumImagesToolStripMenuItem.Checked = true;
				return;
			}
			if (imageSize != 128)
			{
				return;
			}
			this.largeImagesToolStripMenuItem.Checked = true;
		}
		private void largeImagesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.UpdateImageSize(128);
		}
		private void mediumImagesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.UpdateImageSize(96);
		}
		private void smallImagesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.UpdateImageSize(64);
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (base.LabelEdit && e.KeyData == Keys.F2 && base.SelectedItems.Count > 0)
			{
				base.SelectedItems[0].BeginEdit();
			}
		}
		private void StartImageThread()
		{
			if (this.m_imageThread != null)
			{
				return;
			}
			this.m_imageThread = new Thread(new ThreadStart(this.ImageThread));
			this.m_imageThread.Start();
		}
		private void ImageThread()
		{
			while (true)
			{
				Inventory.Entry entry = null;
				MemoryStream memoryStream = null;
				List<KeyValuePair<Inventory.Entry, MemoryStream>> pendingImages;
				Monitor.Enter(pendingImages = this.m_pendingImages);
				try
				{
					if (this.m_pendingImages.Count > 0)
					{
						entry = this.m_pendingImages[0].Key;
						memoryStream = this.m_pendingImages[0].Value;
						this.m_pendingImages.RemoveAt(0);
					}
				}
				finally
				{
					Monitor.Exit(pendingImages);
				}
				if (entry == null)
				{
					Thread.Sleep(10);
				}
				else
				{
					Bitmap bitmap = new Bitmap(this.m_imageSize, this.m_imageSize);
					using (Graphics graphics = Graphics.FromImage(bitmap))
					{
						if (entry.GetBackgroundColor().HasValue)
						{
							Brush brush = new SolidBrush(entry.GetBackgroundColor().Value);
							graphics.FillRectangle(brush, new Rectangle(0, 0, this.m_imageSize, this.m_imageSize));
							brush.Dispose();
						}
						graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
						Image image = null;
						if (entry.IsDirectory)
						{
							image = Resources.folder_big;
						}
						else
						{
							if (memoryStream != null)
							{
								image = Image.FromStream(memoryStream);
							}
						}
						if (image != null)
						{
							RectangleF rect = default(RectangleF);
							rect.Width = (float)Math.Min(image.Width, this.m_imageSize);
							rect.Height = (float)Math.Min(image.Height, this.m_imageSize);
							rect.X = ((float)this.m_imageSize - rect.Width) / 2f;
							rect.Y = ((float)this.m_imageSize - rect.Height) / 2f;
							graphics.DrawImage(image, rect);
							image.Dispose();
						}
						Image thumbnailOverlay = entry.GetThumbnailOverlay();
						if (thumbnailOverlay != null)
						{
							graphics.DrawImage(thumbnailOverlay, new Point(0, this.m_imageSize - thumbnailOverlay.Height));
						}
						string textOverlay = entry.GetTextOverlay();
						if (!string.IsNullOrEmpty(textOverlay))
						{
							Font font = new Font("Arial", 8f);
							Brush brush2 = new SolidBrush(Color.Black);
							Brush brush3 = new SolidBrush(Color.White);
							StringFormat stringFormat = new StringFormat();
							stringFormat.Alignment = StringAlignment.Far;
							stringFormat.LineAlignment = StringAlignment.Far;
							PointF origin = new PointF((float)(this.m_imageSize - 1), (float)(this.m_imageSize - 1));
							SizeF size = graphics.MeasureString(textOverlay, font, origin, stringFormat);
							PointF location = new PointF(origin.X - size.Width, origin.Y - size.Height);
							graphics.FillRectangle(brush2, new RectangleF(location, size));
							graphics.DrawString(textOverlay, font, brush3, (float)(this.m_imageSize - 1), (float)(this.m_imageSize - 1), stringFormat);
							font.Dispose();
							brush2.Dispose();
							brush3.Dispose();
						}
					}
					if (memoryStream != null)
					{
						memoryStream.Dispose();
					}
					List<KeyValuePair<Inventory.Entry, Image>> readyImages;
					Monitor.Enter(readyImages = this.m_readyImages);
					try
					{
						this.m_readyImages.Add(new KeyValuePair<Inventory.Entry, Image>(entry, bitmap));
					}
					finally
					{
						Monitor.Exit(readyImages);
					}
				}
			}
		}
		private void StopImageThread()
		{
			if (this.m_imageThread == null)
			{
				return;
			}
			this.m_imageThread.Abort();
			this.m_imageThread.Join();
			this.m_imageThread = null;
		}
		private void RedrawItem(ListViewItem item)
		{
			Rectangle bounds = item.Bounds;
			if (bounds.Right >= 0 && bounds.Bottom >= 0 && bounds.Left <= base.ClientSize.Width && bounds.Top <= base.ClientSize.Height)
			{
				base.RedrawItems(item.Index, item.Index, false);
			}
		}
		private void flushTimer_Tick(object sender, EventArgs e)
		{
			if (this.m_readyImages.Count > 0)
			{
				List<KeyValuePair<Inventory.Entry, Image>> readyImages;
				Monitor.Enter(readyImages = this.m_readyImages);
				try
				{
					int i = 0;
					while (i < this.m_readyImages.Count)
					{
						KeyValuePair<Inventory.Entry, Image> keyValuePair = this.m_readyImages[i];
						this.m_listImages[keyValuePair.Key] = keyValuePair.Value;
						if (this.SlotMode)
						{
							IEnumerator enumerator = base.Items.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									ListViewItem listViewItem = (ListViewItem)enumerator.Current;
									if ((Inventory.Entry)listViewItem.Tag == keyValuePair.Key)
									{
										this.RedrawItem(listViewItem);
									}
								}
								goto IL_CD;
							}
							finally
							{
								IDisposable disposable = enumerator as IDisposable;
								if (disposable != null)
								{
									disposable.Dispose();
								}
							}
							goto IL_B0;
						}
						goto IL_B0;
						IL_CD:
						i++;
						continue;
						IL_B0:
						ListViewItem item;
						if (this.m_listItems.TryGetValue(keyValuePair.Key, out item))
						{
							this.RedrawItem(item);
							goto IL_CD;
						}
						goto IL_CD;
					}
					this.m_readyImages.Clear();
				}
				finally
				{
					Monitor.Exit(readyImages);
				}
			}
			if (this.m_listImages.Count > 800)
			{
				this.CompactImages();
			}
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			int num = 20;
			int num2 = 50;
			if (this.m_pendingPass == 0)
			{
				int num3 = Math.Min(base.Items.Count, this.m_pendingIndexEnd);
				num3 = Math.Min(num3, this.m_pendingIndex + num);
				num3 = Math.Min(num3, this.m_pendingIndex + num2 - this.m_pendingImages.Count);
				List<KeyValuePair<Inventory.Entry, MemoryStream>> pendingImages;
				Monitor.Enter(pendingImages = this.m_pendingImages);
				try
				{
					while (this.m_pendingIndex < num3)
					{
						Inventory.Entry entry = (Inventory.Entry)base.Items[this.m_pendingIndex].Tag;
						if (!this.m_listImages.ContainsKey(entry))
						{
							this.m_pendingImages.Add(new KeyValuePair<Inventory.Entry, MemoryStream>(entry, entry.GetThumbnailData()));
							if (stopwatch.ElapsedMilliseconds > 70L)
							{
								break;
							}
						}
						this.m_pendingIndex++;
					}
				}
				finally
				{
					Monitor.Exit(pendingImages);
				}
				if (this.m_pendingIndex == this.m_pendingIndexEnd)
				{
					this.m_pendingPass = 1;
					this.m_pendingIndex = this.m_pendingIndexMid - 1;
				}
			}
			if (this.m_pendingPass == 1)
			{
				int num4 = this.m_pendingIndexBegin;
				num4 = Math.Max(num4, this.m_pendingIndex - num);
				num4 = Math.Max(num4, this.m_pendingIndex - num2 + this.m_pendingImages.Count);
				List<KeyValuePair<Inventory.Entry, MemoryStream>> pendingImages2;
				Monitor.Enter(pendingImages2 = this.m_pendingImages);
				try
				{
					while (this.m_pendingIndex > num4)
					{
						Inventory.Entry entry2 = (Inventory.Entry)base.Items[this.m_pendingIndex].Tag;
						if (!this.m_listImages.ContainsKey(entry2))
						{
							this.m_pendingImages.Add(new KeyValuePair<Inventory.Entry, MemoryStream>(entry2, entry2.GetThumbnailData()));
							if (stopwatch.ElapsedMilliseconds > 70L)
							{
								break;
							}
						}
						this.m_pendingIndex--;
					}
				}
				finally
				{
					Monitor.Exit(pendingImages2);
				}
				if (this.m_pendingIndex == this.m_pendingIndexBegin)
				{
					this.m_pendingPass = 2;
				}
			}
		}
		private int GetNearestItem(Point pt, int firstIndex, int lastIndex)
		{
			if (firstIndex >= lastIndex)
			{
				return firstIndex;
			}
			int num = (firstIndex + lastIndex) / 2;
			if (base.Items[num].Bounds.Bottom <= pt.Y)
			{
				return this.GetNearestItem(pt, num + 1, lastIndex);
			}
			return this.GetNearestItem(pt, firstIndex, num - 1);
		}
		private void OnScroll()
		{
			int nearestItem = this.GetNearestItem(new Point(0, 0), 0, base.Items.Count - 1);
			int pendingIndexBegin = Math.Max(0, nearestItem - 150);
			int pendingIndexEnd = Math.Min(base.Items.Count, nearestItem + 150);
			this.m_pendingIndexBegin = pendingIndexBegin;
			this.m_pendingIndexMid = nearestItem;
			this.m_pendingIndexEnd = pendingIndexEnd;
			this.m_pendingPass = 0;
			this.m_pendingIndex = this.m_pendingIndexMid;
		}
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 256)
			{
				switch (msg)
				{
				case 276:
				case 277:
					base.WndProc(ref m);
					this.OnScroll();
					return;

				default:
					if (msg == 8270)
					{
						switch (((Win32.NMHDR)m.GetLParam(typeof(Win32.NMHDR))).code)
						{
						case -181:
						case -180:
							base.WndProc(ref m);
							this.OnScroll();
							return;

						default:
							base.WndProc(ref m);
							return;
						}
					}
					else
					{
						base.WndProc(ref m);
					}
					break;
				}
			}
			else
			{
				Win32.ScrollInfo scrollInfo = new Win32.ScrollInfo();
				scrollInfo.fMask = 31;
				Win32.GetScrollInfo(base.Handle, 1, scrollInfo);
				int nPos = scrollInfo.nPos;
				base.WndProc(ref m);
				Win32.GetScrollInfo(base.Handle, 1, scrollInfo);
				int nPos2 = scrollInfo.nPos;
				if (nPos != nPos2)
				{
					this.OnScroll();
					return;
				}
			}
		}
		public void AddEntry(Inventory.Entry entry)
		{
			ListViewItem listViewItem;
			this.m_listItems.TryGetValue(entry, out listViewItem);
			if (listViewItem == null)
			{
				listViewItem = this.CreateItem(entry);
				if (listViewItem == null)
				{
					return;
				}
			}
			base.SelectedItems.Clear();
			if (listViewItem != null)
			{
				listViewItem.Selected = true;
			}
		}
		public void RemoveEntry(Inventory.Entry entry)
		{
			ListViewItem listViewItem;
			this.m_listItems.TryGetValue(entry, out listViewItem);
			if (listViewItem == null)
			{
				return;
			}
			listViewItem.Remove();
		}
		public ListViewItem FindEntry(Inventory.Entry entry)
		{
			ListViewItem result;
			this.m_listItems.TryGetValue(entry, out result);
			return result;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
			this.DoDispose();
		}
		private void InitializeComponent()
		{
			this.components = new Container();
			this.flushTimer = new System.Windows.Forms.Timer(this.components);
			this.contextMenu = new ContextMenuStrip(this.components);
			this.smallImagesToolStripMenuItem = new ToolStripMenuItem();
			this.mediumImagesToolStripMenuItem = new ToolStripMenuItem();
			this.largeImagesToolStripMenuItem = new ToolStripMenuItem();
			this.contextMenu.SuspendLayout();
			base.SuspendLayout();
			this.flushTimer.Enabled = true;
			this.flushTimer.Interval = 150;
			this.flushTimer.Tick += new EventHandler(this.flushTimer_Tick);
			this.contextMenu.Items.AddRange(new ToolStripItem[]
			{
				this.smallImagesToolStripMenuItem,
				this.mediumImagesToolStripMenuItem,
				this.largeImagesToolStripMenuItem
			});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new Size(261, 92);
			this.contextMenu.Opening += new CancelEventHandler(this.contextMenu_Opening);
			this.smallImagesToolStripMenuItem.Name = "smallImagesToolStripMenuItem";
			this.smallImagesToolStripMenuItem.Size = new Size(260, 22);
			this.smallImagesToolStripMenuItem.Text = "MENUITEM_THUMBNAIL_SMALL";
			this.smallImagesToolStripMenuItem.Click += new EventHandler(this.smallImagesToolStripMenuItem_Click);
			this.mediumImagesToolStripMenuItem.Name = "mediumImagesToolStripMenuItem";
			this.mediumImagesToolStripMenuItem.Size = new Size(260, 22);
			this.mediumImagesToolStripMenuItem.Text = "MENUITEM_THUMBNAIL_MEDIUM";
			this.mediumImagesToolStripMenuItem.Click += new EventHandler(this.mediumImagesToolStripMenuItem_Click);
			this.largeImagesToolStripMenuItem.Name = "largeImagesToolStripMenuItem";
			this.largeImagesToolStripMenuItem.Size = new Size(260, 22);
			this.largeImagesToolStripMenuItem.Text = "MENUITEM_THUMBNAIL_LARGE";
			this.largeImagesToolStripMenuItem.Click += new EventHandler(this.largeImagesToolStripMenuItem_Click);
			this.ContextMenuStrip = this.contextMenu;
			this.contextMenu.ResumeLayout(false);
			base.ResumeLayout(false);
			this.DoInitialize();
		}
	}
}
