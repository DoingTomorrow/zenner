﻿// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoReader
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace ProtoBuf
{
  public sealed class ProtoReader : IDisposable
  {
    private Stream source;
    private byte[] ioBuffer;
    private TypeModel model;
    private int fieldNumber;
    private int depth;
    private int dataRemaining;
    private int ioIndex;
    private int position;
    private int available;
    private int blockEnd;
    private WireType wireType;
    private bool isFixedLength;
    private bool internStrings;
    private NetObjectCache netCache;
    private uint trapCount;
    internal const int TO_EOF = -1;
    private SerializationContext context;
    private const long Int64Msb = -9223372036854775808;
    private const int Int32Msb = -2147483648;
    private Dictionary<string, string> stringInterner;
    private static readonly UTF8Encoding encoding = new UTF8Encoding();
    private static readonly byte[] EmptyBlob = new byte[0];
    [ThreadStatic]
    private static ProtoReader lastReader;

    public int FieldNumber => this.fieldNumber;

    public WireType WireType => this.wireType;

    public ProtoReader(Stream source, TypeModel model, SerializationContext context)
    {
      ProtoReader.Init(this, source, model, context, -1);
    }

    public bool InternStrings
    {
      get => this.internStrings;
      set => this.internStrings = value;
    }

    public ProtoReader(Stream source, TypeModel model, SerializationContext context, int length)
    {
      ProtoReader.Init(this, source, model, context, length);
    }

    private static void Init(
      ProtoReader reader,
      Stream source,
      TypeModel model,
      SerializationContext context,
      int length)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      reader.source = source.CanRead ? source : throw new ArgumentException("Cannot read from stream", nameof (source));
      reader.ioBuffer = BufferPool.GetBuffer();
      reader.model = model;
      bool flag = length >= 0;
      reader.isFixedLength = flag;
      reader.dataRemaining = flag ? length : 0;
      if (context == null)
        context = SerializationContext.Default;
      else
        context.Freeze();
      reader.context = context;
      reader.position = reader.available = reader.depth = reader.fieldNumber = reader.ioIndex = 0;
      reader.blockEnd = int.MaxValue;
      reader.internStrings = true;
      reader.wireType = WireType.None;
      reader.trapCount = 1U;
      if (reader.netCache != null)
        return;
      reader.netCache = new NetObjectCache();
    }

    public SerializationContext Context => this.context;

    public void Dispose()
    {
      this.source = (Stream) null;
      this.model = (TypeModel) null;
      BufferPool.ReleaseBufferToPool(ref this.ioBuffer);
      if (this.stringInterner != null)
        this.stringInterner.Clear();
      if (this.netCache == null)
        return;
      this.netCache.Clear();
    }

    internal int TryReadUInt32VariantWithoutMoving(bool trimNegative, out uint value)
    {
      if (this.available < 10)
        this.Ensure(10, false);
      if (this.available == 0)
      {
        value = 0U;
        return 0;
      }
      int ioIndex = this.ioIndex;
      ref uint local = ref value;
      byte[] ioBuffer1 = this.ioBuffer;
      int index1 = ioIndex;
      int num1 = index1 + 1;
      int num2 = (int) ioBuffer1[index1];
      local = (uint) num2;
      if (((int) value & 128) == 0)
        return 1;
      value &= (uint) sbyte.MaxValue;
      if (this.available == 1)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer2 = this.ioBuffer;
      int index2 = num1;
      int num3 = index2 + 1;
      uint num4 = (uint) ioBuffer2[index2];
      value |= (uint) (((int) num4 & (int) sbyte.MaxValue) << 7);
      if (((int) num4 & 128) == 0)
        return 2;
      if (this.available == 2)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer3 = this.ioBuffer;
      int index3 = num3;
      int num5 = index3 + 1;
      uint num6 = (uint) ioBuffer3[index3];
      value |= (uint) (((int) num6 & (int) sbyte.MaxValue) << 14);
      if (((int) num6 & 128) == 0)
        return 3;
      if (this.available == 3)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer4 = this.ioBuffer;
      int index4 = num5;
      int index5 = index4 + 1;
      uint num7 = (uint) ioBuffer4[index4];
      value |= (uint) (((int) num7 & (int) sbyte.MaxValue) << 21);
      if (((int) num7 & 128) == 0)
        return 4;
      if (this.available == 4)
        throw ProtoReader.EoF(this);
      uint num8 = (uint) this.ioBuffer[index5];
      value |= num8 << 28;
      if (((int) num8 & 240) == 0)
        return 5;
      int num9;
      int num10;
      int num11;
      int num12;
      int num13;
      if (trimNegative && ((int) num8 & 240) == 240 && this.available >= 10 && this.ioBuffer[num9 = index5 + 1] == byte.MaxValue && this.ioBuffer[num10 = num9 + 1] == byte.MaxValue && this.ioBuffer[num11 = num10 + 1] == byte.MaxValue && this.ioBuffer[num12 = num11 + 1] == byte.MaxValue && this.ioBuffer[num13 = num12 + 1] == (byte) 1)
        return 10;
      throw ProtoReader.AddErrorData((Exception) new OverflowException(), this);
    }

    private uint ReadUInt32Variant(bool trimNegative)
    {
      uint num1;
      int num2 = this.TryReadUInt32VariantWithoutMoving(trimNegative, out num1);
      if (num2 <= 0)
        throw ProtoReader.EoF(this);
      this.ioIndex += num2;
      this.available -= num2;
      this.position += num2;
      return num1;
    }

    private bool TryReadUInt32Variant(out uint value)
    {
      int num = this.TryReadUInt32VariantWithoutMoving(false, out value);
      if (num <= 0)
        return false;
      this.ioIndex += num;
      this.available -= num;
      this.position += num;
      return true;
    }

    public uint ReadUInt32()
    {
      switch (this.wireType)
      {
        case WireType.Variant:
          return this.ReadUInt32Variant(false);
        case WireType.Fixed64:
          return checked ((uint) this.ReadUInt64());
        case WireType.Fixed32:
          if (this.available < 4)
            this.Ensure(4, true);
          this.position += 4;
          this.available -= 4;
          return (uint) ((int) this.ioBuffer[this.ioIndex++] | (int) this.ioBuffer[this.ioIndex++] << 8 | (int) this.ioBuffer[this.ioIndex++] << 16 | (int) this.ioBuffer[this.ioIndex++] << 24);
        default:
          throw this.CreateWireTypeException();
      }
    }

    public int Position => this.position;

    internal void Ensure(int count, bool strict)
    {
      if (count > this.ioBuffer.Length)
      {
        BufferPool.ResizeAndFlushLeft(ref this.ioBuffer, count, this.ioIndex, this.available);
        this.ioIndex = 0;
      }
      else if (this.ioIndex + count >= this.ioBuffer.Length)
      {
        Helpers.BlockCopy(this.ioBuffer, this.ioIndex, this.ioBuffer, 0, this.available);
        this.ioIndex = 0;
      }
      count -= this.available;
      int offset = this.ioIndex + this.available;
      int count1 = this.ioBuffer.Length - offset;
      if (this.isFixedLength && this.dataRemaining < count1)
        count1 = this.dataRemaining;
      int num;
      while (count > 0 && count1 > 0 && (num = this.source.Read(this.ioBuffer, offset, count1)) > 0)
      {
        this.available += num;
        count -= num;
        count1 -= num;
        offset += num;
        if (this.isFixedLength)
          this.dataRemaining -= num;
      }
      if (strict && count > 0)
        throw ProtoReader.EoF(this);
    }

    public short ReadInt16() => checked ((short) this.ReadInt32());

    public ushort ReadUInt16() => checked ((ushort) this.ReadUInt32());

    public byte ReadByte() => checked ((byte) this.ReadUInt32());

    public sbyte ReadSByte() => checked ((sbyte) this.ReadInt32());

    public int ReadInt32()
    {
      switch (this.wireType)
      {
        case WireType.Variant:
          return (int) this.ReadUInt32Variant(true);
        case WireType.Fixed64:
          return checked ((int) this.ReadInt64());
        case WireType.Fixed32:
          if (this.available < 4)
            this.Ensure(4, true);
          this.position += 4;
          this.available -= 4;
          return (int) this.ioBuffer[this.ioIndex++] | (int) this.ioBuffer[this.ioIndex++] << 8 | (int) this.ioBuffer[this.ioIndex++] << 16 | (int) this.ioBuffer[this.ioIndex++] << 24;
        case WireType.SignedVariant:
          return ProtoReader.Zag(this.ReadUInt32Variant(true));
        default:
          throw this.CreateWireTypeException();
      }
    }

    private static int Zag(uint ziggedValue)
    {
      int num = (int) ziggedValue;
      return -(num & 1) ^ num >> 1 & int.MaxValue;
    }

    private static long Zag(ulong ziggedValue)
    {
      long num = (long) ziggedValue;
      return -(num & 1L) ^ num >> 1 & long.MaxValue;
    }

    public long ReadInt64()
    {
      switch (this.wireType)
      {
        case WireType.Variant:
          return (long) this.ReadUInt64Variant();
        case WireType.Fixed64:
          if (this.available < 8)
            this.Ensure(8, true);
          this.position += 8;
          this.available -= 8;
          return (long) this.ioBuffer[this.ioIndex++] | (long) this.ioBuffer[this.ioIndex++] << 8 | (long) this.ioBuffer[this.ioIndex++] << 16 | (long) this.ioBuffer[this.ioIndex++] << 24 | (long) this.ioBuffer[this.ioIndex++] << 32 | (long) this.ioBuffer[this.ioIndex++] << 40 | (long) this.ioBuffer[this.ioIndex++] << 48 | (long) this.ioBuffer[this.ioIndex++] << 56;
        case WireType.Fixed32:
          return (long) this.ReadInt32();
        case WireType.SignedVariant:
          return ProtoReader.Zag(this.ReadUInt64Variant());
        default:
          throw this.CreateWireTypeException();
      }
    }

    private int TryReadUInt64VariantWithoutMoving(out ulong value)
    {
      if (this.available < 10)
        this.Ensure(10, false);
      if (this.available == 0)
      {
        value = 0UL;
        return 0;
      }
      int ioIndex = this.ioIndex;
      ref ulong local = ref value;
      byte[] ioBuffer1 = this.ioBuffer;
      int index1 = ioIndex;
      int num1 = index1 + 1;
      long num2 = (long) ioBuffer1[index1];
      local = (ulong) num2;
      if (((long) value & 128L) == 0L)
        return 1;
      value &= (ulong) sbyte.MaxValue;
      if (this.available == 1)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer2 = this.ioBuffer;
      int index2 = num1;
      int num3 = index2 + 1;
      ulong num4 = (ulong) ioBuffer2[index2];
      value |= (ulong) (((long) num4 & (long) sbyte.MaxValue) << 7);
      if (((long) num4 & 128L) == 0L)
        return 2;
      if (this.available == 2)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer3 = this.ioBuffer;
      int index3 = num3;
      int num5 = index3 + 1;
      ulong num6 = (ulong) ioBuffer3[index3];
      value |= (ulong) (((long) num6 & (long) sbyte.MaxValue) << 14);
      if (((long) num6 & 128L) == 0L)
        return 3;
      if (this.available == 3)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer4 = this.ioBuffer;
      int index4 = num5;
      int num7 = index4 + 1;
      ulong num8 = (ulong) ioBuffer4[index4];
      value |= (ulong) (((long) num8 & (long) sbyte.MaxValue) << 21);
      if (((long) num8 & 128L) == 0L)
        return 4;
      if (this.available == 4)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer5 = this.ioBuffer;
      int index5 = num7;
      int num9 = index5 + 1;
      ulong num10 = (ulong) ioBuffer5[index5];
      value |= (ulong) (((long) num10 & (long) sbyte.MaxValue) << 28);
      if (((long) num10 & 128L) == 0L)
        return 5;
      if (this.available == 5)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer6 = this.ioBuffer;
      int index6 = num9;
      int num11 = index6 + 1;
      ulong num12 = (ulong) ioBuffer6[index6];
      value |= (ulong) (((long) num12 & (long) sbyte.MaxValue) << 35);
      if (((long) num12 & 128L) == 0L)
        return 6;
      if (this.available == 6)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer7 = this.ioBuffer;
      int index7 = num11;
      int num13 = index7 + 1;
      ulong num14 = (ulong) ioBuffer7[index7];
      value |= (ulong) (((long) num14 & (long) sbyte.MaxValue) << 42);
      if (((long) num14 & 128L) == 0L)
        return 7;
      if (this.available == 7)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer8 = this.ioBuffer;
      int index8 = num13;
      int num15 = index8 + 1;
      ulong num16 = (ulong) ioBuffer8[index8];
      value |= (ulong) (((long) num16 & (long) sbyte.MaxValue) << 49);
      if (((long) num16 & 128L) == 0L)
        return 8;
      if (this.available == 8)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer9 = this.ioBuffer;
      int index9 = num15;
      int index10 = index9 + 1;
      ulong num17 = (ulong) ioBuffer9[index9];
      value |= (ulong) (((long) num17 & (long) sbyte.MaxValue) << 56);
      if (((long) num17 & 128L) == 0L)
        return 9;
      if (this.available == 9)
        throw ProtoReader.EoF(this);
      ulong num18 = (ulong) this.ioBuffer[index10];
      value |= num18 << 63;
      if (((long) num18 & -2L) != 0L)
        throw ProtoReader.AddErrorData((Exception) new OverflowException(), this);
      return 10;
    }

    private ulong ReadUInt64Variant()
    {
      ulong num1;
      int num2 = this.TryReadUInt64VariantWithoutMoving(out num1);
      if (num2 <= 0)
        throw ProtoReader.EoF(this);
      this.ioIndex += num2;
      this.available -= num2;
      this.position += num2;
      return num1;
    }

    private string Intern(string value)
    {
      switch (value)
      {
        case null:
          return (string) null;
        case "":
          return "";
        default:
          if (this.stringInterner == null)
          {
            this.stringInterner = new Dictionary<string, string>();
            this.stringInterner.Add(value, value);
          }
          else
          {
            string str;
            if (this.stringInterner.TryGetValue(value, out str))
              value = str;
            else
              this.stringInterner.Add(value, value);
          }
          return value;
      }
    }

    public string ReadString()
    {
      if (this.wireType != WireType.String)
        throw this.CreateWireTypeException();
      int count = (int) this.ReadUInt32Variant(false);
      if (count == 0)
        return "";
      if (this.available < count)
        this.Ensure(count, true);
      string str = ProtoReader.encoding.GetString(this.ioBuffer, this.ioIndex, count);
      if (this.internStrings)
        str = this.Intern(str);
      this.available -= count;
      this.position += count;
      this.ioIndex += count;
      return str;
    }

    public void ThrowEnumException(Type type, int value)
    {
      throw ProtoReader.AddErrorData((Exception) new ProtoException("No " + (type == (Type) null ? "<null>" : type.FullName) + " enum is mapped to the wire-value " + value.ToString()), this);
    }

    private Exception CreateWireTypeException()
    {
      return this.CreateException("Invalid wire-type; this usually means you have over-written a file without truncating or setting the length; see http://stackoverflow.com/q/2152978/23354");
    }

    private Exception CreateException(string message)
    {
      return ProtoReader.AddErrorData((Exception) new ProtoException(message), this);
    }

    public unsafe double ReadDouble()
    {
      switch (this.wireType)
      {
        case WireType.Fixed64:
          return *(double*) &this.ReadInt64();
        case WireType.Fixed32:
          return (double) this.ReadSingle();
        default:
          throw this.CreateWireTypeException();
      }
    }

    public static object ReadObject(object value, int key, ProtoReader reader)
    {
      return ProtoReader.ReadTypedObject(value, key, reader, (Type) null);
    }

    internal static object ReadTypedObject(object value, int key, ProtoReader reader, Type type)
    {
      SubItemToken token = reader.model != null ? ProtoReader.StartSubItem(reader) : throw ProtoReader.AddErrorData((Exception) new InvalidOperationException("Cannot deserialize sub-objects unless a model is provided"), reader);
      if (key >= 0)
        value = reader.model.Deserialize(key, value, reader);
      else if (!(type != (Type) null) || !reader.model.TryDeserializeAuxiliaryType(reader, DataFormat.Default, 1, type, ref value, true, false, true, false))
        TypeModel.ThrowUnexpectedType(type);
      ProtoReader reader1 = reader;
      ProtoReader.EndSubItem(token, reader1);
      return value;
    }

    public static void EndSubItem(SubItemToken token, ProtoReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      int num = token.value;
      if (reader.wireType == WireType.EndGroup)
      {
        if (num >= 0)
          throw ProtoReader.AddErrorData((Exception) new ArgumentException(nameof (token)), reader);
        if (-num != reader.fieldNumber)
          throw reader.CreateException("Wrong group was ended");
        reader.wireType = WireType.None;
        --reader.depth;
      }
      else
      {
        if (num < reader.position)
          throw reader.CreateException("Sub-message not read entirely");
        if (reader.blockEnd != reader.position && reader.blockEnd != int.MaxValue)
          throw reader.CreateException("Sub-message not read correctly");
        reader.blockEnd = num;
        --reader.depth;
      }
    }

    public static SubItemToken StartSubItem(ProtoReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      switch (reader.wireType)
      {
        case WireType.String:
          int num = (int) reader.ReadUInt32Variant(false);
          if (num < 0)
            throw ProtoReader.AddErrorData((Exception) new InvalidOperationException(), reader);
          int blockEnd = reader.blockEnd;
          reader.blockEnd = reader.position + num;
          ++reader.depth;
          return new SubItemToken(blockEnd);
        case WireType.StartGroup:
          reader.wireType = WireType.None;
          ++reader.depth;
          return new SubItemToken(-reader.fieldNumber);
        default:
          throw reader.CreateWireTypeException();
      }
    }

    public int ReadFieldHeader()
    {
      if (this.blockEnd <= this.position || this.wireType == WireType.EndGroup)
        return 0;
      uint num;
      if (this.TryReadUInt32Variant(out num) && num != 0U)
      {
        this.wireType = (WireType) ((int) num & 7);
        this.fieldNumber = (int) (num >> 3);
        if (this.fieldNumber < 1)
          throw new ProtoException("Invalid field in source data: " + this.fieldNumber.ToString());
      }
      else
      {
        this.wireType = WireType.None;
        this.fieldNumber = 0;
      }
      if (this.wireType != WireType.EndGroup)
        return this.fieldNumber;
      if (this.depth > 0)
        return 0;
      throw new ProtoException("Unexpected end-group in source data; this usually means the source data is corrupt");
    }

    public bool TryReadFieldHeader(int field)
    {
      if (this.blockEnd <= this.position || this.wireType == WireType.EndGroup)
        return false;
      uint num1;
      int num2 = this.TryReadUInt32VariantWithoutMoving(false, out num1);
      WireType wireType;
      if (num2 <= 0 || (int) num1 >> 3 != field || (wireType = (WireType) ((int) num1 & 7)) == WireType.EndGroup)
        return false;
      this.wireType = wireType;
      this.fieldNumber = field;
      this.position += num2;
      this.ioIndex += num2;
      this.available -= num2;
      return true;
    }

    public TypeModel Model => this.model;

    public void Hint(WireType wireType)
    {
      if (this.wireType == wireType || (wireType & (WireType.StartGroup | WireType.EndGroup)) != this.wireType)
        return;
      this.wireType = wireType;
    }

    public void Assert(WireType wireType)
    {
      if (this.wireType == wireType)
        return;
      this.wireType = (wireType & (WireType.StartGroup | WireType.EndGroup)) == this.wireType ? wireType : throw this.CreateWireTypeException();
    }

    public void SkipField()
    {
      switch (this.wireType)
      {
        case WireType.Variant:
        case WireType.SignedVariant:
          long num1 = (long) this.ReadUInt64Variant();
          break;
        case WireType.Fixed64:
          if (this.available < 8)
            this.Ensure(8, true);
          this.available -= 8;
          this.ioIndex += 8;
          this.position += 8;
          break;
        case WireType.String:
          int num2 = (int) this.ReadUInt32Variant(false);
          if (num2 <= this.available)
          {
            this.available -= num2;
            this.ioIndex += num2;
            this.position += num2;
            break;
          }
          this.position += num2;
          int count = num2 - this.available;
          this.ioIndex = this.available = 0;
          if (this.isFixedLength)
          {
            if (count > this.dataRemaining)
              throw ProtoReader.EoF(this);
            this.dataRemaining -= count;
          }
          ProtoReader.Seek(this.source, count, this.ioBuffer);
          break;
        case WireType.StartGroup:
          int fieldNumber = this.fieldNumber;
          ++this.depth;
          while (this.ReadFieldHeader() > 0)
            this.SkipField();
          --this.depth;
          if (this.wireType != WireType.EndGroup || this.fieldNumber != fieldNumber)
            throw this.CreateWireTypeException();
          this.wireType = WireType.None;
          break;
        case WireType.Fixed32:
          if (this.available < 4)
            this.Ensure(4, true);
          this.available -= 4;
          this.ioIndex += 4;
          this.position += 4;
          break;
        default:
          throw this.CreateWireTypeException();
      }
    }

    public ulong ReadUInt64()
    {
      switch (this.wireType)
      {
        case WireType.Variant:
          return this.ReadUInt64Variant();
        case WireType.Fixed64:
          if (this.available < 8)
            this.Ensure(8, true);
          this.position += 8;
          this.available -= 8;
          return (ulong) ((long) this.ioBuffer[this.ioIndex++] | (long) this.ioBuffer[this.ioIndex++] << 8 | (long) this.ioBuffer[this.ioIndex++] << 16 | (long) this.ioBuffer[this.ioIndex++] << 24 | (long) this.ioBuffer[this.ioIndex++] << 32 | (long) this.ioBuffer[this.ioIndex++] << 40 | (long) this.ioBuffer[this.ioIndex++] << 48 | (long) this.ioBuffer[this.ioIndex++] << 56);
        case WireType.Fixed32:
          return (ulong) this.ReadUInt32();
        default:
          throw this.CreateWireTypeException();
      }
    }

    public unsafe float ReadSingle()
    {
      switch (this.wireType)
      {
        case WireType.Fixed64:
          double num1 = this.ReadDouble();
          double num2 = num1;
          if (!Helpers.IsInfinity((float) num2))
            return (float) num2;
          if (Helpers.IsInfinity(num1))
            return (float) num2;
          throw ProtoReader.AddErrorData((Exception) new OverflowException(), this);
        case WireType.Fixed32:
          return *(float*) &this.ReadInt32();
        default:
          throw this.CreateWireTypeException();
      }
    }

    public bool ReadBoolean()
    {
      switch (this.ReadUInt32())
      {
        case 0:
          return false;
        case 1:
          return true;
        default:
          throw this.CreateException("Unexpected boolean value");
      }
    }

    public static byte[] AppendBytes(byte[] value, ProtoReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      switch (reader.wireType)
      {
        case WireType.Variant:
          return new byte[0];
        case WireType.String:
          int count1 = (int) reader.ReadUInt32Variant(false);
          reader.wireType = WireType.None;
          if (count1 == 0)
            return value != null ? value : ProtoReader.EmptyBlob;
          int toIndex;
          if (value == null || value.Length == 0)
          {
            toIndex = 0;
            value = new byte[count1];
          }
          else
          {
            toIndex = value.Length;
            byte[] to = new byte[value.Length + count1];
            Helpers.BlockCopy(value, 0, to, 0, value.Length);
            value = to;
          }
          reader.position += count1;
          while (count1 > reader.available)
          {
            if (reader.available > 0)
            {
              Helpers.BlockCopy(reader.ioBuffer, reader.ioIndex, value, toIndex, reader.available);
              count1 -= reader.available;
              toIndex += reader.available;
              reader.ioIndex = reader.available = 0;
            }
            int count2 = count1 > reader.ioBuffer.Length ? reader.ioBuffer.Length : count1;
            if (count2 > 0)
              reader.Ensure(count2, true);
          }
          if (count1 > 0)
          {
            Helpers.BlockCopy(reader.ioBuffer, reader.ioIndex, value, toIndex, count1);
            reader.ioIndex += count1;
            reader.available -= count1;
          }
          return value;
        default:
          throw reader.CreateWireTypeException();
      }
    }

    private static int ReadByteOrThrow(Stream source)
    {
      int num = source.ReadByte();
      return num >= 0 ? num : throw ProtoReader.EoF((ProtoReader) null);
    }

    public static int ReadLengthPrefix(
      Stream source,
      bool expectHeader,
      PrefixStyle style,
      out int fieldNumber)
    {
      return ProtoReader.ReadLengthPrefix(source, expectHeader, style, out fieldNumber, out int _);
    }

    public static int DirectReadLittleEndianInt32(Stream source)
    {
      return ProtoReader.ReadByteOrThrow(source) | ProtoReader.ReadByteOrThrow(source) << 8 | ProtoReader.ReadByteOrThrow(source) << 16 | ProtoReader.ReadByteOrThrow(source) << 24;
    }

    public static int DirectReadBigEndianInt32(Stream source)
    {
      return ProtoReader.ReadByteOrThrow(source) << 24 | ProtoReader.ReadByteOrThrow(source) << 16 | ProtoReader.ReadByteOrThrow(source) << 8 | ProtoReader.ReadByteOrThrow(source);
    }

    public static int DirectReadVarintInt32(Stream source)
    {
      uint num;
      if (ProtoReader.TryReadUInt32Variant(source, out num) <= 0)
        throw ProtoReader.EoF((ProtoReader) null);
      return (int) num;
    }

    public static void DirectReadBytes(Stream source, byte[] buffer, int offset, int count)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      int num;
      for (; count > 0 && (num = source.Read(buffer, offset, count)) > 0; offset += num)
        count -= num;
      if (count > 0)
        throw ProtoReader.EoF((ProtoReader) null);
    }

    public static byte[] DirectReadBytes(Stream source, int count)
    {
      byte[] buffer = new byte[count];
      ProtoReader.DirectReadBytes(source, buffer, 0, count);
      return buffer;
    }

    public static string DirectReadString(Stream source, int length)
    {
      byte[] numArray = new byte[length];
      ProtoReader.DirectReadBytes(source, numArray, 0, length);
      return Encoding.UTF8.GetString(numArray, 0, length);
    }

    public static int ReadLengthPrefix(
      Stream source,
      bool expectHeader,
      PrefixStyle style,
      out int fieldNumber,
      out int bytesRead)
    {
      fieldNumber = 0;
      switch (style)
      {
        case PrefixStyle.None:
          bytesRead = 0;
          return int.MaxValue;
        case PrefixStyle.Base128:
          bytesRead = 0;
          if (expectHeader)
          {
            uint num1;
            int num2 = ProtoReader.TryReadUInt32Variant(source, out num1);
            bytesRead += num2;
            if (num2 > 0)
            {
              if (((int) num1 & 7) != 2)
                throw new InvalidOperationException();
              fieldNumber = (int) (num1 >> 3);
              int num3 = ProtoReader.TryReadUInt32Variant(source, out num1);
              bytesRead += num3;
              if (bytesRead == 0)
                throw ProtoReader.EoF((ProtoReader) null);
              return (int) num1;
            }
            bytesRead = 0;
            return -1;
          }
          uint num4;
          int num5 = ProtoReader.TryReadUInt32Variant(source, out num4);
          bytesRead += num5;
          return bytesRead >= 0 ? (int) num4 : -1;
        case PrefixStyle.Fixed32:
          int num6 = source.ReadByte();
          if (num6 < 0)
          {
            bytesRead = 0;
            return -1;
          }
          bytesRead = 4;
          return num6 | ProtoReader.ReadByteOrThrow(source) << 8 | ProtoReader.ReadByteOrThrow(source) << 16 | ProtoReader.ReadByteOrThrow(source) << 24;
        case PrefixStyle.Fixed32BigEndian:
          int num7 = source.ReadByte();
          if (num7 < 0)
          {
            bytesRead = 0;
            return -1;
          }
          bytesRead = 4;
          return num7 << 24 | ProtoReader.ReadByteOrThrow(source) << 16 | ProtoReader.ReadByteOrThrow(source) << 8 | ProtoReader.ReadByteOrThrow(source);
        default:
          throw new ArgumentOutOfRangeException(nameof (style));
      }
    }

    private static int TryReadUInt32Variant(Stream source, out uint value)
    {
      value = 0U;
      int num1 = source.ReadByte();
      if (num1 < 0)
        return 0;
      value = (uint) num1;
      if (((int) value & 128) == 0)
        return 1;
      value &= (uint) sbyte.MaxValue;
      int num2 = source.ReadByte();
      if (num2 < 0)
        throw ProtoReader.EoF((ProtoReader) null);
      value |= (uint) ((num2 & (int) sbyte.MaxValue) << 7);
      if ((num2 & 128) == 0)
        return 2;
      int num3 = source.ReadByte();
      if (num3 < 0)
        throw ProtoReader.EoF((ProtoReader) null);
      value |= (uint) ((num3 & (int) sbyte.MaxValue) << 14);
      if ((num3 & 128) == 0)
        return 3;
      int num4 = source.ReadByte();
      if (num4 < 0)
        throw ProtoReader.EoF((ProtoReader) null);
      value |= (uint) ((num4 & (int) sbyte.MaxValue) << 21);
      if ((num4 & 128) == 0)
        return 4;
      int num5 = source.ReadByte();
      if (num5 < 0)
        throw ProtoReader.EoF((ProtoReader) null);
      value |= (uint) (num5 << 28);
      if ((num5 & 240) == 0)
        return 5;
      throw new OverflowException();
    }

    internal static void Seek(Stream source, int count, byte[] buffer)
    {
      if (source.CanSeek)
      {
        source.Seek((long) count, SeekOrigin.Current);
        count = 0;
      }
      else if (buffer != null)
      {
        int num1;
        while (count > buffer.Length && (num1 = source.Read(buffer, 0, buffer.Length)) > 0)
          count -= num1;
        int num2;
        while (count > 0 && (num2 = source.Read(buffer, 0, count)) > 0)
          count -= num2;
      }
      else
      {
        buffer = BufferPool.GetBuffer();
        try
        {
          int num3;
          while (count > buffer.Length && (num3 = source.Read(buffer, 0, buffer.Length)) > 0)
            count -= num3;
          int num4;
          for (; count > 0; count -= num4)
          {
            if ((num4 = source.Read(buffer, 0, count)) <= 0)
              break;
          }
        }
        finally
        {
          BufferPool.ReleaseBufferToPool(ref buffer);
        }
      }
      if (count > 0)
        throw ProtoReader.EoF((ProtoReader) null);
    }

    internal static Exception AddErrorData(Exception exception, ProtoReader source)
    {
      if (exception != null && source != null && !exception.Data.Contains((object) "protoSource"))
        exception.Data.Add((object) "protoSource", (object) string.Format("tag={0}; wire-type={1}; offset={2}; depth={3}", (object) source.fieldNumber, (object) source.wireType, (object) source.position, (object) source.depth));
      return exception;
    }

    private static Exception EoF(ProtoReader source)
    {
      return ProtoReader.AddErrorData((Exception) new EndOfStreamException(), source);
    }

    public void AppendExtensionData(IExtensible instance)
    {
      IExtension extension = instance != null ? instance.GetExtensionObject(true) : throw new ArgumentNullException(nameof (instance));
      bool commit = false;
      Stream stream = extension.BeginAppend();
      try
      {
        using (ProtoWriter writer = new ProtoWriter(stream, this.model, (SerializationContext) null))
        {
          this.AppendExtensionField(writer);
          writer.Close();
        }
        commit = true;
      }
      finally
      {
        extension.EndAppend(stream, commit);
      }
    }

    private void AppendExtensionField(ProtoWriter writer)
    {
      ProtoWriter.WriteFieldHeader(this.fieldNumber, this.wireType, writer);
      switch (this.wireType)
      {
        case WireType.Variant:
        case WireType.Fixed64:
        case WireType.SignedVariant:
          ProtoWriter.WriteInt64(this.ReadInt64(), writer);
          break;
        case WireType.String:
          ProtoWriter.WriteBytes(ProtoReader.AppendBytes((byte[]) null, this), writer);
          break;
        case WireType.StartGroup:
          SubItemToken token1 = ProtoReader.StartSubItem(this);
          SubItemToken token2 = ProtoWriter.StartSubItem((object) null, writer);
          while (this.ReadFieldHeader() > 0)
            this.AppendExtensionField(writer);
          ProtoReader.EndSubItem(token1, this);
          ProtoWriter.EndSubItem(token2, writer);
          break;
        case WireType.Fixed32:
          ProtoWriter.WriteInt32(this.ReadInt32(), writer);
          break;
        default:
          throw this.CreateWireTypeException();
      }
    }

    public static bool HasSubValue(WireType wireType, ProtoReader source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (source.blockEnd <= source.position || wireType == WireType.EndGroup)
        return false;
      source.wireType = wireType;
      return true;
    }

    internal int GetTypeKey(ref Type type) => this.model.GetKey(ref type);

    internal NetObjectCache NetCache => this.netCache;

    internal Type DeserializeType(string value) => TypeModel.DeserializeType(this.model, value);

    internal void SetRootObject(object value)
    {
      this.netCache.SetKeyedObject(0, value);
      --this.trapCount;
    }

    public static void NoteObject(object value, ProtoReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      if (reader.trapCount == 0U)
        return;
      reader.netCache.RegisterTrappedObject(value);
      --reader.trapCount;
    }

    public Type ReadType() => TypeModel.DeserializeType(this.model, this.ReadString());

    internal void TrapNextObject(int newObjectKey)
    {
      ++this.trapCount;
      this.netCache.SetKeyedObject(newObjectKey, (object) null);
    }

    internal void CheckFullyConsumed()
    {
      if (this.isFixedLength)
      {
        if (this.dataRemaining != 0)
          throw new ProtoException("Incorrect number of bytes consumed");
      }
      else if (this.available != 0)
        throw new ProtoException("Unconsumed data left in the buffer; this suggests corrupt input");
    }

    public static object Merge(ProtoReader parent, object from, object to)
    {
      TypeModel typeModel = parent != null ? parent.Model : throw new ArgumentNullException(nameof (parent));
      SerializationContext context = parent.Context;
      if (typeModel == null)
        throw new InvalidOperationException("Types cannot be merged unless a type-model has been specified");
      using (MemoryStream memoryStream = new MemoryStream())
      {
        typeModel.Serialize((Stream) memoryStream, from, context);
        memoryStream.Position = 0L;
        return typeModel.Deserialize((Stream) memoryStream, to, (Type) null);
      }
    }

    internal static ProtoReader Create(
      Stream source,
      TypeModel model,
      SerializationContext context,
      int len)
    {
      ProtoReader recycled = ProtoReader.GetRecycled();
      if (recycled == null)
        return new ProtoReader(source, model, context, len);
      ProtoReader.Init(recycled, source, model, context, len);
      return recycled;
    }

    private static ProtoReader GetRecycled()
    {
      ProtoReader lastReader = ProtoReader.lastReader;
      ProtoReader.lastReader = (ProtoReader) null;
      return lastReader;
    }

    internal static void Recycle(ProtoReader reader)
    {
      if (reader == null)
        return;
      reader.Dispose();
      ProtoReader.lastReader = reader;
    }
  }
}
