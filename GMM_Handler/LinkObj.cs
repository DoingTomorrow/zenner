// Decompiled with JetBrains decompiler
// Type: GMM_Handler.LinkObj
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System.Text;

#nullable disable
namespace GMM_Handler
{
  public class LinkObj
  {
    internal int Address = -1;
    public int Size = -1;
    internal byte[] LinkByteList;
    internal string[] LinkByteComment;

    internal LinkObj()
    {
    }

    internal virtual void GetObjectInfo(StringBuilder InfoString, Meter TheMeter)
    {
    }

    internal virtual void GetObjectInfo(
      StringBuilder InfoString,
      Meter TheMeter,
      ref int RAM_Address)
    {
    }
  }
}
