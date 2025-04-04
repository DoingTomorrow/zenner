// Decompiled with JetBrains decompiler
// Type: System.Reactive.ReflectionUtils
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;

#nullable disable
namespace System.Reactive
{
  internal static class ReflectionUtils
  {
    public static TDelegate CreateDelegate<TDelegate>(object o, MethodInfo method)
    {
      return (TDelegate) Delegate.CreateDelegate(typeof (TDelegate), o, method);
    }

    public static Delegate CreateDelegate(Type delegateType, object o, MethodInfo method)
    {
      return Delegate.CreateDelegate(delegateType, o, method);
    }

    public static void GetEventMethods<TSender, TEventArgs>(
      Type targetType,
      object target,
      string eventName,
      out MethodInfo addMethod,
      out MethodInfo removeMethod,
      out Type delegateType,
      out bool isWinRT)
    {
      EventInfo eventEx;
      if (target == null)
      {
        eventEx = targetType.GetEventEx(eventName, true);
        if (eventEx == (EventInfo) null)
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Strings_Linq.COULD_NOT_FIND_STATIC_EVENT, new object[2]
          {
            (object) eventName,
            (object) targetType.FullName
          }));
      }
      else
      {
        eventEx = targetType.GetEventEx(eventName, false);
        if (eventEx == (EventInfo) null)
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Strings_Linq.COULD_NOT_FIND_INSTANCE_EVENT, new object[2]
          {
            (object) eventName,
            (object) targetType.FullName
          }));
      }
      addMethod = eventEx.GetAddMethod();
      removeMethod = eventEx.GetRemoveMethod();
      if (addMethod == (MethodInfo) null)
        throw new InvalidOperationException(Strings_Linq.EVENT_MISSING_ADD_METHOD);
      if (removeMethod == (MethodInfo) null)
        throw new InvalidOperationException(Strings_Linq.EVENT_MISSING_REMOVE_METHOD);
      ParameterInfo[] parameters1 = addMethod.GetParameters();
      if (parameters1.Length != 1)
        throw new InvalidOperationException(Strings_Linq.EVENT_ADD_METHOD_SHOULD_TAKE_ONE_PARAMETER);
      ParameterInfo[] parameters2 = removeMethod.GetParameters();
      if (parameters2.Length != 1)
        throw new InvalidOperationException(Strings_Linq.EVENT_REMOVE_METHOD_SHOULD_TAKE_ONE_PARAMETER);
      isWinRT = false;
      if (addMethod.ReturnType == typeof (EventRegistrationToken))
      {
        isWinRT = true;
        if (parameters2[0].ParameterType != typeof (EventRegistrationToken))
          throw new InvalidOperationException(Strings_Linq.EVENT_WINRT_REMOVE_METHOD_SHOULD_TAKE_ERT);
      }
      delegateType = parameters1[0].ParameterType;
      MethodInfo method = delegateType.GetMethod("Invoke");
      ParameterInfo[] parameters3 = method.GetParameters();
      if (parameters3.Length != 2)
        throw new InvalidOperationException(Strings_Linq.EVENT_PATTERN_REQUIRES_TWO_PARAMETERS);
      if (!typeof (TSender).IsAssignableFrom(parameters3[0].ParameterType))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Strings_Linq.EVENT_SENDER_NOT_ASSIGNABLE, new object[1]
        {
          (object) typeof (TSender).FullName
        }));
      if (!typeof (TEventArgs).IsAssignableFrom(parameters3[1].ParameterType))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Strings_Linq.EVENT_ARGS_NOT_ASSIGNABLE, new object[1]
        {
          (object) typeof (TEventArgs).FullName
        }));
      if (method.ReturnType != typeof (void))
        throw new InvalidOperationException(Strings_Linq.EVENT_MUST_RETURN_VOID);
    }

    public static EventInfo GetEventEx(this Type type, string name, bool isStatic)
    {
      return type.GetEvent(name, isStatic ? BindingFlags.Static | BindingFlags.Public : BindingFlags.Instance | BindingFlags.Public);
    }
  }
}
