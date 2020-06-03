using System;
using System.Management;
using Microsoft.VisualBasic.Devices;

namespace Soranobot.Classes
{
	// Token: 0x0200000D RID: 13
	internal class Identification
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00002F48 File Offset: 0x00001148
		public static string getHardwareID()
		{
			return Misc.hash(Identification.identifier("Win32_Processor", "ProcessorId") + "-" + Identification.identifier("Win32_BIOS", "SerialNumber") + "-" + Identification.identifier("Win32_DiskDrive", "Signature") + "-" + Identification.identifier("Win32_BaseBoard", "SerialNumber") + "-" + Identification.identifier("Win32_VideoController", "Name"));
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002FD0 File Offset: 0x000011D0
		private static string identifier(string wmiClass, string wmiProperty)
		{
			string text = "";
			foreach (ManagementBaseObject managementBaseObject in new ManagementClass(wmiClass).GetInstances())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				if (text == "")
				{
					try
					{
						text = managementObject[wmiProperty].ToString();
						break;
					}
					catch
					{
					}
				}
			}
			return text;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002189 File Offset: 0x00000389
		public static string osName()
		{
			return new ComputerInfo().OSFullName.Replace("Microsoft ", "") + " " + Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
		}
	}
}
