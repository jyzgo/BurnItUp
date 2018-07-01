using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace MTUnity.Utils
{
	public static class FileUtil
	{
		public static readonly UTF8Encoding utf8NoBOM = new UTF8Encoding (false);

		/// <summary>
		/// <para>将源目录下的内容拷贝到目标目录。</para>
		/// <para>fileFilter 用于过滤要拷贝的文件</para>
		/// <para>skipMetaFile 是否略过 .meta 文件，主要用于Unity编辑器环境中。</para>
		/// <para>sync 是否使用同步模式，如果为 true，则会删除目标目录下的非拷贝内容。</para>
		/// </summary>
		public static void CopyDirectory (string sourcePath, string destinationPath, string fileFilter = "*", bool skipMetaFile = true, bool sync = false)
		{
			if (sourcePath == destinationPath || !Directory.Exists (sourcePath)) {
				return;
			}
			string metaExt = ".meta";
			HashSet<string> pathSet = null;
			if (sync) {
				pathSet = new HashSet<string> ();
			}

			string[] dirPaths = Directory.GetDirectories (sourcePath, "*", SearchOption.AllDirectories);
			foreach (string dirPath in dirPaths) {
				string dirDestPath = dirPath.Replace (sourcePath, destinationPath);
				Directory.CreateDirectory (dirDestPath);
				if (sync) {
					pathSet.Add (dirDestPath);
				}
			}
			if (!Directory.Exists (destinationPath)) {
				Directory.CreateDirectory (destinationPath);
			}

			string[] filePaths = Directory.GetFiles (sourcePath, fileFilter, SearchOption.AllDirectories);
			foreach (string filePath in filePaths) {
				if (filePath.EndsWith (metaExt) && skipMetaFile) {
					continue;
				}

				string fileDestPath = filePath.Replace (sourcePath, destinationPath);
				File.Copy (filePath, fileDestPath, true);
				if (sync) {
					pathSet.Add (fileDestPath);
				}
			}

			if (sync) {
				dirPaths = Directory.GetDirectories (destinationPath, "*", SearchOption.AllDirectories);
				foreach (string dirPath in dirPaths) {
					if (!pathSet.Contains (dirPath) && Directory.Exists (dirPath)) {
						Directory.Delete (dirPath, true);
					}
				}
				filePaths = Directory.GetFiles (destinationPath, "*", SearchOption.AllDirectories);
				foreach (string filePath in filePaths) {
					if (filePath.EndsWith (metaExt) && skipMetaFile) {
						if (pathSet.Contains (filePath.Substring (0, filePath.Length - metaExt.Length))) {
							continue;
						}
					}

					if (!pathSet.Contains (filePath) && File.Exists (filePath)) {
						File.Delete (filePath);
					}
				}
			}
		}

		/// <summary>
		/// <para>读取指定文件的所有字节内容。</para>
		/// <para>成功返回包含所有内容的字节数组，失败返回 null。</para>
		/// </summary>
		public static byte[] ReadAllBytes (string path)
		{
			try {
				return File.ReadAllBytes (path);
			} catch (Exception) {}

			return null;
		}

		/// <summary>
		/// <para>将指定字节内容写入指定文件。</para>
		/// <para>成功返回 true，失败返回 false。</para>
		/// </summary>
		public static bool WriteAllBytes (string path, byte[] bytes)
		{
			try {
				File.WriteAllBytes (path, bytes);
				return true;
			} catch (Exception) {}

			return false;
		}

		/// <summary>
		/// <para>读取指定文件的所有文本内容。</para>
		/// <para>encoding 指定文本编码，默认为不带 BOM 的 Encoding.UTF8。</para>
		/// <para>成功返回包含所有内容的字符串，失败返回 null。</para>
		/// </summary>
		public static string ReadAllText (string path, Encoding encoding = null)
		{
			try {
				return File.ReadAllText (path, encoding == null ? utf8NoBOM : encoding);
			} catch (Exception) {}

			return null;
		}

		/// <summary>
		/// <para>将指定文本内容写入指定文件。</para>
		/// <para>encoding 指定文本编码，默认为不带 BOM 的 Encoding.UTF8。</para>
		/// <para>成功返回 true，失败返回 false。</para>
		/// </summary>
		public static bool WriteAllText (string path, string contents, Encoding encoding = null)
		{
			try {
				File.WriteAllText (path, contents, encoding == null ? utf8NoBOM : encoding);
				return true;
			} catch (Exception) {}

			return false;
		}

		/// <summary>
		/// <para>从指定文件中读取一个对象。</para>
		/// <para>成功返回反序列化后的对象，失败返回 null。</para>
		/// </summary>
		public static object ReadObject (string path)
		{
			FileStream fs = null;
			try {
				fs = File.OpenRead (path);
				BinaryFormatter bf = new BinaryFormatter ();
				return bf.Deserialize (fs);
			} catch (Exception) {
			} finally {
				if (fs != null) {
					try { fs.Close (); } catch (Exception) {}
				}
			}

			return null;
		}

		/// <summary>
		/// <para>将一个对象序列化写入指定文件。</para>
		/// <para>成功返回 true，失败返回 false。</para>
		/// </summary>
		public static bool WriteObject (string path, object o)
		{
			FileStream fs = null;
			try {
				fs = File.Create (path);
				BinaryFormatter bf = new BinaryFormatter ();
				bf.Serialize (fs, o);
				return true;
			} catch (Exception) {
			} finally {
				if (fs != null) {
					try { fs.Close (); } catch (Exception) {}
				}
			}

			return false;
		}

		/// <summary>
		/// <para>重命名指定的文件或目录。</para>
		/// <para>成功返回 true，失败返回 false。</para>
		/// </summary>
		public static bool RenameFile (string path, string newName)
		{
			try {
				FileInfo file = new FileInfo (path);
				if (file.Exists) {
					file.MoveTo (Path.Combine (file.DirectoryName, newName));
					return true;
				}
				DirectoryInfo dir = new DirectoryInfo (path);
				if (dir.Exists) {
					dir.MoveTo (Path.Combine (dir.Parent.FullName, newName));
					return true;
				}
			} catch (Exception) {
			}

			return false;
		}

		public static string ByteArrToString(byte[] content, Encoding encoding = null)
		{
			return (encoding == null ? utf8NoBOM : encoding).GetString (content);
		}

		public static byte[] StringToByteArr(string content, Encoding encoding = null)
		{
			return (encoding == null ? utf8NoBOM : encoding).GetBytes (content);
		}

	}
}
