// Decompiled with JetBrains decompiler
// Type: ConfigurationScope.ConfigurationOfflineContext
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.Common;
using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ConfigurationScope
{
  public class ConfigurationOfflineContext(string cachePath, Uri serviceUri) : SQLiteContext(ConfigurationOfflineContext.GetSchema(), "Configuration", cachePath, serviceUri)
  {
    private const string SyncScopeName = "Configuration";

    private static OfflineSchema GetSchema()
    {
      OfflineSchema schema = new OfflineSchema();
      schema.AddCollection<t_Country>();
      schema.AddCollection<t_StructureNodeType>();
      schema.AddCollection<t_Scenario>();
      schema.AddCollection<t_CelestaReadingDeviceTypes>();
      schema.AddCollection<t_RoomType>();
      schema.AddCollection<t_MeasureUnit>();
      schema.AddCollection<t_ConnectedDeviceType>();
      schema.AddCollection<t_Channel>();
      schema.AddCollection<t_Filters>();
      schema.AddCollection<t_Rules>();
      schema.AddCollection<t_JobDefinitions>();
      schema.AddCollection<t_ScenarioJobDefinitions>();
      schema.AddCollection<t_Provider>();
      schema.AddCollection<t_COMServer>();
      schema.AddCollection<t_MDMConfigs>();
      schema.AddCollection<t_NoteType>();
      return schema;
    }
  }
}
