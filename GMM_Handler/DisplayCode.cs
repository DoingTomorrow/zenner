// Decompiled with JetBrains decompiler
// Type: GMM_Handler.DisplayCode
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System.Collections;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  internal class DisplayCode : LinkBlock
  {
    internal ArrayList AllMenuItems;
    internal SortedList AllMenusByName;
    internal ArrayList FrameCodesList;
    private static string[] SecmentByteNames = new string[7]
    {
      "DII_FRAME_SEGS1",
      "DII_FRAME_SEGS2",
      "DII_FRAME_SEGS3",
      "DII_FRAME_SEGS4",
      "DII_FRAME_MENUPOS",
      "DII_FRAME_BLINK",
      "DII_FRAME_TEXT"
    };
    private const int BaseSecCount = 4;
    private byte[] SecmentByteCodes;

    internal DisplayCode(Meter MyMeterIn)
      : base(MyMeterIn, LinkBlockTypes.DisplayCode)
    {
    }

    internal bool GenerateMenuItemLists()
    {
      this.AllMenuItems = new ArrayList();
      this.AllMenusByName = new SortedList();
      this.FrameCodesList = new ArrayList();
      this.MyMeter.MyFunctionTable.FirstFunctionInColumn = new ArrayList();
      int index1 = -1;
      int num1 = 0;
      int num2 = 0;
      short num3 = 0;
      for (int index2 = 0; index2 < this.MyMeter.MyFunctionTable.FunctionList.Count; ++index2)
      {
        Function function = (Function) this.MyMeter.MyFunctionTable.FunctionList[index2];
        if (index2 >= (int) num3)
        {
          ++index1;
          num1 = 0;
          while (index1 < this.MyMeter.MyFunctionTable.FunctionStartIndexOfMenuColumnList.Count - 1 && (int) (short) this.MyMeter.MyFunctionTable.FunctionStartIndexOfMenuColumnList[index1] == (int) (short) this.MyMeter.MyFunctionTable.FunctionStartIndexOfMenuColumnList[index1 + 1])
          {
            ++index1;
            this.MyMeter.MyFunctionTable.FirstFunctionInColumn.Add((object) null);
          }
          this.MyMeter.MyFunctionTable.FirstFunctionInColumn.Add((object) function);
          num3 = index1 >= this.MyMeter.MyFunctionTable.FunctionStartIndexOfMenuColumnList.Count - 1 ? (short) 9999 : (short) this.MyMeter.MyFunctionTable.FunctionStartIndexOfMenuColumnList[index1 + 1];
        }
        function.ColumnNumber = index1;
        function.RowNumber = num1++;
        foreach (MenuItem menu in function.MenuList)
        {
          menu.MyFunction = function;
          menu.MenuIndex = num2++;
          this.AllMenuItems.Add((object) menu);
          this.AllMenusByName.Add((object) (menu.MyFunction.Name + "." + menu.MenuName), (object) menu);
          foreach (CodeBlock displayCodeBlock in menu.DisplayCodeBlocks)
          {
            if (displayCodeBlock.CodeSequenceType == CodeBlock.CodeSequenceTypes.Framecode)
              this.FrameCodesList.Add((object) displayCodeBlock);
          }
        }
      }
      return true;
    }

    internal bool AdjustDisplayPositions()
    {
      foreach (CodeBlock frameCodes in this.FrameCodesList)
      {
        if (!this.SetDisplayPositionInfo(frameCodes))
          return false;
      }
      return true;
    }

    internal bool AdjustFunctions()
    {
      if (!this.AdjustFrameCodes())
        return false;
      int index = this.MyMeter.MyFunctionTable.FunctionListByNumber.IndexOfKey((object) (ushort) 292);
      if (index >= 0)
      {
        CodeBlock displayCodeBlock = (CodeBlock) ((MenuItem) ((Function) this.MyMeter.MyFunctionTable.FunctionListByNumber.GetByIndex(index)).MenuList[3]).DisplayCodeBlocks[1];
        ((CodeObject) displayCodeBlock.CodeList[6]).CodeValue = (1U << (int) this.MyMeter.MyMath.MyBaseSettings.Vol_SumExpo).ToString();
        int num = ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Vol_VolSum"]).AddressCPU + 4;
        ((CodeObject) displayCodeBlock.CodeList[11]).CodeValue = num.ToString();
      }
      return true;
    }

    internal bool AdjustFrameCodes()
    {
      foreach (CodeBlock frameCodes in this.FrameCodesList)
      {
        if (frameCodes.FrameType == FrameTypes.None)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Illegal frame type");
        }
        else
        {
          if (!this.SetDisplayPositionInfo(frameCodes))
            return false;
          MeterMath.FrameDescription TheFrame;
          int Shift;
          if (frameCodes.FrameType != FrameTypes.Standard && this.MyMeter.MyMath.GetSpecialOverrideFrame(frameCodes.FrameType, frameCodes.SpecialOptions, out TheFrame, out Shift))
          {
            if (Shift != 0)
            {
              Function function = (Function) this.MyMeter.MyFunctionTable.FunctionListByNumber[(object) (ushort) frameCodes.FunctionNumber];
              bool flag1 = false;
              bool flag2 = false;
              foreach (MenuItem menu in function.MenuList)
              {
                foreach (CodeBlock displayCodeBlock in menu.DisplayCodeBlocks)
                {
                  if (frameCodes == displayCodeBlock)
                    flag2 = true;
                  if (flag2 && displayCodeBlock.CodeSequenceType == CodeBlock.CodeSequenceTypes.InlineRuntimecode)
                  {
                    foreach (CodeObject code in displayCodeBlock.CodeList)
                    {
                      if (code.OverrideMark == "_HR_ENERGY")
                      {
                        int num1;
                        try
                        {
                          num1 = int.Parse(code.CodeValue);
                        }
                        catch
                        {
                          return this.MyMeter.MyHandler.AddErrorPointMessage("Illegal factor");
                        }
                        int num2 = num1 + Shift;
                        code.LineInfo = "@old: " + code.CodeValue;
                        code.CodeValue = num2.ToString();
                        flag1 = true;
                      }
                    }
                  }
                }
              }
              if (!flag1)
                return this.MyMeter.MyHandler.AddErrorPointMessage("_HR_ENERGY not found");
            }
            if (!this.GenerateNewFrame(frameCodes, TheFrame))
              return false;
          }
        }
      }
      return true;
    }

    private bool SetDisplayPositionInfo(CodeBlock FrameBlock)
    {
      ArrayList codeList = FrameBlock.CodeList;
      try
      {
        this.MyMeter.MyCompiler.CompileCodeObject((CodeObject) FrameBlock.CodeList[0]);
        byte codeValueCompiled = (byte) ((CodeObject) FrameBlock.CodeList[0]).CodeValueCompiled;
        if (((int) codeValueCompiled & 16) == 0)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal frame control byte");
          return false;
        }
        short index1 = 1;
        for (byte index2 = 1; index2 < (byte) 16; index2 <<= 1)
        {
          if (((int) codeValueCompiled & (int) index2) > 0)
            ++index1;
        }
        Function function = (Function) this.MyMeter.MyFunctionTable.FunctionListByNumber[(object) (ushort) FrameBlock.FunctionNumber];
        byte num = (byte) (((uint) (byte) (function.ColumnNumber & 15) << 4) + (uint) (byte) (function.RowNumber & 15));
        ((CodeObject) FrameBlock.CodeList[(int) index1]).CodeValue = num.ToString();
      }
      catch
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Display position error");
        return false;
      }
      return true;
    }

    private bool GenerateNewFrame(CodeBlock FrameBlock, MeterMath.FrameDescription NewFrame)
    {
      if (FrameBlock.FrameType == FrameTypes.BC)
      {
        if (NewFrame.FrameByteDescription.Length != 4 || FrameBlock.CodeList.Count != 11)
          return this.MyMeter.MyHandler.AddErrorPointMessage("Illegal BCFrame");
        for (int index = 0; index < 4; ++index)
          ((CodeObject) FrameBlock.CodeList[index + 7]).CodeValue = NewFrame.FrameByteDescription[index];
        return true;
      }
      if (FrameBlock.FrameType == FrameTypes.ImpulsValue)
      {
        if (NewFrame.FrameByteDescription.Length != 12 || FrameBlock.CodeList.Count != 13)
          return this.MyMeter.MyHandler.AddErrorPointMessage("Illegal ImpulsValueFrame");
        for (int index = 0; index < NewFrame.FrameByteDescription.Length; ++index)
        {
          if (!(NewFrame.FrameByteDescription[index] == "-"))
            ((CodeObject) FrameBlock.CodeList[index + 1]).CodeValue = NewFrame.FrameByteDescription[index];
        }
        return true;
      }
      bool[] flagArray = new bool[DisplayCode.SecmentByteNames.Length];
      CodeObject code1 = (CodeObject) FrameBlock.CodeList[0];
      code1.LineInfo = "@old: " + code1.CodeValue;
      if (!this.GarantHeaderStringFormat(code1))
        return false;
      for (int index = 0; index < DisplayCode.SecmentByteNames.Length; ++index)
      {
        if (code1.CodeValue.IndexOf(DisplayCode.SecmentByteNames[index]) >= 0)
        {
          flagArray[index] = true;
        }
        else
        {
          flagArray[index] = false;
          if (index < 4)
          {
            CodeObject codeObject = new CodeObject(code1.FunctionNumber);
            codeObject.CodeType = CodeObject.CodeTypes.BYTE;
            codeObject.CodeValue = string.Empty;
            codeObject.Size = 1;
            FrameBlock.CodeList.Insert(index + 1, (object) codeObject);
          }
        }
      }
      StringBuilder stringBuilder = new StringBuilder(300);
      for (int index1 = 0; index1 < 4 && index1 < NewFrame.FrameByteDescription.Length; ++index1)
      {
        CodeObject code2 = (CodeObject) FrameBlock.CodeList[index1 + 1];
        string[] strArray1 = code2.CodeValue.Split(' ');
        string[] strArray2 = NewFrame.FrameByteDescription[index1].Split(' ');
        stringBuilder.Length = 0;
        for (int index2 = 0; index2 < strArray2.Length; ++index2)
        {
          if (strArray2[index2].Length > 0)
            stringBuilder.Append(strArray2[index2] + " ");
        }
        for (int index3 = 0; index3 < strArray1.Length; ++index3)
        {
          if (strArray1[index3].Length > 0 && stringBuilder.ToString().IndexOf(strArray1[index3]) < 0 && ZelsiusMath.ClearZelsiusUnitFrameMasks[index1].IndexOf(strArray1[index3]) < 0)
            stringBuilder.Append(strArray1[index3] + " ");
        }
        code2.LineInfo = "@old: " + code2.CodeValue;
        code2.CodeValue = stringBuilder.ToString().Trim();
      }
      stringBuilder.Length = 0;
      for (int index = DisplayCode.SecmentByteNames.Length - 1; index >= 0; --index)
      {
        if (index >= 4)
        {
          if (flagArray[index])
            stringBuilder.Append(DisplayCode.SecmentByteNames[index] + " ");
        }
        else if (flagArray[index] || ((CodeObject) FrameBlock.CodeList[index + 1]).CodeValue.Length > 0)
          stringBuilder.Append(DisplayCode.SecmentByteNames[index] + " ");
        else
          FrameBlock.CodeList.RemoveAt(index + 1);
      }
      code1.CodeValue = stringBuilder.ToString();
      return true;
    }

    private bool GarantHeaderStringFormat(CodeObject HeaderCodeObject)
    {
      if (!char.IsDigit(HeaderCodeObject.CodeValue[0]))
        return true;
      if (this.SecmentByteCodes == null)
      {
        this.SecmentByteCodes = new byte[DisplayCode.SecmentByteNames.Length];
        for (int index = 0; index < this.SecmentByteCodes.Length; ++index)
        {
          if (!this.MyMeter.MyCompiler.CompileStringToByte(DisplayCode.SecmentByteNames[index], out this.SecmentByteCodes[index]))
            return false;
        }
      }
      StringBuilder stringBuilder = new StringBuilder(300);
      int num;
      try
      {
        num = int.Parse(HeaderCodeObject.CodeValue);
      }
      catch
      {
        return false;
      }
      for (int index = 0; index < this.SecmentByteCodes.Length; ++index)
      {
        if (((uint) num & (uint) this.SecmentByteCodes[index]) > 0U)
          stringBuilder.Append(DisplayCode.SecmentByteNames[index] + " ");
      }
      HeaderCodeObject.CodeValue = stringBuilder.ToString().Trim();
      CodeObject codeObject = HeaderCodeObject;
      codeObject.LineInfo = codeObject.LineInfo + " = " + HeaderCodeObject.CodeValue;
      return true;
    }
  }
}
