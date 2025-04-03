// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.DictionaryAdapterBase
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public abstract class DictionaryAdapterBase : 
    IDictionaryAdapter,
    IDictionaryEdit,
    IEditableObject,
    IRevertibleChangeTracking,
    IChangeTracking,
    IDictionaryNotify,
    INotifyPropertyChanging,
    IDictionaryCreate,
    IDictionaryValidate,
    IDataErrorInfo,
    INotifyPropertyChanged
  {
    private int suppressEditingCount;
    private Stack<Dictionary<string, DictionaryAdapterBase.Edit>> updates;
    private HashSet<IEditableObject> editDependencies;
    private int suppressNotificationCount;
    private bool propagateChildNotifications = true;
    private Dictionary<object, object> composedChildNotifications;
    [ThreadStatic]
    private static DictionaryAdapterBase.TrackPropertyChangeScope readonlyTrackingScope;
    private ICollection<IDictionaryValidator> validators;

    public T Create<T>() => this.Create<T>((IDictionary) new HybridDictionary());

    public object Create(Type type) => this.Create(type, (IDictionary) new HybridDictionary());

    public T Create<T>(IDictionary dictionary)
    {
      return (T) this.Create(typeof (T), dictionary ?? (IDictionary) new HybridDictionary());
    }

    public object Create(Type type, IDictionary dictionary)
    {
      if (this.This.CreateStrategy != null)
        return this.This.CreateStrategy.Create((IDictionaryAdapter) this, type, dictionary);
      dictionary = dictionary ?? (IDictionary) new HybridDictionary();
      return this.This.Factory.GetAdapter(type, dictionary, this.This.Descriptor);
    }

    public T Create<T>(Action<T> init)
    {
      return this.Create<T>((IDictionary) new HybridDictionary(), init);
    }

    public T Create<T>(IDictionary dictionary, Action<T> init)
    {
      T obj = this.Create<T>(dictionary ?? (IDictionary) new HybridDictionary());
      if (init != null)
        init(obj);
      return obj;
    }

    public DictionaryAdapterBase(DictionaryAdapterInstance instance)
    {
      this.This = instance;
      this.CanEdit = typeof (IEditableObject).IsAssignableFrom(this.Meta.Type);
      this.CanNotify = typeof (INotifyPropertyChanged).IsAssignableFrom(this.Meta.Type);
      this.CanValidate = typeof (IDataErrorInfo).IsAssignableFrom(this.Meta.Type);
      this.Initialize();
    }

    public abstract DictionaryAdapterMeta Meta { get; }

    public DictionaryAdapterInstance This { get; private set; }

    public string GetKey(string propertyName)
    {
      PropertyDescriptor propertyDescriptor;
      return this.This.Properties.TryGetValue(propertyName, out propertyDescriptor) ? propertyDescriptor.GetKey((IDictionaryAdapter) this, propertyName, this.This.Descriptor) : (string) null;
    }

    public virtual object GetProperty(string propertyName, bool ifExists)
    {
      PropertyDescriptor property;
      if (!this.This.Properties.TryGetValue(propertyName, out property))
        return (object) null;
      object propertyValue = property.GetPropertyValue((IDictionaryAdapter) this, propertyName, (object) null, this.This.Descriptor, ifExists);
      if (propertyValue is IEditableObject)
        this.AddEditDependency((IEditableObject) propertyValue);
      this.ComposeChildNotifications(property, (object) null, propertyValue);
      return propertyValue;
    }

    public T GetPropertyOfType<T>(string propertyName)
    {
      object property = this.GetProperty(propertyName, false);
      return property == null ? default (T) : (T) property;
    }

    public object ReadProperty(string key)
    {
      object propertyValue = (object) null;
      if (!this.GetEditedProperty(key, out propertyValue))
      {
        IDictionary dictionary = DictionaryAdapterBase.GetDictionary(this.This.Dictionary, ref key);
        if (dictionary != null)
          propertyValue = dictionary[(object) key];
      }
      return propertyValue;
    }

    public virtual bool SetProperty(string propertyName, ref object value)
    {
      bool flag1 = false;
      PropertyDescriptor property1;
      if (this.This.Properties.TryGetValue(propertyName, out property1))
      {
        if (!this.ShouldNotify)
        {
          bool flag2 = property1.SetPropertyValue((IDictionaryAdapter) this, propertyName, ref value, this.This.Descriptor);
          this.Invalidate();
          return flag2;
        }
        object property2 = this.GetProperty(propertyName, true);
        if (!this.NotifyPropertyChanging(property1, property2, value))
          return false;
        DictionaryAdapterBase.TrackPropertyChangeScope propertyChangeScope = this.TrackPropertyChange(property1, property2, value);
        flag1 = property1.SetPropertyValue((IDictionaryAdapter) this, propertyName, ref value, this.This.Descriptor);
        if (flag1 && propertyChangeScope != null)
          propertyChangeScope.Notify();
      }
      return flag1;
    }

    public void StoreProperty(PropertyDescriptor property, string key, object value)
    {
      if (property != null && this.EditProperty(property, key, value))
        return;
      IDictionary dictionary = DictionaryAdapterBase.GetDictionary(this.This.Dictionary, ref key);
      if (dictionary == null)
        return;
      dictionary[(object) key] = value;
    }

    public void ClearProperty(PropertyDescriptor property, string key)
    {
      if (property != null && this.ClearEditProperty(property, key))
        return;
      DictionaryAdapterBase.GetDictionary(this.This.Dictionary, ref key)?.Remove((object) key);
    }

    public void CopyTo(IDictionaryAdapter other)
    {
      this.CopyTo(other, (Predicate<PropertyDescriptor>) null);
    }

    public void CopyTo(IDictionaryAdapter other, Predicate<PropertyDescriptor> selector)
    {
      if (!this.Meta.Type.IsAssignableFrom(other.Meta.Type))
        throw new ArgumentException(string.Format("Unable to copy to {0}.  Type must be assignable from {1}.", (object) other.Meta.Type.FullName, (object) this.Meta.Type.FullName));
      selector = selector ?? (Predicate<PropertyDescriptor>) (p => true);
      foreach (PropertyDescriptor propertyDescriptor in this.This.Properties.Values.Where<PropertyDescriptor>((Func<PropertyDescriptor, bool>) (p => selector(p))))
      {
        object property = this.GetProperty(propertyDescriptor.PropertyName, true);
        if (property != null)
          other.SetProperty(propertyDescriptor.PropertyName, ref property);
      }
    }

    public T Coerce<T>() where T : class
    {
      return (T) this.This.Factory.GetAdapter(typeof (T), this.This.Dictionary, this.This.Descriptor);
    }

    public override bool Equals(object obj)
    {
      if (!(obj is IDictionaryAdapter adapter2))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      if (this.Meta.Type != adapter2.Meta.Type)
        return false;
      return this.This.EqualityHashCodeStrategy != null ? this.This.EqualityHashCodeStrategy.Equals((IDictionaryAdapter) this, adapter2) : base.Equals(obj);
    }

    public override int GetHashCode()
    {
      if (this.This.OldHashCode.HasValue)
        return this.This.OldHashCode.Value;
      int hashCode;
      if (this.This.EqualityHashCodeStrategy == null || !this.This.EqualityHashCodeStrategy.GetHashCode((IDictionaryAdapter) this, out hashCode))
        hashCode = base.GetHashCode();
      this.This.OldHashCode = new int?(hashCode);
      return hashCode;
    }

    protected void Initialize()
    {
      foreach (IDictionaryInitializer initializer in this.This.Initializers)
        initializer.Initialize((IDictionaryAdapter) this, this.Meta.Behaviors);
      foreach (PropertyDescriptor propertyDescriptor in this.This.Properties.Values.Where<PropertyDescriptor>((Func<PropertyDescriptor, bool>) (p => p.Fetch)))
        this.GetProperty(propertyDescriptor.PropertyName, false);
    }

    private static IDictionary GetDictionary(IDictionary dictionary, ref string key)
    {
      if (!key.StartsWith("!"))
      {
        string[] strArray = key.Split(',');
        for (int index = 0; index < strArray.Length - 1; ++index)
        {
          dictionary = dictionary[(object) strArray[index]] as IDictionary;
          if (dictionary == null)
            return (IDictionary) null;
        }
        key = strArray[strArray.Length - 1];
      }
      return dictionary;
    }

    public bool CanEdit
    {
      get => this.suppressEditingCount == 0 && this.updates != null;
      set
      {
        this.updates = value ? new Stack<Dictionary<string, DictionaryAdapterBase.Edit>>() : (Stack<Dictionary<string, DictionaryAdapterBase.Edit>>) null;
      }
    }

    public bool IsEditing => this.CanEdit && this.updates != null && this.updates.Count > 0;

    public bool SupportsMultiLevelEdit { get; set; }

    public bool IsChanged
    {
      get
      {
        return this.IsEditing && this.updates.Any<Dictionary<string, DictionaryAdapterBase.Edit>>((Func<Dictionary<string, DictionaryAdapterBase.Edit>, bool>) (level => level.Count > 0)) || this.This.Properties.Values.Where<PropertyDescriptor>((Func<PropertyDescriptor, bool>) (prop => typeof (IChangeTracking).IsAssignableFrom(prop.PropertyType))).Select<PropertyDescriptor, object>((Func<PropertyDescriptor, object>) (prop => this.GetProperty(prop.PropertyName, true))).Cast<IChangeTracking>().Any<IChangeTracking>((Func<IChangeTracking, bool>) (track => track != null && track.IsChanged));
      }
    }

    public void BeginEdit()
    {
      if (!this.CanEdit || this.IsEditing && !this.SupportsMultiLevelEdit)
        return;
      this.updates.Push(new Dictionary<string, DictionaryAdapterBase.Edit>());
    }

    public void CancelEdit()
    {
      if (!this.IsEditing)
        return;
      if (this.editDependencies != null)
      {
        foreach (IEditableObject editableObject in this.editDependencies.ToArray<IEditableObject>())
          editableObject.CancelEdit();
        this.editDependencies.Clear();
      }
      using (this.SuppressEditingBlock())
      {
        using (this.TrackReadonlyPropertyChanges())
        {
          Dictionary<string, DictionaryAdapterBase.Edit> dictionary = this.updates.Peek();
          if (dictionary.Count <= 0)
            return;
          foreach (DictionaryAdapterBase.Edit edit in dictionary.Values)
            edit.PropertyValue = this.GetProperty(edit.Property.PropertyName, true);
          this.updates.Pop();
          foreach (DictionaryAdapterBase.Edit edit in dictionary.Values.ToArray<DictionaryAdapterBase.Edit>())
          {
            object propertyValue = edit.PropertyValue;
            object property = this.GetProperty(edit.Property.PropertyName, true);
            if (!object.Equals(propertyValue, property))
            {
              this.NotifyPropertyChanging(edit.Property, propertyValue, property);
              this.NotifyPropertyChanged(edit.Property, propertyValue, property);
            }
          }
        }
      }
    }

    public void EndEdit()
    {
      if (!this.IsEditing)
        return;
      using (this.SuppressEditingBlock())
      {
        Dictionary<string, DictionaryAdapterBase.Edit> source = this.updates.Pop();
        if (source.Count > 0)
        {
          foreach (KeyValuePair<string, DictionaryAdapterBase.Edit> keyValuePair in source.ToArray<KeyValuePair<string, DictionaryAdapterBase.Edit>>())
            this.StoreProperty((PropertyDescriptor) null, keyValuePair.Key, keyValuePair.Value.PropertyValue);
        }
      }
      if (this.editDependencies == null)
        return;
      foreach (IEditableObject editableObject in this.editDependencies.ToArray<IEditableObject>())
        editableObject.EndEdit();
      this.editDependencies.Clear();
    }

    public void RejectChanges() => this.CancelEdit();

    public void AcceptChanges() => this.EndEdit();

    public IDisposable SuppressEditingBlock()
    {
      return (IDisposable) new DictionaryAdapterBase.SuppressEditingScope(this);
    }

    public void SuppressEditing() => ++this.suppressEditingCount;

    public void ResumeEditing() => --this.suppressEditingCount;

    protected bool GetEditedProperty(string propertyName, out object propertyValue)
    {
      if (this.updates != null)
      {
        foreach (Dictionary<string, DictionaryAdapterBase.Edit> dictionary in this.updates.ToArray())
        {
          DictionaryAdapterBase.Edit edit;
          if (dictionary.TryGetValue(propertyName, out edit))
          {
            propertyValue = edit.PropertyValue;
            return true;
          }
        }
      }
      propertyValue = (object) null;
      return false;
    }

    protected bool EditProperty(PropertyDescriptor property, string key, object propertyValue)
    {
      if (!this.IsEditing)
        return false;
      this.updates.Peek()[key] = new DictionaryAdapterBase.Edit(property, propertyValue);
      return true;
    }

    protected bool ClearEditProperty(PropertyDescriptor property, string key)
    {
      if (!this.IsEditing)
        return false;
      this.updates.Peek().Remove(key);
      return true;
    }

    protected void AddEditDependency(IEditableObject editDependency)
    {
      if (!this.IsEditing)
        return;
      if (this.editDependencies == null)
        this.editDependencies = new HashSet<IEditableObject>();
      if (!this.editDependencies.Add(editDependency))
        return;
      editDependency.BeginEdit();
    }

    public event PropertyChangingEventHandler PropertyChanging;

    public event PropertyChangedEventHandler PropertyChanged;

    public bool CanNotify { get; set; }

    public bool ShouldNotify => this.CanNotify && this.suppressNotificationCount == 0;

    public bool PropagateChildNotifications
    {
      get => this.propagateChildNotifications;
      set => this.propagateChildNotifications = value;
    }

    public IDisposable SuppressNotificationsBlock()
    {
      return (IDisposable) new DictionaryAdapterBase.SuppressNotificationsScope(this);
    }

    public void SuppressNotifications() => ++this.suppressNotificationCount;

    public void ResumeNotifications() => --this.suppressNotificationCount;

    protected bool NotifyPropertyChanging(
      PropertyDescriptor property,
      object oldValue,
      object newValue)
    {
      if (!property.SuppressNotifications)
      {
        PropertyChangingEventHandler propertyChanging = this.PropertyChanging;
        if (propertyChanging != null)
        {
          PropertyModifyingEventArgs e = new PropertyModifyingEventArgs(property.PropertyName, oldValue, newValue);
          propertyChanging((object) this, (PropertyChangingEventArgs) e);
          return !e.Cancel;
        }
      }
      return true;
    }

    protected void NotifyPropertyChanged(
      PropertyDescriptor property,
      object oldValue,
      object newValue)
    {
      if (property.SuppressNotifications)
        return;
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      this.ComposeChildNotifications(property, oldValue, newValue);
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, (PropertyChangedEventArgs) new PropertyModifiedEventArgs(property.PropertyName, oldValue, newValue));
    }

    protected void NotifyPropertyChanged(string propertyName)
    {
      if (!this.ShouldNotify)
        return;
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    protected DictionaryAdapterBase.TrackPropertyChangeScope TrackPropertyChange(
      PropertyDescriptor property,
      object oldValue,
      object newValue)
    {
      return this.ShouldNotify && !property.SuppressNotifications ? new DictionaryAdapterBase.TrackPropertyChangeScope(this, property, oldValue) : (DictionaryAdapterBase.TrackPropertyChangeScope) null;
    }

    protected DictionaryAdapterBase.TrackPropertyChangeScope TrackReadonlyPropertyChanges()
    {
      if (!this.ShouldNotify || DictionaryAdapterBase.readonlyTrackingScope != null)
        return (DictionaryAdapterBase.TrackPropertyChangeScope) null;
      DictionaryAdapterBase.TrackPropertyChangeScope propertyChangeScope = new DictionaryAdapterBase.TrackPropertyChangeScope(this);
      DictionaryAdapterBase.readonlyTrackingScope = propertyChangeScope;
      return propertyChangeScope;
    }

    private void ComposeChildNotifications(
      PropertyDescriptor property,
      object oldValue,
      object newValue)
    {
      if (this.composedChildNotifications == null)
        this.composedChildNotifications = new Dictionary<object, object>();
      object obj;
      if (oldValue != null && this.composedChildNotifications.TryGetValue(oldValue, out obj))
      {
        this.composedChildNotifications.Remove(oldValue);
        if (oldValue is INotifyPropertyChanged)
        {
          ((INotifyPropertyChanged) oldValue).PropertyChanged -= new PropertyChangedEventHandler(this.Child_PropertyChanged);
          if (oldValue is INotifyPropertyChanging)
            ((INotifyPropertyChanging) oldValue).PropertyChanging -= new PropertyChangingEventHandler(this.Child_PropertyChanging);
        }
        else if (oldValue is IBindingList)
          ((IBindingList) oldValue).ListChanged -= (ListChangedEventHandler) obj;
      }
      if (newValue == null || this.composedChildNotifications.ContainsKey(newValue))
        return;
      switch (newValue)
      {
        case INotifyPropertyChanged _:
          ((INotifyPropertyChanged) newValue).PropertyChanged += new PropertyChangedEventHandler(this.Child_PropertyChanged);
          if (newValue is INotifyPropertyChanging)
            ((INotifyPropertyChanging) newValue).PropertyChanging += new PropertyChangingEventHandler(this.Child_PropertyChanging);
          this.composedChildNotifications.Add(newValue, (object) null);
          break;
        case IBindingList _:
          ListChangedEventHandler changedEventHandler = (ListChangedEventHandler) ((sender, args) =>
          {
            if (!this.propagateChildNotifications)
              return;
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged == null)
              return;
            if (args.PropertyDescriptor != null)
            {
              string name = args.PropertyDescriptor.Name;
              propertyChanged(sender, new PropertyChangedEventArgs(name));
            }
            propertyChanged((object) this, new PropertyChangedEventArgs(property.PropertyName));
          });
          ((IBindingList) newValue).ListChanged += changedEventHandler;
          this.composedChildNotifications.Add(newValue, (object) changedEventHandler);
          break;
      }
    }

    private void Child_PropertyChanging(object sender, PropertyChangingEventArgs e)
    {
      if (!this.propagateChildNotifications)
        return;
      PropertyChangingEventHandler propertyChanging = this.PropertyChanging;
      if (propertyChanging == null)
        return;
      propertyChanging(sender, e);
    }

    private void Child_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!this.propagateChildNotifications)
        return;
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged(sender, e);
    }

    public bool CanValidate { get; set; }

    public bool IsValid
    {
      get
      {
        return this.CanValidate && this.validators != null ? !this.validators.Any<IDictionaryValidator>((Func<IDictionaryValidator, bool>) (v => !v.IsValid((IDictionaryAdapter) this))) : !this.CanValidate;
      }
    }

    public string Error
    {
      get
      {
        return this.CanValidate && this.validators != null ? string.Join(Environment.NewLine, this.validators.Select<IDictionaryValidator, string>((Func<IDictionaryValidator, string>) (v => v.Validate((IDictionaryAdapter) this))).Where<string>((Func<string, bool>) (e => !string.IsNullOrEmpty(e))).ToArray<string>()) : string.Empty;
      }
    }

    public string this[string columnName]
    {
      get
      {
        if (this.CanValidate && this.validators != null)
        {
          PropertyDescriptor property;
          if (this.This.Properties.TryGetValue(columnName, out property))
            return string.Join(Environment.NewLine, this.validators.Select<IDictionaryValidator, string>((Func<IDictionaryValidator, string>) (v => v.Validate((IDictionaryAdapter) this, property))).Where<string>((Func<string, bool>) (e => !string.IsNullOrEmpty(e))).ToArray<string>());
        }
        return string.Empty;
      }
    }

    public DictionaryValidateGroup ValidateGroups(params object[] groups)
    {
      return new DictionaryValidateGroup(groups, (IDictionaryAdapter) this);
    }

    public IEnumerable<IDictionaryValidator> Validators
    {
      get
      {
        return (IEnumerable<IDictionaryValidator>) this.validators ?? Enumerable.Empty<IDictionaryValidator>();
      }
    }

    public void AddValidator(IDictionaryValidator validator)
    {
      if (this.validators == null)
        this.validators = (ICollection<IDictionaryValidator>) new HashSet<IDictionaryValidator>();
      this.validators.Add(validator);
    }

    protected internal void Invalidate()
    {
      if (!this.CanValidate)
        return;
      if (this.validators != null)
      {
        foreach (IDictionaryValidator validator in (IEnumerable<IDictionaryValidator>) this.validators)
          validator.Invalidate((IDictionaryAdapter) this);
      }
      this.NotifyPropertyChanged("IsValid");
    }

    private struct Edit(PropertyDescriptor property, object propertyValue)
    {
      public readonly PropertyDescriptor Property = property;
      public object PropertyValue = propertyValue;
    }

    private class SuppressEditingScope : IDisposable
    {
      private readonly DictionaryAdapterBase adapter;

      public SuppressEditingScope(DictionaryAdapterBase adapter)
      {
        this.adapter = adapter;
        this.adapter.SuppressEditing();
      }

      public void Dispose() => this.adapter.ResumeEditing();
    }

    private class SuppressNotificationsScope : IDisposable
    {
      private readonly DictionaryAdapterBase adapter;

      public SuppressNotificationsScope(DictionaryAdapterBase adapter)
      {
        this.adapter = adapter;
        this.adapter.SuppressNotifications();
      }

      public void Dispose() => this.adapter.ResumeNotifications();
    }

    public class TrackPropertyChangeScope : IDisposable
    {
      private readonly DictionaryAdapterBase adapter;
      private readonly PropertyDescriptor property;
      private readonly object existingValue;
      private IDictionary<PropertyDescriptor, object> readonlyProperties;

      public TrackPropertyChangeScope(DictionaryAdapterBase adapter)
      {
        this.adapter = adapter;
        this.readonlyProperties = (IDictionary<PropertyDescriptor, object>) adapter.This.Properties.Values.Where<PropertyDescriptor>((Func<PropertyDescriptor, bool>) (pd => !pd.Property.CanWrite || pd.IsDynamicProperty)).ToDictionary<PropertyDescriptor, PropertyDescriptor, object>((Func<PropertyDescriptor, PropertyDescriptor>) (pd => pd), (Func<PropertyDescriptor, object>) (pd => this.GetEffectivePropertyValue(pd)));
      }

      public TrackPropertyChangeScope(
        DictionaryAdapterBase adapter,
        PropertyDescriptor property,
        object existingValue)
        : this(adapter)
      {
        this.property = property;
        this.existingValue = existingValue;
        existingValue = adapter.GetProperty(property.PropertyName, true);
      }

      public bool Notify()
      {
        if (DictionaryAdapterBase.readonlyTrackingScope == this)
        {
          DictionaryAdapterBase.readonlyTrackingScope = (DictionaryAdapterBase.TrackPropertyChangeScope) null;
          return this.NotifyReadonly();
        }
        if (!this.NotifyIfChanged(this.property, this.existingValue, this.GetEffectivePropertyValue(this.property)))
          return false;
        if (DictionaryAdapterBase.readonlyTrackingScope == null)
          this.NotifyReadonly();
        return true;
      }

      private bool NotifyReadonly()
      {
        bool flag = false;
        foreach (KeyValuePair<PropertyDescriptor, object> readonlyProperty in (IEnumerable<KeyValuePair<PropertyDescriptor, object>>) this.readonlyProperties)
        {
          PropertyDescriptor key = readonlyProperty.Key;
          object effectivePropertyValue = this.GetEffectivePropertyValue(key);
          flag |= this.NotifyIfChanged(key, readonlyProperty.Value, effectivePropertyValue);
        }
        this.adapter.Invalidate();
        return flag;
      }

      private bool NotifyIfChanged(PropertyDescriptor descriptor, object oldValue, object newValue)
      {
        if (object.Equals(oldValue, newValue))
          return false;
        this.adapter.NotifyPropertyChanged(descriptor, oldValue, newValue);
        return true;
      }

      private object GetEffectivePropertyValue(PropertyDescriptor property)
      {
        object property1 = this.adapter.GetProperty(property.PropertyName, true);
        if (property1 != null & property.IsDynamicProperty)
          property1 = ((IDynamicValue) property1).GetValue();
        return property1;
      }

      public void Dispose() => this.Notify();
    }
  }
}
