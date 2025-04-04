// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Utils.DynamicClass
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using System.Reflection;
using System.Text;

#nullable disable
namespace MSS.PartialSyncData.Utils
{
  public abstract class DynamicClass
  {
    public override string ToString()
    {
      PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("{");
      for (int index = 0; index < properties.Length; ++index)
      {
        if (index > 0)
          stringBuilder.Append(", ");
        stringBuilder.Append(properties[index].Name);
        stringBuilder.Append("=");
        stringBuilder.Append(properties[index].GetValue((object) this, (object[]) null));
      }
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }
  }
}
