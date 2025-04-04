// Decompiled with JetBrains decompiler
// Type: S4_Handler.S4_SmartFunctionInfo
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using System.Text;

#nullable disable
namespace S4_Handler
{
  public class S4_SmartFunctionInfo
  {
    public byte InterpreterVersion { get; private set; }

    public byte NumberOfLoadedFunctions { get; private set; }

    public ushort FlashBytesAvailable { get; private set; }

    public ushort FlashBytesUsed { get; private set; }

    public ushort RamBytesAvailable { get; private set; }

    public ushort RamBytesUsed { get; private set; }

    public ushort BackupBytesAvailable { get; private set; }

    public ushort BackupBytesUsed { get; private set; }

    internal S4_SmartFunctionInfo(byte interpreterVersion)
    {
      this.InterpreterVersion = interpreterVersion;
    }

    public S4_SmartFunctionInfo(byte[] receivedData)
    {
      int offset = 0;
      this.InterpreterVersion = ByteArrayScanner.ScanByte(receivedData, ref offset);
      this.NumberOfLoadedFunctions = ByteArrayScanner.ScanByte(receivedData, ref offset);
      this.FlashBytesAvailable = ByteArrayScanner.ScanUInt16(receivedData, ref offset);
      this.FlashBytesUsed = ByteArrayScanner.ScanUInt16(receivedData, ref offset);
      this.RamBytesAvailable = ByteArrayScanner.ScanUInt16(receivedData, ref offset);
      this.RamBytesUsed = ByteArrayScanner.ScanUInt16(receivedData, ref offset);
      this.BackupBytesAvailable = ByteArrayScanner.ScanUInt16(receivedData, ref offset);
      this.BackupBytesUsed = ByteArrayScanner.ScanUInt16(receivedData, ref offset);
    }

    public override string ToString()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.Append("IVer:" + this.InterpreterVersion.ToString());
      stringBuilder1.Append("; FCount:" + this.NumberOfLoadedFunctions.ToString());
      StringBuilder stringBuilder2 = stringBuilder1;
      ushort num1 = this.FlashBytesUsed;
      string str1 = num1.ToString();
      num1 = this.FlashBytesAvailable;
      string str2 = num1.ToString();
      string str3 = "; Flash:" + str1 + "/" + str2;
      stringBuilder2.Append(str3);
      StringBuilder stringBuilder3 = stringBuilder1;
      ushort num2 = this.RamBytesUsed;
      string str4 = num2.ToString();
      num2 = this.RamBytesAvailable;
      string str5 = num2.ToString();
      string str6 = "; RAM:" + str4 + "/" + str5;
      stringBuilder3.Append(str6);
      StringBuilder stringBuilder4 = stringBuilder1;
      ushort num3 = this.BackupBytesUsed;
      string str7 = num3.ToString();
      num3 = this.BackupBytesAvailable;
      string str8 = num3.ToString();
      string str9 = "; Backup:" + str7 + "/" + str8;
      stringBuilder4.Append(str9);
      return stringBuilder1.ToString();
    }

    public string ToTextBlock()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("*** Smart function Info ***");
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("Interpreter version: " + this.InterpreterVersion.ToString());
      stringBuilder.AppendLine("Number of loaded functions: " + this.NumberOfLoadedFunctions.ToString());
      stringBuilder.AppendLine("Available flash bytes: " + this.FlashBytesAvailable.ToString());
      stringBuilder.AppendLine("Used flash bytes: " + this.FlashBytesUsed.ToString());
      stringBuilder.AppendLine("Available RAM bytes: " + this.RamBytesAvailable.ToString());
      stringBuilder.AppendLine("Used RAM bytes: " + this.RamBytesUsed.ToString());
      stringBuilder.AppendLine("Available backup bytes: " + this.BackupBytesAvailable.ToString());
      stringBuilder.AppendLine("Used backup bytes: " + this.BackupBytesUsed.ToString());
      return stringBuilder.ToString();
    }
  }
}
