using System;
using System.Security.Cryptography;

namespace MTUnity {
	public class MTSecurity {

		private static MD5CryptoServiceProvider _md5 = null;
		private static System.Text.UTF8Encoding _ue = null;
		public static string Md5Sum(string strToEncrypt) {
			if (_ue == null) {
				_ue = new System.Text.UTF8Encoding();;
			}
			byte[] bytes = _ue.GetBytes(strToEncrypt);

			// encrypt bytes
			if (_md5 == null) {
				_md5 = new MD5CryptoServiceProvider();
			}
			byte[] hashBytes = _md5.ComputeHash(bytes);

			// Convert the encrypted bytes back to a string (base 16)
			string hashString = "";

			for (int i = 0; i < hashBytes.Length; i++)
			{
				hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
			}

			return hashString.PadLeft(32, '0');
		}
	}
}

