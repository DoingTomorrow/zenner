// Decompiled with JetBrains decompiler
// Type: NHibernate.Exceptions.AdoExceptionContextInfo
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Exceptions
{
  [Serializable]
  public class AdoExceptionContextInfo
  {
    public Exception SqlException { get; set; }

    public string Message { get; set; }

    public string Sql { get; set; }

    public string EntityName { get; set; }

    public object EntityId { get; set; }
  }
}
