// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.UUIDStringGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;
using System.Text;

#nullable disable
namespace NHibernate.Id
{
  public class UUIDStringGenerator : IIdentifierGenerator
  {
    public object Generate(ISessionImplementor session, object obj)
    {
      StringBuilder stringBuilder = new StringBuilder(16, 16);
      foreach (char ch in Guid.NewGuid().ToByteArray())
        stringBuilder.Append(ch);
      return (object) stringBuilder.ToString();
    }
  }
}
