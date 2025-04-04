// Decompiled with JetBrains decompiler
// Type: NHibernate.Exceptions.TemplatedViolatedConstraintNameExtracter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Data.Common;

#nullable disable
namespace NHibernate.Exceptions
{
  public abstract class TemplatedViolatedConstraintNameExtracter : IViolatedConstraintNameExtracter
  {
    protected string ExtractUsingTemplate(string templateStart, string templateEnd, string message)
    {
      int num = message.IndexOf(templateStart);
      if (num < 0)
        return (string) null;
      int startIndex = num + templateStart.Length;
      int length = message.IndexOf(templateEnd, startIndex);
      if (length < 0)
        length = message.Length;
      return message.Substring(startIndex, length);
    }

    public abstract string ExtractConstraintName(DbException sqle);
  }
}
