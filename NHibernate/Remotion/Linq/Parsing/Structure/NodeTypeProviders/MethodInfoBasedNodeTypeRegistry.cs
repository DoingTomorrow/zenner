// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.NodeTypeProviders.MethodInfoBasedNodeTypeRegistry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Parsing.Structure.IntermediateModel;
using Remotion.Linq.Utilities;
using Remotion.Linq1735139;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.NodeTypeProviders
{
  public class MethodInfoBasedNodeTypeRegistry : INodeTypeProvider
  {
    private readonly Dictionary<MethodInfo, Type> _registeredMethodInfoTypes = new Dictionary<MethodInfo, Type>();

    public static MethodInfoBasedNodeTypeRegistry CreateFromTypes(IEnumerable<Type> searchedTypes)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<Type>>(nameof (searchedTypes), searchedTypes);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      IEnumerable<\u003C\u003Ef__AnonymousType3<Type, IEnumerable<MethodInfo>>> linq1735139FAnonymousType3s = searchedTypes.Where<Type>((Func<Type, bool>) (t => typeof (IExpressionNode).IsAssignableFrom(t))).Select<Type, \u003C\u003Ef__AnonymousType2<Type, FieldInfo>>((Func<Type, \u003C\u003Ef__AnonymousType2<Type, FieldInfo>>) (t => new \u003C\u003Ef__AnonymousType2<Type, FieldInfo>(t, t.GetField("SupportedMethods", BindingFlags.Static | BindingFlags.Public)))).Select<\u003C\u003Ef__AnonymousType2<Type, FieldInfo>, \u003C\u003Ef__AnonymousType3<Type, IEnumerable<MethodInfo>>>((Func<\u003C\u003Ef__AnonymousType2<Type, FieldInfo>, \u003C\u003Ef__AnonymousType3<Type, IEnumerable<MethodInfo>>>) (_param0 => new \u003C\u003Ef__AnonymousType3<Type, IEnumerable<MethodInfo>>(_param0.t, _param0.supportedMethodsField != null ? (IEnumerable<MethodInfo>) _param0.supportedMethodsField.GetValue((object) null) : Enumerable.Empty<MethodInfo>())));
      MethodInfoBasedNodeTypeRegistry fromTypes = new MethodInfoBasedNodeTypeRegistry();
      foreach (\u003C\u003Ef__AnonymousType3<Type, IEnumerable<MethodInfo>> linq1735139FAnonymousType3 in linq1735139FAnonymousType3s)
        fromTypes.Register(linq1735139FAnonymousType3.Methods, linq1735139FAnonymousType3.Type);
      return fromTypes;
    }

    public static MethodInfo GetRegisterableMethodDefinition(MethodInfo method)
    {
      MethodInfo methodDefinition = method.IsGenericMethod ? method.GetGenericMethodDefinition() : method;
      if (!methodDefinition.DeclaringType.IsGenericType)
        return methodDefinition;
      Type genericTypeDefinition = methodDefinition.DeclaringType.GetGenericTypeDefinition();
      return (MethodInfo) MethodBase.GetMethodFromHandle(methodDefinition.MethodHandle, genericTypeDefinition.TypeHandle);
    }

    public int RegisteredMethodInfoCount => this._registeredMethodInfoTypes.Count;

    public void Register(IEnumerable<MethodInfo> methods, Type nodeType)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<MethodInfo>>(nameof (methods), methods);
      ArgumentUtility.CheckNotNull<Type>(nameof (nodeType), nodeType);
      ArgumentUtility.CheckTypeIsAssignableFrom(nameof (nodeType), nodeType, typeof (IExpressionNode));
      foreach (MethodInfo method in methods)
      {
        if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
          throw new InvalidOperationException(string.Format("Cannot register closed generic method '{0}', try to register its generic method definition instead.", (object) method.Name));
        if (method.DeclaringType.IsGenericType && !method.DeclaringType.IsGenericTypeDefinition)
          throw new InvalidOperationException(string.Format("Cannot register method '{0}' in closed generic type '{1}', try to register its equivalent in the generic type definition instead.", (object) method.Name, (object) method.DeclaringType));
        this._registeredMethodInfoTypes[method] = nodeType;
      }
    }

    public bool IsRegistered(MethodInfo method)
    {
      ArgumentUtility.CheckNotNull<MethodInfo>(nameof (method), method);
      return this._registeredMethodInfoTypes.ContainsKey(MethodInfoBasedNodeTypeRegistry.GetRegisterableMethodDefinition(method));
    }

    public Type GetNodeType(MethodInfo method)
    {
      ArgumentUtility.CheckNotNull<MethodInfo>(nameof (method), method);
      Type nodeType;
      this._registeredMethodInfoTypes.TryGetValue(MethodInfoBasedNodeTypeRegistry.GetRegisterableMethodDefinition(method), out nodeType);
      return nodeType;
    }
  }
}
