// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.DynamicViewDataDictionary
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Dynamic;

#nullable disable
namespace System.Web.Mvc
{
  internal sealed class DynamicViewDataDictionary : DynamicObject
  {
    private readonly Func<ViewDataDictionary> _viewDataThunk;

    public DynamicViewDataDictionary(Func<ViewDataDictionary> viewDataThunk)
    {
      this._viewDataThunk = viewDataThunk;
    }

    private ViewDataDictionary ViewData => this._viewDataThunk();

    public override IEnumerable<string> GetDynamicMemberNames()
    {
      return (IEnumerable<string>) this.ViewData.Keys;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
      result = this.ViewData[binder.Name];
      return true;
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
      this.ViewData[binder.Name] = value;
      return true;
    }
  }
}
