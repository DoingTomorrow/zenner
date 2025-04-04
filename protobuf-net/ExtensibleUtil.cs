// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ExtensibleUtil
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Meta;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace ProtoBuf
{
  internal static class ExtensibleUtil
  {
    internal static IEnumerable<TValue> GetExtendedValues<TValue>(
      IExtensible instance,
      int tag,
      DataFormat format,
      bool singleton,
      bool allowDefinedTag)
    {
      foreach (TValue extendedValue in ExtensibleUtil.GetExtendedValues((TypeModel) RuntimeTypeModel.Default, typeof (TValue), instance, tag, format, singleton, allowDefinedTag))
        yield return extendedValue;
    }

    internal static IEnumerable GetExtendedValues(
      TypeModel model,
      Type type,
      IExtensible instance,
      int tag,
      DataFormat format,
      bool singleton,
      bool allowDefinedTag)
    {
      if (instance == null)
        throw new ArgumentNullException(nameof (instance));
      if (tag <= 0)
        throw new ArgumentOutOfRangeException(nameof (tag));
      IExtension extn = instance.GetExtensionObject(false);
      if (extn != null)
      {
        Stream stream = extn.BeginQuery();
        object extendedValue = (object) null;
        ProtoReader reader = (ProtoReader) null;
        try
        {
          reader = ProtoReader.Create(stream, model, new SerializationContext(), -1);
          while (model.TryDeserializeAuxiliaryType(reader, format, tag, type, ref extendedValue, true, false, false, false) && extendedValue != null)
          {
            if (!singleton)
            {
              yield return extendedValue;
              extendedValue = (object) null;
            }
          }
          if (singleton && extendedValue != null)
            yield return extendedValue;
        }
        finally
        {
          ProtoReader.Recycle(reader);
          extn.EndQuery(stream);
        }
      }
    }

    internal static void AppendExtendValue(
      TypeModel model,
      IExtensible instance,
      int tag,
      DataFormat format,
      object value)
    {
      if (instance == null)
        throw new ArgumentNullException(nameof (instance));
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      IExtension extensionObject = instance.GetExtensionObject(true);
      if (extensionObject == null)
        throw new InvalidOperationException("No extension object available; appended data would be lost.");
      bool commit = false;
      Stream stream = extensionObject.BeginAppend();
      try
      {
        using (ProtoWriter writer = new ProtoWriter(stream, model, (SerializationContext) null))
        {
          model.TrySerializeAuxiliaryType(writer, (Type) null, format, tag, value, false);
          writer.Close();
        }
        commit = true;
      }
      finally
      {
        extensionObject.EndAppend(stream, commit);
      }
    }
  }
}
