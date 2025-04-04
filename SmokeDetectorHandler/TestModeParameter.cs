// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.TestModeParameter
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace SmokeDetectorHandler
{
  public class TestModeParameter
  {
    public byte[] Buffer { get; private set; }

    public RadioMode RadioMode { get; set; }

    public FunctionTestMode FunctionTestMode { get; set; }

    public byte[] InternalDiagnosticParameter { get; set; }

    internal static TestModeParameter Parse(byte[] buffer)
    {
      if (buffer == null)
        throw new NullReferenceException("Can not parse the test mode parameters of smoke detector! The buffer is null.");
      if (buffer.Length != 12)
        throw new ArgumentException("Can not parse the test mode parameters of smoke detector! Unknown buffer. Length: " + buffer.Length.ToString() + " bytes.");
      TestModeParameter testModeParameter = new TestModeParameter();
      testModeParameter.Buffer = buffer;
      testModeParameter.RadioMode = (RadioMode) buffer[0];
      testModeParameter.FunctionTestMode = (FunctionTestMode) buffer[1];
      byte[] dst = new byte[10];
      System.Buffer.BlockCopy((Array) buffer, 2, (Array) dst, 0, dst.Length);
      testModeParameter.InternalDiagnosticParameter = dst;
      return testModeParameter;
    }

    public string ToString(int spaces)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Radio Mode: ".PadRight(spaces, ' ')).AppendLine(this.RadioMode.ToString());
      stringBuilder.Append("Function Test Mode: ".PadRight(spaces, ' ')).AppendLine(this.FunctionTestMode.ToString());
      stringBuilder.Append("Internal Diagnostic Parameter: ".PadRight(spaces, ' ')).AppendLine(Util.ByteArrayToHexString(this.InternalDiagnosticParameter));
      stringBuilder.Append("Buffer: ").AppendLine(Util.ByteArrayToHexString(this.Buffer));
      return stringBuilder.ToString();
    }

    internal TestModeParameter DeepCopy()
    {
      return new TestModeParameter()
      {
        Buffer = this.Buffer,
        RadioMode = this.RadioMode,
        FunctionTestMode = this.FunctionTestMode,
        InternalDiagnosticParameter = this.InternalDiagnosticParameter
      };
    }

    public byte[] CreateWriteBuffer()
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) this.RadioMode);
      byteList.Add((byte) this.FunctionTestMode);
      if (this.InternalDiagnosticParameter != null)
        byteList.AddRange((IEnumerable<byte>) this.InternalDiagnosticParameter);
      return byteList.ToArray();
    }
  }
}
