// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.Compiler
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

#nullable disable
namespace SmartFunctionCompiler
{
  internal class Compiler
  {
    internal static Compiler TheOnlyOneCompiler;
    internal string FunctionDescription;
    internal string RequiredFunctions;
    internal string MemberOfGroups;
    private string FunctionName;
    private byte FunctionVersion;
    private byte MinimalInterpreterVersion;
    private FirmwareEvents Event;
    private List<RuntimeParameter> AllParameters;
    internal List<LineCode> Codes;
    internal byte[] CompiledFunctionCode;
    private int ResetCodeBytes;
    private string[] sourceLines;

    internal Compiler(string[] SourceLines) => this.sourceLines = SourceLines;

    internal string Compile()
    {
      int lineNumber = 0;
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        string[] poorCodeLines = this.GetPoorCodeLines(this.sourceLines);
        this.FunctionName = HeaderInfo.GetTokenValue("Function name:", poorCodeLines, ref lineNumber);
        this.RequiredFunctions = HeaderInfo.GetTokenValue("Required functions:", poorCodeLines, ref lineNumber, true, (int) byte.MaxValue, true);
        this.MemberOfGroups = HeaderInfo.GetTokenValue("Member of groups:", poorCodeLines, ref lineNumber, true, (int) byte.MaxValue, true);
        this.FunctionVersion = byte.Parse(HeaderInfo.GetTokenValue("Function version:", poorCodeLines, ref lineNumber));
        this.MinimalInterpreterVersion = byte.Parse(HeaderInfo.GetTokenValue("Interpreter version:", poorCodeLines, ref lineNumber));
        this.Event = (FirmwareEvents) Enum.Parse(typeof (FirmwareEvents), HeaderInfo.GetTokenValue("Event:", poorCodeLines, ref lineNumber));
        this.AllParameters = new List<RuntimeParameter>();
        StorageTypeCodes storageCode = StorageTypeCodes.flash;
        while (true)
        {
          RuntimeParameter runtimeParameter = RuntimeParameter.GetRuntimeParameter(poorCodeLines, ref storageCode, ref lineNumber);
          if (runtimeParameter != null)
          {
            if (runtimeParameter.Name == "Logger")
            {
              if (this.AllParameters.Count == 0)
              {
                if (runtimeParameter.StorageCode == StorageTypeCodes.flash && (this.Event < FirmwareEvents.Hour || this.Event > FirmwareEvents.Year))
                  goto label_6;
              }
              else
                break;
            }
            this.AllParameters.Add(runtimeParameter);
          }
          else
            goto label_9;
        }
        throw new Exception("The Logger parameter has to be always the first parameter.");
label_6:
        throw new Exception("Cycle < 1 hour is not allowed for flash loggers.");
label_9:
        this.Codes = new List<LineCode>();
        for (; lineNumber < poorCodeLines.Length; ++lineNumber)
        {
          LineCode lineCode = new LineCode(poorCodeLines, this.AllParameters, ref lineNumber);
          if (lineCode.SourceLine >= 0)
          {
            if (this.Codes.Count > 0 && this.Codes[this.Codes.Count - 1].Nead_IsAccuA_NaN_follow && lineCode.Opcode != (byte) 8)
              stringBuilder.AppendLine("Function: " + this.FunctionName + Environment.NewLine + "Warning on line " + lineNumber.ToString() + ": The code " + this.Codes[this.Codes.Count - 1].SourceText + " nead a check for " + OpcodeNoParameter.IsAccuA_NaN.ToString());
            this.Codes.Add(lineCode);
          }
        }
        this.CreateBinaryCode();
        return stringBuilder.ToString();
      }
      catch (Exception ex)
      {
        throw new Exception("Error on line " + lineNumber.ToString() + " -> " + ex.Message);
      }
    }

