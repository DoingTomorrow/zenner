// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.EmulatedLengthSubstringFunction
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
  public class EmulatedLengthSubstringFunction : StandardSQLFunction
  {
    public EmulatedLengthSubstringFunction()
      : base("substring", (IType) NHibernateUtil.String)
    {
    }

    public override SqlString Render(IList args, ISessionFactoryImplementor factory)
    {
      if (args.Count < 2 || args.Count > 3)
        throw new QueryException("substring(): Incorrect number of parameters (expected 2 or 3, got " + (object) args.Count + ")");
      if (args.Count == 2)
      {
        SqlString sqlString = new SqlString(new object[5]
        {
          (object) "len(",
          args[0],
          (object) ") + 1 - (",
          args[1],
          (object) ")"
        });
        args = (IList) new object[3]
        {
          args[0],
          args[1],
          (object) sqlString
        };
      }
      return base.Render(args, factory);
    }
  }
}
