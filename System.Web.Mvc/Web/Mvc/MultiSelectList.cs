// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.MultiSelectList
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;

#nullable disable
namespace System.Web.Mvc
{
  public class MultiSelectList : IEnumerable<SelectListItem>, IEnumerable
  {
    public MultiSelectList(IEnumerable items)
      : this(items, (IEnumerable) null)
    {
    }

    public MultiSelectList(IEnumerable items, IEnumerable selectedValues)
      : this(items, (string) null, (string) null, selectedValues)
    {
    }

    public MultiSelectList(IEnumerable items, string dataValueField, string dataTextField)
      : this(items, dataValueField, dataTextField, (IEnumerable) null)
    {
    }

    public MultiSelectList(
      IEnumerable items,
      string dataValueField,
      string dataTextField,
      IEnumerable selectedValues)
    {
      this.Items = items != null ? items : throw new ArgumentNullException(nameof (items));
      this.DataValueField = dataValueField;
      this.DataTextField = dataTextField;
      this.SelectedValues = selectedValues;
    }

    public string DataTextField { get; private set; }

    public string DataValueField { get; private set; }

    public IEnumerable Items { get; private set; }

    public IEnumerable SelectedValues { get; private set; }

    public virtual IEnumerator<SelectListItem> GetEnumerator()
    {
      return this.GetListItems().GetEnumerator();
    }

    internal IList<SelectListItem> GetListItems()
    {
      return string.IsNullOrEmpty(this.DataValueField) ? this.GetListItemsWithoutValueField() : this.GetListItemsWithValueField();
    }

    private IList<SelectListItem> GetListItemsWithValueField()
    {
      HashSet<string> selectedValues = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      if (this.SelectedValues != null)
        selectedValues.UnionWith(this.SelectedValues.Cast<object>().Select<object, string>((Func<object, string>) (value => Convert.ToString(value, (IFormatProvider) CultureInfo.CurrentCulture))));
      return (IList<SelectListItem>) this.Items.Cast<object>().Select(item => new
      {
        item = item,
        value = MultiSelectList.Eval(item, this.DataValueField)
      }).Select(_param1 => new SelectListItem()
      {
        Value = _param1.value,
        Text = MultiSelectList.Eval(_param1.item, this.DataTextField),
        Selected = selectedValues.Contains(_param1.value)
      }).ToList<SelectListItem>();
    }

    private IList<SelectListItem> GetListItemsWithoutValueField()
    {
      HashSet<object> selectedValues = new HashSet<object>();
      if (this.SelectedValues != null)
        selectedValues.UnionWith(this.SelectedValues.Cast<object>());
      return (IList<SelectListItem>) this.Items.Cast<object>().Select<object, SelectListItem>((Func<object, SelectListItem>) (item => new SelectListItem()
      {
        Text = MultiSelectList.Eval(item, this.DataTextField),
        Selected = selectedValues.Contains(item)
      })).ToList<SelectListItem>();
    }

    private static string Eval(object container, string expression)
    {
      object obj = container;
      if (!string.IsNullOrEmpty(expression))
        obj = DataBinder.Eval(container, expression);
      return Convert.ToString(obj, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
