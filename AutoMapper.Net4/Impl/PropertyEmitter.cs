// Decompiled with JetBrains decompiler
// Type: AutoMapper.Impl.PropertyEmitter
// Assembly: AutoMapper.Net4, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: 30ECE8B3-1802-489A-86AE-267466F9FF1F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.Net4.dll

using System;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace AutoMapper.Impl
{
  public class PropertyEmitter
  {
    private static readonly MethodInfo proxyBase_NotifyPropertyChanged = typeof (ProxyBase).GetMethod("NotifyPropertyChanged", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new Type[2]
    {
      typeof (PropertyChangedEventHandler),
      typeof (string)
    }, (ParameterModifier[]) null);
    private readonly FieldBuilder fieldBuilder;
    private readonly MethodBuilder getterBuilder;
    private readonly TypeBuilder owner;
    private readonly PropertyBuilder propertyBuilder;
    private readonly FieldBuilder propertyChangedField;
    private readonly MethodBuilder setterBuilder;

    public PropertyEmitter(
      TypeBuilder owner,
      string name,
      Type propertyType,
      FieldBuilder propertyChangedField)
    {
      this.owner = owner;
      this.propertyChangedField = propertyChangedField;
      this.fieldBuilder = owner.DefineField(string.Format("<{0}>", (object) name), propertyType, FieldAttributes.Private);
      this.getterBuilder = owner.DefineMethod(string.Format("get_{0}", (object) name), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName, propertyType, Type.EmptyTypes);
      ILGenerator ilGenerator1 = this.getterBuilder.GetILGenerator();
      ilGenerator1.Emit(OpCodes.Ldarg_0);
      ilGenerator1.Emit(OpCodes.Ldfld, (FieldInfo) this.fieldBuilder);
      ilGenerator1.Emit(OpCodes.Ret);
      this.setterBuilder = owner.DefineMethod(string.Format("set_{0}", (object) name), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName, typeof (void), new Type[1]
      {
        propertyType
      });
      ILGenerator ilGenerator2 = this.setterBuilder.GetILGenerator();
      ilGenerator2.Emit(OpCodes.Ldarg_0);
      ilGenerator2.Emit(OpCodes.Ldarg_1);
      ilGenerator2.Emit(OpCodes.Stfld, (FieldInfo) this.fieldBuilder);
      if ((FieldInfo) propertyChangedField != (FieldInfo) null)
      {
        ilGenerator2.Emit(OpCodes.Ldarg_0);
        ilGenerator2.Emit(OpCodes.Dup);
        ilGenerator2.Emit(OpCodes.Ldfld, (FieldInfo) propertyChangedField);
        ilGenerator2.Emit(OpCodes.Ldstr, name);
        ilGenerator2.Emit(OpCodes.Call, PropertyEmitter.proxyBase_NotifyPropertyChanged);
      }
      ilGenerator2.Emit(OpCodes.Ret);
      this.propertyBuilder = owner.DefineProperty(name, PropertyAttributes.None, propertyType, (Type[]) null);
      this.propertyBuilder.SetGetMethod(this.getterBuilder);
      this.propertyBuilder.SetSetMethod(this.setterBuilder);
    }

    public Type PropertyType => this.propertyBuilder.PropertyType;

    public MethodBuilder GetGetter(Type requiredType)
    {
      if (!requiredType.IsAssignableFrom(this.PropertyType))
        throw new InvalidOperationException("Types are not compatible");
      return this.getterBuilder;
    }

    public MethodBuilder GetSetter(Type requiredType)
    {
      if (!this.PropertyType.IsAssignableFrom(requiredType))
        throw new InvalidOperationException("Types are not compatible");
      return this.setterBuilder;
    }
  }
}
