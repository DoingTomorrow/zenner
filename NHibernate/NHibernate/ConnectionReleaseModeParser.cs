// Decompiled with JetBrains decompiler
// Type: NHibernate.ConnectionReleaseModeParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate
{
  public static class ConnectionReleaseModeParser
  {
    public static ConnectionReleaseMode Convert(string value)
    {
      switch (value)
      {
        case "after_statement":
          throw new HibernateException("aggressive connection release (after_statement) not supported by NHibernate");
        case "after_transaction":
          return ConnectionReleaseMode.AfterTransaction;
        case "on_close":
          return ConnectionReleaseMode.OnClose;
        default:
          throw new HibernateException("could not determine appropriate connection release mode [" + value + "]");
      }
    }

    public static string ToString(ConnectionReleaseMode value)
    {
      switch (value)
      {
        case ConnectionReleaseMode.AfterStatement:
          return "after_statement";
        case ConnectionReleaseMode.AfterTransaction:
          return "after_transaction";
        case ConnectionReleaseMode.OnClose:
          return "on_close";
        default:
          throw new ArgumentOutOfRangeException(nameof (value));
      }
    }
  }
}
