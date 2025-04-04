// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelProtectedRangeCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelProtectedRangeCollection : 
    XmlHelper,
    IEnumerable<ExcelProtectedRange>,
    IEnumerable
  {
    private List<ExcelProtectedRange> _baseList = new List<ExcelProtectedRange>();

    internal ExcelProtectedRangeCollection(
      XmlNamespaceManager nsm,
      XmlNode topNode,
      ExcelWorksheet ws)
      : base(nsm, topNode)
    {
      foreach (XmlNode selectNode in topNode.SelectNodes("d:protectedRanges/d:protectedRange", nsm))
      {
        if (selectNode is XmlElement)
          this._baseList.Add(new ExcelProtectedRange(selectNode.Attributes["name"].Value, new ExcelAddress(SqRefUtility.FromSqRefAddress(selectNode.Attributes["sqref"].Value)), nsm, topNode));
      }
    }

    public ExcelProtectedRange Add(string name, ExcelAddress address)
    {
      if (!this.ExistNode("d:protectedRanges"))
        this.CreateNode("d:protectedRanges");
      XmlNode node = this.CreateNode("d:protectedRanges/d:protectedRange");
      ExcelProtectedRange excelProtectedRange = new ExcelProtectedRange(name, address, this.NameSpaceManager, node);
      this._baseList.Add(excelProtectedRange);
      return excelProtectedRange;
    }

    public void Clear()
    {
      this.DeleteNode("d:protectedRanges");
      this._baseList.Clear();
    }

    public bool Contains(ExcelProtectedRange item) => this._baseList.Contains(item);

    public void CopyTo(ExcelProtectedRange[] array, int arrayIndex)
    {
      this._baseList.CopyTo(array, arrayIndex);
    }

    public int Count => this._baseList.Count;

    public bool Remove(ExcelProtectedRange item)
    {
      this.DeleteAllNode("d:protectedRanges/d:protectedRange[@name='" + item.Name + "' and @sqref='" + item.Address.Address + "']");
      if (this._baseList.Count == 0)
        this.DeleteNode("d:protectedRanges");
      return this._baseList.Remove(item);
    }

    public int IndexOf(ExcelProtectedRange item) => this._baseList.IndexOf(item);

    public void RemoveAt(int index) => this._baseList.RemoveAt(index);

    public ExcelProtectedRange this[int index] => this._baseList[index];

    IEnumerator<ExcelProtectedRange> IEnumerable<ExcelProtectedRange>.GetEnumerator()
    {
      return (IEnumerator<ExcelProtectedRange>) this._baseList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._baseList.GetEnumerator();
  }
}
