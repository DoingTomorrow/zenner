// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.KeysRequiredEventArgs
// Assembly: ICSharpCode.SharpZipLib, Version=0.85.5.452, Culture=neutral, PublicKeyToken=1b03e6acf1164f73
// MVID: 6C7CFC05-3661-46A0-98AB-FC6F81793937
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ICSharpCode.SharpZipLib.dll

using System;

#nullable disable
namespace ICSharpCode.SharpZipLib.Zip
{
  public class KeysRequiredEventArgs : EventArgs
  {
    private string fileName;
    private byte[] key;

    public KeysRequiredEventArgs(string name) => this.fileName = name;

    public KeysRequiredEventArgs(string name, byte[] keyValue)
    {
      this.fileName = name;
      this.key = keyValue;
    }

    public string FileName => this.fileName;

    public byte[] Key
    {
      get => this.key;
      set => this.key = value;
    }
  }
}
