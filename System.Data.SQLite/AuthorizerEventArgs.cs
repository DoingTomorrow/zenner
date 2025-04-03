// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.AuthorizerEventArgs
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public class AuthorizerEventArgs : EventArgs
  {
    public readonly IntPtr UserData;
    public readonly SQLiteAuthorizerActionCode ActionCode;
    public readonly string Argument1;
    public readonly string Argument2;
    public readonly string Database;
    public readonly string Context;
    public SQLiteAuthorizerReturnCode ReturnCode;

    private AuthorizerEventArgs()
    {
      this.UserData = IntPtr.Zero;
      this.ActionCode = SQLiteAuthorizerActionCode.None;
      this.Argument1 = (string) null;
      this.Argument2 = (string) null;
      this.Database = (string) null;
      this.Context = (string) null;
      this.ReturnCode = SQLiteAuthorizerReturnCode.Ok;
    }

    internal AuthorizerEventArgs(
      IntPtr pUserData,
      SQLiteAuthorizerActionCode actionCode,
      string argument1,
      string argument2,
      string database,
      string context,
      SQLiteAuthorizerReturnCode returnCode)
      : this()
    {
      this.UserData = pUserData;
      this.ActionCode = actionCode;
      this.Argument1 = argument1;
      this.Argument2 = argument2;
      this.Database = database;
      this.Context = context;
      this.ReturnCode = returnCode;
    }
  }
}
