﻿// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Extensible
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Meta;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace ProtoBuf
{
  public abstract class Extensible : IExtensible
  {
    private IExtension extensionObject;

    IExtension IExtensible.GetExtensionObject(bool createIfMissing)
    {
      return this.GetExtensionObject(createIfMissing);
    }

    protected virtual IExtension GetExtensionObject(bool createIfMissing)
    {
      return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
    }

    public static IExtension GetExtensionObject(
      ref IExtension extensionObject,
      bool createIfMissing)
    {
      if (createIfMissing && extensionObject == null)
        extensionObject = (IExtension) new BufferExtension();
      return extensionObject;
    }

    public static void AppendValue<TValue>(IExtensible instance, int tag, TValue value)
    {
      Extensible.AppendValue<TValue>(instance, tag, DataFormat.Default, value);
    }

    public static void AppendValue<TValue>(
      IExtensible instance,
      int tag,
      DataFormat format,
      TValue value)
    {
      ExtensibleUtil.AppendExtendValue((TypeModel) RuntimeTypeModel.Default, instance, tag, format, (object) value);
    }

    public static TValue GetValue<TValue>(IExtensible instance, int tag)
    {
      return Extensible.GetValue<TValue>(instance, tag, DataFormat.Default);
    }

    public static TValue GetValue<TValue>(IExtensible instance, int tag, DataFormat format)
    {
      TValue obj;
      Extensible.TryGetValue<TValue>(instance, tag, format, out obj);
      return obj;
    }

    public static bool TryGetValue<TValue>(IExtensible instance, int tag, out TValue value)
    {
      return Extensible.TryGetValue<TValue>(instance, tag, DataFormat.Default, out value);
    }

    public static bool TryGetValue<TValue>(
      IExtensible instance,
      int tag,
      DataFormat format,
      out TValue value)
    {
      return Extensible.TryGetValue<TValue>(instance, tag, format, false, out value);
    }

    public static bool TryGetValue<TValue>(
      IExtensible instance,
      int tag,
      DataFormat format,
      bool allowDefinedTag,
      out TValue value)
    {
      value = default (TValue);
      bool flag = false;
      foreach (TValue extendedValue in ExtensibleUtil.GetExtendedValues<TValue>(instance, tag, format, true, allowDefinedTag))
      {
        value = extendedValue;
        flag = true;
      }
      return flag;
    }

    public static IEnumerable<TValue> GetValues<TValue>(IExtensible instance, int tag)
    {
      return ExtensibleUtil.GetExtendedValues<TValue>(instance, tag, DataFormat.Default, false, false);
    }

    public static IEnumerable<TValue> GetValues<TValue>(
      IExtensible instance,
      int tag,
      DataFormat format)
    {
      return ExtensibleUtil.GetExtendedValues<TValue>(instance, tag, format, false, false);
    }

    public static bool TryGetValue(
      TypeModel model,
      Type type,
      IExtensible instance,
      int tag,
      DataFormat format,
      bool allowDefinedTag,
      out object value)
    {
      value = (object) null;
      bool flag = false;
      foreach (object extendedValue in ExtensibleUtil.GetExtendedValues(model, type, instance, tag, format, true, allowDefinedTag))
      {
        value = extendedValue;
        flag = true;
      }
      return flag;
    }

    public static IEnumerable GetValues(
      TypeModel model,
      Type type,
      IExtensible instance,
      int tag,
      DataFormat format)
    {
      return ExtensibleUtil.GetExtendedValues(model, type, instance, tag, format, false, false);
    }

    public static void AppendValue(
      TypeModel model,
      IExtensible instance,
      int tag,
      DataFormat format,
      object value)
    {
      ExtensibleUtil.AppendExtendValue(model, instance, tag, format, value);
    }
  }
}
