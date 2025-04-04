// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.TypeModel
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

#nullable disable
namespace ProtoBuf.Meta
{
  public abstract class TypeModel
  {
    private static readonly Type ilist = typeof (IList);

    protected internal virtual bool SerializeDateTimeKind() => false;

    protected internal Type MapType(Type type) => this.MapType(type, true);

    protected internal virtual Type MapType(Type type, bool demand) => type;

    private WireType GetWireType(
      ProtoTypeCode code,
      DataFormat format,
      ref Type type,
      out int modelKey)
    {
      modelKey = -1;
      if (Helpers.IsEnum(type))
      {
        modelKey = this.GetKey(ref type);
        return WireType.Variant;
      }
      switch (code)
      {
        case ProtoTypeCode.Boolean:
        case ProtoTypeCode.Char:
        case ProtoTypeCode.SByte:
        case ProtoTypeCode.Byte:
        case ProtoTypeCode.Int16:
        case ProtoTypeCode.UInt16:
        case ProtoTypeCode.Int32:
        case ProtoTypeCode.UInt32:
          return format != DataFormat.FixedSize ? WireType.Variant : WireType.Fixed32;
        case ProtoTypeCode.Int64:
        case ProtoTypeCode.UInt64:
          return format != DataFormat.FixedSize ? WireType.Variant : WireType.Fixed64;
        case ProtoTypeCode.Single:
          return WireType.Fixed32;
        case ProtoTypeCode.Double:
          return WireType.Fixed64;
        case ProtoTypeCode.Decimal:
        case ProtoTypeCode.DateTime:
        case ProtoTypeCode.String:
        case ProtoTypeCode.TimeSpan:
        case ProtoTypeCode.ByteArray:
        case ProtoTypeCode.Guid:
        case ProtoTypeCode.Uri:
          return WireType.String;
        default:
          return (modelKey = this.GetKey(ref type)) >= 0 ? WireType.String : WireType.None;
      }
    }

    internal bool TrySerializeAuxiliaryType(
      ProtoWriter writer,
      Type type,
      DataFormat format,
      int tag,
      object value,
      bool isInsideList)
    {
      if (type == (Type) null)
        type = value.GetType();
      ProtoTypeCode typeCode = Helpers.GetTypeCode(type);
      int modelKey;
      WireType wireType = this.GetWireType(typeCode, format, ref type, out modelKey);
      if (modelKey >= 0)
      {
        if (Helpers.IsEnum(type))
        {
          this.Serialize(modelKey, value, writer);
          return true;
        }
        ProtoWriter.WriteFieldHeader(tag, wireType, writer);
        switch (wireType)
        {
          case WireType.None:
            throw ProtoWriter.CreateException(writer);
          case WireType.String:
          case WireType.StartGroup:
            SubItemToken token = ProtoWriter.StartSubItem(value, writer);
            this.Serialize(modelKey, value, writer);
            ProtoWriter writer1 = writer;
            ProtoWriter.EndSubItem(token, writer1);
            return true;
          default:
            this.Serialize(modelKey, value, writer);
            return true;
        }
      }
      else
      {
        if (wireType != WireType.None)
          ProtoWriter.WriteFieldHeader(tag, wireType, writer);
        switch (typeCode)
        {
          case ProtoTypeCode.Boolean:
            ProtoWriter.WriteBoolean((bool) value, writer);
            return true;
          case ProtoTypeCode.Char:
            ProtoWriter.WriteUInt16((ushort) (char) value, writer);
            return true;
          case ProtoTypeCode.SByte:
            ProtoWriter.WriteSByte((sbyte) value, writer);
            return true;
          case ProtoTypeCode.Byte:
            ProtoWriter.WriteByte((byte) value, writer);
            return true;
          case ProtoTypeCode.Int16:
            ProtoWriter.WriteInt16((short) value, writer);
            return true;
          case ProtoTypeCode.UInt16:
            ProtoWriter.WriteUInt16((ushort) value, writer);
            return true;
          case ProtoTypeCode.Int32:
            ProtoWriter.WriteInt32((int) value, writer);
            return true;
          case ProtoTypeCode.UInt32:
            ProtoWriter.WriteUInt32((uint) value, writer);
            return true;
          case ProtoTypeCode.Int64:
            ProtoWriter.WriteInt64((long) value, writer);
            return true;
          case ProtoTypeCode.UInt64:
            ProtoWriter.WriteUInt64((ulong) value, writer);
            return true;
          case ProtoTypeCode.Single:
            ProtoWriter.WriteSingle((float) value, writer);
            return true;
          case ProtoTypeCode.Double:
            ProtoWriter.WriteDouble((double) value, writer);
            return true;
          case ProtoTypeCode.Decimal:
            BclHelpers.WriteDecimal((Decimal) value, writer);
            return true;
          case ProtoTypeCode.DateTime:
            if (this.SerializeDateTimeKind())
              BclHelpers.WriteDateTimeWithKind((DateTime) value, writer);
            else
              BclHelpers.WriteDateTime((DateTime) value, writer);
            return true;
          case ProtoTypeCode.String:
            ProtoWriter.WriteString((string) value, writer);
            return true;
          case ProtoTypeCode.TimeSpan:
            BclHelpers.WriteTimeSpan((TimeSpan) value, writer);
            return true;
          case ProtoTypeCode.ByteArray:
            ProtoWriter.WriteBytes((byte[]) value, writer);
            return true;
          case ProtoTypeCode.Guid:
            BclHelpers.WriteGuid((Guid) value, writer);
            return true;
          case ProtoTypeCode.Uri:
            ProtoWriter.WriteString(((Uri) value).AbsoluteUri, writer);
            return true;
          default:
            if (!(value is IEnumerable enumerable))
              return false;
            if (isInsideList)
              throw TypeModel.CreateNestedListsNotSupported();
            foreach (object obj in enumerable)
            {
              if (obj == null)
                throw new NullReferenceException();
              if (!this.TrySerializeAuxiliaryType(writer, (Type) null, format, tag, obj, true))
                TypeModel.ThrowUnexpectedType(obj.GetType());
            }
            return true;
        }
      }
    }

