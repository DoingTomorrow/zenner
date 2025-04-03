// Decompiled with JetBrains decompiler
// Type: GMM_Handler.MenuItem
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System.Collections;

#nullable disable
namespace GMM_Handler
{
  internal class MenuItem
  {
    internal readonly ushort FunctionNumber;
    internal readonly string MenuName;
    internal Function MyFunction;
    internal int MenuIndex;
    internal int XPos;
    internal int YPos;
    internal readonly int InterpreterCode;
    internal ArrayList DisplayCodeBlocks = new ArrayList();
    internal string ClickEvent;
    internal string PressEvent;
    internal string HoldEvent;
    internal string TimeoutEvent;
    internal string Description;

    internal MenuItem(ushort FunctionNumberIn, string MenuNameIn, int InterpreterCodeReference)
    {
      this.FunctionNumber = FunctionNumberIn;
      this.MenuName = MenuNameIn;
      this.InterpreterCode = InterpreterCodeReference;
    }

    internal MenuItem Clone()
    {
      MenuItem menuItem = new MenuItem(this.FunctionNumber, this.MenuName, this.InterpreterCode);
      menuItem.XPos = this.XPos;
      menuItem.YPos = this.YPos;
      if (this.DisplayCodeBlocks != null)
      {
        menuItem.DisplayCodeBlocks = new ArrayList();
        foreach (CodeBlock displayCodeBlock in this.DisplayCodeBlocks)
          menuItem.DisplayCodeBlocks.Add((object) displayCodeBlock.Clone());
      }
      menuItem.ClickEvent = this.ClickEvent;
      menuItem.PressEvent = this.PressEvent;
      menuItem.HoldEvent = this.HoldEvent;
      menuItem.TimeoutEvent = this.TimeoutEvent;
      menuItem.Description = this.Description;
      return menuItem;
    }
  }
}
