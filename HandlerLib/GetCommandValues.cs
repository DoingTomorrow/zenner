// Decompiled with JetBrains decompiler
// Type: HandlerLib.GetCommandValues
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace HandlerLib
{
  public static class GetCommandValues
  {
    public static Dictionary<string, string> GetAllPrivateStaticFieldValuesForCommands(object obj)
    {
      return ((IEnumerable<FieldInfo>) obj.GetType().GetFields(BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.NonPublic)).Where<FieldInfo>((Func<FieldInfo, bool>) (f => f.FieldType == typeof (string) && f.Name.Contains("CMD_"))).ToDictionary<FieldInfo, string, string>((Func<FieldInfo, string>) (f => f.Name), (Func<FieldInfo, string>) (f => (string) f.GetValue((object) null)));
    }
  }
}
