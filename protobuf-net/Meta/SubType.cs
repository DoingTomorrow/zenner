// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.SubType
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Serializers;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace ProtoBuf.Meta
{
  public sealed class SubType
  {
    private readonly int fieldNumber;
    private readonly MetaType derivedType;
    private readonly DataFormat dataFormat;
    private IProtoSerializer serializer;

    public int FieldNumber => this.fieldNumber;

    public MetaType DerivedType => this.derivedType;

    public SubType(int fieldNumber, MetaType derivedType, DataFormat format)
    {
      if (derivedType == null)
        throw new ArgumentNullException(nameof (derivedType));
      this.fieldNumber = fieldNumber > 0 ? fieldNumber : throw new ArgumentOutOfRangeException(nameof (fieldNumber));
      this.derivedType = derivedType;
      this.dataFormat = format;
    }

    internal IProtoSerializer Serializer
    {
      get
      {
        if (this.serializer == null)
          this.serializer = this.BuildSerializer();
        return this.serializer;
      }
    }

    private IProtoSerializer BuildSerializer()
    {
      WireType wireType = WireType.String;
      if (this.dataFormat == DataFormat.Group)
        wireType = WireType.StartGroup;
      IProtoSerializer tail = (IProtoSerializer) new SubItemSerializer(this.derivedType.Type, this.derivedType.GetKey(false, false), (ISerializerProxy) this.derivedType, false);
      return (IProtoSerializer) new TagDecorator(this.fieldNumber, wireType, false, tail);
    }

    internal sealed class Comparer : IComparer, IComparer<SubType>
    {
      public static readonly SubType.Comparer Default = new SubType.Comparer();

      public int Compare(object x, object y) => this.Compare(x as SubType, y as SubType);

      public int Compare(SubType x, SubType y)
      {
        if (x == y)
          return 0;
        if (x == null)
          return -1;
        return y == null ? 1 : x.FieldNumber.CompareTo(y.FieldNumber);
      }
    }
  }
}
