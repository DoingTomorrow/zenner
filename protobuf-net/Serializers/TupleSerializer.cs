// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.TupleSerializer
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;
using System.Reflection;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class TupleSerializer : IProtoTypeSerializer, IProtoSerializer
  {
    private readonly MemberInfo[] members;
    private readonly ConstructorInfo ctor;
    private IProtoSerializer[] tails;

    public TupleSerializer(RuntimeTypeModel model, ConstructorInfo ctor, MemberInfo[] members)
    {
      if (ctor == (ConstructorInfo) null)
        throw new ArgumentNullException(nameof (ctor));
      if (members == null)
        throw new ArgumentNullException(nameof (members));
      this.ctor = ctor;
      this.members = members;
      this.tails = new IProtoSerializer[members.Length];
      ParameterInfo[] parameters = ctor.GetParameters();
      for (int index = 0; index < members.Length; ++index)
      {
        Type parameterType = parameters[index].ParameterType;
        Type itemType = (Type) null;
        Type defaultType = (Type) null;
        MetaType.ResolveListTypes((TypeModel) model, parameterType, ref itemType, ref defaultType);
        Type type = itemType == (Type) null ? parameterType : itemType;
        bool asReference = false;
        if (model.FindOrAddAuto(type, false, true, false) >= 0)
          asReference = model[type].AsReferenceDefault;
        WireType defaultWireType;
        IProtoSerializer tail = (IProtoSerializer) new TagDecorator(index + 1, defaultWireType, false, ValueMember.TryGetCoreSerializer(model, DataFormat.Default, type, out defaultWireType, asReference, false, false, true) ?? throw new InvalidOperationException("No serializer defined for type: " + type.FullName));
        IProtoSerializer protoSerializer = !(itemType == (Type) null) ? (!parameterType.IsArray ? (IProtoSerializer) ListDecorator.Create((TypeModel) model, parameterType, defaultType, tail, index + 1, false, defaultWireType, true, false, false) : (IProtoSerializer) new ArrayDecorator((TypeModel) model, tail, index + 1, false, defaultWireType, parameterType, false, false)) : tail;
        this.tails[index] = protoSerializer;
      }
    }

    public bool HasCallbacks(TypeModel.CallbackType callbackType) => false;

    public void EmitCallback(
      CompilerContext ctx,
      Local valueFrom,
      TypeModel.CallbackType callbackType)
    {
    }

    public Type ExpectedType => this.ctor.DeclaringType;

    void IProtoTypeSerializer.Callback(
      object value,
      TypeModel.CallbackType callbackType,
      SerializationContext context)
    {
    }

    object IProtoTypeSerializer.CreateInstance(ProtoReader source)
    {
      throw new NotSupportedException();
    }

    private object GetValue(object obj, int index)
    {
      PropertyInfo member1;
      if ((member1 = this.members[index] as PropertyInfo) != (PropertyInfo) null)
      {
        if (obj != null)
          return member1.GetValue(obj, (object[]) null);
        return !Helpers.IsValueType(member1.PropertyType) ? (object) null : Activator.CreateInstance(member1.PropertyType);
      }
      FieldInfo member2;
      if (!((member2 = this.members[index] as FieldInfo) != (FieldInfo) null))
        throw new InvalidOperationException();
      if (obj != null)
        return member2.GetValue(obj);
      return !Helpers.IsValueType(member2.FieldType) ? (object) null : Activator.CreateInstance(member2.FieldType);
    }

    public object Read(object value, ProtoReader source)
    {
      object[] parameters = new object[this.members.Length];
      bool flag = false;
      if (value == null)
        flag = true;
      for (int index = 0; index < parameters.Length; ++index)
        parameters[index] = this.GetValue(value, index);
      int num;
      while ((num = source.ReadFieldHeader()) > 0)
      {
        flag = true;
        if (num <= this.tails.Length)
        {
          IProtoSerializer tail = this.tails[num - 1];
          parameters[num - 1] = this.tails[num - 1].Read(tail.RequiresOldValue ? parameters[num - 1] : (object) null, source);
        }
        else
          source.SkipField();
      }
      return !flag ? value : this.ctor.Invoke(parameters);
    }

    public void Write(object value, ProtoWriter dest)
    {
      for (int index = 0; index < this.tails.Length; ++index)
      {
        object obj = this.GetValue(value, index);
        if (obj != null)
          this.tails[index].Write(obj, dest);
      }
    }

    public bool RequiresOldValue => true;

    public bool ReturnsValue => false;

    private Type GetMemberType(int index)
    {
      Type memberType = Helpers.GetMemberType(this.members[index]);
      return !(memberType == (Type) null) ? memberType : throw new InvalidOperationException();
    }

    bool IProtoTypeSerializer.CanCreateInstance() => false;

    public void EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      using (Local localWithValue = ctx.GetLocalWithValue(this.ctor.DeclaringType, valueFrom))
      {
        for (int index = 0; index < this.tails.Length; ++index)
        {
          Type memberType = this.GetMemberType(index);
          ctx.LoadAddress(localWithValue, this.ExpectedType);
          if ((object) (this.members[index] as FieldInfo) != null)
            ctx.LoadValue((FieldInfo) this.members[index]);
          else if ((object) (this.members[index] as PropertyInfo) != null)
            ctx.LoadValue((PropertyInfo) this.members[index]);
          ctx.WriteNullCheckedTail(memberType, this.tails[index], (Local) null);
        }
      }
    }

    void IProtoTypeSerializer.EmitCreateInstance(CompilerContext ctx)
    {
      throw new NotSupportedException();
    }

    public void EmitRead(CompilerContext ctx, Local incoming)
    {
      using (Local localWithValue = ctx.GetLocalWithValue(this.ExpectedType, incoming))
      {
        Local[] localArray = new Local[this.members.Length];
        try
        {
          for (int index = 0; index < localArray.Length; ++index)
          {
            Type memberType = this.GetMemberType(index);
            bool flag = true;
            localArray[index] = new Local(ctx, memberType);
            if (!Helpers.IsValueType(this.ExpectedType))
            {
              if (Helpers.IsValueType(memberType))
              {
                switch (Helpers.GetTypeCode(memberType))
                {
                  case ProtoTypeCode.Boolean:
                  case ProtoTypeCode.SByte:
                  case ProtoTypeCode.Byte:
                  case ProtoTypeCode.Int16:
                  case ProtoTypeCode.UInt16:
                  case ProtoTypeCode.Int32:
                  case ProtoTypeCode.UInt32:
                    ctx.LoadValue(0);
                    break;
                  case ProtoTypeCode.Int64:
                  case ProtoTypeCode.UInt64:
                    ctx.LoadValue(0L);
                    break;
                  case ProtoTypeCode.Single:
                    ctx.LoadValue(0.0f);
                    break;
                  case ProtoTypeCode.Double:
                    ctx.LoadValue(0.0);
                    break;
                  case ProtoTypeCode.Decimal:
                    ctx.LoadValue(0M);
                    break;
                  case ProtoTypeCode.Guid:
                    ctx.LoadValue(Guid.Empty);
                    break;
                  default:
                    ctx.LoadAddress(localArray[index], memberType);
                    ctx.EmitCtor(memberType);
                    flag = false;
                    break;
                }
              }
              else
                ctx.LoadNullRef();
              if (flag)
                ctx.StoreValue(localArray[index]);
            }
          }
          CodeLabel label1 = Helpers.IsValueType(this.ExpectedType) ? new CodeLabel() : ctx.DefineLabel();
          if (!Helpers.IsValueType(this.ExpectedType))
          {
            ctx.LoadAddress(localWithValue, this.ExpectedType);
            ctx.BranchIfFalse(label1, false);
          }
          for (int index = 0; index < this.members.Length; ++index)
          {
            ctx.LoadAddress(localWithValue, this.ExpectedType);
            if ((object) (this.members[index] as FieldInfo) != null)
              ctx.LoadValue((FieldInfo) this.members[index]);
            else if ((object) (this.members[index] as PropertyInfo) != null)
              ctx.LoadValue((PropertyInfo) this.members[index]);
            ctx.StoreValue(localArray[index]);
          }
          if (!Helpers.IsValueType(this.ExpectedType))
            ctx.MarkLabel(label1);
          using (Local local = new Local(ctx, ctx.MapType(typeof (int))))
          {
            CodeLabel label2 = ctx.DefineLabel();
            CodeLabel label3 = ctx.DefineLabel();
            CodeLabel label4 = ctx.DefineLabel();
            ctx.Branch(label2, false);
            CodeLabel[] jumpTable = new CodeLabel[this.members.Length];
            for (int index = 0; index < this.members.Length; ++index)
              jumpTable[index] = ctx.DefineLabel();
            ctx.MarkLabel(label3);
            ctx.LoadValue(local);
            ctx.LoadValue(1);
            ctx.Subtract();
            ctx.Switch(jumpTable);
            ctx.Branch(label4, false);
            for (int index = 0; index < jumpTable.Length; ++index)
            {
              ctx.MarkLabel(jumpTable[index]);
              IProtoSerializer tail = this.tails[index];
              Local valueFrom = tail.RequiresOldValue ? localArray[index] : (Local) null;
              ctx.ReadNullCheckedTail(localArray[index].Type, tail, valueFrom);
              if (tail.ReturnsValue)
              {
                if (Helpers.IsValueType(localArray[index].Type))
                {
                  ctx.StoreValue(localArray[index]);
                }
                else
                {
                  CodeLabel label5 = ctx.DefineLabel();
                  CodeLabel label6 = ctx.DefineLabel();
                  ctx.CopyValue();
                  ctx.BranchIfTrue(label5, true);
                  ctx.DiscardValue();
                  ctx.Branch(label6, true);
                  ctx.MarkLabel(label5);
                  ctx.StoreValue(localArray[index]);
                  ctx.MarkLabel(label6);
                }
              }
              ctx.Branch(label2, false);
            }
            ctx.MarkLabel(label4);
            ctx.LoadReaderWriter();
            ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("SkipField"));
            ctx.MarkLabel(label2);
            ctx.EmitBasicRead("ReadFieldHeader", ctx.MapType(typeof (int)));
            ctx.CopyValue();
            ctx.StoreValue(local);
            ctx.LoadValue(0);
            ctx.BranchIfGreater(label3, false);
          }
          for (int index = 0; index < localArray.Length; ++index)
            ctx.LoadValue(localArray[index]);
          ctx.EmitCtor(this.ctor);
          ctx.StoreValue(localWithValue);
        }
        finally
        {
          for (int index = 0; index < localArray.Length; ++index)
          {
            if (localArray[index] != null)
              localArray[index].Dispose();
          }
        }
      }
    }
  }
}
