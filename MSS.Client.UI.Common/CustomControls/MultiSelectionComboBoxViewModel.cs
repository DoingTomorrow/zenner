// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.CustomControls.MultiSelectionComboBoxViewModel
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using MSS.Localisation;
using MSS.Utils.Controls.CheckableComboBox;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace MSS.Client.UI.Common.CustomControls
{
  public class MultiSelectionComboBoxViewModel : ViewModelBase
  {
    private readonly string _selectAllText = Resources.MSS_Client_SelectAll;
    private List<string> _selectedItems = new List<string>();
    private IList<CheckableComboBoxItem> _itemsList;
    private string _selectedItemText;

    public IList<CheckableComboBoxItem> ItemsList
    {
      get => this._itemsList;
      set
      {
        this._itemsList = value;
        this._itemsList.Insert(0, new CheckableComboBoxItem()
        {
          Text = this._selectAllText
        });
        this.OnPropertyChanged(nameof (ItemsList));
      }
    }

    public string SelectedItemText
    {
      get => this._selectedItemText;
      set
      {
        if (value != null && value.Contains("CheckableComboBoxItem"))
          return;
        this._selectedItemText = !string.IsNullOrEmpty(value) ? value : this.GetFormattedSelection();
        this.OnPropertyChanged(nameof (SelectedItemText));
      }
    }

    public ICommand OnCheckChangedCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          CheckableComboBoxItem checkableComboBoxItem1 = parameter as CheckableComboBoxItem;
          CheckableComboBoxItem checkableComboBoxItem2 = this.ItemsList.FirstOrDefault<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.Text == this._selectAllText));
          if (checkableComboBoxItem1 == checkableComboBoxItem2)
          {
            if (checkableComboBoxItem2.IsChecked)
            {
              this._selectedItems.Clear();
              this.ItemsList.ForEach<CheckableComboBoxItem>((Action<CheckableComboBoxItem>) (item =>
              {
                item.IsChecked = true;
                this._selectedItems.Add(item.Text);
              }));
            }
            else
            {
              this._selectedItems.Clear();
              this.ItemsList.ForEach<CheckableComboBoxItem>((Action<CheckableComboBoxItem>) (item => item.IsChecked = false));
            }
          }
          else
          {
            checkableComboBoxItem2.IsChecked = this.ItemsList.Count<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.IsChecked)) == this.ItemsList.Count - 1 && !this.ItemsList[0].IsChecked;
            this._selectedItems = this.ItemsList.Where<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.IsChecked)).Select<CheckableComboBoxItem, string>((Func<CheckableComboBoxItem, string>) (item => item.Text)).ToList<string>();
          }
          this.SelectedItemText = this.GetFormattedSelection();
        }));
      }
    }

    private string GetFormattedSelection()
    {
      return this._selectedItems.Any<string>() ? string.Join(";", this._selectedItems.Where<string>((Func<string, bool>) (_ => _ != this._selectAllText))) : string.Empty;
    }

    public string[] SelectedItemsAsList
    {
      get
      {
        return this._selectedItems.Where<string>((Func<string, bool>) (_ => _ != this._selectAllText)).ToArray<string>();
      }
    }

    public void RefreshTextAfterInitialization()
    {
      this._selectedItems = this.ItemsList.Where<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (x => x.IsChecked)).Select<CheckableComboBoxItem, string>((Func<CheckableComboBoxItem, string>) (x => x.Text)).ToList<string>();
      this.SelectedItemText = this.GetFormattedSelection();
    }
  }
}
