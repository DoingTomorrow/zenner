// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.StandardSafeSQLFunction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Dialect.Function
{
  [Serializable]
  public class StandardSafeSQLFunction : StandardSQLFunction
  {
    private int allowedArgsCount = 1;

    public StandardSafeSQLFunction(string name, int allowedArgsCount)
      : base(name)
    {
      this.allowedArgsCount = allowedArgsCount;
    }

    public StandardSafeSQLFunction(string name, IType typeValue, int allowedArgsCount)
      : base(name, typeValue)
    {
      this.allowedArgsCount = allowedArgsCount;
    }

    public override SqlString Render(IList args, ISessionFactoryImplementor factory)
    {
      if (args.Count != this.allowedArgsCount)
        throw new QueryException(string.Format("function '{0}' takes {1} arguments.", (object) this.name, (object) this.allowedArgsCount));
      return base.Render(args, factory);
    }
  }
}
