// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.NodeTypeProviders.CompoundNodeTypeProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.NodeTypeProviders
{
  public class CompoundNodeTypeProvider : INodeTypeProvider
  {
    private readonly List<INodeTypeProvider> _innerProviders;

    public CompoundNodeTypeProvider(IEnumerable<INodeTypeProvider> innerProviders)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<INodeTypeProvider>>(nameof (innerProviders), innerProviders);
      this._innerProviders = new List<INodeTypeProvider>(innerProviders);
    }

    public IList<INodeTypeProvider> InnerProviders
    {
      get => (IList<INodeTypeProvider>) this._innerProviders;
    }

    public bool IsRegistered(MethodInfo method)
    {
      ArgumentUtility.CheckNotNull<MethodInfo>(nameof (method), method);
      return this._innerProviders.Any<INodeTypeProvider>((Func<INodeTypeProvider, bool>) (p => p.IsRegistered(method)));
    }

    public Type GetNodeType(MethodInfo method)
    {
      ArgumentUtility.CheckNotNull<MethodInfo>(nameof (method), method);
      return this.InnerProviders.Select<INodeTypeProvider, Type>((Func<INodeTypeProvider, Type>) (p => p.GetNodeType(method))).FirstOrDefault<Type>((Func<Type, bool>) (t => t != null));
    }
  }
}
