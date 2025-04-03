// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.CachedIDAtom
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

#nullable disable
namespace ZR_ClassLibrary
{
  internal class CachedIDAtom
  {
    internal string TableName;
    internal string FieldName;
    internal long NextID;
    internal long LastID;

    internal CachedIDAtom(
      string theTableName,
      string theFieldName,
      long theFirstID,
      long theLastID)
    {
      this.TableName = theTableName;
      this.FieldName = theFieldName;
      this.NextID = theFirstID;
      this.LastID = theLastID;
    }

    internal bool getActualID(out long newID)
    {
      bool actualId = false;
      newID = -1L;
      if (this.NextID <= this.LastID)
      {
        actualId = true;
        newID = this.NextID++;
      }
      return actualId;
    }
  }
}
