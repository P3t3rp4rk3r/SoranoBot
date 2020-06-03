using System;
using System.IO;

namespace Soranobot
{
	// Token: 0x02000002 RID: 2
	internal static class Helper
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002052 File Offset: 0x00000252
		public static string GetRandomString()
		{
			return Path.GetRandomFileName().Replace(".", "");
		}
	}
}
