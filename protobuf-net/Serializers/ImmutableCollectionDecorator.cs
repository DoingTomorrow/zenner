// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.ImmutableCollectionDecorator
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
  internal sealed class ImmutableCollectionDecorator : ListDecorator
  {
    private readonly MethodInfo builderFactory;
    private readonly MethodInfo add;
    private readonly MethodInfo addRange;
    private readonly MethodInfo finish;

    protected override bool RequireAdd => false;

    private static Type ResolveIReadOnlyCollection(Type declaredType, Type t)
    {
      foreach (Type type in declaredType.GetInterfaces())
      {
        if (type.IsGenericType && type.Name.StartsWith("IReadOnlyCollection`"))
        {
          if (t != (Type) null)
          {
            Type[] genericArguments = type.GetGenericArguments();
            if (genericArguments.Length != 1 && genericArguments[0] != t)
              continue;
          }
          return type;
        }
      }
      return (Type) null;
    }

    internal static bool IdentifyImmutable(
      TypeModel model,
      Type declaredType,
      out MethodInfo builderFactory,
      out MethodInfo add,
      out MethodInfo addRange,
      out MethodInfo finish)
    {
      builderFactory = add = addRange = finish = (MethodInfo) null;
      if (model == null || declaredType == (Type) null)
        return false;
      Type type1 = declaredType;
      if (!type1.IsGenericType)
        return false;
      Type[] genericArguments = type1.GetGenericArguments();
      Type[] types;
      switch (genericArguments.Length)
      {
        case 1:
          types = genericArguments;
          break;
        case 2:
          Type type2 = model.MapType(typeof (KeyValuePair<,>));
          if (type2 == (Type) null)
            return false;
          types = new Type[1]
          {
            type2.MakeGenericType(genericArguments)
          };
          break;
        default:
          return false;
      }
      if (ImmutableCollectionDecorator.ResolveIReadOnlyCollection(declaredType, (Type) null) == (Type) null)
        return false;
      string name = declaredType.Name;
      int length = name.IndexOf('`');
      if (length <= 0)
        return false;
      string str = type1.IsInterface ? name.Substring(1, length - 1) : name.Substring(0, length);
      Type type3 = model.GetType(declaredType.Namespace + "." + str, type1.Assembly);
      if (type3 == (Type) null && str == "ImmutableSet")
        type3 = model.GetType(declaredType.Namespace + ".ImmutableHashSet", type1.Assembly);
      if (type3 == (Type) null)
        return false;
      foreach (MethodInfo method in type3.GetMethods())
      {
        if (method.IsStatic && !(method.Name != "CreateBuilder") && method.IsGenericMethodDefinition && method.GetParameters().Length == 0 && method.GetGenericArguments().Length == genericArguments.Length)
        {
          builderFactory = method.MakeGenericMethod(genericArguments);
          break;
        }
      }
      Type type4 = model.MapType(typeof (void));
      if (builderFactory == (MethodInfo) null || builderFactory.ReturnType == (Type) null || builderFactory.ReturnType == type4)
        return false;
      add = Helpers.GetInstanceMethod(builderFactory.ReturnType, "Add", types);
      if (add == (MethodInfo) null)
        return false;
      finish = Helpers.GetInstanceMethod(builderFactory.ReturnType, "ToImmutable", Helpers.EmptyTypes);
      if (finish == (MethodInfo) null || finish.ReturnType == (Type) null || finish.ReturnType == type4 || !(finish.ReturnType == declaredType) && !Helpers.IsAssignableFrom(declaredType, finish.ReturnType))
        return false;
      addRange = Helpers.GetInstanceMethod(builderFactory.ReturnType, "AddRange", new Type[1]
      {
        declaredType
      });
      if (addRange == (MethodInfo) null)
      {
        Type type5 = model.MapType(typeof (IEnumerable<>), false);
        if (type5 != (Type) null)
          addRange = Helpers.GetInstanceMethod(builderFactory.ReturnType, "AddRange", new Type[1]
          {
            type5.MakeGenericType(types)
          });
      }
      return true;
    }

    internal ImmutableCollectionDecorator(
      TypeModel model,
      Type declaredType,
      Type concreteType,
      IProtoSerializer tail,
      int fieldNumber,
      bool writePacked,
      WireType packedWireType,
      bool returnList,
      bool overwriteList,
      bool supportNull,
      MethodInfo builderFactory,
      MethodInfo add,
      MethodInfo addRange,
      MethodInfo finish)
      : base(model, declaredType, concreteType, tail, fieldNumber, writePacked, packedWireType, returnList, overwriteList, supportNull)
    {
      this.builderFactory = builderFactory;
      this.add = add;
      this.addRange = addRange;
      this.finish = finish;
    }

    public override object Read(object value, ProtoReader source)
    {
      object obj1 = this.builderFactory.Invoke((object) null, (object[]) null);
      int fieldNumber = source.FieldNumber;
      object[] parameters = new object[1];
      if (this.AppendToCollection && value != null && ((ICollection) value).Count != 0)
      {
        if (this.addRange != (MethodInfo) null)
        {
          parameters[0] = value;
          this.addRange.Invoke(obj1, parameters);
        }
        else
        {
          foreach (object obj2 in (IEnumerable) value)
          {
            parameters[0] = obj2;
            this.add.Invoke(obj1, parameters);
          }
        }
      }
      if (this.packedWireType != WireType.None && source.WireType == WireType.String)
      {
        SubItemToken token = ProtoReader.StartSubItem(source);
        while (ProtoReader.HasSubValue(this.packedWireType, source))
        {
          parameters[0] = this.Tail.Read((object) null, source);
          this.add.Invoke(obj1, parameters);
        }
        ProtoReader.EndSubItem(token, source);
      }
      else
      {
        do
        {
          parameters[0] = this.Tail.Read((object) null, source);
          this.add.Invoke(obj1, parameters);
        }
        while (source.TryReadFieldHeader(fieldNumber));
      }
      return this.finish.Invoke(obj1, (object[]) null);
    }

    protected override void EmitRead(CompilerContext ctx, Local valueFrom)
    {
      using (Local localWithValue = this.AppendToCollection ? ctx.GetLocalWithValue(this.ExpectedType, valueFrom) : (Local) null)
      {
        using (Local local1 = new Local(ctx, this.builderFactory.ReturnType))
        {
          ctx.EmitCall(this.builderFactory);
          ctx.StoreValue(local1);
          if (this.AppendToCollection)
          {
            CodeLabel label1 = ctx.DefineLabel();
            if (!Helpers.IsValueType(this.ExpectedType))
            {
              ctx.LoadValue(localWithValue);
              ctx.BranchIfFalse(label1, false);
            }
            Type expectedType = this.ExpectedType;
            PropertyInfo property = Helpers.GetProperty(expectedType, "Length", false);
            if (property == (PropertyInfo) null)
              property = Helpers.GetProperty(expectedType, "Count", false);
            if (property == (PropertyInfo) null)
              property = Helpers.GetProperty(ImmutableCollectionDecorator.ResolveIReadOnlyCollection(this.ExpectedType, this.Tail.ExpectedType), "Count", false);
            ctx.LoadAddress(localWithValue, localWithValue.Type);
            ctx.EmitCall(Helpers.GetGetMethod(property, false, false));
            ctx.BranchIfFalse(label1, false);
            Type type = ctx.MapType(typeof (void));
            if (this.addRange != (MethodInfo) null)
            {
              ctx.LoadValue(local1);
              ctx.LoadValue(localWithValue);
              ctx.EmitCall(this.addRange);
              if (this.addRange.ReturnType != (Type) null && this.add.ReturnType != type)
                ctx.DiscardValue();
            }
            else
            {
              MethodInfo moveNext;
              MethodInfo current;
              MethodInfo enumeratorInfo = this.GetEnumeratorInfo(ctx.Model, out moveNext, out current);
              Type returnType = enumeratorInfo.ReturnType;
              using (Local local2 = new Local(ctx, returnType))
              {
                ctx.LoadAddress(localWithValue, this.ExpectedType);
                ctx.EmitCall(enumeratorInfo);
                ctx.StoreValue(local2);
                using (ctx.Using(local2))
                {
                  CodeLabel label2 = ctx.DefineLabel();
                  CodeLabel label3 = ctx.DefineLabel();
                  ctx.Branch(label3, false);
                  ctx.MarkLabel(label2);
                  ctx.LoadAddress(local1, local1.Type);
                  ctx.LoadAddress(local2, returnType);
                  ctx.EmitCall(current);
                  ctx.EmitCall(this.add);
                  if (this.add.ReturnType != (Type) null && this.add.ReturnType != type)
                    ctx.DiscardValue();
                  ctx.MarkLabel(label3);
                  ctx.LoadAddress(local2, returnType);
                  ctx.EmitCall(moveNext);
                  ctx.BranchIfTrue(label2, false);
                }
              }
            }
            ctx.MarkLabel(label1);
          }
          ListDecorator.EmitReadList(ctx, local1, this.Tail, this.add, this.packedWireType, false);
          ctx.LoadAddress(local1, local1.Type);
          ctx.EmitCall(this.finish);
          if (!(this.ExpectedType != this.finish.ReturnType))
            return;
          ctx.Cast(this.ExpectedType);
        }
      }
    }
  }
}
