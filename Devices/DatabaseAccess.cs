// Decompiled with JetBrains decompiler
// Type: Devices.DatabaseAccess
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using GmmDbLib;
using System.Data;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace Devices
{
  internal class DatabaseAccess
  {
    private IDbConnection myPrimaryConnection;
    internal ZRDataAdapter MBusTranslationsData_Adapter;
    internal Schema.MBusParameterTranslationDataTable MBusTranslationsTable;

    internal DatabaseAccess() => this.myPrimaryConnection = DbBasis.PrimaryDB.GetDbConnection();
  }
}
