// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.BindingListInitializer`1
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class BindingListInitializer<T> : IValueInitializer
  {
    private readonly Action<int, object> addAt;
    private readonly Func<object> addNew;
    private readonly Action<int, object> setAt;
    private readonly Action<int> removeAt;
    private bool addingNew;

    public BindingListInitializer(
      Action<int, object> addAt,
      Func<object> addNew,
      Action<int, object> setAt,
      Action<int> removeAt)
    {
      this.addAt = addAt;
      this.addNew = addNew;
      this.setAt = setAt;
      this.removeAt = removeAt;
    }

    public void Initialize(IDictionaryAdapter dictionaryAdapter, object value)
    {
      BindingList<T> bindingList = (BindingList<T>) value;
      if (this.addNew != null)
        bindingList.AddingNew += (AddingNewEventHandler) ((sender, args) =>
        {
          args.NewObject = this.addNew();
          this.addingNew = true;
        });
      bindingList.ListChanged += (ListChangedEventHandler) ((sender, args) =>
      {
        switch (args.ListChangedType)
        {
          case ListChangedType.ItemAdded:
            if (!this.addingNew && this.addAt != null)
              this.addAt(args.NewIndex, (object) bindingList[args.NewIndex]);
            this.addingNew = false;
            break;
          case ListChangedType.ItemDeleted:
            if (this.removeAt == null)
              break;
            this.removeAt(args.NewIndex);
            break;
          case ListChangedType.ItemChanged:
            if (this.setAt == null)
              break;
            this.setAt(args.NewIndex, (object) bindingList[args.NewIndex]);
            break;
        }
      });
    }
  }
}
