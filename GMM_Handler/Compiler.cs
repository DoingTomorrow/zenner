// Decompiled with JetBrains decompiler
// Type: GMM_Handler.Compiler
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Collections;
using System.Globalization;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  internal class Compiler
  {
    private Meter MyMeter;
    internal SortedList Includes;

    public Compiler(Meter MyMeterIn) => this.MyMeter = MyMeterIn;

    public Compiler Clone(Meter NewMeter)
    {
      return new Compiler(NewMeter)
      {
        Includes = this.Includes
      };
    }

    internal bool SetEpromParametersToDefault()
    {
      foreach (DictionaryEntry allParameter in this.MyMeter.AllParameters)
      {
        Parameter parameter = (Parameter) allParameter.Value;
        if (parameter.ExistOnEprom && !parameter.EpromValueIsInitialised)
          parameter.ValueEprom = parameter.DefaultValue;
      }
      return true;
    }

    internal bool CompileFunctions()
    {
      this.MyMeter.MyLinker.LinkPointerList = new ArrayList();
      for (int index = 0; index < this.MyMeter.MyFunctionTable.FunctionList.Count; ++index)
      {
        Function function = (Function) this.MyMeter.MyFunctionTable.FunctionList[index];
        foreach (CodeBlock runtimeCodeBlock in function.RuntimeCodeBlockList)
        {
          foreach (CodeObject code in runtimeCodeBlock.CodeList)
          {
            if (!this.CompileCodeObject(code))
              return false;
          }
        }
        foreach (MenuItem menu in function.MenuList)
        {
          foreach (CodeBlock displayCodeBlock in menu.DisplayCodeBlocks)
          {
            foreach (CodeObject code in displayCodeBlock.CodeList)
            {
              if (!this.CompileCodeObject(code))
                return false;
            }
          }
        }
      }
      if (this.MyMeter.MyLinker.LinkerCodeBlockList != null)
      {
        foreach (CodeBlock linkerCodeBlock in this.MyMeter.MyLinker.LinkerCodeBlockList)
        {
          foreach (CodeObject code in linkerCodeBlock.CodeList)
          {
            if (!this.CompileCodeObject(code))
              return false;
          }
        }
      }
      return true;
    }

    internal bool GenerateCodeFromCodeBlockList(ArrayList CodeBlockList)
    {
      foreach (CodeBlock codeBlock in CodeBlockList)
      {
        foreach (CodeObject code in codeBlock.CodeList)
          this.GenerateCodeFromCodeObject(code);
      }
      return true;
    }

    internal void GenerateCodeFromCodeObject(CodeObject TheCodeObject)
    {
      TheCodeObject.LinkByteList = new byte[TheCodeObject.Size];
      for (int index = 0; index < TheCodeObject.Size; ++index)
        TheCodeObject.LinkByteList[index] = (byte) (TheCodeObject.CodeValueCompiled >> index * 8);
    }

    internal void GenerateCodeFromCodeObjectAndCopyToEprom(CodeObject TheCodeObject)
    {
      this.GenerateCodeFromCodeObject(TheCodeObject);
      this.CopyToEprom(TheCodeObject);
    }

    internal bool PriCompileCodeObject(CodeObject TheCodeObject)
    {
      switch (TheCodeObject.CodeType)
      {
        case CodeObject.CodeTypes.BYTE:
          TheCodeObject.Size = 1;
          break;
        case CodeObject.CodeTypes.ePTR:
          TheCodeObject.Size = 2;
          break;
        case CodeObject.CodeTypes.iPTR:
          TheCodeObject.Size = 2;
          break;
        case CodeObject.CodeTypes.LONG:
          TheCodeObject.Size = 4;
          break;
        case CodeObject.CodeTypes.WORD:
          TheCodeObject.Size = 2;
          break;
        default:
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal code object");
          return false;
      }
      return true;
    }

    internal bool CompileCodeObject(CodeObject TheCodeObject)
    {
      switch (TheCodeObject.CodeType)
      {
        case CodeObject.CodeTypes.BYTE:
          if (!this.CompileIncludesAndConstants(TheCodeObject, 1))
            return false;
          break;
        case CodeObject.CodeTypes.ePTR:
          if (!this.AddPointer(TheCodeObject))
            return false;
          break;
        case CodeObject.CodeTypes.iPTR:
          if (!this.AddPointer(TheCodeObject))
            return false;
          break;
        case CodeObject.CodeTypes.LONG:
          if (!this.CompileIncludesAndConstants(TheCodeObject, 4))
            return false;
          break;
        case CodeObject.CodeTypes.WORD:
          if (!this.CompileIncludesAndConstants(TheCodeObject, 2))
            return false;
          break;
        default:
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal code object");
          return false;
      }
      return true;
    }

    internal bool AddPointer(CodeObject TheCodeObject)
    {
      this.MyMeter.MyLinker.LinkPointerList.Add((object) new LinkPointer()
      {
        LinkPointerType = (TheCodeObject.CodeType != CodeObject.CodeTypes.ePTR ? LinkPointer.LinkPointerTypes.iPTR : LinkPointer.LinkPointerTypes.ePTR),
        PointerObject = TheCodeObject
      });
      return true;
    }

    internal bool CompileIncludesAndConstants(CodeObject TheCodeObject, int ByteSize)
    {
      string[] strArray = TheCodeObject.CodeValue.Split(' ');
      TheCodeObject.CodeValueCompiled = 0L;
      int index1 = 0;
      try
      {
        for (index1 = 0; index1 < strArray.Length; ++index1)
        {
          if (strArray[index1].Length >= 1)
          {
            if (strArray[index1].StartsWith("0x"))
              TheCodeObject.CodeValueCompiled = (long) ((uint) TheCodeObject.CodeValueCompiled | uint.Parse(strArray[index1].Substring(2), NumberStyles.HexNumber));
            else if (char.IsDigit(strArray[index1][0]))
              TheCodeObject.CodeValueCompiled = (long) ((uint) TheCodeObject.CodeValueCompiled | uint.Parse(strArray[index1]));
            else if (this.Includes.ContainsKey((object) strArray[index1]))
              TheCodeObject.CodeValueCompiled = (long) ((uint) TheCodeObject.CodeValueCompiled | (uint) (int) this.Includes[(object) strArray[index1]]);
          }
        }
        TheCodeObject.Size = ByteSize;
        TheCodeObject.LinkByteList = new byte[ByteSize];
        for (int index2 = 0; index2 < ByteSize; ++index2)
          TheCodeObject.LinkByteList[index2] = (byte) (TheCodeObject.CodeValueCompiled >> 8 * index2);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Unkonwn compiler constante: " + ZR_Constants.SystemNewLine + strArray[index1] + ZR_Constants.SystemNewLine + ex.ToString());
        return false;
      }
      return true;
    }

    internal bool CompileStringToByte(string CodeString, out byte ByteValue)
    {
      ByteValue = (byte) 0;
      string[] strArray = CodeString.Split(' ');
      try
      {
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (strArray[index].Length >= 1)
          {
            if (strArray[index].StartsWith("0x"))
              ByteValue += byte.Parse(strArray[index].Substring(2), NumberStyles.HexNumber);
            else if (char.IsDigit(strArray[index][0]))
              ByteValue += byte.Parse(strArray[index]);
            else
              ByteValue += (byte) (int) this.Includes[(object) strArray[index]];
          }
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Unkonwn compiler constante: " + ZR_Constants.SystemNewLine + ex.ToString());
        return false;
      }
      return true;
    }

    internal bool GenerateMenuObjects()
    {
      this.MyMeter.MyDisplayCode.GenerateMenuItemLists();
      for (int index1 = 0; index1 < this.MyMeter.MyDisplayCode.AllMenuItems.Count; ++index1)
      {
        MenuItem allMenuItem = (MenuItem) this.MyMeter.MyDisplayCode.AllMenuItems[index1];
        MenuItemBrunch menuItemBrunch = new MenuItemBrunch(allMenuItem, this.MyMeter.MyDisplayCode.AllMenuItems);
        CodeObject codeObject = new CodeObject((int) allMenuItem.FunctionNumber);
        codeObject.Size = 1;
        codeObject.CodeType = CodeObject.CodeTypes.BYTE;
        codeObject.CodeValue = string.Empty;
        menuItemBrunch.CodeList.Add((object) codeObject);
        string EventPointer;
        if (menuItemBrunch.ClickAvailable)
        {
          if (!this.GetEventPointer(allMenuItem, allMenuItem.ClickEvent, out EventPointer))
            return false;
          if (EventPointer.Length > 0)
          {
            if (EventPointer != ".")
            {
              this.AddBrunchPointer(menuItemBrunch.CodeList, (int) allMenuItem.FunctionNumber, EventPointer);
              codeObject.CodeValue += "DII_BY_CLICK_JUMP ";
            }
          }
          else
            codeObject.CodeValue += "DII_BY_CLICK_FOLLOW ";
        }
        if (menuItemBrunch.PressAvailable)
        {
          if (!this.GetEventPointer(allMenuItem, allMenuItem.PressEvent, out EventPointer))
            return false;
          if (EventPointer.Length > 0)
          {
            if (EventPointer != ".")
            {
              this.AddBrunchPointer(menuItemBrunch.CodeList, (int) allMenuItem.FunctionNumber, EventPointer);
              codeObject.CodeValue += "DII_BY_PRESS_JUMP ";
            }
          }
          else
            codeObject.CodeValue += "DII_BY_PRESS_FOLLOW ";
        }
        if (menuItemBrunch.HoldAvailable)
        {
          if (!this.GetEventPointer(allMenuItem, allMenuItem.HoldEvent, out EventPointer))
            return false;
          if (EventPointer.Length > 0)
          {
            if (EventPointer != ".")
            {
              this.AddBrunchPointer(menuItemBrunch.CodeList, (int) allMenuItem.FunctionNumber, EventPointer);
              codeObject.CodeValue += "DII_BY_HOLD_JUMP ";
            }
          }
          else
            codeObject.CodeValue += "DII_BY_HOLD_FOLLOW ";
        }
        if (menuItemBrunch.TimeoutAvailable)
        {
          if (!this.GetEventPointer(allMenuItem, allMenuItem.TimeoutEvent, out EventPointer))
            return false;
          if (EventPointer.Length > 0)
          {
            if (EventPointer != ".")
            {
              this.AddBrunchPointer(menuItemBrunch.CodeList, (int) allMenuItem.FunctionNumber, EventPointer);
              codeObject.CodeValue += "DII_BY_TIMEOUT_JUMP";
            }
          }
          else
            codeObject.CodeValue += "DII_BY_TIMEOUT_FOLLOW";
        }
        if (codeObject.CodeValue != string.Empty)
          allMenuItem.DisplayCodeBlocks.Add((object) menuItemBrunch);
        for (int index2 = 0; index2 < allMenuItem.DisplayCodeBlocks.Count; ++index2)
        {
          foreach (CodeObject code in ((CodeBlock) allMenuItem.DisplayCodeBlocks[index2]).CodeList)
          {
            if (code.CodeType == CodeObject.CodeTypes.ePTR)
            {
              Function destinationFunction = this.GetRelativeDestinationFunction(allMenuItem, code.CodeValue);
              if (destinationFunction != null)
              {
                MenuItem menu = (MenuItem) destinationFunction.MenuList[0];
                code.LineInfo = code.LineInfo == null ? code.CodeValue : code.CodeValue + "  " + code.LineInfo;
                code.CodeValue = destinationFunction.Name + "." + menu.MenuName;
              }
            }
          }
        }
      }
      return true;
    }

    private void AddBrunchPointer(ArrayList CodeList, int FunctionNumber, string EventPointer)
    {
      CodeObject codeObject = new CodeObject(FunctionNumber);
      codeObject.Size = 2;
      codeObject.CodeType = CodeObject.CodeTypes.ePTR;
      codeObject.CodeValue = EventPointer;
      CodeList.Add((object) codeObject);
    }

    internal bool GetEventPointer(MenuItem TheMenuItem, string Event, out string EventPointer)
    {
      MenuItem menuItem = (MenuItem) null;
      EventPointer = string.Empty;
      try
      {
        if (Event.Length == 0)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Missing event data");
          return false;
        }
        Function function = this.GetRelativeDestinationFunction(TheMenuItem, Event);
        if (function != null)
        {
          if (function.MenuList.Count == 0)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Empty menu list on: " + function.Name);
            return false;
          }
          menuItem = (MenuItem) function.MenuList[0];
        }
        else
        {
          function = TheMenuItem.MyFunction;
          foreach (MenuItem menu in TheMenuItem.MyFunction.MenuList)
          {
            if (menu.MenuName == Event)
              menuItem = menu;
          }
          if (TheMenuItem == null)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Missing menu item");
            return false;
          }
        }
        if (TheMenuItem == menuItem)
        {
          EventPointer = ".";
          return true;
        }
        if (this.MyMeter.MyDisplayCode.AllMenuItems.IndexOf((object) menuItem) == this.MyMeter.MyDisplayCode.AllMenuItems.IndexOf((object) TheMenuItem) + 1 && TheMenuItem.MyFunction.MenuList.Count == 1 && menuItem.MyFunction.ColumnNumber == TheMenuItem.MyFunction.ColumnNumber && TheMenuItem.MyFunction.RowNumber + 1 < 40)
          return true;
        EventPointer = function.Name + "." + menuItem.MenuName;
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Missing event data -> " + ex.ToString());
        return false;
      }
    }

    private Function GetRelativeDestinationFunction(MenuItem TheMenuItem, string Event)
    {
      Function destinationFunction = (Function) null;
      switch (Event)
      {
        case "MAIN":
          destinationFunction = (Function) this.MyMeter.MyFunctionTable.FunctionList[0];
          break;
        case "RIGHT":
          int index = TheMenuItem.MyFunction.ColumnNumber + 1;
          if (index < this.MyMeter.MyFunctionTable.FirstFunctionInColumn.Count - 1)
          {
            do
            {
              destinationFunction = (Function) this.MyMeter.MyFunctionTable.FirstFunctionInColumn[index];
              ++index;
            }
            while (destinationFunction == null && index < this.MyMeter.MyFunctionTable.FirstFunctionInColumn.Count - 1);
            if (destinationFunction == null)
            {
              destinationFunction = (Function) this.MyMeter.MyFunctionTable.FunctionList[0];
              break;
            }
            break;
          }
          destinationFunction = (Function) this.MyMeter.MyFunctionTable.FunctionList[0];
          break;
        case "NEXT":
          int num = this.MyMeter.MyFunctionTable.FunctionList.IndexOf((object) TheMenuItem.MyFunction);
          if (num + 1 < this.MyMeter.MyFunctionTable.FunctionList.Count)
          {
            destinationFunction = (Function) this.MyMeter.MyFunctionTable.FunctionList[num + 1];
            if (destinationFunction.ColumnNumber != TheMenuItem.MyFunction.ColumnNumber)
            {
              destinationFunction = (Function) this.MyMeter.MyFunctionTable.FirstFunctionInColumn[TheMenuItem.MyFunction.ColumnNumber];
              break;
            }
            break;
          }
          destinationFunction = (Function) this.MyMeter.MyFunctionTable.FirstFunctionInColumn[TheMenuItem.MyFunction.ColumnNumber];
          break;
        case "FIRST":
          destinationFunction = (Function) this.MyMeter.MyFunctionTable.FirstFunctionInColumn[TheMenuItem.MyFunction.ColumnNumber];
          break;
      }
      return destinationFunction;
    }

    internal bool CopyToEprom()
    {
      int blockStartAddress = this.MyMeter.MyEpromHeader.BlockStartAddress;
      foreach (LinkBlock linkBlock in this.MyMeter.MyLinker.LinkBlockList)
      {
        try
        {
          linkBlock.BlockStartAddress = blockStartAddress;
          foreach (object linkObj1 in linkBlock.LinkObjList)
          {
            if (linkObj1 is Parameter)
            {
              LinkObj linkObj2 = (LinkObj) linkObj1;
              for (int index = 0; index < linkObj2.LinkByteList.Length; ++index)
                this.MyMeter.Eprom[blockStartAddress++] = linkObj2.LinkByteList[index];
            }
            else
            {
              foreach (LinkObj code in ((CodeBlock) linkObj1).CodeList)
              {
                for (int index = 0; index < code.LinkByteList.Length; ++index)
                  this.MyMeter.Eprom[blockStartAddress++] = code.LinkByteList[index];
              }
            }
          }
        }
        catch
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "CopyToEEProm: " + blockStartAddress.ToString("x"));
          return false;
        }
      }
      return true;
    }

    internal void CopyToEprom(CodeObject TheCodeObject)
    {
      int address = TheCodeObject.Address;
      for (int index = 0; index < TheCodeObject.LinkByteList.Length; ++index)
        this.MyMeter.Eprom[address++] = TheCodeObject.LinkByteList[index];
    }
  }
}
