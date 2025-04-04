// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.DescriptorUtil
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;

#nullable disable
namespace System.Web.Mvc
{
  internal static class DescriptorUtil
  {
    private static void AppendPartToUniqueIdBuilder(StringBuilder builder, object part)
    {
      if (part == null)
      {
        builder.Append("[-1]");
      }
      else
      {
        string str = Convert.ToString(part, (IFormatProvider) CultureInfo.InvariantCulture);
        builder.AppendFormat("[{0}]{1}", (object) str.Length, (object) str);
      }
    }

    public static string CreateUniqueId(params object[] parts)
    {
      return DescriptorUtil.CreateUniqueId((IEnumerable<object>) parts);
    }

    public static string CreateUniqueId(IEnumerable<object> parts)
    {
      StringBuilder builder = new StringBuilder();
      foreach (object part in parts)
      {
        MemberInfo memberInfo = part as MemberInfo;
        if (memberInfo != (MemberInfo) null)
        {
          DescriptorUtil.AppendPartToUniqueIdBuilder(builder, (object) memberInfo.Module.ModuleVersionId);
          DescriptorUtil.AppendPartToUniqueIdBuilder(builder, (object) memberInfo.MetadataToken);
        }
        else if (part is IUniquelyIdentifiable uniquelyIdentifiable)
          DescriptorUtil.AppendPartToUniqueIdBuilder(builder, (object) uniquelyIdentifiable.UniqueId);
        else
          DescriptorUtil.AppendPartToUniqueIdBuilder(builder, part);
      }
      return builder.ToString();
    }

    public static TDescriptor[] LazilyFetchOrCreateDescriptors<TReflection, TDescriptor>(
      ref TDescriptor[] cacheLocation,
      Func<TReflection[]> initializer,
      Func<TReflection, TDescriptor> converter)
    {
      TDescriptor[] descriptors = Interlocked.CompareExchange<TDescriptor[]>(ref cacheLocation, (TDescriptor[]) null, (TDescriptor[]) null);
      if (descriptors != null)
        return descriptors;
      TReflection[] reflectionArray = initializer();
      List<TDescriptor> descriptorList = new List<TDescriptor>(reflectionArray.Length);
      for (int index = 0; index < reflectionArray.Length; ++index)
      {
        TDescriptor descriptor = converter(reflectionArray[index]);
        if ((object) descriptor != null)
          descriptorList.Add(descriptor);
      }
      TDescriptor[] array = descriptorList.ToArray();
      return Interlocked.CompareExchange<TDescriptor[]>(ref cacheLocation, array, (TDescriptor[]) null) ?? array;
    }
  }
}
