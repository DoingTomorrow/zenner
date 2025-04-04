// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.PersistentIdGeneratorParmsNames
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet.Util;
using System.Runtime.InteropServices;

#nullable disable
namespace NHibernate.Id
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct PersistentIdGeneratorParmsNames
  {
    public static readonly string Schema = "schema";
    public static readonly string Table = "target_table";
    public static readonly string Tables = "identity_tables";
    public static readonly string PK = "target_column";
    public static readonly string Catalog = "catalog";
    public static readonly SqlStatementLogger SqlStatementLogger = new SqlStatementLogger(false, false);
  }
}
