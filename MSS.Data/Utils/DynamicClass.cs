// Decompiled with JetBrains decompiler
// Type: MSS.Data.Utils.DynamicClass
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using System.Reflection;
using System.Text;

#nullable disable
namespace MSS.Data.Utils
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
