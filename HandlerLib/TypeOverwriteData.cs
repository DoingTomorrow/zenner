// Decompiled with JetBrains decompiler
// Type: HandlerLib.TypeOverwriteData
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System.Text;

#nullable disable
namespace HandlerLib
{
  public class TypeOverwriteData
  {
    public int MeterInfoID;
    public OverwriteConditions Condition;
    public CommonOverwriteGroups[] OverwriteGroups;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.MeterInfoID);
      stringBuilder.Append(" Cond:" + this.Condition.ToString());
      stringBuilder.Append(" Groups:");
      if (this.OverwriteGroups != null)
      {
        for (int index = 0; index < this.OverwriteGroups.Length; ++index)
        {
          if (index > 0)
            stringBuilder.Append(",");
          stringBuilder.Append(this.OverwriteGroups[index].ToString());
        }
      }
      return stringBuilder.ToString();
    }
  }
}
