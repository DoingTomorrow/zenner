// Decompiled with JetBrains decompiler
// Type: MinoConnect.MinoConnectVersions
// Assembly: MinoConnect, Version=1.5.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E4D0ECC-943B-4E96-B8E2-CE02CEE9906B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinoConnect.exe

using System;

#nullable disable
namespace MinoConnect
{
  public class MinoConnectVersions
  {
    public int VersionNumber;
    public int MainVersion;
    public int SubVersion;
    public int Revision;
    public MinoConnectVersions.MinoConnectTypes MinoConnectType;

    public MinoConnectVersions(string versionText)
    {
      this.VersionNumber = 0;
      try
      {
        if (versionText != null && versionText.Length > 20 && versionText.StartsWith("#Ver:"))
        {
          string[] strArray = versionText.Split('|')[0].Substring(5).Split('.');
          if (strArray.Length == 3)
          {
            this.MainVersion = int.Parse(strArray[0]);
            this.SubVersion = int.Parse(strArray[1]);
            this.Revision = int.Parse(strArray[2]);
            if (versionText.Contains("|BTVer"))
              this.MinoConnectType = MinoConnectVersions.MinoConnectTypes.Bluetooth;
            else if (versionText.Contains("|USB"))
              this.MinoConnectType = MinoConnectVersions.MinoConnectTypes.USB;
            else if (versionText.Contains("|RS232"))
              this.MinoConnectType = MinoConnectVersions.MinoConnectTypes.RS232;
            else if (versionText.Contains("|USB_RADIO"))
              this.MinoConnectType = MinoConnectVersions.MinoConnectTypes.USB_RADIO;
            else if (versionText.Contains("|USB_RADIO_X"))
            {
              this.MinoConnectType = MinoConnectVersions.MinoConnectTypes.USB_RADIO_PROTOTYPE;
            }
            else
            {
              if (!versionText.Contains("|HW_UnKnown"))
                throw new Exception("Unknown MinoConnect type");
              this.MinoConnectType = MinoConnectVersions.MinoConnectTypes.UNKNOWN;
            }
            this.VersionNumber = (this.MainVersion << 16) + (this.SubVersion << 8) + this.Revision;
          }
        }
      }
      catch
      {
      }
      if (this.VersionNumber == 0)
        throw new Exception("Illegal MinoConnect version string");
    }

    public enum MinoConnectTypes
    {
      Bluetooth,
      USB,
      USB_RADIO,
      USB_RADIO_PROTOTYPE,
      RS232,
      UNKNOWN,
    }
  }
}
