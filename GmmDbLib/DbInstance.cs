// Decompiled with JetBrains decompiler
// Type: GmmDbLib.DbInstance
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System.Data.Common;

#nullable disable
namespace GmmDbLib
{
  public class DbInstance
  {
    public static BaseDbConnection PrimaryDb;
    public static BaseDbConnection SecundaryDb;
    public DbProviderFactory ProviderFactory;
    public DbConnection Connecton;
    public DbConnectionInfo ConnectionInfo;
  }
}
