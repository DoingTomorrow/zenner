// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.IfxViolatedConstraintExtracter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Exceptions;
using System.Data.Common;

#nullable disable
namespace NHibernate.Dialect
{
  public class IfxViolatedConstraintExtracter : TemplatedViolatedConstraintNameExtracter
  {
    public override string ExtractConstraintName(DbException sqle)
    {
      string constraintName = (string) null;
      switch (new ReflectionBasedSqlStateExtracter().ExtractErrorCode(sqle))
      {
        case -692:
          constraintName = this.ExtractUsingTemplate("Key value for constraint (", ") is still being referenced.", sqle.Message);
          break;
        case -691:
          constraintName = this.ExtractUsingTemplate("Missing key in referenced table for referential constraint (", ").", sqle.Message);
          break;
        case -268:
          constraintName = this.ExtractUsingTemplate("Unique constraint (", ") violated.", sqle.Message);
          break;
      }
      if (constraintName != null)
      {
        int num = constraintName.IndexOf('.');
        if (num != -1)
          constraintName = constraintName.Substring(num + 1);
      }
      return constraintName;
    }
  }
}
