// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.SQLFunctionTemplate
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace NHibernate.Dialect.Function
{
  [Serializable]
  public class SQLFunctionTemplate : ISQLFunction
  {
    private const int InvalidArgumentIndex = -1;
    private static readonly Regex SplitRegex = new Regex("(\\?[0-9]+)");
    private readonly IType returnType;
    private readonly bool hasArguments;
    private readonly bool hasParenthesesIfNoArgs;
    private readonly string template;
    private SQLFunctionTemplate.TemplateChunk[] chunks;

    public SQLFunctionTemplate(IType type, string template)
      : this(type, template, true)
    {
    }

    public SQLFunctionTemplate(IType type, string template, bool hasParenthesesIfNoArgs)
    {
      this.returnType = type;
      this.template = template;
      this.hasParenthesesIfNoArgs = hasParenthesesIfNoArgs;
      this.InitFromTemplate();
      this.hasArguments = this.chunks.Length > 1;
    }

    private void InitFromTemplate()
    {
      string[] strArray = SQLFunctionTemplate.SplitRegex.Split(this.template);
      this.chunks = new SQLFunctionTemplate.TemplateChunk[strArray.Length];
      for (int index = 0; index < strArray.Length; ++index)
      {
        string chunk = strArray[index];
        if (index % 2 == 0)
        {
          this.chunks[index] = new SQLFunctionTemplate.TemplateChunk(chunk, -1);
        }
        else
        {
          int argIndex = int.Parse(chunk.Substring(1), (IFormatProvider) CultureInfo.InvariantCulture);
          this.chunks[index] = new SQLFunctionTemplate.TemplateChunk(strArray[index], argIndex);
        }
      }
    }

    public IType ReturnType(IType columnType, IMapping mapping)
    {
      return this.returnType != null ? this.returnType : columnType;
    }

    public bool HasArguments => this.hasArguments;

    public bool HasParenthesesIfNoArguments => this.hasParenthesesIfNoArgs;

    public SqlString Render(IList args, ISessionFactoryImplementor factory)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      foreach (SQLFunctionTemplate.TemplateChunk chunk in this.chunks)
      {
        if (chunk.ArgumentIndex != -1)
        {
          int index = chunk.ArgumentIndex - 1;
          object part = index < args.Count ? args[index] : (object) null;
          if (part != null)
          {
            if ((object) (part as Parameter) != null || part is SqlString)
              sqlStringBuilder.AddObject(part);
            else
              sqlStringBuilder.Add(part.ToString());
          }
        }
        else
          sqlStringBuilder.Add(chunk.Text);
      }
      return sqlStringBuilder.ToSqlString();
    }

    public override string ToString() => this.template;

    private struct TemplateChunk(string chunk, int argIndex)
    {
      public string Text = chunk;
      public int ArgumentIndex = argIndex;
    }
  }
}
