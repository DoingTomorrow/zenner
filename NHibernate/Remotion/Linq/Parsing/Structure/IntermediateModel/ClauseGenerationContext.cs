// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.ClauseGenerationContext
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public struct ClauseGenerationContext
  {
    private readonly Dictionary<IExpressionNode, object> _lookup;
    private readonly INodeTypeProvider _nodeTypeProvider;

    public ClauseGenerationContext(INodeTypeProvider nodeTypeProvider)
      : this()
    {
      ArgumentUtility.CheckNotNull<INodeTypeProvider>(nameof (nodeTypeProvider), nodeTypeProvider);
      this._lookup = new Dictionary<IExpressionNode, object>();
      this._nodeTypeProvider = nodeTypeProvider;
    }

    public INodeTypeProvider NodeTypeProvider => this._nodeTypeProvider;

    public int Count => this._lookup.Count;

    public void AddContextInfo(IExpressionNode node, object contextInfo)
    {
      ArgumentUtility.CheckNotNull<IExpressionNode>(nameof (node), node);
      ArgumentUtility.CheckNotNull<object>(nameof (contextInfo), contextInfo);
      try
      {
        this._lookup.Add(node, contextInfo);
      }
      catch (ArgumentException ex)
      {
        throw new InvalidOperationException("Node already has associated context info.");
      }
    }

    public object GetContextInfo(IExpressionNode node)
    {
      ArgumentUtility.CheckNotNull<IExpressionNode>(nameof (node), node);
      object contextInfo;
      if (!this._lookup.TryGetValue(node, out contextInfo))
        throw new KeyNotFoundException("Node has no associated context info.");
      return contextInfo;
    }
  }
}
