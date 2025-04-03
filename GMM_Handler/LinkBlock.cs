// Decompiled with JetBrains decompiler
// Type: GMM_Handler.LinkBlock
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System.Collections;

#nullable disable
namespace GMM_Handler
{
  internal class LinkBlock
  {
    protected Meter MyMeter;
    internal LinkBlockTypes LinkBlockType;
    internal int BlockStartAddress;
    internal int StartAddressOfNextBlock = -1;
    internal ArrayList LinkObjList;

    internal LinkBlock(Meter MyMeterIn, LinkBlockTypes TheLinkBlockType)
    {
      this.MyMeter = MyMeterIn;
      this.LinkObjList = new ArrayList();
      this.LinkBlockType = TheLinkBlockType;
    }
  }
}
