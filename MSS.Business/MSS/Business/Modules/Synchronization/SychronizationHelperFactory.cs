// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Synchronization.SychronizationHelperFactory
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Modules.Synchronization.Sqlite;
using System;
using System.Configuration;

#nullable disable
namespace MSS.Business.Modules.Synchronization
{
  public class SychronizationHelperFactory
  {
    public static SynchronizationHelperBase GetSynchronizationHelper()
    {
      switch (ConfigurationManager.AppSettings["DatabaseEngine"])
      {
        case "MSSQLDatabase":
          return (SynchronizationHelperBase) new SynchronizationHelper();
        case "SQLiteDatabase":
          return (SynchronizationHelperBase) new SqliteSynchronizationHelper();
        default:
          throw new Exception("No database engine provided!");
      }
    }
  }
}
