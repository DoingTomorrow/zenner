// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.Formulas.ExcelDataValidationFormulaList
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.DataValidation.Formulas.Contracts;
using OfficeOpenXml.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.DataValidation.Formulas
{
  internal class ExcelDataValidationFormulaList : 
    ExcelDataValidationFormula,
    IExcelDataValidationFormulaList,
    IExcelDataValidationFormula
  {
    private string _formulaPath;

    public ExcelDataValidationFormulaList(
      XmlNamespaceManager namespaceManager,
      XmlNode itemNode,
      string formulaPath)
      : base(namespaceManager, itemNode, formulaPath)
    {
      Require.Argument<string>(formulaPath).IsNotNullOrEmpty(nameof (formulaPath));
      this._formulaPath = formulaPath;
      ExcelDataValidationFormulaList.DataValidationList dataValidationList = new ExcelDataValidationFormulaList.DataValidationList();
      dataValidationList.ListChanged += new EventHandler<EventArgs>(this.values_ListChanged);
      this.Values = (IList<string>) dataValidationList;
      this.SetInitialValues();
    }

    private void SetInitialValues()
    {
      string xmlNodeString = this.GetXmlNodeString(this._formulaPath);
      if (string.IsNullOrEmpty(xmlNodeString))
        return;
      if (xmlNodeString.StartsWith("\"") && xmlNodeString.EndsWith("\""))
      {
        string str1 = xmlNodeString.TrimStart('"').TrimEnd('"');
        char[] separator = new char[1]{ ',' };
        foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
          this.Values.Add(str2);
      }
      else
        this.ExcelFormula = xmlNodeString;
    }

    private void values_ListChanged(object sender, EventArgs e)
    {
      if (this.Values.Count > 0)
        this.State = FormulaState.Value;
      this.SetXmlNodeString(this._formulaPath, this.GetValueAsString());
    }

    public IList<string> Values { get; private set; }

    protected override string GetValueAsString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string str in (IEnumerable<string>) this.Values)
      {
        if (stringBuilder.Length == 0)
        {
          stringBuilder.Append("\"");
          stringBuilder.Append(str);
        }
        else
          stringBuilder.AppendFormat(", {0}", (object) str);
      }
      stringBuilder.Append("\"");
      return stringBuilder.ToString();
    }

    internal override void ResetValue() => this.Values.Clear();

    private class DataValidationList : 
      IList<string>,
      ICollection<string>,
      IEnumerable<string>,
      ICollection,
      IEnumerable
    {
      private IList<string> _items = (IList<string>) new List<string>();
      private EventHandler<EventArgs> _listChanged;

      public event EventHandler<EventArgs> ListChanged
      {
        add => this._listChanged += value;
        remove => this._listChanged -= value;
      }

      private void OnListChanged()
      {
        if (this._listChanged == null)
          return;
        this._listChanged((object) this, EventArgs.Empty);
      }

      int IList<string>.IndexOf(string item) => this._items.IndexOf(item);

      void IList<string>.Insert(int index, string item)
      {
        this._items.Insert(index, item);
        this.OnListChanged();
      }

      void IList<string>.RemoveAt(int index)
      {
        this._items.RemoveAt(index);
        this.OnListChanged();
      }

      string IList<string>.this[int index]
      {
        get => this._items[index];
        set
        {
          this._items[index] = value;
          this.OnListChanged();
        }
      }

      void ICollection<string>.Add(string item)
      {
        this._items.Add(item);
        this.OnListChanged();
      }

      void ICollection<string>.Clear()
      {
        this._items.Clear();
        this.OnListChanged();
      }

      bool ICollection<string>.Contains(string item) => this._items.Contains(item);

      void ICollection<string>.CopyTo(string[] array, int arrayIndex)
      {
        this._items.CopyTo(array, arrayIndex);
      }

      int ICollection<string>.Count => this._items.Count;

      bool ICollection<string>.IsReadOnly => false;

      bool ICollection<string>.Remove(string item)
      {
        bool flag = this._items.Remove(item);
        this.OnListChanged();
        return flag;
      }

      IEnumerator<string> IEnumerable<string>.GetEnumerator() => this._items.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._items.GetEnumerator();

      public void CopyTo(Array array, int index) => this._items.CopyTo((string[]) array, index);

      int ICollection.Count => this._items.Count;

      public bool IsSynchronized => ((ICollection) this._items).IsSynchronized;

      public object SyncRoot => ((ICollection) this._items).SyncRoot;
    }
  }
}
