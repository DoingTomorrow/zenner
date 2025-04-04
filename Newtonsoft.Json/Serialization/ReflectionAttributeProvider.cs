// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ReflectionAttributeProvider
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;

#nullable disable
namespace Newtonsoft.Json.Serialization
{
  public class ReflectionAttributeProvider : IAttributeProvider
  {
    private readonly object _attributeProvider;

    public ReflectionAttributeProvider(object attributeProvider)
    {
      ValidationUtils.ArgumentNotNull(attributeProvider, nameof (attributeProvider));
      this._attributeProvider = attributeProvider;
    }

    public IList<Attribute> GetAttributes(bool inherit)
    {
      return (IList<Attribute>) ReflectionUtils.GetAttributes(this._attributeProvider, (Type) null, inherit);
    }

    public IList<Attribute> GetAttributes(Type attributeType, bool inherit)
    {
      return (IList<Attribute>) ReflectionUtils.GetAttributes(this._attributeProvider, attributeType, inherit);
    }
  }
}
