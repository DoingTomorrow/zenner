// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.DynamicHttpApplicationState
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Dynamic;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.WebPages
{
  internal class DynamicHttpApplicationState : DynamicObject
  {
    private HttpApplicationStateBase _state;

    public DynamicHttpApplicationState(HttpApplicationStateBase state) => this._state = state;

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
      result = this._state[binder.Name];
      return true;
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
      this._state[binder.Name] = value;
      return true;
    }

    public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
    {
      if (indexes == null || indexes.Length != 1)
        throw new ArgumentException(WebPageResources.DynamicDictionary_InvalidNumberOfIndexes);
      result = (object) null;
      if (indexes[0] is string index)
        result = this._state[index];
      else
        result = indexes[0] is int ? this._state[(int) indexes[0]] : throw new ArgumentException(WebPageResources.DynamicHttpApplicationState_UseOnlyStringOrIntToGet);
      return true;
    }

    public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
    {
      if (indexes == null || indexes.Length != 1)
        throw new ArgumentException(WebPageResources.DynamicDictionary_InvalidNumberOfIndexes);
      if (!(indexes[0] is string index))
        throw new ArgumentException(WebPageResources.DynamicHttpApplicationState_UseOnlyStringToSet);
      this._state[index] = value;
      return true;
    }
  }
}
