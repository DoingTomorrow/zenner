// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Helpers.Prebuilt.BuiltIdConvention
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System;

#nullable disable
namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
  internal class BuiltIdConvention(
    Action<IAcceptanceCriteria<IIdentityInspector>> accept,
    Action<IIdentityInstance> convention) : 
    BuiltConventionBase<IIdentityInspector, IIdentityInstance>(accept, convention),
    IIdConvention,
    IConvention<IIdentityInspector, IIdentityInstance>,
    IConvention,
    IIdConventionAcceptance,
    IConventionAcceptance<IIdentityInspector>
  {
  }
}
