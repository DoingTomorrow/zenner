// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.CounterGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace NHibernate.Id
{
  public class CounterGenerator : IIdentifierGenerator
  {
    private static short counter;

    protected short Count
    {
      [MethodImpl(MethodImplOptions.Synchronized)] get
      {
        if (CounterGenerator.counter < (short) 0)
          CounterGenerator.counter = (short) 0;
        return CounterGenerator.counter++;
      }
    }

    public object Generate(ISessionImplementor cache, object obj)
    {
      return (object) ((DateTime.Now.Ticks << 16) + (long) this.Count);
    }
  }
}
