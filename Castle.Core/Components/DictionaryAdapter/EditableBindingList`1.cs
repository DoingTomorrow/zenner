// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.EditableBindingList`1
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class EditableBindingList<T> : 
    BindingList<T>,
    IList<T>,
    ICollection<T>,
    IEnumerable<T>,
    IEnumerable,
    IEditableObject,
    IRevertibleChangeTracking,
    IChangeTracking
  {
    private bool isEditing;
    private List<T> snapshot;

    public EditableBindingList()
    {
    }

    public EditableBindingList(IList<T> initial)
      : base(initial)
    {
    }

    public bool IsChanged
    {
      get
      {
        if (this.snapshot == null || this.snapshot.Count != this.Count)
          return false;
        IEnumerator<T> enumerator1 = this.GetEnumerator();
        List<T>.Enumerator enumerator2 = this.snapshot.GetEnumerator();
        while (enumerator1.MoveNext() && enumerator2.MoveNext() && object.ReferenceEquals((object) enumerator1.Current, (object) enumerator2.Current))
        {
          if (enumerator1.Current is IChangeTracking current && current.IsChanged)
            return true;
        }
        return false;
      }
    }

    public void BeginEdit()
    {
      if (this.isEditing)
        return;
      this.snapshot = new List<T>((IEnumerable<T>) this);
      this.isEditing = true;
    }

    public void EndEdit()
    {
      this.isEditing = false;
      this.snapshot = (List<T>) null;
    }

    public void CancelEdit()
    {
      if (!this.isEditing)
        return;
      this.Clear();
      foreach (T obj in this.snapshot)
        this.Add(obj);
      this.snapshot = (List<T>) null;
      this.isEditing = false;
    }

    public void AcceptChanges() => this.BeginEdit();

    public void RejectChanges() => this.CancelEdit();
  }
}
