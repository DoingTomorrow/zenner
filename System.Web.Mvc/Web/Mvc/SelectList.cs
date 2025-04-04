// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.SelectList
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;

#nullable disable
namespace System.Web.Mvc
{
  public class SelectList : MultiSelectList
  {
    public SelectList(IEnumerable items)
      : this(items, (object) null)
    {
    }

    public SelectList(IEnumerable items, object selectedValue)
      : this(items, (string) null, (string) null, selectedValue)
    {
    }

    public SelectList(IEnumerable items, string dataValueField, string dataTextField)
      : this(items, dataValueField, dataTextField, (object) null)
    {
    }

    public SelectList(
      IEnumerable items,
      string dataValueField,
      string dataTextField,
      object selectedValue)
      : base(items, dataValueField, dataTextField, SelectList.ToEnumerable(selectedValue))
    {
      this.SelectedValue = selectedValue;
    }

    public object SelectedValue { get; private set; }

    private static IEnumerable ToEnumerable(object selectedValue)
    {
      if (selectedValue == null)
        return (IEnumerable) null;
      return (IEnumerable) new object[1]{ selectedValue };
    }
  }
}
