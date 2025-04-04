// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.AttachedProperties.DropIndicationDetails
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Common.AttachedProperties
{
  public class DropIndicationDetails : ViewModelBase
  {
    private object currentDraggedItem;
    private DropPosition currentDropPosition;
    private object currentDraggedOverItem;

    public object CurrentDraggedOverItem
    {
      get => this.currentDraggedOverItem;
      set
      {
        if (this.currentDraggedOverItem == value)
          return;
        this.currentDraggedOverItem = value;
        this.OnPropertyChanged(nameof (CurrentDraggedOverItem));
      }
    }

    public int DropIndex { get; set; }

    public DropPosition CurrentDropPosition
    {
      get => this.currentDropPosition;
      set
      {
        if (this.currentDropPosition == value)
          return;
        this.currentDropPosition = value;
        this.OnPropertyChanged(nameof (CurrentDropPosition));
      }
    }

    public object CurrentDraggedItem
    {
      get => this.currentDraggedItem;
      set
      {
        if (this.currentDraggedItem == value)
          return;
        this.currentDraggedItem = value;
        this.OnPropertyChanged(nameof (CurrentDraggedItem));
      }
    }
  }
}
