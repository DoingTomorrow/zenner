// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Utilities.SeparatedStringBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Remotion.Linq.Utilities
{
  public static class SeparatedStringBuilder
  {
    public static string Build<T>(string separator, IEnumerable<T> sequence)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      foreach (T obj in sequence)
      {
        if (!flag)
          stringBuilder.Append(separator);
        stringBuilder.Append((object) obj);
        flag = false;
      }
      return stringBuilder.ToString();
    }
  }
}
