// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ActionDescriptorHelper
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace System.Web.Mvc
{
  internal static class ActionDescriptorHelper
  {
    public static ICollection<ActionSelector> GetSelectors(MethodInfo methodInfo)
    {
      return (ICollection<ActionSelector>) Array.ConvertAll<ActionMethodSelectorAttribute, ActionSelector>((ActionMethodSelectorAttribute[]) methodInfo.GetCustomAttributes(typeof (ActionMethodSelectorAttribute), true), (Converter<ActionMethodSelectorAttribute, ActionSelector>) (attr => (ActionSelector) (controllerContext => attr.IsValidForRequest(controllerContext, methodInfo))));
    }

    public static bool IsDefined(MemberInfo methodInfo, Type attributeType, bool inherit)
    {
      return methodInfo.IsDefined(attributeType, inherit);
    }

    public static object[] GetCustomAttributes(MemberInfo methodInfo, bool inherit)
    {
      return methodInfo.GetCustomAttributes(inherit);
    }

    public static object[] GetCustomAttributes(
      MemberInfo methodInfo,
      Type attributeType,
      bool inherit)
    {
      return methodInfo.GetCustomAttributes(attributeType, inherit);
    }

    public static ParameterDescriptor[] GetParameters(
      ActionDescriptor actionDescriptor,
      MethodInfo methodInfo,
      ref ParameterDescriptor[] parametersCache)
    {
      return (ParameterDescriptor[]) ActionDescriptorHelper.LazilyFetchParametersCollection(actionDescriptor, methodInfo, ref parametersCache).Clone();
    }

    private static ParameterDescriptor[] LazilyFetchParametersCollection(
      ActionDescriptor actionDescriptor,
      MethodInfo methodInfo,
      ref ParameterDescriptor[] parametersCache)
    {
      return DescriptorUtil.LazilyFetchOrCreateDescriptors<ParameterInfo, ParameterDescriptor>(ref parametersCache, new Func<ParameterInfo[]>(((MethodBase) methodInfo).GetParameters), (Func<ParameterInfo, ParameterDescriptor>) (parameterInfo => (ParameterDescriptor) new ReflectedParameterDescriptor(parameterInfo, actionDescriptor)));
    }
  }
}
