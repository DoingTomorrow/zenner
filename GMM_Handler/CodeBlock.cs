// Decompiled with JetBrains decompiler
// Type: GMM_Handler.CodeBlock
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Collections;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  public class CodeBlock
  {
    private static CodeBlock.CodeSequenceTypeIdent[] CodeSequenceTypeIdentList = new CodeBlock.CodeSequenceTypeIdent[20]
    {
      new CodeBlock.CodeSequenceTypeIdent("RAM-Runtime", CodeBlock.CodeSequenceTypes.RAM_Runtime, FrameTypes.None),
      new CodeBlock.CodeSequenceTypeIdent("Interval-Runtime", CodeBlock.CodeSequenceTypes.Interval_Runtime, FrameTypes.None),
      new CodeBlock.CodeSequenceTypeIdent("Event-Runtime", CodeBlock.CodeSequenceTypes.Event_Runtime, FrameTypes.None),
      new CodeBlock.CodeSequenceTypeIdent("RESET-Runtime", CodeBlock.CodeSequenceTypes.RESET_Runtime, FrameTypes.None),
      new CodeBlock.CodeSequenceTypeIdent("EEPROM-Interval-Runtime", CodeBlock.CodeSequenceTypes.EEPROM_Interval_Runtime, FrameTypes.None),
      new CodeBlock.CodeSequenceTypeIdent("Mesurement-Runtime", CodeBlock.CodeSequenceTypes.Mesurement_Runtime, FrameTypes.None),
      new CodeBlock.CodeSequenceTypeIdent("MBus-Runtime", CodeBlock.CodeSequenceTypes.MBus_Runtime, FrameTypes.None),
      new CodeBlock.CodeSequenceTypeIdent("Displaycode", CodeBlock.CodeSequenceTypes.Displaycode, FrameTypes.None),
      new CodeBlock.CodeSequenceTypeIdent("Inline Runtimecode", CodeBlock.CodeSequenceTypes.InlineRuntimecode, FrameTypes.None),
      new CodeBlock.CodeSequenceTypeIdent("BCFrame", CodeBlock.CodeSequenceTypes.Framecode, FrameTypes.BC),
      new CodeBlock.CodeSequenceTypeIdent("EnergyFrame", CodeBlock.CodeSequenceTypes.Framecode, FrameTypes.Energy),
      new CodeBlock.CodeSequenceTypeIdent("FlowFrame", CodeBlock.CodeSequenceTypes.Framecode, FrameTypes.Flow),
      new CodeBlock.CodeSequenceTypeIdent("Framecode", CodeBlock.CodeSequenceTypes.Framecode, FrameTypes.Standard),
      new CodeBlock.CodeSequenceTypeIdent("Input1Frame", CodeBlock.CodeSequenceTypes.Framecode, FrameTypes.Input1),
      new CodeBlock.CodeSequenceTypeIdent("Input1ImpValFrame", CodeBlock.CodeSequenceTypes.Framecode, FrameTypes.Input1ImpVal),
      new CodeBlock.CodeSequenceTypeIdent("Input2Frame", CodeBlock.CodeSequenceTypes.Framecode, FrameTypes.Input2),
      new CodeBlock.CodeSequenceTypeIdent("Input2ImpValFrame", CodeBlock.CodeSequenceTypes.Framecode, FrameTypes.Input2ImpVal),
      new CodeBlock.CodeSequenceTypeIdent("PowerFrame", CodeBlock.CodeSequenceTypes.Framecode, FrameTypes.Power),
      new CodeBlock.CodeSequenceTypeIdent("VolumeFrame", CodeBlock.CodeSequenceTypes.Framecode, FrameTypes.Volume),
      new CodeBlock.CodeSequenceTypeIdent("ImpulsValueFrame", CodeBlock.CodeSequenceTypes.Framecode, FrameTypes.ImpulsValue)
    };
    internal readonly CodeBlock.CodeSequenceTypes CodeSequenceType;
    internal readonly FrameTypes FrameType;
    internal readonly int FunctionNumber;
    internal string CodeSequenceName;
    internal int FunctionMenuIndex = -1;
    internal ArrayList CodeList = new ArrayList();
    internal string SpecialOptions;
    internal string CodeSequenceInfo;

    internal CodeBlock(
      CodeBlock.CodeSequenceTypes TheType,
      FrameTypes TheFrameType,
      int FunctionNumberIn)
    {
      this.CodeSequenceType = TheType;
      this.FrameType = TheFrameType;
      this.FunctionNumber = FunctionNumberIn;
    }

    internal CodeBlock Clone(Meter MyMeter, ArrayList FunctionParameterList)
    {
      return this.CloneFinish(!(this is IntervalAndLogger) ? new CodeBlock(this.CodeSequenceType, this.FrameType, this.FunctionNumber) : (CodeBlock) ((IntervalAndLogger) this).Clone(MyMeter, this.CodeSequenceType, this.FrameType, this.FunctionNumber, FunctionParameterList));
    }

    internal CodeBlock Clone()
    {
      if (this is IntervalAndLogger)
        throw new ArgumentOutOfRangeException("Illegal InterfalAndLogger block");
      return this.CloneFinish(new CodeBlock(this.CodeSequenceType, this.FrameType, this.FunctionNumber));
    }

    private CodeBlock CloneFinish(CodeBlock CodeBlockCopy)
    {
      CodeBlockCopy.CodeSequenceName = this.CodeSequenceName;
      CodeBlockCopy.FunctionMenuIndex = this.FunctionMenuIndex;
      if (this.CodeList != null)
      {
        CodeBlockCopy.CodeList = new ArrayList();
        foreach (CodeObject code in this.CodeList)
          CodeBlockCopy.CodeList.Add((object) code.Clone());
      }
      CodeBlockCopy.SpecialOptions = this.SpecialOptions;
      CodeBlockCopy.CodeSequenceInfo = this.CodeSequenceInfo;
      return CodeBlockCopy;
    }

    internal static bool GetCodeSequenceType(
      string SequenceTypeName,
      out CodeBlock.CodeSequenceTypes CodeSequenceType,
      out FrameTypes FrameType,
      out string SpecialOptions)
    {
      SpecialOptions = string.Empty;
      CodeSequenceType = CodeBlock.CodeSequenceTypes.Unknown;
      FrameType = FrameTypes.None;
      for (int index = 0; index < CodeBlock.CodeSequenceTypeIdentList.Length; ++index)
      {
        if (SequenceTypeName.StartsWith(CodeBlock.CodeSequenceTypeIdentList[index].DatabaseName))
        {
          CodeSequenceType = CodeBlock.CodeSequenceTypeIdentList[index].SequenceType;
          FrameType = CodeBlock.CodeSequenceTypeIdentList[index].FrameType;
          if (SequenceTypeName.Length > CodeBlock.CodeSequenceTypeIdentList[index].DatabaseName.Length)
            SpecialOptions = SequenceTypeName.Substring(CodeBlock.CodeSequenceTypeIdentList[index].DatabaseName.Length).Trim();
          return true;
        }
      }
      return false;
    }

    internal void GetObjectInfo(StringBuilder InfoString, Meter TheMeter)
    {
      if (!TheMeter.MyHandler.MyInfoFlags.ShowBlockTypes)
        return;
      if (this.CodeSequenceType == CodeBlock.CodeSequenceTypes.Displaycode)
        InfoString.Append(ZR_Constants.SystemNewLine);
      InfoString.Append("*** CodeBlock: " + this.CodeSequenceType.ToString());
      if (this.CodeSequenceName != null && this.CodeSequenceName.Length > 0)
        InfoString.Append(": '" + this.CodeSequenceName.PadRight(20) + "'");
      if (TheMeter.MyHandler.MyInfoFlags.ShowFunctionNumbers)
        InfoString.Append(" F:" + this.FunctionNumber.ToString("d4"));
      if (TheMeter.MyHandler.MyInfoFlags.ShowFunctionNames && this.FunctionNumber >= 0)
        InfoString.Append(" Fn:" + ((Function) TheMeter.MyHandler.MyLoadedFunctions.LoadedFunctionHeaders[(object) (ushort) this.FunctionNumber]).Name);
      InfoString.Append(ZR_Constants.SystemNewLine);
    }

    internal enum CodeSequenceTypes
    {
      RAM_Runtime,
      Interval_Runtime,
      Event_Runtime,
      RESET_Runtime,
      EEPROM_Interval_Runtime,
      Mesurement_Runtime,
      MBus_Runtime,
      Displaycode,
      Framecode,
      InlineRuntimecode,
      Brunch,
      LinkerGeneratedCodeBlock,
      Unknown,
    }

    internal struct CodeSequenceTypeIdent
    {
      internal string DatabaseName;
      internal CodeBlock.CodeSequenceTypes SequenceType;
      internal FrameTypes FrameType;

      internal CodeSequenceTypeIdent(
        string DatabaseNameIn,
        CodeBlock.CodeSequenceTypes SequenceTypeIn,
        FrameTypes FrameTypeIn)
      {
        this.DatabaseName = DatabaseNameIn;
        this.SequenceType = SequenceTypeIn;
        this.FrameType = FrameTypeIn;
      }
    }
  }
}
