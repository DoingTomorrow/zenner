// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.AttributesToAvoidReplicating
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public static class AttributesToAvoidReplicating
  {
    private static readonly IList<Type> attributes = (IList<Type>) new List<Type>();

    static AttributesToAvoidReplicating()
    {
      AttributesToAvoidReplicating.Add<ComImportAttribute>();
      AttributesToAvoidReplicating.Add<SecurityPermissionAttribute>();
    }

    public static void Add(Type attribute)
    {
      if (AttributesToAvoidReplicating.attributes.Contains(attribute))
        return;
      AttributesToAvoidReplicating.attributes.Add(attribute);
    }

    public static void Add<T>() => AttributesToAvoidReplicating.Add(typeof (T));

    public static bool Contains(Type type)
    {
      return AttributesToAvoidReplicating.attributes.Contains(type);
    }
  }
}
