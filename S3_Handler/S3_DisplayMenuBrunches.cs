// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_DisplayMenuBrunches
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class S3_DisplayMenuBrunches : S3_MemoryBlock
  {
    private const byte DII_BY_CLICK_FOLLOW = 8;
    private const byte DII_BY_PRESS_FOLLOW = 4;
    private const byte DII_BY_HOLD_FOLLOW = 2;
    private const byte DII_BY_TIMEOUT_FOLLOW = 1;
    private const byte DII_BY_CLICK_JUMP = 128;
    private const byte DII_BY_PRESS_JUMP = 64;
    private const byte DII_BY_HOLD_JUMP = 32;
    private const byte DII_BY_TIMEOUT_JUMP = 16;
    internal S3_DisplayMenu MyMenu;
    internal byte BrunchControl;
    internal ushort ClickAddress;
    internal ushort PressAddress;
    internal ushort HoldAddress;
    internal ushort TimeoutAddress;
    internal string ClickPointerName;
    internal string PressPointerName;
    internal string HoldPointerName;
    internal string TimeoutPointerName;
    internal string followName;

    internal bool ClickJump
    {
      get => ((uint) this.BrunchControl & 128U) > 0U;
      set
      {
        if (value)
        {
          this.BrunchControl &= (byte) 247;
          this.BrunchControl |= (byte) 128;
        }
        else
          this.BrunchControl &= (byte) 127;
      }
    }

    internal bool PressJump
    {
      get => ((uint) this.BrunchControl & 64U) > 0U;
      set
      {
        if (value)
        {
          this.BrunchControl &= (byte) 251;
          this.BrunchControl |= (byte) 64;
        }
        else
          this.BrunchControl &= (byte) 191;
      }
    }

    internal bool HoldJump
    {
      get => ((uint) this.BrunchControl & 32U) > 0U;
      set
      {
        if (value)
        {
          this.BrunchControl &= (byte) 253;
          this.BrunchControl |= (byte) 32;
        }
        else
          this.BrunchControl &= (byte) 223;
      }
    }

    internal bool TimeoutJump
    {
      get => ((uint) this.BrunchControl & 16U) > 0U;
      set
      {
        if (value)
        {
          this.BrunchControl &= (byte) 254;
          this.BrunchControl |= (byte) 16;
        }
        else
          this.BrunchControl &= (byte) 239;
      }
    }

    internal bool ClickFollow
    {
      get => ((uint) this.BrunchControl & 8U) > 0U;
      set
      {
        if (value)
        {
          this.BrunchControl &= (byte) 127;
          this.BrunchControl |= (byte) 8;
        }
        else
          this.BrunchControl &= (byte) 247;
      }
    }

    internal bool PressFollow
    {
      get => ((uint) this.BrunchControl & 4U) > 0U;
      set
      {
        if (value)
        {
          this.BrunchControl &= (byte) 191;
          this.BrunchControl |= (byte) 4;
        }
        else
          this.BrunchControl &= (byte) 251;
      }
    }

    internal bool HoldFollow
    {
      get => ((uint) this.BrunchControl & 2U) > 0U;
      set
      {
        if (value)
        {
          this.BrunchControl &= (byte) 223;
          this.BrunchControl |= (byte) 2;
        }
        else
          this.BrunchControl &= (byte) 253;
      }
    }

    internal bool TimeoutFollow
    {
      get => ((uint) this.BrunchControl & 1U) > 0U;
      set
      {
        if (value)
        {
          this.BrunchControl &= (byte) 239;
          this.BrunchControl |= (byte) 1;
        }
        else
          this.BrunchControl &= (byte) 254;
      }
    }

    public S3_DisplayMenuBrunches(
      S3_Meter MyMeter,
      S3_DisplayMenu MyMenu,
      S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.DisplayJumpBlock, parentMemoryBlock)
    {
      this.MyMenu = MyMenu;
      this.Alignment = 1;
      this.ByteSize = 1;
    }

    public S3_DisplayMenuBrunches(
      S3_Meter MyMeter,
      S3_DisplayMenu MyMenu,
      S3_MemoryBlock parentMemoryBlock,
      S3_DisplayMenuBrunches sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, (S3_MemoryBlock) sourceMemoryBlock)
    {
      this.MyMenu = MyMenu;
      this.Alignment = 1;
      this.ByteSize = 1;
    }

    public S3_DisplayMenuBrunches(
      S3_Meter MyMeter,
      S3_DisplayMenu MyMenu,
      S3_MemoryBlock parentMemoryBlock,
      int insertIndex)
      : base(MyMeter, S3_MemorySegment.DisplayJumpBlock, parentMemoryBlock, insertIndex)
    {
      this.MyMenu = MyMenu;
      this.Alignment = 1;
      this.ByteSize = 1;
    }

    internal S3_DisplayMenuBrunches Clone(
      S3_Meter theCloneMeter,
      S3_DisplayMenu theCloneMenu,
      S3_MemoryBlock cloneParentMemoryBlock)
    {
      return new S3_DisplayMenuBrunches(theCloneMeter, theCloneMenu, cloneParentMemoryBlock, this);
    }

    internal bool AdjustPointersAndSize()
    {
      this.ByteSize = 1;
      this.followName = string.Empty;
      this.BrunchControl = (byte) 0;
      if (!this.AdjustPointerNames())
        return false;
      int num1 = this.parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) this);
      if (this.parentMemoryBlock.childMemoryBlocks.Count > num1 + 1)
      {
        S3_DisplayMenu childMemoryBlock = (S3_DisplayMenu) this.parentMemoryBlock.childMemoryBlocks[num1 + 1];
        if (((S3_FunctionLayer) childMemoryBlock.My_S3_Function.parentMemoryBlock).LayerNr == ((S3_FunctionLayer) this.MyMenu.My_S3_Function.parentMemoryBlock).LayerNr)
          this.followName = childMemoryBlock.MenuName;
      }
      else
      {
        int num2 = this.parentMemoryBlock.parentMemoryBlock.childMemoryBlocks.IndexOf(this.parentMemoryBlock);
        if (this.parentMemoryBlock.parentMemoryBlock.childMemoryBlocks.Count > num2 + 1)
        {
          List<S3_MemoryBlock> childMemoryBlocks = this.parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[num2 + 1].childMemoryBlocks;
          if (childMemoryBlocks == null || childMemoryBlocks.Count == 0)
            return false;
          S3_DisplayMenu s3DisplayMenu = (S3_DisplayMenu) childMemoryBlocks[0];
          if (((S3_FunctionLayer) s3DisplayMenu.My_S3_Function.parentMemoryBlock).LayerNr == ((S3_FunctionLayer) this.MyMenu.My_S3_Function.parentMemoryBlock).LayerNr)
            this.followName = s3DisplayMenu.MenuName;
        }
      }
      if (this.ClickPointerName != null)
      {
        if (this.ClickPointerName == this.followName)
        {
          this.ClickFollow = true;
        }
        else
        {
          this.ClickJump = true;
          this.ByteSize += 2;
        }
      }
      if (this.PressPointerName != null)
      {
        if (this.PressPointerName == this.followName)
        {
          this.PressFollow = true;
        }
        else
        {
          this.PressJump = true;
          this.ByteSize += 2;
        }
      }
      if (this.HoldPointerName != null)
      {
        if (this.HoldPointerName == this.followName)
        {
          this.HoldFollow = true;
        }
        else
        {
          this.HoldJump = true;
          this.ByteSize += 2;
        }
      }
      if (this.TimeoutPointerName != null)
      {
        if (this.TimeoutPointerName == this.followName)
        {
          this.TimeoutFollow = true;
        }
        else
        {
          this.TimeoutJump = true;
          this.ByteSize += 2;
        }
      }
      return true;
    }

    internal void GetFollowInfo(ref StringBuilder infoText)
    {
      int length = infoText.Length;
      infoText.Insert(length, " Follow ->");
      if (this.ClickFollow)
        infoText.Append(" Click:   " + this.ClickPointerName);
      if (this.PressFollow)
        infoText.Append(" Press:   " + this.PressPointerName);
      if (this.HoldFollow)
        infoText.Append(" Hold:    " + this.HoldPointerName);
      if (this.TimeoutFollow)
        infoText.Append(" Timeout: " + this.PressPointerName);
      if (length != infoText.Length)
        return;
      infoText.Insert(length, " None");
    }

    private bool AdjustPointerNames()
    {
      return this.AdjustPointerName(ref this.ClickPointerName) && this.AdjustPointerName(ref this.PressPointerName) && this.AdjustPointerName(ref this.HoldPointerName) && this.AdjustPointerName(ref this.TimeoutPointerName);
    }

    private bool AdjustPointerName(ref string pointerString)
    {
      if (pointerString != null)
      {
        if (pointerString == "MAIN")
        {
          S3_Function childMemoryBlock = (S3_Function) this.MyMenu.My_S3_Function.parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[1].childMemoryBlocks[0];
          if (childMemoryBlock.DisplayCode == null)
          {
            if (this.MyMenu.My_S3_Function.parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[1].childMemoryBlocks.Count <= 1)
              throw new NotImplementedException();
            childMemoryBlock = (S3_Function) this.MyMenu.My_S3_Function.parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[1].childMemoryBlocks[1];
          }
          pointerString = childMemoryBlock.Name + "." + childMemoryBlock.Name;
        }
        else if (pointerString == "FIRST")
        {
          S3_Function childMemoryBlock = (S3_Function) this.MyMenu.My_S3_Function.parentMemoryBlock.childMemoryBlocks[0];
          if (childMemoryBlock.DisplayCode == null)
          {
            if (this.MyMenu.My_S3_Function.parentMemoryBlock.childMemoryBlocks.Count <= 1)
              throw new NotImplementedException();
            childMemoryBlock = (S3_Function) this.MyMenu.My_S3_Function.parentMemoryBlock.childMemoryBlocks[1];
          }
          pointerString = childMemoryBlock.Name + "." + childMemoryBlock.Name;
        }
        else if (pointerString == "NEXT")
        {
          S3_MemoryBlock parentMemoryBlock = this.MyMenu.My_S3_Function.parentMemoryBlock;
          int num = parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) this.MyMenu.My_S3_Function);
          if (parentMemoryBlock.childMemoryBlocks.Count > num + 1)
          {
            S3_Function s3Function = (S3_Function) parentMemoryBlock.childMemoryBlocks[num + 1];
            if (s3Function.DisplayCode == null)
              s3Function = parentMemoryBlock.childMemoryBlocks.Count <= num + 2 ? (S3_Function) this.MyMenu.My_S3_Function.parentMemoryBlock.childMemoryBlocks[0] : (S3_Function) parentMemoryBlock.childMemoryBlocks[num + 2];
            pointerString = s3Function.Name + "." + s3Function.Name;
          }
          else
          {
            S3_Function childMemoryBlock = (S3_Function) this.MyMenu.My_S3_Function.parentMemoryBlock.childMemoryBlocks[0];
            if (childMemoryBlock.DisplayCode == null)
            {
              if (this.MyMenu.My_S3_Function.parentMemoryBlock.childMemoryBlocks.Count <= 1)
                throw new NotImplementedException();
              childMemoryBlock = (S3_Function) this.MyMenu.My_S3_Function.parentMemoryBlock.childMemoryBlocks[1];
            }
            pointerString = childMemoryBlock.Name + "." + childMemoryBlock.Name;
          }
        }
        else if (pointerString == "RIGHT")
        {
          S3_MemoryBlock parentMemoryBlock = this.MyMenu.My_S3_Function.parentMemoryBlock;
          int num = parentMemoryBlock.parentMemoryBlock.childMemoryBlocks.IndexOf(parentMemoryBlock);
          if (parentMemoryBlock.parentMemoryBlock.childMemoryBlocks.Count > num + 2)
          {
            S3_MemoryBlock childMemoryBlock1 = parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[num + 1];
            if (childMemoryBlock1.childMemoryBlocks == null || childMemoryBlock1.childMemoryBlocks.Count == 0)
            {
              S3_Function childMemoryBlock2 = (S3_Function) parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[1].childMemoryBlocks[0];
              if (childMemoryBlock2.DisplayCode == null)
              {
                if (this.MyMenu.My_S3_Function.parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[1].childMemoryBlocks.Count <= 1)
                  throw new NotImplementedException();
                childMemoryBlock2 = (S3_Function) this.MyMenu.My_S3_Function.parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[1].childMemoryBlocks[1];
              }
              pointerString = childMemoryBlock2.Name + "." + childMemoryBlock2.Name;
            }
            else
            {
              S3_Function childMemoryBlock3 = (S3_Function) parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[num + 1].childMemoryBlocks[0];
              if (childMemoryBlock3.DisplayCode == null)
              {
                if (parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[num + 1].childMemoryBlocks.Count <= 1)
                  throw new NotImplementedException();
                childMemoryBlock3 = (S3_Function) parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[num + 1].childMemoryBlocks[1];
              }
              pointerString = childMemoryBlock3.Name + "." + childMemoryBlock3.Name;
            }
          }
          else
          {
            S3_Function childMemoryBlock = (S3_Function) parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[1].childMemoryBlocks[0];
            if (childMemoryBlock.DisplayCode == null)
            {
              if (parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[1].childMemoryBlocks.Count <= 0)
                throw new NotImplementedException();
              childMemoryBlock = (S3_Function) parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[1].childMemoryBlocks[1];
            }
            pointerString = childMemoryBlock.Name + "." + childMemoryBlock.Name;
          }
        }
      }
      return true;
    }

    internal bool InsertData()
    {
      return this.MyMeter.MyDeviceMemory.SetByteValue(this.BlockStartAddress, this.BrunchControl);
    }

    internal bool InsertBrunches(List<string> outOfProtectionBrunches)
    {
      int num1 = 1;
      if (this.ClickJump)
      {
        if (!this.GetAddress(this.ClickPointerName, ref this.ClickAddress, outOfProtectionBrunches) || !this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + num1, this.ClickAddress))
          return false;
        num1 += 2;
      }
      if (this.PressJump)
      {
        if (!this.GetAddress(this.PressPointerName, ref this.PressAddress, outOfProtectionBrunches) || !this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + num1, this.PressAddress))
          return false;
        num1 += 2;
      }
      if (this.HoldJump)
      {
        if (!this.GetAddress(this.HoldPointerName, ref this.HoldAddress, outOfProtectionBrunches) || !this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + num1, this.HoldAddress))
          return false;
        num1 += 2;
      }
      if (this.TimeoutJump)
      {
        if (!this.GetAddress(this.TimeoutPointerName, ref this.TimeoutAddress, outOfProtectionBrunches) || !this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + num1, this.TimeoutAddress))
          return false;
        int num2 = num1 + 2;
      }
      return true;
    }

    private bool GetAddress(string name, ref ushort address, List<string> outOfProtectionBrunches)
    {
      if (!this.MyMeter.MyLinker.MyLabelAddresses.ContainsKey(name))
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Lable not found: " + name, FunctionManager.logger);
      address = (ushort) this.MyMeter.MyLinker.MyLabelAddresses[name];
      if (outOfProtectionBrunches != null && (int) address >= this.MyMeter.MyDeviceMemory.BlockDisplayCode.BlockStartAddress)
      {
        if (outOfProtectionBrunches.Contains(name))
        {
          address = (ushort) outOfProtectionBrunches.IndexOf(name);
        }
        else
        {
          address = (ushort) outOfProtectionBrunches.Count;
          outOfProtectionBrunches.Add(name);
        }
      }
      return true;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.ClickPointerName != null)
        stringBuilder.Append(" Click:" + this.ClickPointerName);
      if (this.PressPointerName != null)
        stringBuilder.Append(" Click:" + this.PressPointerName);
      if (this.HoldPointerName != null)
        stringBuilder.Append(" Click:" + this.HoldPointerName);
      if (this.TimeoutPointerName != null)
        stringBuilder.Append(" Click:" + this.TimeoutPointerName);
      return stringBuilder.ToString();
    }
  }
}
