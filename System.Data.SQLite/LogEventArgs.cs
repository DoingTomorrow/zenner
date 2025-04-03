// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.LogEventArgs
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public class LogEventArgs : EventArgs
  {
    public readonly object ErrorCode;
    public readonly string Message;
    public readonly object Data;

    internal LogEventArgs(IntPtr pUserData, object errorCode, string message, object data)
    {
      this.ErrorCode = errorCode;
      this.Message = message;
      this.Data = data;
    }
  }
}
