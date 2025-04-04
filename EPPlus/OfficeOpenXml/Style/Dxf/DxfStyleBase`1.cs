// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.Dxf.DxfStyleBase`1
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.Style.Dxf
{
  public abstract class DxfStyleBase<T>
  {
    protected ExcelStyles _styles;

    internal DxfStyleBase(ExcelStyles styles)
    {
      this._styles = styles;
      this.AllowChange = false;
    }

    protected internal abstract string Id { get; }

    protected internal abstract bool HasValue { get; }

    protected internal abstract void CreateNodes(XmlHelper helper, string path);

    protected internal abstract T Clone();

    protected void SetValueColor(XmlHelper helper, string path, ExcelDxfColor color)
    {
      if (color == null || !color.HasValue)
        return;
      if (color.Color.HasValue)
        this.SetValue(helper, path + "/@rgb", (object) color.Color.Value.ToArgb().ToString("x"));
      else if (color.Auto.HasValue)
        this.SetValueBool(helper, path + "/@auto", color.Auto);
      else if (color.Theme.HasValue)
        this.SetValue(helper, path + "/@theme", (object) color.Theme);
      else if (color.Index.HasValue)
        this.SetValue(helper, path + "/@indexed", (object) color.Index);
      if (!color.Tint.HasValue)
        return;
      this.SetValue(helper, path + "/@tint", (object) color.Tint);
    }

    protected void SetValueEnum(XmlHelper helper, string path, Enum v)
    {
      if (v == null)
      {
        helper.DeleteNode(path);
      }
      else
      {
        string str1 = v.ToString();
        string str2 = str1.Substring(0, 1).ToLower() + str1.Substring(1);
        helper.SetXmlNodeString(path, str2);
      }
    }

    protected void SetValue(XmlHelper helper, string path, object v)
    {
      if (v == null)
        helper.DeleteNode(path);
      else
        helper.SetXmlNodeString(path, v.ToString());
    }

    protected void SetValueBool(XmlHelper helper, string path, bool? v)
    {
      if (!v.HasValue)
        helper.DeleteNode(path);
      else
        helper.SetXmlNodeBool(path, v.Value);
    }

    protected internal string GetAsString(object v) => (v ?? (object) "").ToString();

    protected internal bool AllowChange { get; set; }
  }
}
