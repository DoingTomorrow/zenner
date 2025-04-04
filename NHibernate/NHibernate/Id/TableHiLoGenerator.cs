// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.TableHiLoGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace NHibernate.Id
{
  public class TableHiLoGenerator : TableGenerator
  {
    public const string MaxLo = "max_lo";
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (TableHiLoGenerator));
    private long hi;
    private long lo;
    private long maxLo;
    private System.Type returnClass;

    public override void Configure(IType type, IDictionary<string, string> parms, NHibernate.Dialect.Dialect dialect)
    {
      base.Configure(type, parms, dialect);
      this.maxLo = PropertiesHelper.GetInt64("max_lo", parms, (long) short.MaxValue);
      this.lo = this.maxLo + 1L;
      this.returnClass = type.ReturnedClass;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override object Generate(ISessionImplementor session, object obj)
    {
      if (this.maxLo < 1L)
      {
        long int64 = Convert.ToInt64(base.Generate(session, obj));
        if (int64 == 0L)
          int64 = Convert.ToInt64(base.Generate(session, obj));
        return IdentifierGeneratorFactory.CreateNumber(int64, this.returnClass);
      }
      if (this.lo > this.maxLo)
      {
        long int64 = Convert.ToInt64(base.Generate(session, obj));
        this.lo = int64 == 0L ? 1L : 0L;
        this.hi = int64 * (this.maxLo + 1L);
        TableHiLoGenerator.log.Debug((object) ("New high value: " + (object) int64));
      }
      return IdentifierGeneratorFactory.CreateNumber(this.hi + this.lo++, this.returnClass);
    }
  }
}
