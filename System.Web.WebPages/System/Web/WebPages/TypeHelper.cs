// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.TypeHelper
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Routing;

#nullable disable
namespace System.Web.WebPages
{
  internal static class TypeHelper
  {
    internal static IDictionary<string, object> ObjectToDictionary(object value)
    {
      return (IDictionary<string, object>) new RouteValueDictionary(value);
    }

    internal static void AddAnonymousObjectToDictionary(
      IDictionary<string, object> dictionary,
      object value)
    {
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) TypeHelper.ObjectToDictionary(value))
        dictionary.Add(keyValuePair);
    }

    internal static bool IsAnonymousType(Type type)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (!Attribute.IsDefined((MemberInfo) type, typeof (CompilerGeneratedAttribute), false) || !type.IsGenericType || !type.Name.Contains("AnonymousType") || !type.Name.StartsWith("<>", StringComparison.OrdinalIgnoreCase) && !type.Name.StartsWith("VB$", StringComparison.OrdinalIgnoreCase))
        return false;
      int attributes = (int) type.Attributes;
      return true;
    }
  }
}