    public string[] GetPoorCodeLines(string[] sourceLines)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string[] poorCodeLines = new string[sourceLines.Length];
      bool flag = false;
      for (int index = 0; index < sourceLines.Length; ++index)
      {
        int length = sourceLines[index].IndexOf('#');
        if (length >= 0)
        {
          if (length > 0)
            flag = true;
          if (!flag)
          {
            if (sourceLines[index].Length > 1)
              stringBuilder.AppendLine(sourceLines[index].Substring(1));
            else
              stringBuilder.AppendLine();
          }
          poorCodeLines[index] = sourceLines[index].Substring(0, length).Trim();
        }
        else
        {
          string str = sourceLines[index].Trim();
          if (str.Length != 0)
            flag = true;
          if (!flag)
            stringBuilder.AppendLine();
          poorCodeLines[index] = str;
        }
      }
      this.FunctionDescription = stringBuilder.ToString();
      return poorCodeLines;
    }

    private void CreateBinaryCode()
    {
      List<byte> collection = new List<byte>();
      collection.AddRange((IEnumerable<byte>) RuntimeParameter.ByteArrayFromString(this.FunctionName));
      collection.Add(this.FunctionVersion);
      collection.Add(this.MinimalInterpreterVersion);
      collection.Add((byte) this.Event);
      collection.Add((byte) this.AllParameters.Count);
      foreach (RuntimeParameter allParameter in this.AllParameters)
        collection.AddRange((IEnumerable<byte>) allParameter.ParameterBytes);
      List<byte> byteList1 = new List<byte>((IEnumerable<byte>) collection);
      byteList1.Add((byte) 0);
      SortedList<string, LineCode> labelList = new SortedList<string, LineCode>();
      foreach (LineCode code in this.Codes)
      {
        code.AddLableToList(labelList);
        code.CodeStartOffset = (ushort) byteList1.Count;
        byteList1.AddRange((IEnumerable<byte>) code.CodeBytes);
      }
      int index = labelList.IndexOfKey("EventCode");
      if (index < 0)
        throw new Exception("EventCode: lable missed");
      this.ResetCodeBytes = (int) labelList.Values[index].CodeStartOffset - collection.Count - 1;
      if (this.ResetCodeBytes > (int) byte.MaxValue)
        throw new Exception("ResetCode to long");
      List<byte> byteList2 = new List<byte>((IEnumerable<byte>) collection);
      byteList2.Add((byte) this.ResetCodeBytes);
      foreach (LineCode code in this.Codes)
      {
        code.SetJumpOffset(labelList);
        byteList2.AddRange((IEnumerable<byte>) code.CodeBytes);
      }
      this.CompiledFunctionCode = byteList2.ToArray();
    }

    internal static byte[] GetBinaryCodeFromText(string text)
    {
      string[] strArray = text.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      List<byte> byteList = new List<byte>();
      foreach (string str1 in strArray)
      {
        if (str1.StartsWith("0x") && !(str1.Substring(5, 2) != ": "))
        {
          for (int startIndex = 7; startIndex + 1 < str1.Length; startIndex += 2)
          {
            string str2 = str1.Substring(startIndex, 2);
            byte result;
            if (!str2.Contains<char>(' ') && !str2.Contains<char>('.') && byte.TryParse(str2, NumberStyles.HexNumber, (IFormatProvider) null, out result))
              byteList.Add(result);
            else
              break;
          }
        }
      }
      return byteList.ToArray();
    }

    public override string ToString()
    {
      if (this.Codes == null)
        return "Not compiled";
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("*** Compiler result ".PadRight(60, '*'));
      ushort num1 = 0;
      stringBuilder1.AppendLine(this.GetOffsetHeader(ref num1, this.FunctionName.Length + 1) + "FunctionName: " + this.FunctionName.ToString());
      stringBuilder1.AppendLine(this.GetOffsetHeader(ref num1, 1) + "FunctionVersion: " + this.FunctionVersion.ToString());
      stringBuilder1.AppendLine(this.GetOffsetHeader(ref num1, 1) + "MinimalInterpreterVersion: " + this.MinimalInterpreterVersion.ToString());
      stringBuilder1.AppendLine(this.GetOffsetHeader(ref num1, 1) + "Event: " + this.Event.ToString());
      stringBuilder1.AppendLine(this.GetOffsetHeader(ref num1, 1) + "Number of parameters: " + this.AllParameters.Count.ToString());
      stringBuilder1.AppendLine();
      if (this.AllParameters.Count > 0)
      {
        stringBuilder1.AppendLine("*** Parameters ".PadRight(60, '*'));
        foreach (RuntimeParameter allParameter in this.AllParameters)
        {
          ushort codeOffset = num1;
          RuntimeParameter runtimeParameter = new RuntimeParameter(this.CompiledFunctionCode, ref num1);
          stringBuilder1.AppendLine(runtimeParameter.ToString(ref codeOffset));
        }
      }
      stringBuilder1.AppendLine(this.GetOffsetHeader(ref num1, 1) + "Number of reset code bytes: " + this.ResetCodeBytes.ToString());
      stringBuilder1.AppendLine();
      stringBuilder1.AppendLine("*** Code lines ".PadRight(60, '*'));
      foreach (LineCode code in this.Codes)
        stringBuilder1.Append(code.ToString());
      stringBuilder1.AppendLine();
      stringBuilder1.AppendLine("*** Complete code ".PadRight(60, '*'));
      stringBuilder1.AppendLine("Number of bytes: " + this.CompiledFunctionCode.Length.ToString());
      stringBuilder1.Append("Code as hex string: ");
      stringBuilder1.AppendLine(Compiler.GetHexStringFromArray(this.CompiledFunctionCode));
      stringBuilder1.Append("Code as init list: ");
      stringBuilder1.AppendLine(Compiler.GetHexInitStringFromArray(this.CompiledFunctionCode));
      stringBuilder1.AppendLine();
      stringBuilder1.AppendLine("*** Code view including ascii characters ".PadRight(60, '*'));
      StringBuilder stringBuilder2 = new StringBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      foreach (byte num2 in this.CompiledFunctionCode)
      {
        stringBuilder2.Append(num2.ToString("x02"));
        if (num2 >= (byte) 20 && num2 < (byte) 125)
        {
          string str = " " + Encoding.ASCII.GetString(new byte[1]
          {
            num2
          });
          stringBuilder3.Append(str);
        }
        else
          stringBuilder3.Append("..");
      }
      int num3 = 0;
      int num4 = 0;
      int index1 = 0;
      for (int index2 = 0; index2 < stringBuilder2.Length; ++index2)
      {
        stringBuilder1.Append(stringBuilder2[index2]);
        ++num3;
        ++num4;
        if (num3 == 100 || index2 == stringBuilder2.Length - 1)
        {
          num3 = 0;
          stringBuilder1.AppendLine();
          for (; index1 < num4; ++index1)
            stringBuilder1.Append(stringBuilder3[index1]);
          stringBuilder1.AppendLine();
          stringBuilder1.AppendLine();
        }
      }
      return stringBuilder1.ToString();
    }

    private string GetOffsetHeader(ref ushort codeOffset, int byteSize)
    {
      StringBuilder stringBuilder = new StringBuilder("0x" + codeOffset.ToString("x03") + ": ");
      int num1 = byteSize;
      int num2 = 20;
      if (byteSize > num2)
        num2 = byteSize;
      for (int index = 0; index < num2; ++index)
      {
        if (num1 > 0)
        {
          --num1;
          stringBuilder.Append(this.CompiledFunctionCode[(int) codeOffset + index].ToString("x02"));
        }
        else
          stringBuilder.Append(". ");
      }
      if (stringBuilder[stringBuilder.Length - 1] != ' ')
        stringBuilder.Append(" ");
      codeOffset += (ushort) byteSize;
      return stringBuilder.ToString();
    }

    public static string GetHexStringFromArray(byte[] bytes, int startOffset = 0, bool splitBytes = false)
    {
      if (startOffset >= bytes.Length)
        throw new Exception("startOffset out of range");
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = startOffset; index < bytes.Length; ++index)
      {
        if (splitBytes && stringBuilder.Length > 0)
          stringBuilder.Append(' ');
        stringBuilder.Append(bytes[index].ToString("x02"));
      }
      return stringBuilder.ToString();
    }

    public static string GetHexInitStringFromArray(byte[] bytes)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte num in bytes)
      {
        if (stringBuilder.Length == 0)
          stringBuilder.Append("0x");
        else
          stringBuilder.Append(",0x");
        stringBuilder.Append(num.ToString("x02"));
      }
      return stringBuilder.ToString();
    }
  }
}
