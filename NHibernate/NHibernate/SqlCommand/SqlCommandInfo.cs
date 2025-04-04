// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.SqlCommandInfo
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlTypes;
using System.Data;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class SqlCommandInfo
  {
    private readonly SqlString text;
    private readonly SqlType[] parameterTypes;

    public SqlCommandInfo(SqlString text, SqlType[] parameterTypes)
    {
      this.text = text;
      this.parameterTypes = parameterTypes;
    }

    public CommandType CommandType => CommandType.Text;

    public SqlString Text => this.text;

    public SqlType[] ParameterTypes => this.parameterTypes;

    public override string ToString()
    {
      return this.Text == null ? this.GetType().FullName : this.Text.ToString().Trim();
    }
  }
}
