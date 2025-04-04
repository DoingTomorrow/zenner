// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DynamicProxy.ProxyObjectReference
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Proxy.DynamicProxy
{
  [Serializable]
  public class ProxyObjectReference : IObjectReference, ISerializable
  {
    private readonly Type _baseType;
    private readonly IProxy _proxy;

    protected ProxyObjectReference(SerializationInfo info, StreamingContext context)
    {
      this._baseType = Type.GetType(info.GetString("__baseType"), true, false);
      List<Type> typeList = new List<Type>();
      int int32 = info.GetInt32("__baseInterfaceCount");
      for (int index = 0; index < int32; ++index)
      {
        string name = string.Format("__baseInterface{0}", (object) index);
        Type type = Type.GetType(info.GetString(name), true, false);
        typeList.Add(type);
      }
      this._proxy = (IProxy) Activator.CreateInstance(new ProxyFactory().CreateProxyType(this._baseType, typeList.ToArray()), (object) info, (object) context);
    }

    public object GetRealObject(StreamingContext context) => (object) this._proxy;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
    }
  }
}
