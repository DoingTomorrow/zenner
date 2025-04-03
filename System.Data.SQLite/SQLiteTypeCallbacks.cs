// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteTypeCallbacks
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public sealed class SQLiteTypeCallbacks
  {
    private string typeName;
    private SQLiteBindValueCallback bindValueCallback;
    private SQLiteReadValueCallback readValueCallback;
    private object bindValueUserData;
    private object readValueUserData;

    private SQLiteTypeCallbacks(
      SQLiteBindValueCallback bindValueCallback,
      SQLiteReadValueCallback readValueCallback,
      object bindValueUserData,
      object readValueUserData)
    {
      this.bindValueCallback = bindValueCallback;
      this.readValueCallback = readValueCallback;
      this.bindValueUserData = bindValueUserData;
      this.readValueUserData = readValueUserData;
    }

    public static SQLiteTypeCallbacks Create(
      SQLiteBindValueCallback bindValueCallback,
      SQLiteReadValueCallback readValueCallback,
      object bindValueUserData,
      object readValueUserData)
    {
      return new SQLiteTypeCallbacks(bindValueCallback, readValueCallback, bindValueUserData, readValueUserData);
    }

    public string TypeName
    {
      get => this.typeName;
      internal set => this.typeName = value;
    }

    public SQLiteBindValueCallback BindValueCallback => this.bindValueCallback;

    public SQLiteReadValueCallback ReadValueCallback => this.readValueCallback;

    public object BindValueUserData => this.bindValueUserData;

    public object ReadValueUserData => this.readValueUserData;
  }
}
