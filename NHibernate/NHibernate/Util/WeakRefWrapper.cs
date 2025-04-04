// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.WeakRefWrapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.DebugHelpers;
using System;
using System.Diagnostics;

#nullable disable
namespace NHibernate.Util
{
  [DebuggerTypeProxy(typeof (DictionaryProxy))]
  [Serializable]
  public class WeakRefWrapper
  {
    private WeakReference reference;
    private int hashCode;

    public WeakRefWrapper(object target)
    {
      this.reference = new WeakReference(target);
      this.hashCode = target.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      if (!(obj is WeakRefWrapper weakRefWrapper))
        return false;
      object target = this.Target;
      return target != null && this.hashCode == weakRefWrapper.hashCode && object.Equals(target, weakRefWrapper.Target);
    }

    public override int GetHashCode() => this.hashCode;

    public object Target => this.reference.Target;

    public bool IsAlive => this.reference.IsAlive;

    public static WeakRefWrapper Wrap(object value) => new WeakRefWrapper(value);

    public static object Unwrap(object value) => ((WeakRefWrapper) value)?.Target;
  }
}
