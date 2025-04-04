// Decompiled with JetBrains decompiler
// Type: Ninject.Parameters.PropertyValue
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Planning.Targets;
using System;

#nullable disable
namespace Ninject.Parameters
{
  public class PropertyValue : Parameter
  {
    public PropertyValue(string name, object value)
      : base(name, value, false)
    {
    }

    public PropertyValue(string name, Func<IContext, object> valueCallback)
      : base(name, valueCallback, false)
    {
    }

    public PropertyValue(string name, Func<IContext, ITarget, object> valueCallback)
      : base(name, valueCallback, false)
    {
    }
  }
}
