// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.StringUtilities
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace InTheHand.Net
{
  internal static class StringUtilities
  {
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static bool IsNullOrEmpty(string value) => string.IsNullOrEmpty(value);

    public static string String_Join<T>(IList<T> objects)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (T obj in (IEnumerable<T>) objects)
      {
        stringBuilder.Append(obj.ToString());
        stringBuilder.Append(", ");
      }
      if (stringBuilder.Length > 0)
        stringBuilder.Length -= ", ".Length;
      return stringBuilder.ToString();
    }
  }
}
