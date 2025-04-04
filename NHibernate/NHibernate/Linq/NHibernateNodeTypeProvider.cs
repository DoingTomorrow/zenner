// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.NHibernateNodeTypeProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.EagerFetching.Parsing;
using Remotion.Linq.Parsing.Structure;
using Remotion.Linq.Parsing.Structure.NodeTypeProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq
{
  public class NHibernateNodeTypeProvider : INodeTypeProvider
  {
    private INodeTypeProvider defaultNodeTypeProvider;

    public NHibernateNodeTypeProvider()
    {
      MethodInfoBasedNodeTypeRegistry nodeTypeRegistry = new MethodInfoBasedNodeTypeRegistry();
      nodeTypeRegistry.Register((IEnumerable<MethodInfo>) new MethodInfo[1]
      {
        typeof (EagerFetchingExtensionMethods).GetMethod("Fetch")
      }, typeof (FetchOneExpressionNode));
      nodeTypeRegistry.Register((IEnumerable<MethodInfo>) new MethodInfo[1]
      {
        typeof (EagerFetchingExtensionMethods).GetMethod("FetchMany")
      }, typeof (FetchManyExpressionNode));
      nodeTypeRegistry.Register((IEnumerable<MethodInfo>) new MethodInfo[1]
      {
        typeof (EagerFetchingExtensionMethods).GetMethod("ThenFetch")
      }, typeof (ThenFetchOneExpressionNode));
      nodeTypeRegistry.Register((IEnumerable<MethodInfo>) new MethodInfo[1]
      {
        typeof (EagerFetchingExtensionMethods).GetMethod("ThenFetchMany")
      }, typeof (ThenFetchManyExpressionNode));
      nodeTypeRegistry.Register((IEnumerable<MethodInfo>) new MethodInfo[3]
      {
        typeof (LinqExtensionMethods).GetMethod("Cacheable"),
        typeof (LinqExtensionMethods).GetMethod("CacheMode"),
        typeof (LinqExtensionMethods).GetMethod("CacheRegion")
      }, typeof (CacheableExpressionNode));
      nodeTypeRegistry.Register((IEnumerable<MethodInfo>) new MethodInfo[2]
      {
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => default (IEnumerable).AsQueryable())),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => default (IEnumerable<object>).AsQueryable<object>()))
      }, typeof (AsQueryableExpressionNode));
      nodeTypeRegistry.Register((IEnumerable<MethodInfo>) new MethodInfo[1]
      {
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => default (IQueryable<object>).Timeout<object>(0)))
      }, typeof (TimeoutExpressionNode));
      CompoundNodeTypeProvider nodeTypeProvider = ExpressionTreeParser.CreateDefaultNodeTypeProvider();
      nodeTypeProvider.InnerProviders.Add((INodeTypeProvider) nodeTypeRegistry);
      this.defaultNodeTypeProvider = (INodeTypeProvider) nodeTypeProvider;
    }

    public bool IsRegistered(MethodInfo method)
    {
      return (method.DeclaringType != typeof (IDictionary) || !(method.Name == "Contains")) && this.defaultNodeTypeProvider.IsRegistered(method);
    }

    public Type GetNodeType(MethodInfo method) => this.defaultNodeTypeProvider.GetNodeType(method);
  }
}
