// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.Enhanced.OptimizerFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace NHibernate.Id.Enhanced
{
  public class OptimizerFactory
  {
    public const string None = "none";
    public const string HiLo = "hilo";
    public const string Pool = "pooled";
    public const string PoolLo = "pooled-lo";
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (OptimizerFactory));
    private static readonly Type[] CtorSignature = new Type[2]
    {
      typeof (Type),
      typeof (int)
    };

    private static IOptimizer BuildOptimizer(string type, Type returnClass, int incrementSize)
    {
      if (string.IsNullOrEmpty(type))
        throw new ArgumentNullException(nameof (type));
      if (returnClass == null)
        throw new ArgumentNullException(nameof (returnClass));
      string name;
      switch (type)
      {
        case "none":
          name = typeof (OptimizerFactory.NoopOptimizer).FullName;
          break;
        case "hilo":
          name = typeof (OptimizerFactory.HiLoOptimizer).FullName;
          break;
        case "pooled":
          name = typeof (OptimizerFactory.PooledOptimizer).FullName;
          break;
        case "pooled-lo":
          name = typeof (OptimizerFactory.PooledLoOptimizer).FullName;
          break;
        default:
          name = type;
          break;
      }
      try
      {
        return (IOptimizer) (ReflectHelper.ClassForName(name).GetConstructor(OptimizerFactory.CtorSignature) ?? throw new HibernateException("Optimizer does not have expected contructor")).Invoke(new object[2]
        {
          (object) returnClass,
          (object) incrementSize
        });
      }
      catch (Exception ex)
      {
        OptimizerFactory.Log.Error((object) "Unable to instantiate id generator optimizer.");
      }
      return (IOptimizer) new OptimizerFactory.NoopOptimizer(returnClass, incrementSize);
    }

    public static IOptimizer BuildOptimizer(
      string type,
      Type returnClass,
      int incrementSize,
      long explicitInitialValue)
    {
      IOptimizer optimizer = OptimizerFactory.BuildOptimizer(type, returnClass, incrementSize);
      if (optimizer is OptimizerFactory.IInitialValueAwareOptimizer)
        ((OptimizerFactory.IInitialValueAwareOptimizer) optimizer).InjectInitialValue(explicitInitialValue);
      return optimizer;
    }

    public interface IInitialValueAwareOptimizer
    {
      void InjectInitialValue(long initialValue);
    }

    public abstract class OptimizerSupport : IOptimizer
    {
      protected OptimizerSupport(Type returnClass, int incrementSize)
      {
        this.ReturnClass = returnClass != null ? returnClass : throw new HibernateException("return class is required");
        this.IncrementSize = incrementSize;
      }

      public Type ReturnClass { get; protected set; }

      public int IncrementSize { get; protected set; }

      public abstract long LastSourceValue { get; }

      public abstract bool ApplyIncrementSizeToSourceValues { get; }

      public abstract object Generate(IAccessCallback param);

      protected virtual object Make(long value)
      {
        return IdentifierGeneratorFactory.CreateNumber(value, this.ReturnClass);
      }
    }

    public class HiLoOptimizer : OptimizerFactory.OptimizerSupport
    {
      private long _upperLimit;
      private long _lastSourceValue = -1;
      private long _value;

      public HiLoOptimizer(Type returnClass, int incrementSize)
        : base(returnClass, incrementSize)
      {
        if (incrementSize < 1)
          throw new HibernateException("increment size cannot be less than 1");
        if (!OptimizerFactory.Log.IsDebugEnabled)
          return;
        OptimizerFactory.Log.Debug((object) ("Creating hilo optimizer with [incrementSize=" + (object) incrementSize + "; returnClass=" + returnClass.FullName + "]"));
      }

      public override long LastSourceValue => this._lastSourceValue;

      public long LastValue => this._value - 1L;

      public long HiValue => this._upperLimit;

      public override bool ApplyIncrementSizeToSourceValues => false;

      [MethodImpl(MethodImplOptions.Synchronized)]
      public override object Generate(IAccessCallback callback)
      {
        if (this._lastSourceValue < 0L)
        {
          this._lastSourceValue = callback.GetNextValue();
          while (this._lastSourceValue <= 0L)
            this._lastSourceValue = callback.GetNextValue();
          this._upperLimit = this._lastSourceValue * (long) this.IncrementSize + 1L;
          this._value = this._upperLimit - (long) this.IncrementSize;
        }
        else if (this._upperLimit <= this._value)
        {
          this._lastSourceValue = callback.GetNextValue();
          this._upperLimit = this._lastSourceValue * (long) this.IncrementSize + 1L;
        }
        return this.Make(this._value++);
      }
    }

    public class NoopOptimizer(Type returnClass, int incrementSize) : 
      OptimizerFactory.OptimizerSupport(returnClass, incrementSize)
    {
      private long _lastSourceValue = -1;

      public override long LastSourceValue => this._lastSourceValue;

      public override bool ApplyIncrementSizeToSourceValues => false;

      public override object Generate(IAccessCallback callback)
      {
        long num = -1;
        while (num <= 0L)
          num = callback.GetNextValue();
        this._lastSourceValue = num;
        return this.Make(num);
      }
    }

    public class PooledOptimizer : 
      OptimizerFactory.OptimizerSupport,
      OptimizerFactory.IInitialValueAwareOptimizer
    {
      private long _hiValue = -1;
      private long _value;
      private long _initialValue;

      public PooledOptimizer(Type returnClass, int incrementSize)
        : base(returnClass, incrementSize)
      {
        if (incrementSize < 1)
          throw new HibernateException("increment size cannot be less than 1");
        if (!OptimizerFactory.Log.IsDebugEnabled)
          return;
        OptimizerFactory.Log.Debug((object) ("Creating pooled optimizer with [incrementSize=" + (object) incrementSize + "; returnClass=" + returnClass.FullName + "]"));
      }

      public override long LastSourceValue => this._hiValue;

      public long LastValue => this._value - 1L;

      public override bool ApplyIncrementSizeToSourceValues => true;

      public void InjectInitialValue(long initialValue) => this._initialValue = initialValue;

      [MethodImpl(MethodImplOptions.Synchronized)]
      public override object Generate(IAccessCallback callback)
      {
        if (this._hiValue < 0L)
        {
          this._value = callback.GetNextValue();
          if (this._value < 1L)
            OptimizerFactory.Log.Info((object) ("pooled optimizer source reported [" + (object) this._value + "] as the initial value; use of 1 or greater highly recommended"));
          if (this._initialValue == -1L && this._value < (long) this.IncrementSize || this._value == this._initialValue)
          {
            this._hiValue = callback.GetNextValue();
          }
          else
          {
            this._hiValue = this._value;
            this._value = this._hiValue - (long) this.IncrementSize;
          }
        }
        else if (this._value >= this._hiValue)
        {
          this._hiValue = callback.GetNextValue();
          this._value = this._hiValue - (long) this.IncrementSize;
        }
        return this.Make(this._value++);
      }
    }

    public class PooledLoOptimizer : OptimizerFactory.OptimizerSupport
    {
      private long _lastSourceValue = -1;
      private long _value;

      public PooledLoOptimizer(Type returnClass, int incrementSize)
        : base(returnClass, incrementSize)
      {
        if (incrementSize < 1)
          throw new HibernateException("increment size cannot be less than 1");
        if (!OptimizerFactory.Log.IsDebugEnabled)
          return;
        OptimizerFactory.Log.DebugFormat("Creating pooled optimizer (lo) with [incrementSize={0}; returnClass={1}]", (object) incrementSize, (object) returnClass.FullName);
      }

      [MethodImpl(MethodImplOptions.Synchronized)]
      public override object Generate(IAccessCallback callback)
      {
        if (this._lastSourceValue < 0L || this._value >= this._lastSourceValue + (long) this.IncrementSize)
        {
          this._lastSourceValue = callback.GetNextValue();
          this._value = this._lastSourceValue;
          while (this._value < 1L)
            ++this._value;
        }
        return this.Make(this._value++);
      }

      public override long LastSourceValue => this._lastSourceValue;

      public override bool ApplyIncrementSizeToSourceValues => true;

      public long LastValue => this._value - 1L;
    }
  }
}
