// Decompiled with JetBrains decompiler
// Type: ApplicationScope.ApplicationOfflineContext
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.Common;
using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class ApplicationOfflineContext(string cachePath, Uri serviceUri) : SQLiteContext(ApplicationOfflineContext.GetSchema(), "Application", cachePath, serviceUri)
  {
    private const string SyncScopeName = "Application";

    private static OfflineSchema GetSchema()
    {
      OfflineSchema schema = new OfflineSchema();
      schema.AddCollection<t_Minomat>();
      schema.AddCollection<t_MinomatRadioDetails>();
      schema.AddCollection<t_StructureNode>();
      schema.AddCollection<t_StructureNodeLinks>();
      schema.AddCollection<t_Meter>();
      schema.AddCollection<t_MeterRadioDetails>();
      schema.AddCollection<t_Order>();
      schema.AddCollection<t_OrderUser>();
      schema.AddCollection<t_Location>();
      schema.AddCollection<t_Tenant>();
      schema.AddCollection<t_MeterReplacementHistory>();
      schema.AddCollection<t_OrderMessages>();
      schema.AddCollection<t_MeterMbusRadio>();
      schema.AddCollection<t_MinomatMeters>();
      schema.AddCollection<t_StructureNodeEquipmentSettings>();
      schema.AddCollection<t_Note>();
      return schema;
    }
  }
}
