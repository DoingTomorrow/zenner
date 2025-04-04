// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Targets.ParameterTarget
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using System;
using System.Reflection;

#nullable disable
namespace Ninject.Planning.Targets
{
  public class ParameterTarget : Target<ParameterInfo>
  {
    private readonly Future<object> defaultValue;

    public override string Name => this.Site.Name;

    public override Type Type => this.Site.ParameterType;

    public override bool HasDefaultValue => this.defaultValue.Value != DBNull.Value;

    public override object DefaultValue
    {
      get => !this.HasDefaultValue ? base.DefaultValue : this.defaultValue.Value;
    }

    public ParameterTarget(MethodBase method, ParameterInfo site)
      : base((MemberInfo) method, site)
    {
      this.defaultValue = new Future<object>((Func<object>) (() => site.DefaultValue));
    }
  }
}
