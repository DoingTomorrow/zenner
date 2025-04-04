// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.NodeTypeProviders.NameBasedRegistrationInfo
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.NodeTypeProviders
{
  public class NameBasedRegistrationInfo
  {
    private readonly string _name;
    private readonly Func<MethodInfo, bool> _filter;

    public NameBasedRegistrationInfo(string name, Func<MethodInfo, bool> filter)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (name), name);
      ArgumentUtility.CheckNotNull<Func<MethodInfo, bool>>(nameof (filter), filter);
      this._name = name;
      this._filter = filter;
    }

    public string Name => this._name;

    public Func<MethodInfo, bool> Filter => this._filter;
  }
}
