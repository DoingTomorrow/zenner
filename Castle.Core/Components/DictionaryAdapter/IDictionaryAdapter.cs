// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.IDictionaryAdapter
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public interface IDictionaryAdapter : 
    IDictionaryEdit,
    IEditableObject,
    IRevertibleChangeTracking,
    IChangeTracking,
    IDictionaryNotify,
    INotifyPropertyChanging,
    IDictionaryValidate,
    IDataErrorInfo,
    INotifyPropertyChanged,
    IDictionaryCreate
  {
    DictionaryAdapterMeta Meta { get; }

    DictionaryAdapterInstance This { get; }

    string GetKey(string propertyName);

    object GetProperty(string propertyName, bool ifExists);

    object ReadProperty(string key);

    T GetPropertyOfType<T>(string propertyName);

    bool SetProperty(string propertyName, ref object value);

    void StoreProperty(PropertyDescriptor property, string key, object value);

    void ClearProperty(PropertyDescriptor property, string key);

    void CopyTo(IDictionaryAdapter other);

    void CopyTo(IDictionaryAdapter other, Predicate<PropertyDescriptor> selector);

    T Coerce<T>() where T : class;
  }
}
