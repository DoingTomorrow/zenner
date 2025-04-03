// Decompiled with JetBrains decompiler
// Type: UsersScope.UsersOfflineContext
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.Common;
using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace UsersScope
{
  public class UsersOfflineContext(string cachePath, Uri serviceUri) : SQLiteContext(UsersOfflineContext.GetSchema(), "Users", cachePath, serviceUri)
  {
    private const string SyncScopeName = "Users";

    private static OfflineSchema GetSchema()
    {
      OfflineSchema schema = new OfflineSchema();
      schema.AddCollection<t_User>();
      schema.AddCollection<t_Role>();
      schema.AddCollection<t_Operation>();
      schema.AddCollection<t_UserRole>();
      schema.AddCollection<t_RoleOperation>();
      schema.AddCollection<t_UserDeviceTypeSettings>();
      return schema;
    }
  }
}
