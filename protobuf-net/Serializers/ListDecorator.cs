// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.ListDecorator
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal class ListDecorator : ProtoDecoratorBase
  {
    private readonly byte options;
    private const byte OPTIONS_IsList = 1;
    private const byte OPTIONS_SuppressIList = 2;
    private const byte OPTIONS_WritePacked = 4;
    private const byte OPTIONS_ReturnList = 8;
    private const byte OPTIONS_OverwriteList = 16;
    private const byte OPTIONS_SupportNull = 32;
    private readonly Type declaredType;
    private readonly Type concreteType;
    private readonly MethodInfo add;
    private readonly int fieldNumber;
    protected readonly WireType packedWireType;
    private static readonly Type ienumeratorType = typeof (IEnumerator);
    private static readonly Type ienumerableType = typeof (IEnumerable);

    internal static bool CanPack(WireType wireType)
    {
      if (wireType <= WireType.Fixed64)
      {
        if (wireType != WireType.Variant && wireType != WireType.Fixed64)
          goto label_4;
      }
      else if (wireType != WireType.Fixed32 && wireType != WireType.SignedVariant)
        goto label_4;
      return true;
label_4:
      return false;
    }

    private bool IsList => ((uint) this.options & 1U) > 0U;

    private bool SuppressIList => ((uint) this.options & 2U) > 0U;

    private bool WritePacked => ((uint) this.options & 4U) > 0U;

    private bool SupportNull => ((uint) this.options & 32U) > 0U;

    private bool ReturnList => ((uint) this.options & 8U) > 0U;

    internal static ListDecorator Create(
      TypeModel model,
      Type declaredType,
      Type concreteType,
      IProtoSerializer tail,
      int fieldNumber,
      bool writePacked,
      WireType packedWireType,
      bool returnList,
      bool overwriteList,
      bool supportNull)
    {
      MethodInfo builderFactory;
      MethodInfo add;
      MethodInfo addRange;
      MethodInfo finish;
      return returnList && ImmutableCollectionDecorator.IdentifyImmutable(model, declaredType, out builderFactory, out add, out addRange, out finish) ? (ListDecorator) new ImmutableCollectionDecorator(model, declaredType, concreteType, tail, fieldNumber, writePacked, packedWireType, returnList, overwriteList, supportNull, builderFactory, add, addRange, finish) : new ListDecorator(model, declaredType, concreteType, tail, fieldNumber, writePacked, packedWireType, returnList, overwriteList, supportNull);
    }

    protected ListDecorator(
      TypeModel model,
      Type declaredType,
      Type concreteType,
      IProtoSerializer tail,
      int fieldNumber,
      bool writePacked,
      WireType packedWireType,
      bool returnList,
      bool overwriteList,
      bool supportNull)
      : base(tail)
    {
      if (returnList)
        this.options |= (byte) 8;
      if (overwriteList)
        this.options |= (byte) 16;
      if (supportNull)
        this.options |= (byte) 32;
      if ((writePacked || packedWireType != WireType.None) && fieldNumber <= 0)
        throw new ArgumentOutOfRangeException(nameof (fieldNumber));
      if (!ListDecorator.CanPack(packedWireType))
      {
        if (writePacked)
          throw new InvalidOperationException("Only simple data-types can use packed encoding");
        packedWireType = WireType.None;
      }
      this.fieldNumber = fieldNumber;
      if (writePacked)
        this.options |= (byte) 4;
      this.packedWireType = packedWireType;
      if (declaredType == (Type) null)
        throw new ArgumentNullException(nameof (declaredType));
      this.declaredType = !declaredType.IsArray ? declaredType : throw new ArgumentException("Cannot treat arrays as lists", nameof (declaredType));
      this.concreteType = concreteType;
      if (!this.RequireAdd)
        return;
      bool isList;
      this.add = TypeModel.ResolveListAdd(model, declaredType, tail.ExpectedType, out isList);
      if (isList)
      {
        this.options |= (byte) 1;
        string fullName = declaredType.FullName;
        if (fullName != null && fullName.StartsWith("System.Data.Linq.EntitySet`1[["))
          this.options |= (byte) 2;
      }
      if (this.add == (MethodInfo) null)
        throw new InvalidOperationException("Unable to resolve a suitable Add method for " + declaredType.FullName);
    }

    protected virtual bool RequireAdd => true;

    public override Type ExpectedType => this.declaredType;

    public override bool RequiresOldValue => this.AppendToCollection;

    public override bool ReturnsValue => this.ReturnList;

    protected bool AppendToCollection => ((int) this.options & 16) == 0;

    protected override void EmitRead(CompilerContext ctx, Local valueFrom)
    {
      bool returnList = this.ReturnList;
      using (Local local1 = this.AppendToCollection ? ctx.GetLocalWithValue(this.ExpectedType, valueFrom) : new Local(ctx, this.declaredType))
      {
        using (Local local2 = !returnList || !this.AppendToCollection || Helpers.IsValueType(this.ExpectedType) ? (Local) null : new Local(ctx, this.ExpectedType))
        {
          if (!this.AppendToCollection)
          {
            ctx.LoadNullRef();
            ctx.StoreValue(local1);
          }
          else if (returnList && local2 != null)
          {
            ctx.LoadValue(local1);
            ctx.StoreValue(local2);
          }
          if (this.concreteType != (Type) null)
          {
            ctx.LoadValue(local1);
            CodeLabel label = ctx.DefineLabel();
            ctx.BranchIfTrue(label, true);
            ctx.EmitCtor(this.concreteType);
            ctx.StoreValue(local1);
            ctx.MarkLabel(label);
          }
          bool castListForAdd = !this.add.DeclaringType.IsAssignableFrom(this.declaredType);
          ListDecorator.EmitReadList(ctx, local1, this.Tail, this.add, this.packedWireType, castListForAdd);
          if (!returnList)
            return;
          if (this.AppendToCollection && local2 != null)
          {
            ctx.LoadValue(local2);
            ctx.LoadValue(local1);
            CodeLabel label1 = ctx.DefineLabel();
            CodeLabel label2 = ctx.DefineLabel();
            ctx.BranchIfEqual(label1, true);
            ctx.LoadValue(local1);
            ctx.Branch(label2, true);
            ctx.MarkLabel(label1);
            ctx.LoadNullRef();
            ctx.MarkLabel(label2);
          }
          else
            ctx.LoadValue(local1);
        }
      }
    }

    internal static void EmitReadList(
      CompilerContext ctx,
      Local list,
      IProtoSerializer tail,
      MethodInfo add,
      WireType packedWireType,
      bool castListForAdd)
    {
      using (Local local = new Local(ctx, ctx.MapType(typeof (int))))
      {
        CodeLabel label1 = packedWireType == WireType.None ? new CodeLabel() : ctx.DefineLabel();
        if (packedWireType != WireType.None)
        {
          ctx.LoadReaderWriter();
          ctx.LoadValue(typeof (ProtoReader).GetProperty("WireType"));
          ctx.LoadValue(2);
          ctx.BranchIfEqual(label1, false);
        }
        ctx.LoadReaderWriter();
        ctx.LoadValue(typeof (ProtoReader).GetProperty("FieldNumber"));
        ctx.StoreValue(local);
        CodeLabel label2 = ctx.DefineLabel();
        ctx.MarkLabel(label2);
        ListDecorator.EmitReadAndAddItem(ctx, list, tail, add, castListForAdd);
        ctx.LoadReaderWriter();
        ctx.LoadValue(local);
        ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("TryReadFieldHeader"));
        ctx.BranchIfTrue(label2, false);
        if (packedWireType == WireType.None)
          return;
        CodeLabel label3 = ctx.DefineLabel();
        ctx.Branch(label3, false);
        ctx.MarkLabel(label1);
        ctx.LoadReaderWriter();
        ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("StartSubItem"));
        CodeLabel label4 = ctx.DefineLabel();
        CodeLabel label5 = ctx.DefineLabel();
        ctx.MarkLabel(label4);
        ctx.LoadValue((int) packedWireType);
        ctx.LoadReaderWriter();
        ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("HasSubValue"));
        ctx.BranchIfFalse(label5, false);
        ListDecorator.EmitReadAndAddItem(ctx, list, tail, add, castListForAdd);
        ctx.Branch(label4, false);
        ctx.MarkLabel(label5);
        ctx.LoadReaderWriter();
        ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("EndSubItem"));
        ctx.MarkLabel(label3);
      }
    }

    private static void EmitReadAndAddItem(
      CompilerContext ctx,
      Local list,
      IProtoSerializer tail,
      MethodInfo add,
      bool castListForAdd)
    {
      ctx.LoadAddress(list, list.Type);
      if (castListForAdd)
        ctx.Cast(add.DeclaringType);
      Type expectedType = tail.ExpectedType;
      bool returnsValue = tail.ReturnsValue;
      if (tail.RequiresOldValue)
      {
        if (Helpers.IsValueType(expectedType) || !returnsValue)
        {
          using (Local local = new Local(ctx, expectedType))
          {
            if (Helpers.IsValueType(expectedType))
            {
              ctx.LoadAddress(local, expectedType);
              ctx.EmitCtor(expectedType);
            }
            else
            {
              ctx.LoadNullRef();
              ctx.StoreValue(local);
            }
            tail.EmitRead(ctx, local);
            if (!returnsValue)
              ctx.LoadValue(local);
          }
        }
        else
        {
          ctx.LoadNullRef();
          tail.EmitRead(ctx, (Local) null);
        }
      }
      else
      {
        if (!returnsValue)
          throw new InvalidOperationException();
        tail.EmitRead(ctx, (Local) null);
      }
      Type parameterType = add.GetParameters()[0].ParameterType;
      if (parameterType != expectedType)
      {
        if (parameterType == ctx.MapType(typeof (object)))
        {
          ctx.CastToObject(expectedType);
        }
        else
        {
          ConstructorInfo ctor = Helpers.GetUnderlyingType(parameterType) == expectedType ? Helpers.GetConstructor(parameterType, new Type[1]
          {
            expectedType
          }, false) : throw new InvalidOperationException("Conflicting item/add type");
          ctx.EmitCtor(ctor);
        }
      }
      ctx.EmitCall(add, list.Type);
      if (!(add.ReturnType != ctx.MapType(typeof (void))))
        return;
      ctx.DiscardValue();
    }

    protected MethodInfo GetEnumeratorInfo(
      TypeModel model,
      out MethodInfo moveNext,
      out MethodInfo current)
    {
      Type declaringType = (Type) null;
      Type expectedType1 = this.ExpectedType;
      MethodInfo instanceMethod1 = Helpers.GetInstanceMethod(expectedType1, "GetEnumerator", (Type[]) null);
      Type expectedType2 = this.Tail.ExpectedType;
      if (instanceMethod1 != (MethodInfo) null)
      {
        Type returnType = instanceMethod1.ReturnType;
        moveNext = Helpers.GetInstanceMethod(returnType, "MoveNext", (Type[]) null);
        PropertyInfo property = Helpers.GetProperty(returnType, "Current", false);
        current = property == (PropertyInfo) null ? (MethodInfo) null : Helpers.GetGetMethod(property, false, false);
        if (moveNext == (MethodInfo) null && model.MapType(ListDecorator.ienumeratorType).IsAssignableFrom(returnType))
          moveNext = Helpers.GetInstanceMethod(model.MapType(ListDecorator.ienumeratorType), "MoveNext", (Type[]) null);
        if (moveNext != (MethodInfo) null && moveNext.ReturnType == model.MapType(typeof (bool)) && current != (MethodInfo) null && current.ReturnType == expectedType2)
          return instanceMethod1;
        ref MethodInfo local1 = ref moveNext;
        ref MethodInfo local2 = ref current;
        // ISSUE: variable of the null type
        __Null local3;
        MethodInfo methodInfo1 = (MethodInfo) (local3 = null);
        MethodInfo methodInfo2 = (MethodInfo) local3;
        local2 = (MethodInfo) local3;
        MethodInfo methodInfo3 = methodInfo2;
        local1 = methodInfo3;
      }
      Type type = model.MapType(typeof (IEnumerable<>), false);
      if (type != (Type) null)
        declaringType = type.MakeGenericType(expectedType2);
      if (declaringType != (Type) null && declaringType.IsAssignableFrom(expectedType1))
      {
        MethodInfo instanceMethod2 = Helpers.GetInstanceMethod(declaringType, "GetEnumerator");
        Type returnType = instanceMethod2.ReturnType;
        moveNext = Helpers.GetInstanceMethod(model.MapType(ListDecorator.ienumeratorType), "MoveNext");
        current = Helpers.GetGetMethod(Helpers.GetProperty(returnType, "Current", false), false, false);
        return instanceMethod2;
      }
      MethodInfo instanceMethod3 = Helpers.GetInstanceMethod(model.MapType(ListDecorator.ienumerableType), "GetEnumerator");
      Type returnType1 = instanceMethod3.ReturnType;
      moveNext = Helpers.GetInstanceMethod(returnType1, "MoveNext");
      current = Helpers.GetGetMethod(Helpers.GetProperty(returnType1, "Current", false), false, false);
      return instanceMethod3;
    }

    protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      using (Local localWithValue = ctx.GetLocalWithValue(this.ExpectedType, valueFrom))
      {
        MethodInfo moveNext;
        MethodInfo current;
        MethodInfo enumeratorInfo = this.GetEnumeratorInfo(ctx.Model, out moveNext, out current);
        Type returnType = enumeratorInfo.ReturnType;
        bool writePacked = this.WritePacked;
        using (Local local1 = new Local(ctx, returnType))
        {
          using (Local local2 = writePacked ? new Local(ctx, ctx.MapType(typeof (SubItemToken))) : (Local) null)
          {
            if (writePacked)
            {
              ctx.LoadValue(this.fieldNumber);
              ctx.LoadValue(2);
              ctx.LoadReaderWriter();
              ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("WriteFieldHeader"));
              ctx.LoadValue(localWithValue);
              ctx.LoadReaderWriter();
              ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("StartSubItem"));
              ctx.StoreValue(local2);
              ctx.LoadValue(this.fieldNumber);
              ctx.LoadReaderWriter();
              ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("SetPackedField"));
            }
            ctx.LoadAddress(localWithValue, this.ExpectedType);
            ctx.EmitCall(enumeratorInfo, this.ExpectedType);
            ctx.StoreValue(local1);
            using (ctx.Using(local1))
            {
              CodeLabel label1 = ctx.DefineLabel();
              CodeLabel label2 = ctx.DefineLabel();
              ctx.Branch(label2, false);
              ctx.MarkLabel(label1);
              ctx.LoadAddress(local1, returnType);
              ctx.EmitCall(current, returnType);
              Type expectedType = this.Tail.ExpectedType;
              if (expectedType != ctx.MapType(typeof (object)) && current.ReturnType == ctx.MapType(typeof (object)))
                ctx.CastFromObject(expectedType);
              this.Tail.EmitWrite(ctx, (Local) null);
              ctx.MarkLabel(label2);
              ctx.LoadAddress(local1, returnType);
              ctx.EmitCall(moveNext, returnType);
              ctx.BranchIfTrue(label1, false);
            }
            if (!writePacked)
              return;
            ctx.LoadValue(local2);
            ctx.LoadReaderWriter();
            ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("EndSubItem"));
          }
        }
      }
    }

    public override void Write(object value, ProtoWriter dest)
    {
      bool writePacked = this.WritePacked;
      SubItemToken token;
      if (writePacked)
      {
        ProtoWriter.WriteFieldHeader(this.fieldNumber, WireType.String, dest);
        token = ProtoWriter.StartSubItem(value, dest);
        ProtoWriter.SetPackedField(this.fieldNumber, dest);
      }
      else
        token = new SubItemToken();
      bool flag = !this.SupportNull;
      foreach (object obj in (IEnumerable) value)
      {
        if (flag && obj == null)
          throw new NullReferenceException();
        this.Tail.Write(obj, dest);
      }
      if (!writePacked)
        return;
      ProtoWriter.EndSubItem(token, dest);
    }

    public override object Read(object value, ProtoReader source)
    {
      try
      {
        int fieldNumber = source.FieldNumber;
        object obj = value;
        if (value == null)
          value = Activator.CreateInstance(this.concreteType);
        bool flag = this.IsList && !this.SuppressIList;
        if (this.packedWireType != WireType.None && source.WireType == WireType.String)
        {
          SubItemToken token = ProtoReader.StartSubItem(source);
          if (flag)
          {
            IList list = (IList) value;
            while (ProtoReader.HasSubValue(this.packedWireType, source))
              list.Add(this.Tail.Read((object) null, source));
          }
          else
          {
            object[] parameters = new object[1];
            while (ProtoReader.HasSubValue(this.packedWireType, source))
            {
              parameters[0] = this.Tail.Read((object) null, source);
              this.add.Invoke(value, parameters);
            }
          }
          ProtoReader.EndSubItem(token, source);
        }
        else if (flag)
        {
          IList list = (IList) value;
          do
          {
            list.Add(this.Tail.Read((object) null, source));
          }
          while (source.TryReadFieldHeader(fieldNumber));
        }
        else
        {
          object[] parameters = new object[1];
          do
          {
            parameters[0] = this.Tail.Read((object) null, source);
            this.add.Invoke(value, parameters);
          }
          while (source.TryReadFieldHeader(fieldNumber));
        }
        return obj == value ? (object) null : value;
      }
      catch (TargetInvocationException ex)
      {
        if (ex.InnerException != null)
          throw ex.InnerException;
        throw;
      }
    }
  }
}
