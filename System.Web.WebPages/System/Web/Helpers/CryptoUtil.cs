// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.CryptoUtil
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace System.Web.Helpers
{
  internal static class CryptoUtil
  {
    private static readonly Func<SHA256> _sha256Factory = CryptoUtil.GetSHA256Factory();

    public static bool AreByteArraysEqual(byte[] a, byte[] b)
    {
      if (a == null || b == null || a.Length != b.Length)
        return false;
      bool flag = true;
      for (int index = 0; index < a.Length; ++index)
        flag &= (int) a[index] == (int) b[index];
      return flag;
    }

    public static byte[] ComputeSHA256(IList<string> parameters)
    {
      using (MemoryStream output = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) output))
        {
          foreach (string parameter in (IEnumerable<string>) parameters)
            binaryWriter.Write(parameter);
          binaryWriter.Flush();
          using (SHA256 shA256 = CryptoUtil._sha256Factory())
            return shA256.ComputeHash(output.GetBuffer(), 0, checked ((int) output.Length));
        }
      }
    }

    private static Func<SHA256> GetSHA256Factory()
    {
      if (!CryptoConfig.AllowOnlyFipsAlgorithms)
        return (Func<SHA256>) (() => (SHA256) new SHA256Managed());
      try
      {
        using (new SHA256Cng())
          return (Func<SHA256>) (() => (SHA256) new SHA256Cng());
      }
      catch (PlatformNotSupportedException ex)
      {
      }
      return (Func<SHA256>) (() => (SHA256) new SHA256CryptoServiceProvider());
    }
  }
}
