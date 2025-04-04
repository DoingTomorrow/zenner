// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ZR_PlaySound
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace ZR_ClassLibrary
{
  public class ZR_PlaySound
  {
    [DllImport("winmm.dll", CharSet = CharSet.Auto)]
    private static extern int PlaySound(string pszSound, int hmod, int falgs);

    public static void PlaySound(string pszSound)
    {
      try
      {
        if (!(pszSound != string.Empty) || !File.Exists(pszSound))
          return;
        ZR_PlaySound.PlaySound(pszSound, 0, 139264);
      }
      catch
      {
      }
    }

    public static void PlayOKSound()
    {
      try
      {
        string str = Path.Combine(SystemValues.AppPath, "OK.wav");
        if (!(str != string.Empty) || !File.Exists(str))
          return;
        ZR_PlaySound.PlaySound(str, 0, 139265);
      }
      catch
      {
      }
    }

    public static void PlayERRORSound()
    {
      try
      {
        string str = Path.Combine(SystemValues.AppPath, "ERROR.wav");
        if (!(str != string.Empty) || !File.Exists(str))
          return;
        ZR_PlaySound.PlaySound(str, 0, 139265);
      }
      catch
      {
      }
    }

    public static void PlaySoundEvent(string pszSound)
    {
      try
      {
        ZR_PlaySound.PlaySound(pszSound, 0, 73728);
      }
      catch
      {
      }
    }

    public enum SND
    {
      SND_SYNC = 0,
      SND_ASYNC = 1,
      SND_NODEFAULT = 2,
      SND_MEMORY = 4,
      SND_LOOP = 8,
      SND_NOSTOP = 16, // 0x00000010
      SND_PURGE = 64, // 0x00000040
      SND_APPLICATION = 128, // 0x00000080
      SND_NOWAIT = 8192, // 0x00002000
      SND_ALIAS = 65536, // 0x00010000
      SND_FILENAME = 131072, // 0x00020000
      SND_RESOURCE = 262148, // 0x00040004
      SND_ALIAS_ID = 1114112, // 0x00110000
    }
  }
}
