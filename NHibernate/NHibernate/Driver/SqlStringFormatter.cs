// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.SqlStringFormatter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine.Query;
using NHibernate.SqlCommand;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace NHibernate.Driver
{
  public class SqlStringFormatter : ISqlStringVisitor
  {
    private readonly StringBuilder result = new StringBuilder();
    private int parameterIndex;
    private readonly ISqlParameterFormatter formatter;
    private readonly string multipleQueriesSeparator;
    private bool hasReturnParameter;
    private bool foundReturnParameter;
    private IList<string> assignedParameterNames = (IList<string>) new List<string>();

    public SqlStringFormatter(ISqlParameterFormatter formatter, string multipleQueriesSeparator)
    {
      this.formatter = formatter;
      this.multipleQueriesSeparator = multipleQueriesSeparator;
    }

    public void Format(SqlString text)
    {
      this.hasReturnParameter = this.DetermineIfSqlStringHasReturnParameter(text);
      text.Visit((ISqlStringVisitor) this);
    }

    public string GetFormattedText() => this.result.ToString();

    void ISqlStringVisitor.String(string text) => this.result.Append(text);

    void ISqlStringVisitor.String(SqlString sqlString) => this.result.Append(sqlString.ToString());

    void ISqlStringVisitor.Parameter(Parameter parameter)
    {
      if (this.hasReturnParameter && !this.foundReturnParameter)
      {
        this.result.Append((object) parameter);
        this.assignedParameterNames.Add(string.Empty);
        this.foundReturnParameter = true;
      }
      else
      {
        string parameterName = this.formatter.GetParameterName(parameter.ParameterPosition ?? this.parameterIndex);
        this.assignedParameterNames.Add(parameterName);
        ++this.parameterIndex;
        this.result.Append(parameterName);
      }
    }

    private bool DetermineIfSqlStringHasReturnParameter(SqlString text)
    {
      CallableParser.Detail detail = CallableParser.Parse(text.ToString());
      return detail.IsCallable && detail.HasReturn;
    }

    public bool HasReturnParameter => this.foundReturnParameter;

    public string[] AssignedParameterNames => this.assignedParameterNames.ToArray<string>();
  }
}
