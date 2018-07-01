using UnityEngine;
using System.Collections.Generic;

namespace MTUnity.Utils
{
	public class AssetManager
	{
		static Material _spritesDefaultMaterial;
		static Dictionary<Color, Texture2D> _smallTextures = new Dictionary<Color, Texture2D> ();

		/// <summary>
		/// 获取 Sprites-Default 材质。
		/// </summary>
		public static Material spritesDefaultMaterial {
			get {
				if (_spritesDefaultMaterial == null) {
					GameObject go = new GameObject ();
					SpriteRenderer sr = go.AddComponent<SpriteRenderer> ();
					_spritesDefaultMaterial = sr.sharedMaterial;
					GameObject.Destroy (go);
				}
				return _spritesDefaultMaterial;
			}
		}

		/// <summary>
		/// 获取一个 4x4 大小和指定颜色的纹理。
		/// </summary>
		public static Texture2D GetSmallTexture (Color c)
		{
			if (!_smallTextures.ContainsKey (c)) {
				Texture2D tex = new Texture2D (4, 4, TextureFormat.RGBA32, false);
				tex.anisoLevel = 0;
				for (int x = 0; x < 4; x++) {
					for (int y = 0; y < 4; y++) {
						tex.SetPixel (x, y, c);
					}
				}
				tex.Apply ();
				_smallTextures.Add (c, tex);
			}
			return _smallTextures [c];
		}

		/// <summary>
		/// 获取文件路径的 URL 表示形式。
		/// </summary>
		public static string GetFileURL (string filePath)
		{
			string url = "";
			#if UNITY_WEBGL
			url = "";
			#elif UNITY_ANDROID
			url = filePath.StartsWith ("jar:file://") ? "" : "file://";
			#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_WINRT || UNITY_WINRT_8_0 || UNITY_WINRT_8_1 || UNITY_WINRT_10_0
			url = "file:///";
			#else
			url = "file://";
			#endif
			url = System.Uri.EscapeUriString (url + filePath);
			return url;
		}

	}
}