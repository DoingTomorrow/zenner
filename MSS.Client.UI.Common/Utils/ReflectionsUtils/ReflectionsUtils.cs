// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.Utils.ReflectionsUtils.ReflectionsUtils
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using System;
using System.Reflection;

#nullable disable
namespace MSS.Client.UI.Common.Utils.ReflectionsUtils
{
  public static class ReflectionsUtils
  {
    public static void SetPrivateFieldValue<T>(this object obj, string propName, T val)
    {
      Type type = obj != null ? obj.GetType() : throw new ArgumentNullException(nameof (obj));
      FieldInfo fieldInfo;
      for (fieldInfo = (FieldInfo) null; fieldInfo == (FieldInfo) null && type != (Type) null; type = type.BaseType)
        fieldInfo = type.GetField(propName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (fieldInfo == (FieldInfo) null)
        throw new ArgumentOutOfRangeException(nameof (propName), string.Format("Field {0} was not found in Type {1}", (object) propName, (object) obj.GetType().FullName));
      fieldInfo.SetValue(obj, (object) val);
    }
  }
}
