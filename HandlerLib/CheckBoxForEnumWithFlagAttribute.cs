// Decompiled with JetBrains decompiler
// Type: HandlerLib.CheckBoxForEnumWithFlagAttribute
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandlerLib
{
  public class CheckBoxForEnumWithFlagAttribute : CheckBox
  {
    public static DependencyProperty EnumValueProperty = DependencyProperty.Register(nameof (EnumValue), typeof (object), typeof (CheckBoxForEnumWithFlagAttribute), new PropertyMetadata(new PropertyChangedCallback(CheckBoxForEnumWithFlagAttribute.EnumValueChangedCallback)));
    public static DependencyProperty EnumFlagProperty = DependencyProperty.Register(nameof (EnumFlag), typeof (object), typeof (CheckBoxForEnumWithFlagAttribute), new PropertyMetadata(new PropertyChangedCallback(CheckBoxForEnumWithFlagAttribute.EnumFlagChangedCallback)));

    public CheckBoxForEnumWithFlagAttribute()
    {
      this.Checked += new RoutedEventHandler(this.CheckBoxForEnumWithFlag_Checked);
      this.Unchecked += new RoutedEventHandler(this.CheckBoxForEnumWithFlag_Unchecked);
    }

    private static void EnumValueChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      if (!(dependencyObject is CheckBoxForEnumWithFlagAttribute withFlagAttribute))
        return;
      withFlagAttribute.RefreshCheckBoxState();
    }

    private static void EnumFlagChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      if (!(dependencyObject is CheckBoxForEnumWithFlagAttribute withFlagAttribute))
        return;
      withFlagAttribute.RefreshCheckBoxState();
    }

    public object EnumValue
    {
      get => this.GetValue(CheckBoxForEnumWithFlagAttribute.EnumValueProperty);
      set => this.SetValue(CheckBoxForEnumWithFlagAttribute.EnumValueProperty, value);
    }

    public object EnumFlag
    {
      get => this.GetValue(CheckBoxForEnumWithFlagAttribute.EnumFlagProperty);
      set => this.SetValue(CheckBoxForEnumWithFlagAttribute.EnumFlagProperty, value);
    }

    private void RefreshCheckBoxState()
    {
      if (this.EnumValue == null || !(this.EnumValue is Enum))
        return;
      Type underlyingType = Enum.GetUnderlyingType(this.EnumValue.GetType());
      object obj1 = Convert.ChangeType(this.EnumValue, underlyingType);
      object obj2 = Convert.ChangeType(this.EnumFlag, underlyingType);
      // ISSUE: reference to a compiler-generated field
      if (CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__11.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__11.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool?>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool?), typeof (CheckBoxForEnumWithFlagAttribute)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool?> target1 = CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__11.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool?>> p2 = CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__11.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__11.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__11.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof (CheckBoxForEnumWithFlagAttribute), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target2 = CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__11.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p1 = CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__11.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__11.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__11.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.And, typeof (CheckBoxForEnumWithFlagAttribute), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__11.\u003C\u003Ep__0.Target((CallSite) CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__11.\u003C\u003Ep__0, obj1, obj2);
      object obj4 = target2((CallSite) p1, obj3, 0);
      this.IsChecked = target1((CallSite) p2, obj4);
    }

    private void CheckBoxForEnumWithFlag_Checked(object sender, RoutedEventArgs e)
    {
      this.RefreshEnumValue();
    }

    private void CheckBoxForEnumWithFlag_Unchecked(object sender, RoutedEventArgs e)
    {
      this.RefreshEnumValue();
    }

    private void RefreshEnumValue()
    {
      if (this.EnumValue == null || !(this.EnumValue is Enum))
        return;
      Type underlyingType = Enum.GetUnderlyingType(this.EnumValue.GetType());
      object obj1 = Convert.ChangeType(this.EnumValue, underlyingType);
      object obj2 = Convert.ChangeType(this.EnumFlag, underlyingType);
      bool? isChecked = this.IsChecked;
      bool flag = true;
      object obj3;
      if (isChecked.GetValueOrDefault() == flag & isChecked.HasValue)
      {
        // ISSUE: reference to a compiler-generated field
        if (CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Or, typeof (CheckBoxForEnumWithFlagAttribute), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        obj3 = CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__0.Target((CallSite) CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__0, obj1, obj2);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.And, typeof (CheckBoxForEnumWithFlagAttribute), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object, object> target = CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object, object>> p2 = CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__2;
        object obj4 = obj1;
        // ISSUE: reference to a compiler-generated field
        if (CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.OnesComplement, typeof (CheckBoxForEnumWithFlagAttribute), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj5 = CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__1.Target((CallSite) CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__1, obj2);
        obj3 = target((CallSite) p2, obj4, obj5);
      }
      // ISSUE: reference to a compiler-generated field
      if (CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (CheckBoxForEnumWithFlagAttribute), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p4 = CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (CheckBoxForEnumWithFlagAttribute), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__3.Target((CallSite) CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__3, obj3, obj1);
      if (target1((CallSite) p4, obj6))
      {
        // ISSUE: reference to a compiler-generated field
        if (CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__5 = CallSite<Func<CallSite, Type, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToObject", (IEnumerable<Type>) null, typeof (CheckBoxForEnumWithFlagAttribute), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.EnumValue = CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__5.Target((CallSite) CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__14.\u003C\u003Ep__5, typeof (Enum), this.EnumValue.GetType(), obj3);
      }
    }

    public void SetChecked(Enum value, Enum flag)
    {
      this.EnumFlag = (object) flag;
      Type underlyingType = Enum.GetUnderlyingType(value.GetType());
      object obj1 = Convert.ChangeType((object) value, underlyingType);
      object obj2 = Convert.ChangeType((object) flag, underlyingType);
      // ISSUE: reference to a compiler-generated field
      if (CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__15.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__15.\u003C\u003Ep__1 = CallSite<Func<CallSite, Type, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToObject", (IEnumerable<Type>) null, typeof (CheckBoxForEnumWithFlagAttribute), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, Type, object, object> target = CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__15.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, Type, object, object>> p1 = CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__15.\u003C\u003Ep__1;
      Type type1 = typeof (Enum);
      Type type2 = value.GetType();
      // ISSUE: reference to a compiler-generated field
      if (CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__15.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__15.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.And, typeof (CheckBoxForEnumWithFlagAttribute), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__15.\u003C\u003Ep__0.Target((CallSite) CheckBoxForEnumWithFlagAttribute.\u003C\u003Eo__15.\u003C\u003Ep__0, obj1, obj2);
      this.EnumValue = target((CallSite) p1, type1, type2, obj3);
    }
  }
}