    private void SerializeCore(ProtoWriter writer, object value)
    {
      Type type = value != null ? value.GetType() : throw new ArgumentNullException(nameof (value));
      int key = this.GetKey(ref type);
      if (key >= 0)
      {
        this.Serialize(key, value, writer);
      }
      else
      {
        if (this.TrySerializeAuxiliaryType(writer, type, DataFormat.Default, 1, value, false))
          return;
        TypeModel.ThrowUnexpectedType(type);
      }
    }

    public void Serialize(Stream dest, object value)
    {
      this.Serialize(dest, value, (SerializationContext) null);
    }

    public void Serialize(Stream dest, object value, SerializationContext context)
    {
      using (ProtoWriter writer = new ProtoWriter(dest, this, context))
      {
        writer.SetRootObject(value);
        this.SerializeCore(writer, value);
        writer.Close();
      }
    }

    public void Serialize(ProtoWriter dest, object value)
    {
      if (dest == null)
        throw new ArgumentNullException(nameof (dest));
      dest.CheckDepthFlushlock();
      dest.SetRootObject(value);
      this.SerializeCore(dest, value);
      dest.CheckDepthFlushlock();
      ProtoWriter.Flush(dest);
    }

    public object DeserializeWithLengthPrefix(
      Stream source,
      object value,
      Type type,
      PrefixStyle style,
      int fieldNumber)
    {
      return this.DeserializeWithLengthPrefix(source, value, type, style, fieldNumber, (Serializer.TypeResolver) null, out int _);
    }

    public object DeserializeWithLengthPrefix(
      Stream source,
      object value,
      Type type,
      PrefixStyle style,
      int expectedField,
      Serializer.TypeResolver resolver)
    {
      return this.DeserializeWithLengthPrefix(source, value, type, style, expectedField, resolver, out int _);
    }

    public object DeserializeWithLengthPrefix(
      Stream source,
      object value,
      Type type,
      PrefixStyle style,
      int expectedField,
      Serializer.TypeResolver resolver,
      out int bytesRead)
    {
      return this.DeserializeWithLengthPrefix(source, value, type, style, expectedField, resolver, out bytesRead, out bool _, (SerializationContext) null);
    }

    private object DeserializeWithLengthPrefix(
      Stream source,
      object value,
      Type type,
      PrefixStyle style,
      int expectedField,
      Serializer.TypeResolver resolver,
      out int bytesRead,
      out bool haveObject,
      SerializationContext context)
    {
      haveObject = false;
      bytesRead = 0;
      if (type == (Type) null && (style != PrefixStyle.Base128 || resolver == null))
        throw new InvalidOperationException("A type must be provided unless base-128 prefixing is being used in combination with a resolver");
      int num;
      bool flag;
      do
      {
        bool expectHeader = expectedField > 0 || resolver != null;
        int fieldNumber;
        int bytesRead1;
        num = ProtoReader.ReadLengthPrefix(source, expectHeader, style, out fieldNumber, out bytesRead1);
        if (bytesRead1 == 0)
          return value;
        bytesRead += bytesRead1;
        if (num < 0)
          return value;
        if (style == PrefixStyle.Base128)
        {
          if (expectHeader && expectedField == 0 && type == (Type) null && resolver != null)
          {
            type = resolver(fieldNumber);
            flag = type == (Type) null;
          }
          else
            flag = expectedField != fieldNumber;
        }
        else
          flag = false;
        if (flag)
        {
          if (num == int.MaxValue)
            throw new InvalidOperationException();
          ProtoReader.Seek(source, num, (byte[]) null);
          bytesRead += num;
        }
      }
      while (flag);
      ProtoReader protoReader = (ProtoReader) null;
      try
      {
        protoReader = ProtoReader.Create(source, this, context, num);
        int key = this.GetKey(ref type);
        if (key >= 0 && !Helpers.IsEnum(type))
          value = this.Deserialize(key, value, protoReader);
        else if (!this.TryDeserializeAuxiliaryType(protoReader, DataFormat.Default, 1, type, ref value, true, false, true, false) && num != 0)
          TypeModel.ThrowUnexpectedType(type);
        bytesRead += protoReader.Position;
        haveObject = true;
        return value;
      }
      finally
      {
        ProtoReader.Recycle(protoReader);
      }
    }

