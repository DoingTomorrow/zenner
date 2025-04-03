// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.ProgressEventArgs
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public class ProgressEventArgs : EventArgs
  {
    public readonly IntPtr UserData;
    public SQLiteProgressReturnCode ReturnCode;

    private ProgressEventArgs()
    {
      this.UserData = IntPtr.Zero;
      this.ReturnCode = SQLiteProgressReturnCode.Continue;
    }

    internal ProgressEventArgs(IntPtr pUserData, SQLiteProgressReturnCode returnCode)
      : this()
    {
      this.UserData = pUserData;
      this.ReturnCode = returnCode;
    }
  }
}
