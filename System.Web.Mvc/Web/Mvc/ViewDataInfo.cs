// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewDataInfo
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.ComponentModel;

#nullable disable
namespace System.Web.Mvc
{
  public class ViewDataInfo
  {
    private object _value;
    private Func<object> _valueAccessor;

    public ViewDataInfo()
    {
    }

    public ViewDataInfo(Func<object> valueAccessor) => this._valueAccessor = valueAccessor;

    public object Container { get; set; }

    public PropertyDescriptor PropertyDescriptor { get; set; }

    public object Value
    {
      get
      {
        if (this._valueAccessor != null)
        {
          this._value = this._valueAccessor();
          this._valueAccessor = (Func<object>) null;
        }
        return this._value;
      }
      set
      {
        this._value = value;
        this._valueAccessor = (Func<object>) null;
      }
    }
  }
}