    public IEnumerable DeserializeItems(
      Stream source,
      Type type,
      PrefixStyle style,
      int expectedField,
      Serializer.TypeResolver resolver)
    {
      return this.DeserializeItems(source, type, style, expectedField, resolver, (SerializationContext) null);
    }

    public IEnumerable DeserializeItems(
      Stream source,
      Type type,
      PrefixStyle style,
      int expectedField,
      Serializer.TypeResolver resolver,
      SerializationContext context)
    {
      return (IEnumerable) new TypeModel.DeserializeItemsIterator(this, source, type, style, expectedField, resolver, context);
    }

    public IEnumerable<T> DeserializeItems<T>(Stream source, PrefixStyle style, int expectedField)
    {
      return this.DeserializeItems<T>(source, style, expectedField, (SerializationContext) null);
    }

    public IEnumerable<T> DeserializeItems<T>(
      Stream source,
      PrefixStyle style,
      int expectedField,
      SerializationContext context)
    {
      return (IEnumerable<T>) new TypeModel.DeserializeItemsIterator<T>(this, source, style, expectedField, context);
    }

    public void SerializeWithLengthPrefix(
      Stream dest,
      object value,
      Type type,
      PrefixStyle style,
      int fieldNumber)
    {
      this.SerializeWithLengthPrefix(dest, value, type, style, fieldNumber, (SerializationContext) null);
    }

    public void SerializeWithLengthPrefix(
      Stream dest,
      object value,
      Type type,
      PrefixStyle style,
      int fieldNumber,
      SerializationContext context)
    {
      if (type == (Type) null)
        type = value != null ? this.MapType(value.GetType()) : throw new ArgumentNullException(nameof (value));
      int key = this.GetKey(ref type);
      using (ProtoWriter protoWriter = new ProtoWriter(dest, this, context))
      {
        switch (style)
        {
          case PrefixStyle.None:
            this.Serialize(key, value, protoWriter);
            break;
          case PrefixStyle.Base128:
          case PrefixStyle.Fixed32:
          case PrefixStyle.Fixed32BigEndian:
            ProtoWriter.WriteObject(value, key, protoWriter, style, fieldNumber);
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (style));
        }
        protoWriter.Close();
      }
    }

    public object Deserialize(Stream source, object value, Type type)
    {
      return this.Deserialize(source, value, type, (SerializationContext) null);
    }

    public object Deserialize(
      Stream source,
      object value,
      Type type,
      SerializationContext context)
    {
      bool noAutoCreate = this.PrepareDeserialize(value, ref type);
      ProtoReader reader = (ProtoReader) null;
      try
      {
        reader = ProtoReader.Create(source, this, context, -1);
        if (value != null)
          reader.SetRootObject(value);
        object obj = this.DeserializeCore(reader, type, value, noAutoCreate);
        reader.CheckFullyConsumed();
        return obj;
      }
      finally
      {
        ProtoReader.Recycle(reader);
      }
    }

    private bool PrepareDeserialize(object value, ref Type type)
    {
      if (type == (Type) null)
        type = value != null ? this.MapType(value.GetType()) : throw new ArgumentNullException(nameof (type));
      bool flag = true;
      Type underlyingType = Helpers.GetUnderlyingType(type);
      if (underlyingType != (Type) null)
      {
        type = underlyingType;
        flag = false;
      }
      return flag;
    }

    public object Deserialize(Stream source, object value, Type type, int length)
    {
      return this.Deserialize(source, value, type, length, (SerializationContext) null);
    }

    public object Deserialize(
      Stream source,
      object value,
      Type type,
      int length,
      SerializationContext context)
    {
      bool noAutoCreate = this.PrepareDeserialize(value, ref type);
      ProtoReader reader = (ProtoReader) null;
      try
      {
        reader = ProtoReader.Create(source, this, context, length);
        if (value != null)
          reader.SetRootObject(value);
        object obj = this.DeserializeCore(reader, type, value, noAutoCreate);
        reader.CheckFullyConsumed();
        return obj;
      }
      finally
      {
        ProtoReader.Recycle(reader);
      }
    }

