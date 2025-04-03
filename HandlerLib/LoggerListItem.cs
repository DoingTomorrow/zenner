// Decompiled with JetBrains decompiler
// Type: HandlerLib.LoggerListItem
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public class LoggerListItem
  {
    public string LoggerName { get; set; }

    public ushort Entries { get; set; }

    public ushort MaxEntries { get; set; }

    public LoggerListItem(string loggerName, ushort entries, ushort maxEntries)
    {
      this.LoggerName = loggerName;
      this.Entries = entries;
      this.MaxEntries = maxEntries;
    }

    public LoggerListItem(byte[] data, ref int offset)
    {
      this.LoggerName = ByteArrayScanner.ScanString(data, ref offset);
      this.Entries = ByteArrayScanner.ScanUInt16(data, ref offset);
      this.MaxEntries = ByteArrayScanner.ScanUInt16(data, ref offset);
    }
  }
}
