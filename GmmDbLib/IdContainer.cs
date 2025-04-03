// Decompiled with JetBrains decompiler
// Type: GmmDbLib.IdContainer
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System;

#nullable disable
namespace GmmDbLib
{
  public class IdContainer
  {
    public readonly string idTableName;
    public readonly string idColumnName;
    private readonly int startId;
    private int numberOfIds;
    private int usedOffset;

    internal IdContainer(int startId, int numberOfIds, string idTableName, string idColumnName)
    {
      this.idTableName = idTableName;
      this.idColumnName = idColumnName;
      this.startId = startId;
      this.numberOfIds = numberOfIds;
      this.usedOffset = 0;
    }

    public int GetNextID()
    {
      if (this.usedOffset >= this.numberOfIds)
        throw new Exception("No additional id available");
      int nextId = this.startId + this.usedOffset;
      ++this.usedOffset;
      return nextId;
    }

    public void AddNumberOfIds(IdContainer IdsToAdd)
    {
      if (this.startId + this.numberOfIds != IdsToAdd.startId)
        throw new Exception("Illegal id combination");
      this.numberOfIds += IdsToAdd.numberOfIds;
    }

    public int NumberOfUnusedIds => this.numberOfIds - this.usedOffset;

    public int LastId => this.startId + this.numberOfIds;
  }
}
