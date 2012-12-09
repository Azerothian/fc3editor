using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
namespace FC3Editor.Nomad
{
	public class WorldImport
	{
		private class TextureSectorInfo
		{
			public int sectorX;
			public int sectorY;
			public XmlElement xmlSector;
			public string layerFile;
			public List<string> layerIds;
			public List<int> layerIndexes;
		}
		public class ObjectInstance
		{
			public string layer;
			public int sectorX;
			public int sectorY;
			public float localX;
			public float localY;
			public float localZ;
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					(!string.IsNullOrEmpty(this.layer)) ? ("Layer: " + this.layer + " ") : "",
					"Sector X: ",
					this.sectorX,
					" Y: ",
					this.sectorY,
					" Pos: ",
					this.localX.ToString("F2"),
					", ",
					this.localY.ToString("F2"),
					", ",
					this.localZ.ToString("F2")
				});
			}
		}
		private static WorldImport s_instance;
		private string m_worldFile;
		private string m_worldPath;
		public static WorldImport Instance
		{
			get
			{
				if (WorldImport.s_instance == null)
				{
					WorldImport.s_instance = new WorldImport();
				}
				return WorldImport.s_instance;
			}
		}
		public string WorldFile
		{
			get
			{
				return this.m_worldFile;
			}
			set
			{
				this.m_worldFile = value;
				this.m_worldPath = Path.GetDirectoryName(this.m_worldFile);
				if (this.m_worldFile != null)
				{
					using (RegistryKey registrySettings = Editor.GetRegistrySettings())
					{
						registrySettings.SetValue("ImportWorldFile", this.m_worldFile);
					}
				}
			}
		}
		public WorldImport()
		{
			using (RegistryKey registrySettings = Editor.GetRegistrySettings())
			{
				this.WorldFile = Editor.GetRegistryString(registrySettings, "ImportWorldFile", null);
			}
		}
		private void ImportHeightmap(string sourceDir, string targetDir, Rectangle sectorRect, Rectangle sectionRect)
		{
			byte[] array = new byte[526338];
			for (int i = sectionRect.Top; i <= sectionRect.Bottom; i++)
			{
				for (int j = sectionRect.Left; j <= sectionRect.Right; j++)
				{
					string path = string.Concat(new string[]
					{
						sourceDir,
						"heightmap",
						j.ToString("D4"),
						"_",
						i.ToString("D4"),
						".raw"
					});
					if (File.Exists(path))
					{
						byte[] sourceArray = File.ReadAllBytes(path);
						int num = j * 4;
						int num2 = i * 4;
						int num3 = Math.Max(num, sectorRect.Left);
						int num4 = Math.Max(num2, sectorRect.Top);
						int num5 = num3 % 4;
						int num6 = num4 % 4;
						int num7 = Math.Min(num + 4, sectorRect.Right) - num3;
						int num8 = Math.Min(num2 + 4, sectorRect.Bottom) - num4;
						int num9 = num5 * 64;
						int num10 = num6 * 64;
						int num11 = (num3 - sectorRect.Left) * 64;
						int num12 = (num4 - sectorRect.Top) * 64;
						int num13 = Math.Min(num7 * 64 + 1, 257 - num9);
						int num14 = Math.Min(num8 * 64 + 1, 257 - num10);
						for (int k = 0; k < num14; k++)
						{
							Array.Copy(sourceArray, ((num10 + k) * 257 + num9) * 2, array, ((num12 + k) * 513 + num11) * 2, num13 * 2);
						}
					}
				}
			}
			File.WriteAllBytes(targetDir + "heightmap.raw", array);
		}
		private void ImportHolemap(string sourceDir, string targetDir, Rectangle sectorRect, Rectangle sectionRect)
		{
			byte[] array = new byte[263169];
			for (int i = sectionRect.Top; i <= sectionRect.Bottom; i++)
			{
				for (int j = sectionRect.Left; j <= sectionRect.Right; j++)
				{
					string path = string.Concat(new string[]
					{
						sourceDir,
						"holemap",
						j.ToString("D4"),
						"_",
						i.ToString("D4"),
						".raw"
					});
					if (File.Exists(path))
					{
						byte[] sourceArray = File.ReadAllBytes(path);
						int num = j * 4;
						int num2 = i * 4;
						int num3 = Math.Max(num, sectorRect.Left);
						int num4 = Math.Max(num2, sectorRect.Top);
						int num5 = num3 % 4;
						int num6 = num4 % 4;
						int num7 = Math.Min(num + 4, sectorRect.Right) - num3;
						int num8 = Math.Min(num2 + 4, sectorRect.Bottom) - num4;
						int num9 = num5 * 64;
						int num10 = num6 * 64;
						int num11 = (num3 - sectorRect.Left) * 64;
						int num12 = (num4 - sectorRect.Top) * 64;
						int length = Math.Min(num7 * 64 + 1, 256 - num9);
						int num13 = Math.Min(num8 * 64 + 1, 256 - num10);
						for (int k = 0; k < num13; k++)
						{
							Array.Copy(sourceArray, (num10 + k) * 256 + num9, array, (num12 + k) * 513 + num11, length);
						}
					}
				}
			}
			File.WriteAllBytes(targetDir + "holemap.raw", array);
		}
		private void ImportTextures(string sourceDir, string targetDir, Rectangle sectorRect, Rectangle sectionRect, XmlElement xmlTextureEntries)
		{
			List<string> list = new List<string>();
			List<WorldImport.TextureSectorInfo> list2 = new List<WorldImport.TextureSectorInfo>();
			for (int i = sectionRect.Top; i <= sectionRect.Bottom; i++)
			{
				for (int j = sectionRect.Left; j <= sectionRect.Right; j++)
				{
					string str = j.ToString("D4") + "_" + i.ToString("D4");
					string text = sourceDir + "section" + str + ".xml";
					if (File.Exists(text))
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(text);
						List<string> list3 = new List<string>();
						XmlElement elementByName = XmlHelper.GetElementByName(xmlDocument.DocumentElement, "List");
						foreach (XmlElement current in XmlHelper.GetElementsByName(elementByName, "Entry"))
						{
							list3.Add(current.GetAttribute("Id"));
						}
						XmlElement elementByName2 = XmlHelper.GetElementByName(xmlDocument.DocumentElement, "Sectors");
						foreach (XmlElement current2 in XmlHelper.GetElementsByName(elementByName2, "Sector"))
						{
							int num = int.Parse(current2.GetAttribute("X"));
							int num2 = int.Parse(current2.GetAttribute("Y"));
							if (num >= sectorRect.X && num < sectorRect.Right && num2 >= sectorRect.Y && num2 < sectorRect.Bottom)
							{
								WorldImport.TextureSectorInfo textureSectorInfo = new WorldImport.TextureSectorInfo();
								textureSectorInfo.sectorX = num;
								textureSectorInfo.sectorY = num2;
								textureSectorInfo.xmlSector = current2;
								textureSectorInfo.layerFile = sourceDir + "section" + str + ".png";
								textureSectorInfo.layerIds = new List<string>();
								textureSectorInfo.layerIndexes = new List<int>();
								foreach (XmlElement current3 in XmlHelper.GetElementsByName(current2, "Entry"))
								{
									string attribute = current3.GetAttribute("Id");
									if (!list.Contains(attribute))
									{
										list.Add(attribute);
									}
									textureSectorInfo.layerIds.Add(attribute);
									textureSectorInfo.layerIndexes.Add(list3.IndexOf(attribute));
								}
								list2.Add(textureSectorInfo);
							}
						}
					}
				}
			}
			if (list.Count > 4)
			{
				list.RemoveRange(4, list.Count - 4);
			}
			int[] array = new int[]
			{
				2,
				1,
				0,
				3
			};
			byte[] array2 = new byte[1048576];
			foreach (WorldImport.TextureSectorInfo current4 in list2)
			{
				int sectorX = current4.sectorX;
				int sectorY = current4.sectorY;
				Image image = Image.FromFile(current4.layerFile);
				Bitmap bitmap = new Bitmap(image);
				BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
				int num3 = sectorX % 4;
				int num4 = sectorY % 4;
				int num5 = num3 * 64;
				int num6 = num4 * 64;
				int num7 = (sectorX - sectorRect.Left) * 64;
				int num8 = (sectorY - sectorRect.Top) * 64;
				for (int k = 0; k < current4.layerIds.Count; k++)
				{
					string item = current4.layerIds[k];
					int num9 = current4.layerIndexes[k];
					int num10 = list.IndexOf(item);
					if (num10 != -1)
					{
						num10 = array[num10];
						for (int l = 0; l < 64; l++)
						{
							byte[] array3 = new byte[256];
							Marshal.Copy((IntPtr)(bitmapData.Scan0.ToInt32() + (num6 + num9 * bitmap.Width + l) * bitmapData.Stride + num5 * 4), array3, 0, 256);
							for (int m = 0; m < 64; m++)
							{
								array2[((num8 + l) * 512 + num7 + m) * 4 + num10] = array3[m * 4];
							}
						}
					}
				}
				for (int n = 0; n < 64; n++)
				{
					byte[] destination = new byte[256];
					Marshal.Copy((IntPtr)(bitmapData.Scan0.ToInt32() + (num6 + n) * bitmapData.Stride + num5 * 4), destination, 0, 256);
					for (int num11 = 0; num11 < 64; num11++)
					{
						int num12 = ((num8 + n) * 512 + num7 + num11) * 4;
						int num13 = 255;
						for (int num14 = current4.layerIds.Count - 1; num14 >= 0; num14--)
						{
							string item2 = current4.layerIds[num14];
							int num15 = list.IndexOf(item2);
							if (num15 != -1)
							{
								num15 = array[num15];
								byte b = array2[num12 + num15];
								if (num13 >= (int)b)
								{
									num13 -= (int)b;
								}
								else
								{
									b = (byte)Math.Min((int)b, num13);
									array2[num12 + num15] = b;
									num13 -= (int)b;
								}
							}
						}
						if (num13 != 0)
						{
							int num16 = list.IndexOf(current4.layerIds[0]);
							if (num16 != -1)
							{
								num16 = array[num16];
								byte[] expr_554_cp_0 = array2;
								int expr_554_cp_1 = num12 + num16;
								expr_554_cp_0[expr_554_cp_1] += (byte)num13;
							}
						}
					}
				}
				bitmap.UnlockBits(bitmapData);
				bitmap.Dispose();
				image.Dispose();
			}
			File.WriteAllBytes(targetDir + "texture.mask", array2);
			for (int num17 = 0; num17 < list.Count; num17++)
			{
				XmlElement xmlElement = xmlTextureEntries.OwnerDocument.CreateElement("Entry");
				xmlElement.SetAttribute("Id", list[num17]);
				xmlTextureEntries.AppendChild(xmlElement);
			}
		}
		private void ImportCollections(string sourceDir, string targetDir, Rectangle sectorRect, Rectangle sectionRect, XmlElement xmlCollection)
		{
			new List<string>();
			new List<WorldImport.TextureSectorInfo>();
			SortedList<string, int> collectionsCoverage = new SortedList<string, int>();
			for (int i = sectionRect.Top; i <= sectionRect.Bottom; i++)
			{
				for (int j = sectionRect.Left; j <= sectionRect.Right; j++)
				{
					string str = j.ToString("D4") + "_" + i.ToString("D4");
					string text = sourceDir + "section" + str + ".xml";
					if (File.Exists(text))
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(text);
						XmlElement elementByName = XmlHelper.GetElementByName(xmlDocument.DocumentElement, "List");
						foreach (XmlElement current in XmlHelper.GetElementsByName(elementByName, "Entry"))
						{
							string attribute = current.GetAttribute("Id");
							int num;
							collectionsCoverage.TryGetValue(attribute, out num);
							num += int.Parse(current.GetAttribute("Coverage"));
							collectionsCoverage[attribute] = num;
						}
					}
				}
			}
			List<string> list = new List<string>(collectionsCoverage.Keys);
			list.Sort(delegate(string a, string b)
			{
				int num20 = collectionsCoverage[a];
				int num21 = collectionsCoverage[b];
				return num21 - num20;
			}
			);
			if (list.Count > 8)
			{
				list.RemoveRange(8, list.Count - 8);
			}
			byte[] array = new byte[262144];
			for (int k = sectionRect.Top; k <= sectionRect.Bottom; k++)
			{
				for (int l = sectionRect.Left; l <= sectionRect.Right; l++)
				{
					string str2 = l.ToString("D4") + "_" + k.ToString("D4");
					string text2 = sourceDir + "section" + str2 + ".xml";
					if (File.Exists(text2))
					{
						XmlDocument xmlDocument2 = new XmlDocument();
						xmlDocument2.Load(text2);
						List<string> list2 = new List<string>();
						XmlElement elementByName2 = XmlHelper.GetElementByName(xmlDocument2.DocumentElement, "List");
						foreach (XmlElement current2 in XmlHelper.GetElementsByName(elementByName2, "Entry"))
						{
							string attribute2 = current2.GetAttribute("Id");
							list2.Add(attribute2);
						}
						byte[] array2 = new byte[256];
						for (int m = 0; m < 256; m++)
						{
							array2[m] = (byte)CollectionManager.EmptyCollectionId;
						}
						for (int n = 0; n < list2.Count; n++)
						{
							string item = list2[n];
							int num2 = list.IndexOf(item);
							if (num2 != -1)
							{
								array2[n + 1] = (byte)num2;
							}
						}
						Image image = Image.FromFile(sourceDir + "section" + str2 + ".png");
						Bitmap bitmap = new Bitmap(image);
						BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
						int num3 = l * 4;
						int num4 = k * 4;
						int num5 = Math.Max(num3, sectorRect.Left);
						int num6 = Math.Max(num4, sectorRect.Top);
						int num7 = num5 % 4;
						int num8 = num6 % 4;
						int num9 = Math.Min(num3 + 4, sectorRect.Right) - num5;
						int num10 = Math.Min(num4 + 4, sectorRect.Bottom) - num6;
						int num11 = num7 * 64;
						int num12 = num8 * 64;
						int num13 = (num5 - sectorRect.Left) * 64;
						int num14 = (num6 - sectorRect.Top) * 64;
						int num15 = num9 * 64;
						int num16 = num10 * 64;
						for (int num17 = 0; num17 < num16; num17++)
						{
							byte[] array3 = new byte[num15 * 3];
							Marshal.Copy((IntPtr)(bitmapData.Scan0.ToInt32() + (num12 + num17) * bitmapData.Stride + num11 * 3), array3, 0, num15 * 3);
							for (int num18 = 0; num18 < num15; num18++)
							{
								array[(num14 + num17) * 512 + (num13 + num18)] = array2[(int)array3[num18 * 3]];
							}
						}
						bitmap.UnlockBits(bitmapData);
						bitmap.Dispose();
						image.Dispose();
					}
				}
			}
			File.WriteAllBytes(targetDir + "collection.mask", array);
			XmlElement xmlElement = xmlCollection.OwnerDocument.CreateElement("collectionEntries");
			xmlCollection.AppendChild(xmlElement);
			XmlElement xmlElement2 = xmlCollection.OwnerDocument.CreateElement("collectionSeeds");
			xmlCollection.AppendChild(xmlElement2);
			Random random = new Random();
			for (int num19 = 0; num19 < list.Count; num19++)
			{
				XmlElement xmlElement3 = xmlElement.OwnerDocument.CreateElement("Entry");
				xmlElement3.SetAttribute("id", list[num19]);
				xmlElement.AppendChild(xmlElement3);
				XmlElement xmlElement4 = xmlElement.OwnerDocument.CreateElement("Seed");
				xmlElement4.SetAttribute("id", random.Next().ToString());
				xmlElement2.AppendChild(xmlElement4);
			}
		}
		private void ImportObjects(string sourceDir, string targetDir, Rectangle sectorRect, Rectangle sectionRect, string[] layers, XmlElement xmlObjectManager)
		{
			int num = 0;
			XmlElement xmlElement = xmlObjectManager.OwnerDocument.CreateElement("Objects");
			xmlObjectManager.AppendChild(xmlElement);
			for (int i = sectionRect.Top; i <= sectionRect.Bottom; i++)
			{
				for (int j = sectionRect.Left; j <= sectionRect.Right; j++)
				{
					float num2 = (float)((j * 4 - sectorRect.Left) * 64);
					float num3 = (float)((i * 4 - sectorRect.Top) * 64);
					string text = string.Concat(new string[]
					{
						sourceDir,
						"section",
						j.ToString("D4"),
						"_",
						i.ToString("D4"),
						".xml"
					});
					if (File.Exists(text))
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(text);
						foreach (XmlElement current in XmlHelper.GetElementsByName(xmlDocument.DocumentElement, "Sector"))
						{
							int num4 = int.Parse(current.GetAttribute("X"));
							int num5 = int.Parse(current.GetAttribute("Y"));
							if (num4 >= sectorRect.Left && num4 < sectorRect.Right && num5 >= sectorRect.Top && num5 < sectorRect.Bottom)
							{
								foreach (XmlElement current2 in XmlHelper.GetElementsByName(current, "Object"))
								{
									string attribute = current2.GetAttribute("Id");
									if (!attribute.Contains("ige_archetypes") || !attribute.Contains("_pfx"))
									{
										if (layers != null && current2.HasAttribute("Layer"))
										{
											string attribute2 = current2.GetAttribute("Layer");
											if (Array.IndexOf<string>(layers, attribute2) == -1)
											{
												continue;
											}
										}
										string attribute3 = current2.GetAttribute("Pos");
										string[] array = attribute3.Split(new char[]
										{
											','
										}, StringSplitOptions.RemoveEmptyEntries);
										float[] array2 = new float[]
										{
											float.Parse(array[0]),
											float.Parse(array[1]),
											float.Parse(array[2])
										};
										array2[0] += num2;
										array2[1] += num3;
										XmlElement xmlElement2 = xmlElement.OwnerDocument.CreateElement("Object");
										xmlElement2.SetAttribute("Entry", attribute);
										xmlElement2.SetAttribute("Pos", string.Concat(new string[]
										{
											array2[0].ToString("F3"),
											",",
											array2[1].ToString("F3"),
											",",
											array2[2].ToString("F3")
										}));
										xmlElement2.SetAttribute("Angles", current2.GetAttribute("Angles"));
										xmlElement2.SetAttribute("Offset", "0,0,0");
										xmlElement2.SetAttribute("HasOffset", "1");
										xmlElement2.SetAttribute("Index", num.ToString());
										xmlElement.AppendChild(xmlElement2);
										num++;
									}
								}
							}
						}
					}
				}
			}
			xmlObjectManager.SetAttribute("NumObjects", num.ToString());
			xmlObjectManager.SetAttribute("Version", "4");
		}
		private void ImportWater(XmlElement xmlWaterSectors, Rectangle sectorRect, XmlElement xmlTerrain)
		{
			Dictionary<KeyValuePair<int, int>, XmlElement> dictionary = new Dictionary<KeyValuePair<int, int>, XmlElement>();
			foreach (XmlElement current in XmlHelper.GetElementsByName(xmlWaterSectors, "WaterSector"))
			{
				int key = int.Parse(current.GetAttribute("X"));
				int value = int.Parse(current.GetAttribute("Y"));
				dictionary[new KeyValuePair<int, int>(key, value)] = current;
			}
			XmlElement xmlElement = xmlTerrain.OwnerDocument.CreateElement("WaterLevelSectors");
			xmlTerrain.AppendChild(xmlElement);
			XmlElement xmlElement2 = xmlTerrain.OwnerDocument.CreateElement("WaterEntrySectors");
			xmlTerrain.AppendChild(xmlElement2);
			for (int i = sectorRect.Top; i < sectorRect.Bottom; i++)
			{
				for (int j = sectorRect.Left; j < sectorRect.Right; j++)
				{
					string value2 = "0";
					string value3 = "";
					XmlElement xmlElement3;
					if (dictionary.TryGetValue(new KeyValuePair<int, int>(j, i), out xmlElement3))
					{
						value2 = xmlElement3.GetAttribute("Level");
						value3 = xmlElement3.GetAttribute("Entry");
					}
					XmlElement xmlElement4 = xmlElement.OwnerDocument.CreateElement("Sector");
					xmlElement.AppendChild(xmlElement4);
					xmlElement4.SetAttribute("Level", value2);
					XmlElement xmlElement5 = xmlElement2.OwnerDocument.CreateElement("Entry");
					xmlElement2.AppendChild(xmlElement5);
					xmlElement5.SetAttribute("id", value3);
				}
			}
		}
		public bool ImportWorld(int sectorX1, int sectorY1, string[] layers)
		{
			if (!File.Exists(this.m_worldFile))
			{
				return false;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.m_worldFile);
			MainForm.Instance.SetupImportMap(string.Concat(new object[]
			{
				fileNameWithoutExtension,
				" sector (",
				sectorX1,
				",",
				sectorY1,
				") to (",
				sectorX1 + 7,
				",",
				sectorY1 + 7,
				")"
			}));
			Rectangle sectorRect = new Rectangle(sectorX1, sectorY1, 8, 8);
			Rectangle sectionRect = new Rectangle(sectorRect.Left / 4, sectorRect.Top / 4, sectorRect.Right / 4 - sectorRect.Left / 4, sectorRect.Bottom / 4 - sectorRect.Top / 4);
			string text = Path.GetTempPath();
			text += "\\tmpmap\\";
			if (Directory.Exists(text))
			{
				Directory.Delete(text, true);
			}
			Directory.CreateDirectory(text);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.m_worldFile);
			XmlDocument xmlDocument2 = new XmlDocument();
			XmlElement xmlElement = xmlDocument2.CreateElement("FarCry2.Editor.Map");
			xmlDocument2.AppendChild(xmlElement);
			XmlElement xmlElement2 = xmlDocument2.CreateElement("TerrainManager");
			xmlElement.AppendChild(xmlElement2);
			XmlElement xmlElement3 = xmlDocument2.CreateElement("textureEntries");
			xmlElement2.AppendChild(xmlElement3);
			XmlElement xmlElement4 = xmlDocument2.CreateElement("CollectionManager");
			xmlElement.AppendChild(xmlElement4);
			XmlElement xmlElement5 = xmlDocument2.CreateElement("ObjectManager");
			xmlElement.AppendChild(xmlElement5);
			this.ImportHeightmap(this.m_worldPath + "\\heightmaps\\", text, sectorRect, sectionRect);
			this.ImportHolemap(this.m_worldPath + "\\holemaps\\", text, sectorRect, sectionRect);
			this.ImportTextures(this.m_worldPath + "\\textures\\", text, sectorRect, sectionRect, xmlElement3);
			this.ImportCollections(this.m_worldPath + "\\collections\\", text, sectorRect, sectionRect, xmlElement4);
			this.ImportObjects(this.m_worldPath + "\\objects\\", text, sectorRect, sectionRect, layers, xmlElement5);
			XmlElement elementByName = XmlHelper.GetElementByName(xmlDocument.DocumentElement, "WaterSectors");
			this.ImportWater(elementByName, sectorRect, xmlElement2);
			xmlElement2.SetAttribute("WaterLevel", "100");
			xmlDocument2.Save(text + "map.xml");
			return EditorDocument.LoadPhysical(text);
		}
		public List<WorldImport.ObjectInstance> FindInstances(string entryName)
		{
			List<WorldImport.ObjectInstance> list = new List<WorldImport.ObjectInstance>();
			if (!File.Exists(this.m_worldFile))
			{
				return list;
			}
			string text = this.m_worldPath + "\\objects\\instances.xml";
			if (!File.Exists(text))
			{
				return list;
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(text);
			foreach (XmlElement current in XmlHelper.GetElementsByName(xmlDocument.DocumentElement, "Entry"))
			{
				if (current.GetAttribute("Id") == entryName)
				{
					foreach (XmlElement current2 in XmlHelper.GetElementsByName(current, "Instance"))
					{
						list.Add(new WorldImport.ObjectInstance
						{
							layer = current2.HasAttribute("Layer") ? current2.GetAttribute("Layer") : null,
							sectorX = int.Parse(current2.GetAttribute("SectorX")),
							sectorY = int.Parse(current2.GetAttribute("SectorY")),
							localX = float.Parse(current2.GetAttribute("LocalX")),
							localY = float.Parse(current2.GetAttribute("LocalY")),
							localZ = float.Parse(current2.GetAttribute("LocalZ"))
						});
					}
				}
			}
			return list;
		}
		public bool LoadWorld()
		{
			bool result;
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.DefaultExt = "xml";
				openFileDialog.FileName = "world.xml";
				openFileDialog.Filter = "World file|world.xml";
				if (openFileDialog.ShowDialog() != DialogResult.OK)
				{
					result = false;
				}
				else
				{
					this.WorldFile = openFileDialog.FileName;
					result = true;
				}
			}
			return result;
		}
	}
}
