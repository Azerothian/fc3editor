using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
namespace Nomad.Utils
{
	public class XmlHelper
	{
		private static readonly XmlDocument s_document = new XmlDocument();
		public static XmlCDataSection GetCData(XmlNode root)
		{
			foreach (XmlNode xmlNode in root.ChildNodes)
			{
				if (xmlNode.NodeType == XmlNodeType.CDATA)
				{
					return xmlNode as XmlCDataSection;
				}
			}
			return null;
		}
		public static XmlElement GetElement(XmlNode root)
		{
			foreach (XmlNode xmlNode in root.ChildNodes)
			{
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					return xmlNode as XmlElement;
				}
			}
			return null;
		}
		public static XmlElement GetPrevElement(XmlNode element)
		{
			for (XmlNode previousSibling = element.PreviousSibling; previousSibling != null; previousSibling = previousSibling.PreviousSibling)
			{
				if (previousSibling.NodeType == XmlNodeType.Element)
				{
					return previousSibling as XmlElement;
				}
			}
			return null;
		}
		public static XmlElement GetNextElement(XmlNode element)
		{
			for (XmlNode nextSibling = element.NextSibling; nextSibling != null; nextSibling = nextSibling.NextSibling)
			{
				if (nextSibling.NodeType == XmlNodeType.Element)
				{
					return nextSibling as XmlElement;
				}
			}
			return null;
		}
		public static IEnumerable<XmlElement> GetElements(XmlNode root)
		{
			if (root != null)
			{
				foreach (XmlNode xmlNode in root.ChildNodes)
				{
					if (xmlNode.NodeType == XmlNodeType.Element)
					{
						yield return xmlNode as XmlElement;
					}
				}
			}
			yield break;
		}
		public static IEnumerable<XmlElement> GetElementsByName(XmlNode root, string name)
		{
			foreach (XmlNode xmlNode in root.ChildNodes)
			{
				if (xmlNode.NodeType == XmlNodeType.Element && !(xmlNode.Name != name))
				{
					yield return xmlNode as XmlElement;
				}
			}
			yield break;
		}
		public static XmlElement GetElementByName(XmlNode root, string name)
		{
			foreach (XmlElement current in XmlHelper.GetElements(root))
			{
				if (!(current.Name != name))
				{
					return current;
				}
			}
			return null;
		}
		public static IEnumerable<XmlElement> GetRecursiveElements(XmlNode root)
		{
			foreach (XmlNode xmlNode in root.ChildNodes)
			{
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					XmlElement xmlElement = (XmlElement)xmlNode;
					yield return xmlElement;
					foreach (XmlElement current in XmlHelper.GetRecursiveElements(xmlElement))
					{
						yield return current;
					}
				}
			}
			yield break;
		}
		private static XmlTextWriter Write(XmlNode node, TextWriter s)
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(s);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.Indentation = 1;
			xmlTextWriter.IndentChar = '\t';
			node.WriteTo(xmlTextWriter);
			return xmlTextWriter;
		}
		public static void WriteToFile(XmlNode node, string file)
		{
			XmlTextWriter xmlTextWriter = XmlHelper.Write(node, File.CreateText(file));
			xmlTextWriter.Close();
		}
		public static string WriteToString(XmlNode node)
		{
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = XmlHelper.Write(node, stringWriter);
			string result = stringWriter.ToString();
			xmlTextWriter.Close();
			return result;
		}
		public static MemoryStream WriteToMemory(XmlNode node)
		{
			MemoryStream memoryStream = new MemoryStream();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(new StreamWriter(memoryStream))
			{
				Formatting = Formatting.Indented,
				Indentation = 4,
				IndentChar = ' '
			};
			node.WriteTo(xmlTextWriter);
			xmlTextWriter.Flush();
			return memoryStream;
		}
		public static bool CompareDocToFile(XmlDocument document, string filePath, out string tmpPath)
		{
			tmpPath = filePath + ".TMP";
			XmlHelper.WriteToFile(document, tmpPath);
			if (File.Exists(filePath))
			{
				FileInfo fileInfo = new FileInfo(filePath);
				FileInfo fileInfo2 = new FileInfo(tmpPath);
				long num = fileInfo.Length;
				long length = fileInfo2.Length;
				if (num == length)
				{
					bool flag = true;
					using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
					{
						using (FileStream fileStream2 = new FileStream(tmpPath, FileMode.Open, FileAccess.Read))
						{
							byte[] array = new byte[1024];
							byte[] array2 = new byte[1024];
							while (num > 0L)
							{
								int num2 = fileStream.Read(array, 0, 1024);
								fileStream2.Read(array2, 0, 1024);
								for (int i = 0; i < num2; i++)
								{
									if (array[i] != array2[i])
									{
										flag = false;
										break;
									}
								}
								num -= (long)num2;
							}
						}
					}
					if (flag)
					{
						File.Delete(tmpPath);
						return true;
					}
				}
			}
			return false;
		}
	}
}
