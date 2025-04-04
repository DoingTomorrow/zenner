// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.SQLFunctionRegistry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Dialect.Function
{
  public class SQLFunctionRegistry
  {
    private readonly NHibernate.Dialect.Dialect dialect;
    private readonly IDictionary<string, ISQLFunction> userFunctions;

    public SQLFunctionRegistry(NHibernate.Dialect.Dialect dialect, IDictionary<string, ISQLFunction> userFunctions)
    {
      this.dialect = dialect;
      this.userFunctions = (IDictionary<string, ISQLFunction>) new Dictionary<string, ISQLFunction>(userFunctions, (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);
    }

    public ISQLFunction FindSQLFunction(string functionName)
    {
      if (this.userFunctions.ContainsKey(functionName))
        return this.userFunctions[functionName];
      ISQLFunction sqlFunction;
      this.dialect.Functions.TryGetValue(functionName, out sqlFunction);
      return sqlFunction;
    }

    public bool HasFunction(string functionName)
    {
      return this.userFunctions.ContainsKey(functionName) || this.dialect.Functions.ContainsKey(functionName);
    }
  }
}
