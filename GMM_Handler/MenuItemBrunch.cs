// Decompiled with JetBrains decompiler
// Type: GMM_Handler.MenuItemBrunch
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System.Collections;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  internal class MenuItemBrunch : CodeBlock
  {
    internal MenuItem MyMenuItem;
    internal bool ClickAvailable = false;
    internal bool PressAvailable = false;
    internal bool HoldAvailable = false;
    internal bool TimeoutAvailable = false;

    internal MenuItemBrunch(MenuItem MenuItemIn, ArrayList MenuItemList)
      : base(CodeBlock.CodeSequenceTypes.Brunch, FrameTypes.None, (int) MenuItemIn.FunctionNumber)
    {
      this.MyMenuItem = MenuItemIn;
      if (this.MyMenuItem.ClickEvent != "NONE")
        this.ClickAvailable = true;
      if (this.MyMenuItem.PressEvent != "NONE")
        this.PressAvailable = true;
      if (this.MyMenuItem.HoldEvent != "NONE")
        this.HoldAvailable = true;
      if (!(this.MyMenuItem.TimeoutEvent != "NONE"))
        return;
      this.TimeoutAvailable = true;
    }
  }
}
