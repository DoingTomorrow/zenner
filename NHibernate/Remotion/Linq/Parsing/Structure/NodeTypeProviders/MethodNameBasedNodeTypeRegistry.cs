// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.NodeTypeProviders.MethodNameBasedNodeTypeRegistry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Collections;
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
  public class MethodNameBasedNodeTypeRegistry : INodeTypeProvider
  {
    private readonly MultiDictionary<string, KeyValuePair<NameBasedRegistrationInfo, Type>> _registeredNamedTypes = new MultiDictionary<string, KeyValuePair<NameBasedRegistrationInfo, Type>>();

    public static MethodNameBasedNodeTypeRegistry CreateFromTypes(Type[] searchedTypes)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      IEnumerable<\u003C\u003Ef__AnonymousType1<Type, IEnumerable<NameBasedRegistrationInfo>>> linq1735139FAnonymousType1s = ((IEnumerable<Type>) searchedTypes).Where<Type>((Func<Type, bool>) (t => typeof (IExpressionNode).IsAssignableFrom(t))).Select<Type, \u003C\u003Ef__AnonymousType0<Type, FieldInfo>>((Func<Type, \u003C\u003Ef__AnonymousType0<Type, FieldInfo>>) (t => new \u003C\u003Ef__AnonymousType0<Type, FieldInfo>(t, t.GetField("SupportedMethodNames", BindingFlags.Static | BindingFlags.Public)))).Select<\u003C\u003Ef__AnonymousType0<Type, FieldInfo>, \u003C\u003Ef__AnonymousType1<Type, IEnumerable<NameBasedRegistrationInfo>>>((Func<\u003C\u003Ef__AnonymousType0<Type, FieldInfo>, \u003C\u003Ef__AnonymousType1<Type, IEnumerable<NameBasedRegistrationInfo>>>) (_param0 => new \u003C\u003Ef__AnonymousType1<Type, IEnumerable<NameBasedRegistrationInfo>>(_param0.t, _param0.supportedMethodNamesField != null ? (IEnumerable<NameBasedRegistrationInfo>) _param0.supportedMethodNamesField.GetValue((object) null) : Enumerable.Empty<NameBasedRegistrationInfo>())));
      MethodNameBasedNodeTypeRegistry fromTypes = new MethodNameBasedNodeTypeRegistry();
      foreach (\u003C\u003Ef__AnonymousType1<Type, IEnumerable<NameBasedRegistrationInfo>> linq1735139FAnonymousType1 in linq1735139FAnonymousType1s)
        fromTypes.Register(linq1735139FAnonymousType1.RegistrationInfo, linq1735139FAnonymousType1.Type);
      return fromTypes;
    }

    public int RegisteredNamesCount => this._registeredNamedTypes.CountValues();

    public void Register(
      IEnumerable<NameBasedRegistrationInfo> registrationInfo,
      Type nodeType)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<NameBasedRegistrationInfo>>(nameof (registrationInfo), registrationInfo);
      ArgumentUtility.CheckNotNull<Type>(nameof (nodeType), nodeType);
      foreach (NameBasedRegistrationInfo key in registrationInfo)
        this._registeredNamedTypes.Add(key.Name, new KeyValuePair<NameBasedRegistrationInfo, Type>(key, nodeType));
    }

    public bool IsRegistered(MethodInfo method)
    {
      ArgumentUtility.CheckNotNull<MethodInfo>(nameof (method), method);
      return this.GetNodeType(method) != null;
    }

    public Type GetNodeType(MethodInfo method)
    {
      ArgumentUtility.CheckNotNull<MethodInfo>(nameof (method), method);
      IList<KeyValuePair<NameBasedRegistrationInfo, Type>> source;
      return !this._registeredNamedTypes.TryGetValue(method.Name, out source) ? (Type) null : source.Where<KeyValuePair<NameBasedRegistrationInfo, Type>>((Func<KeyValuePair<NameBasedRegistrationInfo, Type>, bool>) (info => info.Key.Filter(method))).Select<KeyValuePair<NameBasedRegistrationInfo, Type>, Type>((Func<KeyValuePair<NameBasedRegistrationInfo, Type>, Type>) (info => info.Value)).FirstOrDefault<Type>();
    }
  }
}
