// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.PropertyDecorator
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
  internal sealed class PropertyDecorator : ProtoDecoratorBase
  {
    private readonly PropertyInfo property;
    private readonly Type forType;
    private readonly bool readOptionsWriteValue;
    private readonly MethodInfo shadowSetter;

    public override Type ExpectedType => this.forType;

    public override bool RequiresOldValue => true;

    public override bool ReturnsValue => false;

    public PropertyDecorator(
      TypeModel model,
      Type forType,
      PropertyInfo property,
      IProtoSerializer tail)
      : base(tail)
    {
      this.forType = forType;
      this.property = property;
      PropertyDecorator.SanityCheck(model, property, tail, out this.readOptionsWriteValue, true, true);
      this.shadowSetter = PropertyDecorator.GetShadowSetter(model, property);
    }

    private static void SanityCheck(
      TypeModel model,
      PropertyInfo property,
      IProtoSerializer tail,
      out bool writeValue,
      bool nonPublic,
      bool allowInternal)
    {
      if (property == (PropertyInfo) null)
        throw new ArgumentNullException(nameof (property));
      writeValue = tail.ReturnsValue && (PropertyDecorator.GetShadowSetter(model, property) != (MethodInfo) null || property.CanWrite && Helpers.GetSetMethod(property, nonPublic, allowInternal) != (MethodInfo) null);
      if (!property.CanRead || Helpers.GetGetMethod(property, nonPublic, allowInternal) == (MethodInfo) null)
        throw new InvalidOperationException("Cannot serialize property without a get accessor");
      if (!writeValue && (!tail.RequiresOldValue || Helpers.IsValueType(tail.ExpectedType)))
        throw new InvalidOperationException("Cannot apply changes to property " + property.DeclaringType.FullName + "." + property.Name);
    }

    private static MethodInfo GetShadowSetter(TypeModel model, PropertyInfo property)
    {
      MethodInfo instanceMethod = Helpers.GetInstanceMethod(property.ReflectedType, "Set" + property.Name, new Type[1]
      {
        property.PropertyType
      });
      return instanceMethod == (MethodInfo) null || !instanceMethod.IsPublic || instanceMethod.ReturnType != model.MapType(typeof (void)) ? (MethodInfo) null : instanceMethod;
    }

    public override void Write(object value, ProtoWriter dest)
    {
      value = this.property.GetValue(value, (object[]) null);
      if (value == null)
        return;
      this.Tail.Write(value, dest);
    }

    public override object Read(object value, ProtoReader source)
    {
      object obj = this.Tail.Read(this.Tail.RequiresOldValue ? this.property.GetValue(value, (object[]) null) : (object) null, source);
      if (this.readOptionsWriteValue && obj != null)
      {
        if (this.shadowSetter == (MethodInfo) null)
          this.property.SetValue(value, obj, (object[]) null);
        else
          this.shadowSetter.Invoke(value, new object[1]
          {
            obj
          });
      }
      return (object) null;
    }

    protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.LoadAddress(valueFrom, this.ExpectedType);
      ctx.LoadValue(this.property);
      ctx.WriteNullCheckedTail(this.property.PropertyType, this.Tail, (Local) null);
    }

    protected override void EmitRead(CompilerContext ctx, Local valueFrom)
    {
      bool writeValue;
      PropertyDecorator.SanityCheck(ctx.Model, this.property, this.Tail, out writeValue, ctx.NonPublic, ctx.AllowInternal(this.property));
      if (Helpers.IsValueType(this.ExpectedType) && valueFrom == null)
        throw new InvalidOperationException("Attempt to mutate struct on the head of the stack; changes would be lost");
      using (Local localWithValue = ctx.GetLocalWithValue(this.ExpectedType, valueFrom))
      {
        if (this.Tail.RequiresOldValue)
        {
          ctx.LoadAddress(localWithValue, this.ExpectedType);
          ctx.LoadValue(this.property);
        }
        Type propertyType = this.property.PropertyType;
        ctx.ReadNullCheckedTail(propertyType, this.Tail, (Local) null);
        if (writeValue)
        {
          using (Local local = new Local(ctx, this.property.PropertyType))
          {
            ctx.StoreValue(local);
            CodeLabel label = new CodeLabel();
            if (!Helpers.IsValueType(propertyType))
            {
              label = ctx.DefineLabel();
              ctx.LoadValue(local);
              ctx.BranchIfFalse(label, true);
            }
            ctx.LoadAddress(localWithValue, this.ExpectedType);
            ctx.LoadValue(local);
            if (this.shadowSetter == (MethodInfo) null)
              ctx.StoreValue(this.property);
            else
              ctx.EmitCall(this.shadowSetter);
            if (Helpers.IsValueType(propertyType))
              return;
            ctx.MarkLabel(label);
          }
        }
        else
        {
          if (!this.Tail.ReturnsValue)
            return;
          ctx.DiscardValue();
        }
      }
    }

    internal static bool CanWrite(TypeModel model, MemberInfo member)
    {
      PropertyInfo property = !(member == (MemberInfo) null) ? member as PropertyInfo : throw new ArgumentNullException(nameof (member));
      if (!(property != (PropertyInfo) null))
        return member is FieldInfo;
      return property.CanWrite || PropertyDecorator.GetShadowSetter(model, property) != (MethodInfo) null;
    }
  }
}
