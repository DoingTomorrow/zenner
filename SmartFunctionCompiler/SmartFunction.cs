// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.SmartFunction
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using HandlerLib;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace SmartFunctionCompiler
{
  public class SmartFunction
  {
    public List<RuntimeParameter> AllParameters;
    private List<LineCode> AllLines;
    private ushort ResetCodeStartOffset;
    private ushort EventCodeStartOffset;

    public string FunctionName { get; private set; }

    public byte FunctionVersion { get; private set; }

    public byte InterpreterVersion { get; private set; }

    public FirmwareEvents StartEvent { get; private set; }

    public byte[] Code { get; private set; }

    public SmartFunction(byte[] functionCode)
    {
      try
      {
        this.Code = functionCode;
        Interpreter.Error = SmartFunctionResult.NoError;
        ushort offset = 0;
        this.FunctionName = ByteArrayScanner16Bit.ScanString(functionCode, ref offset);
        this.FunctionVersion = ByteArrayScanner16Bit.ScanByte(functionCode, ref offset);
        this.InterpreterVersion = ByteArrayScanner16Bit.ScanByte(functionCode, ref offset);
        this.StartEvent = (FirmwareEvents) ByteArrayScanner16Bit.ScanByte(functionCode, ref offset);
        byte num1 = ByteArrayScanner16Bit.ScanByte(functionCode, ref offset);
        this.AllParameters = new List<RuntimeParameter>();
        for (int index = 0; index < (int) num1; ++index)
          this.AllParameters.Add(new RuntimeParameter(functionCode, ref offset));
        byte num2 = ByteArrayScanner16Bit.ScanByte(functionCode, ref offset);
        this.EventCodeStartOffset = (ushort) ((uint) offset + (uint) num2);
        this.ResetCodeStartOffset = (ushort) ((uint) this.EventCodeStartOffset - (uint) num2);
        this.AllLines = new List<LineCode>();
        while ((int) offset < functionCode.Length)
          this.AllLines.Add(new LineCode(functionCode, ref offset));
      }
      catch (Exception ex)
      {
        throw new Exception(this.GetUncompiled() + Environment.NewLine, ex);
      }
    }

    public override string ToString()
    {
      return "FunctionName: " + this.FunctionName + "; Version: " + this.FunctionVersion.ToString("d02") + " ; Size: " + this.Code.Length.ToString();
    }

    public string GetUncompiled()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Function name: " + this.FunctionName);
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("Function version: " + this.FunctionVersion.ToString());
      stringBuilder.AppendLine("Interpreter version: " + this.InterpreterVersion.ToString());
      stringBuilder.AppendLine("Event: " + this.StartEvent.ToString());
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("# ************* Parameters *********************");
      StorageTypeCodes? nullable = new StorageTypeCodes?();
      foreach (RuntimeParameter allParameter in this.AllParameters)
      {
        if (!nullable.HasValue || allParameter.StorageCode != nullable.Value)
        {
          stringBuilder.AppendLine();
          stringBuilder.AppendLine(allParameter.StorageCode.ToString());
          nullable = new StorageTypeCodes?(allParameter.StorageCode);
        }
        stringBuilder.AppendLine(allParameter.DeCompile());
      }
      stringBuilder.AppendLine();
      stringBuilder.Append("# ************* Reset code ***************");
      foreach (LineCode allLine in this.AllLines)
        allLine.SetParameterName(this.AllParameters);
      if (this.ResetCodeStartOffset == (ushort) 0)
        throw new Exception("ResetCode not defined");
      if (this.EventCodeStartOffset == (ushort) 0)
        throw new Exception("EventCode not defined");
      if ((int) this.ResetCodeStartOffset == (int) this.EventCodeStartOffset)
        throw new Exception("ResetCode == EventCode");
      SortedList<ushort, string> lableOffsets = new SortedList<ushort, string>();
      lableOffsets.Add(this.ResetCodeStartOffset, "ResetCode");
      lableOffsets.Add(this.EventCodeStartOffset, "EventCode");
      foreach (LineCode allLine in this.AllLines)
        allLine.AddJumpDestinationOffset(lableOffsets);
      foreach (LineCode allLine in this.AllLines)
        allLine.AddJump(lableOffsets);
      foreach (LineCode allLine in this.AllLines)
        allLine.AddLable(lableOffsets);
      for (int index = 1; index < this.AllLines.Count; ++index)
      {
        if (!this.AllLines[index].SourceText.StartsWith(Environment.NewLine) && this.AllLines[index - 1].SourceText.StartsWith("Jump"))
          this.AllLines[index - 1].SourceText += Environment.NewLine;
      }
      foreach (LineCode allLine in this.AllLines)
        stringBuilder.AppendLine(allLine.SourceText);
      return stringBuilder.ToString();
    }
  }
}
