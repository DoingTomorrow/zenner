// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.NodeReferences
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class NodeReferences
  {
    public int NodeID;
    public int ParentID;
    public int LayerID;
    public int NodeOrder;

    public override string ToString()
    {
      return string.Format("NodeID: {0}, ParentID: {1}, LayerID: {2}, NodeOrder: {3}", (object) this.NodeID, (object) this.ParentID, (object) this.LayerID, (object) this.NodeOrder);
    }
  }
}
