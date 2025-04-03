// Decompiled with JetBrains decompiler
// Type: ReadingValuesScope.ReadingValuesOfflineContext
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.Common;
using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ReadingValuesScope
{
  public class ReadingValuesOfflineContext(string cachePath, Uri serviceUri) : SQLiteContext(ReadingValuesOfflineContext.GetSchema(), "ReadingValues", cachePath, serviceUri)
  {
    private const string SyncScopeName = "ReadingValues";

    private static OfflineSchema GetSchema()
    {
      OfflineSchema schema = new OfflineSchema();
      schema.AddCollection<t_ReadingValues>();
      schema.AddCollection<t_OrderReadingValues>();
      return schema;
    }
  }
}
