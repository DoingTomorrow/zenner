// Decompiled with JetBrains decompiler
// Type: AutoMapper.Internal.LazyFactory
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Threading;

#nullable disable
namespace AutoMapper.Internal
{
  public static class LazyFactory
  {
    public static ILazy<T> Create<T>(Func<T> valueFactory) where T : class
    {
      return (ILazy<T>) new LazyFactory.LazyImpl<T>(valueFactory);
    }

    private sealed class LazyImpl<T> : ILazy<T> where T : class
    {
      private readonly object _lockObj = new object();
      private readonly Func<T> _valueFactory;
      private bool _isDelegateInvoked;
      private T m_value;

      public LazyImpl(Func<T> valueFactory) => this._valueFactory = valueFactory;

      public T Value
      {
        get
        {
          if (!this._isDelegateInvoked)
          {
            Interlocked.CompareExchange<T>(ref this.m_value, this._valueFactory(), default (T));
            bool flag = false;
            try
            {
              Monitor.Enter(this._lockObj);
              flag = true;
              this._isDelegateInvoked = true;
            }
            finally
            {
              if (flag)
                Monitor.Exit(this._lockObj);
            }
          }
          return this.m_value;
        }
      }
    }
  }
}
