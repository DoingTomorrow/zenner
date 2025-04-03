// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Meters.AddNoteViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Core.Model.Meters;
using MSS.Interfaces;
using MSS.Localisation;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Meters
{
  public class AddNoteViewModel : ViewModelBase
  {
    private string _meterNotesText;
    private KeyValuePair<NoteType, string> _selectedNoteType;

    public AddNoteViewModel(IRepositoryFactory repositoryFactory)
    {
      List<NoteType> list = repositoryFactory.GetRepository<NoteType>().GetAll().ToList<NoteType>();
      this.NoteTypesComboBox = new Dictionary<NoteType, string>();
      foreach (NoteType key in list)
        this.NoteTypesComboBox.Add(key, CultureResources.GetValue("MSS_NoteType_" + key.Description));
      this.NoteTypesComboBox = this.NoteTypesComboBox.OrderBy<KeyValuePair<NoteType, string>, string>((Func<KeyValuePair<NoteType, string>, string>) (_ => _.Value)).ToDictionary<KeyValuePair<NoteType, string>, NoteType, string>((Func<KeyValuePair<NoteType, string>, NoteType>) (x => x.Key), (Func<KeyValuePair<NoteType, string>, string>) (x => x.Value));
      this.Title = Resources.MSS_Client_Add_Notes;
      this.SelectedNoteType = this.NoteTypesComboBox.FirstOrDefault<KeyValuePair<NoteType, string>>((Func<KeyValuePair<NoteType, string>, bool>) (item => item.Key.Description == "None"));
      this.MeterNotesText = Resources.MSS_Client_AddDescription;
    }

    public Dictionary<NoteType, string> NoteTypesComboBox { get; set; }

    public string MeterNotesText
    {
      get => this._meterNotesText;
      set
      {
        this._meterNotesText = value;
        this.OnPropertyChanged(nameof (MeterNotesText));
      }
    }

    public string Title { get; set; }

    public KeyValuePair<NoteType, string> SelectedNoteType
    {
      get => this._selectedNoteType;
      set
      {
        this._selectedNoteType = value;
        this.OnPropertyChanged(nameof (SelectedNoteType));
      }
    }

    public ICommand SaveNoteCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.AddedNote = new Note()
          {
            NoteDescription = this.MeterNotesText == Resources.MSS_Client_AddDescription ? "" : this.MeterNotesText,
            NoteType = this.SelectedNoteType.Key
          };
          this.OnRequestClose(true);
        }));
      }
    }

    public Note AddedNote { get; set; }
  }
}
