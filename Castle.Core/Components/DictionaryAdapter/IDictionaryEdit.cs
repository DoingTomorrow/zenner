// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.IDictionaryEdit
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public interface IDictionaryEdit : IEditableObject, IRevertibleChangeTracking, IChangeTracking
  {
    bool CanEdit { get; }

    bool IsEditing { get; }

    bool SupportsMultiLevelEdit { get; set; }

    IDisposable SuppressEditingBlock();

    void SuppressEditing();

    void ResumeEditing();
  }
}