    public object Deserialize(ProtoReader source, object value, Type type)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      bool noAutoCreate = this.PrepareDeserialize(value, ref type);
      if (value != null)
        source.SetRootObject(value);
      object obj = this.DeserializeCore(source, type, value, noAutoCreate);
      source.CheckFullyConsumed();
      return obj;
    }

    private object DeserializeCore(ProtoReader reader, Type type, object value, bool noAutoCreate)
    {
      int key = this.GetKey(ref type);
      if (key >= 0 && !Helpers.IsEnum(type))
        return this.Deserialize(key, value, reader);
      this.TryDeserializeAuxiliaryType(reader, DataFormat.Default, 1, type, ref value, true, false, noAutoCreate, false);
      return value;
    }

    internal static MethodInfo ResolveListAdd(
      TypeModel model,
      Type listType,
      Type itemType,
      out bool isList)
    {
      Type type = listType;
      isList = model.MapType(TypeModel.ilist).IsAssignableFrom(type);
      Type[] types = new Type[1]{ itemType };
      MethodInfo instanceMethod = Helpers.GetInstanceMethod(type, "Add", types);
      if (instanceMethod == (MethodInfo) null)
      {
        int num = !type.IsInterface ? 0 : (type == model.MapType(typeof (IEnumerable<>)).MakeGenericType(types) ? 1 : 0);
        Type declaringType = model.MapType(typeof (ICollection<>)).MakeGenericType(types);
        if (num != 0 || declaringType.IsAssignableFrom(type))
          instanceMethod = Helpers.GetInstanceMethod(declaringType, "Add", types);
      }
      if (instanceMethod == (MethodInfo) null)
      {
        foreach (Type declaringType in type.GetInterfaces())
        {
          if (declaringType.Name == "IProducerConsumerCollection`1" && declaringType.IsGenericType && declaringType.GetGenericTypeDefinition().FullName == "System.Collections.Concurrent.IProducerConsumerCollection`1")
          {
            instanceMethod = Helpers.GetInstanceMethod(declaringType, "TryAdd", types);
            if (instanceMethod != (MethodInfo) null)
              break;
          }
        }
      }
      if (instanceMethod == (MethodInfo) null)
      {
        types[0] = model.MapType(typeof (object));
        instanceMethod = Helpers.GetInstanceMethod(type, "Add", types);
      }
      if (instanceMethod == (MethodInfo) null & isList)
        instanceMethod = Helpers.GetInstanceMethod(model.MapType(TypeModel.ilist), "Add", types);
      return instanceMethod;
    }

    internal static Type GetListItemType(TypeModel model, Type listType)
    {
      if (listType == model.MapType(typeof (string)) || listType.IsArray || !model.MapType(typeof (IEnumerable)).IsAssignableFrom(listType))
        return (Type) null;
      BasicList candidates = new BasicList();
      foreach (MethodInfo method in listType.GetMethods())
      {
        if (!method.IsStatic && !(method.Name != "Add"))
        {
          ParameterInfo[] parameters = method.GetParameters();
          Type parameterType;
          if (parameters.Length == 1 && !candidates.Contains((object) (parameterType = parameters[0].ParameterType)))
            candidates.Add((object) parameterType);
        }
      }
      string name = listType.Name;
      if ((name == null ? 0 : (name.IndexOf("Queue") >= 0 ? 1 : (name.IndexOf("Stack") >= 0 ? 1 : 0))) == 0)
      {
        TypeModel.TestEnumerableListPatterns(model, candidates, listType);
        foreach (Type iType in listType.GetInterfaces())
          TypeModel.TestEnumerableListPatterns(model, candidates, iType);
      }
      foreach (PropertyInfo property in listType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (!(property.Name != "Item") && !candidates.Contains((object) property.PropertyType))
        {
          ParameterInfo[] indexParameters = property.GetIndexParameters();
          if (indexParameters.Length == 1 && !(indexParameters[0].ParameterType != model.MapType(typeof (int))))
            candidates.Add((object) property.PropertyType);
        }
      }
      switch (candidates.Count)
      {
        case 0:
          return (Type) null;
        case 1:
          return (Type) candidates[0] == listType ? (Type) null : (Type) candidates[0];
        case 2:
          if ((Type) candidates[0] != listType && TypeModel.CheckDictionaryAccessors(model, (Type) candidates[0], (Type) candidates[1]))
            return (Type) candidates[0];
          if ((Type) candidates[1] != listType && TypeModel.CheckDictionaryAccessors(model, (Type) candidates[1], (Type) candidates[0]))
            return (Type) candidates[1];
          break;
      }
      return (Type) null;
    }

    private static void TestEnumerableListPatterns(
      TypeModel model,
      BasicList candidates,
      Type iType)
    {
      if (!iType.IsGenericType)
        return;
      Type genericTypeDefinition = iType.GetGenericTypeDefinition();
      if (!(genericTypeDefinition == model.MapType(typeof (IEnumerable<>))) && !(genericTypeDefinition == model.MapType(typeof (ICollection<>))) && !(genericTypeDefinition.FullName == "System.Collections.Concurrent.IProducerConsumerCollection`1"))
        return;
      Type[] genericArguments = iType.GetGenericArguments();
      if (candidates.Contains((object) genericArguments[0]))
        return;
      candidates.Add((object) genericArguments[0]);
    }

    private static bool CheckDictionaryAccessors(TypeModel model, Type pair, Type value)
    {
      return pair.IsGenericType && pair.GetGenericTypeDefinition() == model.MapType(typeof (KeyValuePair<,>)) && pair.GetGenericArguments()[1] == value;
    }

    private bool TryDeserializeList(
      TypeModel model,
      ProtoReader reader,
      DataFormat format,
      int tag,
      Type listType,
      Type itemType,
      ref object value)
    {
      bool isList;
      MethodInfo methodInfo = TypeModel.ResolveListAdd(model, listType, itemType, out isList);
      if (methodInfo == (MethodInfo) null)
        throw new NotSupportedException("Unknown list variant: " + listType.FullName);
      bool flag = false;
      object obj = (object) null;
      IList list = value as IList;
      object[] parameters = isList ? (object[]) null : new object[1];
      BasicList basicList = listType.IsArray ? new BasicList() : (BasicList) null;
      for (; this.TryDeserializeAuxiliaryType(reader, format, tag, itemType, ref obj, true, true, true, true); obj = (object) null)
      {
        flag = true;
        if (value == null && basicList == null)
        {
          value = TypeModel.CreateListInstance(listType, itemType);
          list = value as IList;
        }
        if (list != null)
          list.Add(obj);
        else if (basicList != null)
        {
          basicList.Add(obj);
        }
        else
        {
          parameters[0] = obj;
          methodInfo.Invoke(value, parameters);
        }
      }
      if (basicList != null)
      {
        if (value != null)
        {
          if (basicList.Count != 0)
          {
            Array sourceArray = (Array) value;
            Array instance = Array.CreateInstance(itemType, sourceArray.Length + basicList.Count);
            Array.Copy(sourceArray, instance, sourceArray.Length);
            basicList.CopyTo(instance, sourceArray.Length);
            value = (object) instance;
          }
        }
        else
        {
          Array instance = Array.CreateInstance(itemType, basicList.Count);
          basicList.CopyTo(instance, 0);
          value = (object) instance;
        }
      }
      return flag;
    }

    private static object CreateListInstance(Type listType, Type itemType)
    {
      Type type = listType;
      if (listType.IsArray)
        return (object) Array.CreateInstance(itemType, 0);
      if (!listType.IsClass || listType.IsAbstract || Helpers.GetConstructor(listType, Helpers.EmptyTypes, true) == (ConstructorInfo) null)
      {
        bool flag = false;
        string fullName;
        if (listType.IsInterface && (fullName = listType.FullName) != null && fullName.IndexOf("Dictionary") >= 0)
        {
          if (listType.IsGenericType && listType.GetGenericTypeDefinition() == typeof (IDictionary<,>))
          {
            type = typeof (Dictionary<,>).MakeGenericType(listType.GetGenericArguments());
            flag = true;
          }
          if (!flag && listType == typeof (IDictionary))
          {
            type = typeof (Hashtable);
            flag = true;
          }
        }
        if (!flag)
        {
          type = typeof (List<>).MakeGenericType(itemType);
          flag = true;
        }
        if (!flag)
          type = typeof (ArrayList);
      }
      return Activator.CreateInstance(type);
    }

    internal bool TryDeserializeAuxiliaryType(
      ProtoReader reader,
      DataFormat format,
      int tag,
      Type type,
      ref object value,
      bool skipOtherFields,
      bool asListItem,
      bool autoCreate,
      bool insideList)
    {
      ProtoTypeCode code = !(type == (Type) null) ? Helpers.GetTypeCode(type) : throw new ArgumentNullException(nameof (type));
      int modelKey;
      WireType wireType = this.GetWireType(code, format, ref type, out modelKey);
      bool flag1 = false;
      if (wireType == WireType.None)
      {
        Type itemType = TypeModel.GetListItemType(this, type);
        if (itemType == (Type) null && type.IsArray && type.GetArrayRank() == 1 && type != typeof (byte[]))
          itemType = type.GetElementType();
        if (itemType != (Type) null)
        {
          if (insideList)
            throw TypeModel.CreateNestedListsNotSupported();
          bool flag2 = this.TryDeserializeList(this, reader, format, tag, type, itemType, ref value);
          if (!flag2 & autoCreate)
            value = TypeModel.CreateListInstance(type, itemType);
          return flag2;
        }
        TypeModel.ThrowUnexpectedType(type);
      }
      while (!(flag1 & asListItem))
      {
        int num = reader.ReadFieldHeader();
        if (num > 0)
        {
          if (num != tag)
          {
            if (!skipOtherFields)
              throw ProtoReader.AddErrorData((Exception) new InvalidOperationException("Expected field " + tag.ToString() + ", but found " + num.ToString()), reader);
            reader.SkipField();
          }
          else
          {
            flag1 = true;
            reader.Hint(wireType);
            if (modelKey >= 0)
            {
              if (wireType == WireType.String || wireType == WireType.StartGroup)
              {
                SubItemToken token = ProtoReader.StartSubItem(reader);
                value = this.Deserialize(modelKey, value, reader);
                ProtoReader reader1 = reader;
                ProtoReader.EndSubItem(token, reader1);
              }
              else
                value = this.Deserialize(modelKey, value, reader);
            }
            else
            {
              switch (code)
              {
                case ProtoTypeCode.Boolean:
                  value = (object) reader.ReadBoolean();
                  continue;
                case ProtoTypeCode.Char:
                  value = (object) (char) reader.ReadUInt16();
                  continue;
                case ProtoTypeCode.SByte:
                  value = (object) reader.ReadSByte();
                  continue;
                case ProtoTypeCode.Byte:
                  value = (object) reader.ReadByte();
                  continue;
                case ProtoTypeCode.Int16:
                  value = (object) reader.ReadInt16();
                  continue;
                case ProtoTypeCode.UInt16:
                  value = (object) reader.ReadUInt16();
                  continue;
                case ProtoTypeCode.Int32:
                  value = (object) reader.ReadInt32();
                  continue;
                case ProtoTypeCode.UInt32:
                  value = (object) reader.ReadUInt32();
                  continue;
                case ProtoTypeCode.Int64:
                  value = (object) reader.ReadInt64();
                  continue;
                case ProtoTypeCode.UInt64:
                  value = (object) reader.ReadUInt64();
                  continue;
                case ProtoTypeCode.Single:
                  value = (object) reader.ReadSingle();
                  continue;
                case ProtoTypeCode.Double:
                  value = (object) reader.ReadDouble();
                  continue;
                case ProtoTypeCode.Decimal:
                  value = (object) BclHelpers.ReadDecimal(reader);
                  continue;
                case ProtoTypeCode.DateTime:
                  value = (object) BclHelpers.ReadDateTime(reader);
                  continue;
                case ProtoTypeCode.String:
                  value = (object) reader.ReadString();
                  continue;
                case ProtoTypeCode.TimeSpan:
                  value = (object) BclHelpers.ReadTimeSpan(reader);
                  continue;
                case ProtoTypeCode.ByteArray:
                  value = (object) ProtoReader.AppendBytes((byte[]) value, reader);
                  continue;
                case ProtoTypeCode.Guid:
                  value = (object) BclHelpers.ReadGuid(reader);
                  continue;
                case ProtoTypeCode.Uri:
                  value = (object) new Uri(reader.ReadString());
                  continue;
                default:
                  continue;
              }
            }
          }
        }
        else
          break;
      }
      if (((flag1 ? 0 : (!asListItem ? 1 : 0)) & (autoCreate ? 1 : 0)) != 0 && type != typeof (string))
        value = Activator.CreateInstance(type);
      return flag1;
    }

    public static RuntimeTypeModel Create() => new RuntimeTypeModel(false);

    protected internal static Type ResolveProxies(Type type)
    {
      if (type == (Type) null)
        return (Type) null;
      if (type.IsGenericParameter)
        return (Type) null;
      Type underlyingType = Helpers.GetUnderlyingType(type);
      if (underlyingType != (Type) null)
        return underlyingType;
      string fullName1 = type.FullName;
      if (fullName1 != null && fullName1.StartsWith("System.Data.Entity.DynamicProxies."))
        return type.BaseType;
      foreach (Type type1 in type.GetInterfaces())
      {
        string fullName2 = type1.FullName;
        if (fullName2 == "NHibernate.Proxy.INHibernateProxy" || fullName2 == "NHibernate.Proxy.DynamicProxy.IProxy" || fullName2 == "NHibernate.Intercept.IFieldInterceptorAccessor")
          return type.BaseType;
      }
      return (Type) null;
    }

    public bool IsDefined(Type type) => this.GetKey(ref type) >= 0;

    protected internal int GetKey(ref Type type)
    {
      if (type == (Type) null)
        return -1;
      int keyImpl = this.GetKeyImpl(type);
      if (keyImpl < 0)
      {
        Type type1 = TypeModel.ResolveProxies(type);
        if (type1 != (Type) null)
        {
          type = type1;
          keyImpl = this.GetKeyImpl(type);
        }
      }
      return keyImpl;
    }

    protected abstract int GetKeyImpl(Type type);

    protected internal abstract void Serialize(int key, object value, ProtoWriter dest);

    protected internal abstract object Deserialize(int key, object value, ProtoReader source);

    public object DeepClone(object value)
    {
      if (value == null)
        return (object) null;
      Type type = value.GetType();
      int key = this.GetKey(ref type);
      if (key >= 0 && !Helpers.IsEnum(type))
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (ProtoWriter dest = new ProtoWriter((Stream) memoryStream, this, (SerializationContext) null))
          {
            dest.SetRootObject(value);
            this.Serialize(key, value, dest);
            dest.Close();
          }
          memoryStream.Position = 0L;
          ProtoReader protoReader = (ProtoReader) null;
          try
          {
            protoReader = ProtoReader.Create((Stream) memoryStream, this, (SerializationContext) null, -1);
            return this.Deserialize(key, (object) null, protoReader);
          }
          finally
          {
            ProtoReader.Recycle(protoReader);
          }
        }
      }
      else
      {
        if (type == typeof (byte[]))
        {
          byte[] from = (byte[]) value;
          byte[] to = new byte[from.Length];
          Helpers.BlockCopy(from, 0, to, 0, from.Length);
          return (object) to;
        }
        int modelKey;
        if (this.GetWireType(Helpers.GetTypeCode(type), DataFormat.Default, ref type, out modelKey) != WireType.None && modelKey < 0)
          return value;
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (ProtoWriter writer = new ProtoWriter((Stream) memoryStream, this, (SerializationContext) null))
          {
            if (!this.TrySerializeAuxiliaryType(writer, type, DataFormat.Default, 1, value, false))
              TypeModel.ThrowUnexpectedType(type);
            writer.Close();
          }
          memoryStream.Position = 0L;
          ProtoReader reader = (ProtoReader) null;
          try
          {
            reader = ProtoReader.Create((Stream) memoryStream, this, (SerializationContext) null, -1);
            value = (object) null;
            this.TryDeserializeAuxiliaryType(reader, DataFormat.Default, 1, type, ref value, true, false, true, false);
            return value;
          }
          finally
          {
            ProtoReader.Recycle(reader);
          }
        }
      }
    }

    protected internal static void ThrowUnexpectedSubtype(Type expected, Type actual)
    {
      if (expected != TypeModel.ResolveProxies(actual))
        throw new InvalidOperationException("Unexpected sub-type: " + actual.FullName);
    }

    protected internal static void ThrowUnexpectedType(Type type)
    {
      string str = type == (Type) null ? "(unknown)" : type.FullName;
      Type type1 = type != (Type) null ? type.BaseType : throw new InvalidOperationException("Type is not expected, and no contract can be inferred: " + str);
      if (type1 != (Type) null && type1.IsGenericType && type1.GetGenericTypeDefinition().Name == "GeneratedMessage`2")
        throw new InvalidOperationException("Are you mixing protobuf-net and protobuf-csharp-port? See http://stackoverflow.com/q/11564914; type: " + str);
    }

    internal static Exception CreateNestedListsNotSupported()
    {
      return (Exception) new NotSupportedException("Nested or jagged lists and arrays are not supported");
    }

    public static void ThrowCannotCreateInstance(Type type)
    {
      throw new ProtoException("No parameterless constructor found for " + (type == (Type) null ? "(null)" : type.Name));
    }

    internal static string SerializeType(TypeModel model, Type type)
    {
      if (model != null)
      {
        TypeFormatEventHandler dynamicTypeFormatting = model.DynamicTypeFormatting;
        if (dynamicTypeFormatting != null)
        {
          TypeFormatEventArgs args = new TypeFormatEventArgs(type);
          dynamicTypeFormatting((object) model, args);
          if (!Helpers.IsNullOrEmpty(args.FormattedName))
            return args.FormattedName;
        }
      }
      return type.AssemblyQualifiedName;
    }

    internal static Type DeserializeType(TypeModel model, string value)
    {
      if (model != null)
      {
        TypeFormatEventHandler dynamicTypeFormatting = model.DynamicTypeFormatting;
        if (dynamicTypeFormatting != null)
        {
          TypeFormatEventArgs args = new TypeFormatEventArgs(value);
          dynamicTypeFormatting((object) model, args);
          if (args.Type != (Type) null)
            return args.Type;
        }
      }
      return Type.GetType(value);
    }

    public bool CanSerializeContractType(Type type) => this.CanSerialize(type, false, true, true);

    public bool CanSerialize(Type type) => this.CanSerialize(type, true, true, true);

    public bool CanSerializeBasicType(Type type) => this.CanSerialize(type, true, false, true);

    private bool CanSerialize(Type type, bool allowBasic, bool allowContract, bool allowLists)
    {
      Type type1 = !(type == (Type) null) ? Helpers.GetUnderlyingType(type) : throw new ArgumentNullException(nameof (type));
      if (type1 != (Type) null)
        type = type1;
      switch (Helpers.GetTypeCode(type))
      {
        case ProtoTypeCode.Empty:
        case ProtoTypeCode.Unknown:
          if (this.GetKey(ref type) >= 0)
            return allowContract;
          if (allowLists)
          {
            Type type2 = (Type) null;
            if (type.IsArray)
            {
              if (type.GetArrayRank() == 1)
                type2 = type.GetElementType();
            }
            else
              type2 = TypeModel.GetListItemType(this, type);
            if (type2 != (Type) null)
              return this.CanSerialize(type2, allowBasic, allowContract, false);
          }
          return false;
        default:
          return allowBasic;
      }
    }

    public virtual string GetSchema(Type type) => throw new NotSupportedException();

    public event TypeFormatEventHandler DynamicTypeFormatting;

    public IFormatter CreateFormatter(Type type)
    {
      return (IFormatter) new TypeModel.Formatter(this, type);
    }

    internal virtual Type GetType(string fullName, Assembly context)
    {
      return TypeModel.ResolveKnownType(fullName, this, context);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static Type ResolveKnownType(string name, TypeModel model, Assembly assembly)
    {
      if (Helpers.IsNullOrEmpty(name))
        return (Type) null;
      try
      {
        Type type = Type.GetType(name);
        if (type != (Type) null)
          return type;
      }
      catch
      {
      }
      try
      {
        int length = name.IndexOf(',');
        string name1 = (length > 0 ? name.Substring(0, length) : name).Trim();
        if (assembly == (Assembly) null)
          assembly = Assembly.GetCallingAssembly();
        Type type = assembly == (Assembly) null ? (Type) null : assembly.GetType(name1);
        if (type != (Type) null)
          return type;
      }
      catch
      {
      }
      return (Type) null;
    }

    private sealed class DeserializeItemsIterator<T>(
      TypeModel model,
      Stream source,
      PrefixStyle style,
      int expectedField,
      SerializationContext context) : 
      TypeModel.DeserializeItemsIterator(model, source, model.MapType(typeof (T)), style, expectedField, (Serializer.TypeResolver) null, context),
      IEnumerator<T>,
      IDisposable,
      IEnumerator,
      IEnumerable<T>,
      IEnumerable
    {
      IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this;

      public T Current => (T) base.Current;

      void IDisposable.Dispose()
      {
      }
    }

    private class DeserializeItemsIterator : IEnumerator, IEnumerable
    {
      private bool haveObject;
      private object current;
      private readonly Stream source;
      private readonly Type type;
      private readonly PrefixStyle style;
      private readonly int expectedField;
      private readonly Serializer.TypeResolver resolver;
      private readonly TypeModel model;
      private readonly SerializationContext context;

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this;

      public bool MoveNext()
      {
        if (this.haveObject)
          this.current = this.model.DeserializeWithLengthPrefix(this.source, (object) null, this.type, this.style, this.expectedField, this.resolver, out int _, out this.haveObject, this.context);
        return this.haveObject;
      }

      void IEnumerator.Reset() => throw new NotSupportedException();

      public object Current => this.current;

      public DeserializeItemsIterator(
        TypeModel model,
        Stream source,
        Type type,
        PrefixStyle style,
        int expectedField,
        Serializer.TypeResolver resolver,
        SerializationContext context)
      {
        this.haveObject = true;
        this.source = source;
        this.type = type;
        this.style = style;
        this.expectedField = expectedField;
        this.resolver = resolver;
        this.model = model;
        this.context = context;
      }
    }

    protected internal enum CallbackType
    {
      BeforeSerialize,
      AfterSerialize,
      BeforeDeserialize,
      AfterDeserialize,
    }

    internal sealed class Formatter : IFormatter
    {
      private readonly TypeModel model;
      private readonly Type type;
      private SerializationBinder binder;
      private StreamingContext context;
      private ISurrogateSelector surrogateSelector;

      internal Formatter(TypeModel model, Type type)
      {
        if (model == null)
          throw new ArgumentNullException(nameof (model));
        if (type == (Type) null)
          throw new ArgumentNullException(nameof (type));
        this.model = model;
        this.type = type;
      }

      public SerializationBinder Binder
      {
        get => this.binder;
        set => this.binder = value;
      }

      public StreamingContext Context
      {
        get => this.context;
        set => this.context = value;
      }

      public object Deserialize(Stream source)
      {
        return this.model.Deserialize(source, (object) null, this.type, -1, (SerializationContext) this.Context);
      }

      public void Serialize(Stream destination, object graph)
      {
        this.model.Serialize(destination, graph, (SerializationContext) this.Context);
      }

      public ISurrogateSelector SurrogateSelector
      {
        get => this.surrogateSelector;
        set => this.surrogateSelector = value;
      }
    }
  }
}
