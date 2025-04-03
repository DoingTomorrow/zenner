// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.DictionaryValidateGroup
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class DictionaryValidateGroup : 
    IDictionaryValidate,
    IDataErrorInfo,
    INotifyPropertyChanged,
    IDisposable
  {
    private readonly object[] groups;
    private readonly IDictionaryAdapter adapter;
    private readonly string[] propertyNames;
    private readonly PropertyChangedEventHandler propertyChanged;

    public DictionaryValidateGroup(object[] groups, IDictionaryAdapter adapter)
    {
      this.groups = groups;
      this.adapter = adapter;
      this.propertyNames = this.adapter.This.Properties.Values.SelectMany((Func<PropertyDescriptor, IEnumerable<GroupAttribute>>) (property => property.Behaviors.OfType<GroupAttribute>()), (property, groupings) => new
      {
        property = property,
        groupings = groupings
      }).Where(_param1 => ((IEnumerable<object>) this.groups).Intersect<object>((IEnumerable<object>) _param1.groupings.Group).Any<object>()).Select(_param0 => _param0.property.PropertyName).Distinct<string>().ToArray<string>();
      if (this.propertyNames.Length <= 0 || !adapter.CanNotify)
        return;
      this.propertyChanged += (PropertyChangedEventHandler) ((sender, args) =>
      {
        if (this.PropertyChanged == null)
          return;
        this.PropertyChanged((object) this, args);
      });
      this.adapter.PropertyChanged += this.propertyChanged;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public bool CanValidate
    {
      get => this.adapter.CanValidate;
      set => this.adapter.CanValidate = value;
    }

    public bool IsValid => string.IsNullOrEmpty(this.Error);

    public string Error
    {
      get
      {
        return string.Join(Environment.NewLine, ((IEnumerable<string>) this.propertyNames).Select<string, string>((Func<string, string>) (propertyName => this.adapter[propertyName])).Where<string>((Func<string, bool>) (errors => !string.IsNullOrEmpty(errors))).ToArray<string>());
      }
    }

    public string this[string columnName]
    {
      get
      {
        return Array.IndexOf<string>(this.propertyNames, columnName) >= 0 ? this.adapter[columnName] : string.Empty;
      }
    }

    public DictionaryValidateGroup ValidateGroups(params object[] groups)
    {
      groups = ((IEnumerable<object>) this.groups).Union<object>((IEnumerable<object>) groups).ToArray<object>();
      return new DictionaryValidateGroup(groups, this.adapter);
    }

    public IEnumerable<IDictionaryValidator> Validators => this.adapter.Validators;

    public void AddValidator(IDictionaryValidator validator) => throw new NotSupportedException();

    public void Dispose() => this.adapter.PropertyChanged -= this.propertyChanged;
  }
}
