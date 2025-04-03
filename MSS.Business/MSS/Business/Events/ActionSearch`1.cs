// Decompiled with JetBrains decompiler
// Type: MSS.Business.Events.ActionSearch`1
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Utils;
using System;

#nullable disable
namespace MSS.Business.Events
{
  public class ActionSearch<T>
  {
    public Guid Id { get; set; }

    public System.Collections.ObjectModel.ObservableCollection<T> ObservableCollection { get; set; }

    public Telerik.Windows.Data.VirtualQueryableCollectionView<T> VirtualQueryableCollectionView { get; set; }

    public ApplicationTabsEnum SelectedTab { get; set; }

    public MSS.DTO.Message.Message Message { get; set; }
  }
}
