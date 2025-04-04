// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializer
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace ProtoBuf
{
  public static class Serializer
  {
    private const string ProtoBinaryField = "proto";
    public const int ListItemTag = 1;

    public static string GetProto<T>()
    {
      return RuntimeTypeModel.Default.GetSchema(RuntimeTypeModel.Default.MapType(typeof (T)));
    }

    public static T DeepClone<T>(T instance)
    {
      return (object) instance != null ? (T) RuntimeTypeModel.Default.DeepClone((object) instance) : instance;
    }

    public static T Merge<T>(Stream source, T instance)
    {
      return (T) RuntimeTypeModel.Default.Deserialize(source, (object) instance, typeof (T));
    }

    public static T Deserialize<T>(Stream source)
    {
      return (T) RuntimeTypeModel.Default.Deserialize(source, (object) null, typeof (T));
    }

    public static object Deserialize(Type type, Stream source)
    {
      return RuntimeTypeModel.Default.Deserialize(source, (object) null, type);
    }

    public static void Serialize<T>(Stream destination, T instance)
    {
      if ((object) instance == null)
        return;
      RuntimeTypeModel.Default.Serialize(destination, (object) instance);
    }

    public static TTo ChangeType<TFrom, TTo>(TFrom instance)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Serializer.Serialize<TFrom>((Stream) memoryStream, instance);
        memoryStream.Position = 0L;
        return Serializer.Deserialize<TTo>((Stream) memoryStream);
      }
    }

    public static void Serialize<T>(SerializationInfo info, T instance) where T : class, ISerializable
    {
      Serializer.Serialize<T>(info, new StreamingContext(StreamingContextStates.Persistence), instance);
    }

    public static void Serialize<T>(SerializationInfo info, StreamingContext context, T instance) where T : class, ISerializable
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      if ((object) instance == null)
        throw new ArgumentNullException(nameof (instance));
      if (instance.GetType() != typeof (T))
        throw new ArgumentException("Incorrect type", nameof (instance));
      using (MemoryStream dest = new MemoryStream())
      {
        RuntimeTypeModel.Default.Serialize((Stream) dest, (object) instance, (SerializationContext) context);
        info.AddValue("proto", (object) dest.ToArray());
      }
    }

    public static void Serialize<T>(XmlWriter writer, T instance) where T : IXmlSerializable
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if ((object) instance == null)
        throw new ArgumentNullException(nameof (instance));
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Serializer.Serialize<T>((Stream) memoryStream, instance);
        writer.WriteBase64(Helpers.GetBuffer(memoryStream), 0, (int) memoryStream.Length);
      }
    }

    public static void Merge<T>(XmlReader reader, T instance) where T : IXmlSerializable
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      if ((object) instance == null)
        throw new ArgumentNullException(nameof (instance));
      byte[] buffer = new byte[4096];
      using (MemoryStream source = new MemoryStream())
      {
        int depth = reader.Depth;
        while (reader.Read() && reader.Depth > depth)
        {
          if (reader.NodeType == XmlNodeType.Text)
          {
            int count;
            while ((count = reader.ReadContentAsBase64(buffer, 0, 4096)) > 0)
              source.Write(buffer, 0, count);
            if (reader.Depth <= depth)
              break;
          }
        }
        source.Position = 0L;
        Serializer.Merge<T>((Stream) source, instance);
      }
    }

    public static void Merge<T>(SerializationInfo info, T instance) where T : class, ISerializable
    {
      Serializer.Merge<T>(info, new StreamingContext(StreamingContextStates.Persistence), instance);
    }

    public static void Merge<T>(SerializationInfo info, StreamingContext context, T instance) where T : class, ISerializable
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      if ((object) instance == null)
        throw new ArgumentNullException(nameof (instance));
      if (instance.GetType() != typeof (T))
        throw new ArgumentException("Incorrect type", nameof (instance));
      using (MemoryStream source = new MemoryStream((byte[]) info.GetValue("proto", typeof (byte[]))))
      {
        if ((object) (T) RuntimeTypeModel.Default.Deserialize((Stream) source, (object) instance, typeof (T), (SerializationContext) context) != (object) instance)
          throw new ProtoException("Deserialization changed the instance; cannot succeed.");
      }
    }

    public static void PrepareSerializer<T>()
    {
      RuntimeTypeModel runtimeTypeModel = RuntimeTypeModel.Default;
      runtimeTypeModel[runtimeTypeModel.MapType(typeof (T))].CompileInPlace();
    }

    public static IFormatter CreateFormatter<T>()
    {
      return RuntimeTypeModel.Default.CreateFormatter(typeof (T));
    }

    public static IEnumerable<T> DeserializeItems<T>(
      Stream source,
      PrefixStyle style,
      int fieldNumber)
    {
      return RuntimeTypeModel.Default.DeserializeItems<T>(source, style, fieldNumber);
    }

    public static T DeserializeWithLengthPrefix<T>(Stream source, PrefixStyle style)
    {
      return Serializer.DeserializeWithLengthPrefix<T>(source, style, 0);
    }

    public static T DeserializeWithLengthPrefix<T>(
      Stream source,
      PrefixStyle style,
      int fieldNumber)
    {
      RuntimeTypeModel runtimeTypeModel = RuntimeTypeModel.Default;
      return (T) runtimeTypeModel.DeserializeWithLengthPrefix(source, (object) null, runtimeTypeModel.MapType(typeof (T)), style, fieldNumber);
    }

    public static T MergeWithLengthPrefix<T>(Stream source, T instance, PrefixStyle style)
    {
      RuntimeTypeModel runtimeTypeModel = RuntimeTypeModel.Default;
      return (T) runtimeTypeModel.DeserializeWithLengthPrefix(source, (object) instance, runtimeTypeModel.MapType(typeof (T)), style, 0);
    }

    public static void SerializeWithLengthPrefix<T>(
      Stream destination,
      T instance,
      PrefixStyle style)
    {
      Serializer.SerializeWithLengthPrefix<T>(destination, instance, style, 0);
    }

    public static void SerializeWithLengthPrefix<T>(
      Stream destination,
      T instance,
      PrefixStyle style,
      int fieldNumber)
    {
      RuntimeTypeModel runtimeTypeModel = RuntimeTypeModel.Default;
      runtimeTypeModel.SerializeWithLengthPrefix(destination, (object) instance, runtimeTypeModel.MapType(typeof (T)), style, fieldNumber);
    }

    public static bool TryReadLengthPrefix(Stream source, PrefixStyle style, out int length)
    {
      int bytesRead;
      length = ProtoReader.ReadLengthPrefix(source, false, style, out int _, out bytesRead);
      return bytesRead > 0;
    }

    public static bool TryReadLengthPrefix(
      byte[] buffer,
      int index,
      int count,
      PrefixStyle style,
      out int length)
    {
      using (Stream source = (Stream) new MemoryStream(buffer, index, count))
        return Serializer.TryReadLengthPrefix(source, style, out length);
    }

    public static void FlushPool() => BufferPool.Flush();

    public static class NonGeneric
    {
      public static object DeepClone(object instance)
      {
        return instance != null ? RuntimeTypeModel.Default.DeepClone(instance) : (object) null;
      }

      public static void Serialize(Stream dest, object instance)
      {
        if (instance == null)
          return;
        RuntimeTypeModel.Default.Serialize(dest, instance);
      }

      public static object Deserialize(Type type, Stream source)
      {
        return RuntimeTypeModel.Default.Deserialize(source, (object) null, type);
      }

      public static object Merge(Stream source, object instance)
      {
        if (instance == null)
          throw new ArgumentNullException(nameof (instance));
        return RuntimeTypeModel.Default.Deserialize(source, instance, instance.GetType(), (SerializationContext) null);
      }

      public static void SerializeWithLengthPrefix(
        Stream destination,
        object instance,
        PrefixStyle style,
        int fieldNumber)
      {
        if (instance == null)
          throw new ArgumentNullException(nameof (instance));
        RuntimeTypeModel runtimeTypeModel = RuntimeTypeModel.Default;
        runtimeTypeModel.SerializeWithLengthPrefix(destination, instance, runtimeTypeModel.MapType(instance.GetType()), style, fieldNumber);
      }

      public static bool TryDeserializeWithLengthPrefix(
        Stream source,
        PrefixStyle style,
        Serializer.TypeResolver resolver,
        out object value)
      {
        value = RuntimeTypeModel.Default.DeserializeWithLengthPrefix(source, (object) null, (Type) null, style, 0, resolver);
        return value != null;
      }

      public static bool CanSerialize(Type type) => RuntimeTypeModel.Default.IsDefined(type);
    }

    public static class GlobalOptions
    {
      [Obsolete("Please use RuntimeTypeModel.Default.InferTagFromNameDefault instead (or on a per-model basis)", false)]
      public static bool InferTagFromName
      {
        get => RuntimeTypeModel.Default.InferTagFromNameDefault;
        set => RuntimeTypeModel.Default.InferTagFromNameDefault = value;
      }
    }

    public delegate Type TypeResolver(int fieldNumber);
  }
}
