// Decompiled with JetBrains decompiler
// Type: GMM_Handler.CodeObject
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  internal class CodeObject : LinkObj
  {
    private static string[] CodeTypeNames = Enum.GetNames(typeof (CodeObject.CodeTypes));
    internal readonly int FunctionNumber;
    internal CodeObject.CodeTypes CodeType;
    internal string CodeValue;
    internal long CodeValueCompiled = -1;
    internal string OverrideMark;
    internal int CodeID;
    internal int LineNr;
    internal string LineInfo;

    internal CodeObject(int TheFunctionNumber) => this.FunctionNumber = TheFunctionNumber;

    internal override void GetObjectInfo(StringBuilder InfoString, Meter TheMeter)
    {
      int RAM_Address = -1;
      this.GetObjectInfo(InfoString, TheMeter, ref RAM_Address);
    }

    internal override void GetObjectInfo(
      StringBuilder InfoString,
      Meter TheMeter,
      ref int RAM_Address)
    {
      if (this.Address < 0)
        InfoString.Append("----");
      else
        InfoString.Append(this.Address.ToString("x04"));
      if (RAM_Address >= 0)
      {
        InfoString.Append("=" + RAM_Address.ToString("x04"));
        RAM_Address += this.Size;
      }
      InfoString.Append(TheMeter.GetEEPromWriteProtectionChar(this.LinkByteList, this.Address));
      if (this.Size < 1)
      {
        InfoString.Append(" -- -- -- --");
      }
      else
      {
        for (int index = 0; index < 4; ++index)
        {
          if (this.LinkByteList == null)
            InfoString.Append(" --");
          else if (index >= this.LinkByteList.Length)
            InfoString.Append(" ..");
          else
            InfoString.Append(" " + this.LinkByteList[index].ToString("x02"));
        }
      }
      InfoString.Append(" T:");
      InfoString.Append(this.CodeType.ToString().PadRight(15));
      InfoString.Append(" V:0x");
      switch (this.CodeType)
      {
        case CodeObject.CodeTypes.BYTE:
          InfoString.Append(this.CodeValueCompiled.ToString("x02"));
          InfoString.Append(" = ");
          InfoString.Append(this.CodeValueCompiled.ToString("d03"));
          break;
        case CodeObject.CodeTypes.ePTR:
          InfoString.Append(this.CodeValueCompiled.ToString("x04"));
          break;
        case CodeObject.CodeTypes.iPTR:
          InfoString.Append(this.CodeValueCompiled.ToString("x04"));
          break;
        case CodeObject.CodeTypes.LONG:
          InfoString.Append(this.CodeValueCompiled.ToString("x08"));
          InfoString.Append(" = ");
          InfoString.Append(this.CodeValueCompiled.ToString("d010"));
          break;
        case CodeObject.CodeTypes.WORD:
          InfoString.Append(this.CodeValueCompiled.ToString("x04"));
          InfoString.Append(" = ");
          InfoString.Append(this.CodeValueCompiled.ToString("d05"));
          break;
      }
      InfoString.Append(" ->'" + this.CodeValue + "'");
      if (this.LineInfo != null && this.LineInfo.Length > 0)
        InfoString.Append(" ### '" + this.LineInfo + "'");
      InfoString.Append(ZR_Constants.SystemNewLine);
    }

    internal CodeObject Clone()
    {
      CodeObject codeObject = new CodeObject(this.FunctionNumber);
      codeObject.Address = this.Address;
      codeObject.Size = this.Size;
      if (this.LinkByteList != null)
        codeObject.LinkByteList = (byte[]) this.LinkByteList.Clone();
      if (this.LinkByteComment != null)
        codeObject.LinkByteComment = (string[]) this.LinkByteComment.Clone();
      codeObject.CodeType = this.CodeType;
      codeObject.CodeValue = this.CodeValue;
      codeObject.CodeValueCompiled = this.CodeValueCompiled;
      if (this.OverrideMark != null)
        codeObject.OverrideMark = this.OverrideMark;
      codeObject.CodeID = this.CodeID;
      codeObject.LineNr = this.LineNr;
      codeObject.LineInfo = this.LineInfo;
      return codeObject;
    }

    internal static CodeObject GetPointerCodeObject(string Info, string TheValue)
    {
      CodeObject pointerCodeObject = new CodeObject(-1);
      pointerCodeObject.LineInfo = Info;
      pointerCodeObject.Size = 2;
      pointerCodeObject.CodeValue = TheValue;
      return pointerCodeObject;
    }

    internal static CodeObject GetCodeObject(string Info, ulong TheValue, int Size)
    {
      CodeObject codeObject = new CodeObject(-1);
      codeObject.LineInfo = Info;
      codeObject.Size = Size;
      codeObject.CodeValueCompiled = (long) TheValue;
      return codeObject;
    }

    internal static CodeObject GetCodeObject(string Info, ushort TheValue)
    {
      CodeObject codeObject = new CodeObject(-1);
      codeObject.LineInfo = Info;
      codeObject.Size = 2;
      codeObject.CodeValueCompiled = (long) TheValue;
      return codeObject;
    }

    internal static CodeObject GetCodeObject(string Info, byte TheValue)
    {
      CodeObject codeObject = new CodeObject(-1);
      codeObject.LineInfo = Info;
      codeObject.Size = 1;
      codeObject.CodeValueCompiled = (long) TheValue;
      return codeObject;
    }

    internal enum CodeTypes
    {
      BYTE,
      ePTR,
      iPTR,
      LONG,
      WORD,
    }
  }
}
