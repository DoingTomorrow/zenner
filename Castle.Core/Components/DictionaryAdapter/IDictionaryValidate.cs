// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.IDictionaryValidate
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public interface IDictionaryValidate : IDataErrorInfo, INotifyPropertyChanged
  {
    bool CanValidate { get; set; }

    bool IsValid { get; }

    DictionaryValidateGroup ValidateGroups(params object[] groups);

    IEnumerable<IDictionaryValidator> Validators { get; }

    void AddValidator(IDictionaryValidator validator);
  }
}
