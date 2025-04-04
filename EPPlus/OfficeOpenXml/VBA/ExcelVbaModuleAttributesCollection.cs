// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.VBA.ExcelVbaModuleAttributesCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Text;

#nullable disable
namespace OfficeOpenXml.VBA
{
  public class ExcelVbaModuleAttributesCollection : ExcelVBACollectionBase<ExcelVbaModuleAttribute>
  {
    internal string GetAttributeText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (ExcelVbaModuleAttribute vbaModuleAttribute in (ExcelVBACollectionBase<ExcelVbaModuleAttribute>) this)
        stringBuilder.AppendFormat("Attribute {0} = {1}\r\n", (object) vbaModuleAttribute.Name, vbaModuleAttribute.DataType == eAttributeDataType.String ? (object) ("\"" + vbaModuleAttribute.Value + "\"") : (object) vbaModuleAttribute.Value);
      return stringBuilder.ToString();
    }
  }
}
