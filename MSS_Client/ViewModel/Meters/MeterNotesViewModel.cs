// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Meters.MeterNotesViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Business.Events;
using MSS.Core.Model.Meters;
using MSS.DIConfiguration;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Meters
{
  public class MeterNotesViewModel : ViewModelBase
  {
    private readonly IWindowFactory _windowFactory;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly StructureNodeDTO _selectedStructureNode;
    private string _meterNotesText;
    private ObservableCollection<NoteWithTranslation> _notesList;
    private NoteWithTranslation _selectedNote;
    private bool _isEditNoteButtonEnabled;
    private bool _isDeleteNoteButtonEnabled;

    public MeterNotesViewModel(
      StructureNodeDTO selectedStructureNode,
      IWindowFactory windowFactory,
      IRepositoryFactory repositoryFactory)
    {
      this._windowFactory = windowFactory;
      this._repositoryFactory = repositoryFactory;
      this._selectedStructureNode = selectedStructureNode;
      this.NotesList = new ObservableCollection<NoteWithTranslation>();
      foreach (Note assignedNote in this._selectedStructureNode.AssignedNotes)
        this.NotesList.Add(new NoteWithTranslation()
        {
          Note = assignedNote
        });
      this.Title = Resources.MSS_Client_Meter_Notes;
      this.IsEditNoteButtonEnabled = false;
      this.IsDeleteNoteButtonEnabled = false;
    }

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

    public ObservableCollection<NoteWithTranslation> NotesList
    {
      get => this._notesList;
      set
      {
        this._notesList = value;
        this.OnPropertyChanged(nameof (NotesList));
      }
    }

    public NoteWithTranslation SelectedNote
    {
      get => this._selectedNote;
      set
      {
        this._selectedNote = value;
        this.IsEditNoteButtonEnabled = this._selectedNote != null;
        this.IsDeleteNoteButtonEnabled = this._selectedNote != null;
        this.OnPropertyChanged(nameof (SelectedNote));
      }
    }

    public bool IsEditNoteButtonEnabled
    {
      get => this._isEditNoteButtonEnabled;
      set
      {
        this._isEditNoteButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsEditNoteButtonEnabled));
      }
    }

    public bool IsDeleteNoteButtonEnabled
    {
      get => this._isDeleteNoteButtonEnabled;
      set
      {
        this._isDeleteNoteButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsDeleteNoteButtonEnabled));
      }
    }

    public ICommand AddNoteCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          AddNoteViewModel addNoteViewModel = DIConfigurator.GetConfigurator().Get<AddNoteViewModel>((IParameter) new ConstructorArgument("repositoryFactory", (object) this._repositoryFactory));
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) addNoteViewModel);
          if (!newModalDialog.HasValue || !newModalDialog.Value)
            return;
          this.NotesList.Add(new NoteWithTranslation()
          {
            Note = addNoteViewModel.AddedNote
          });
        }));
      }
    }

    public ICommand EditNoteCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          EditNoteViewModel editNoteViewModel = DIConfigurator.GetConfigurator().Get<EditNoteViewModel>((IParameter) new ConstructorArgument("repositoryFactory", (object) this._repositoryFactory), (IParameter) new ConstructorArgument("selectedNote", (object) this.SelectedNote.Note));
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) editNoteViewModel);
          if (!newModalDialog.HasValue || !newModalDialog.Value)
            return;
          ObservableCollection<NoteWithTranslation> notesList = this.NotesList;
          NoteWithTranslation editedNote = notesList.FirstOrDefault<NoteWithTranslation>((Func<NoteWithTranslation, bool>) (item => item == this.SelectedNote));
          if (editedNote != null)
          {
            editedNote.Note.NoteDescription = editNoteViewModel.EditedNote.NoteDescription;
            editedNote.Note.NoteType = editNoteViewModel.EditedNote.NoteType;
          }
          this.NotesList = new ObservableCollection<NoteWithTranslation>((IEnumerable<NoteWithTranslation>) notesList);
          this.SelectedNote = this.NotesList.FirstOrDefault<NoteWithTranslation>((Func<NoteWithTranslation, bool>) (item => item == editedNote));
        }));
      }
    }

    public ICommand DeleteNoteCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ => this.NotesList.Contains(this.SelectedNote).IsTrue((Action) (() => this.NotesList.Remove(this.SelectedNote)))));
      }
    }

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          EventPublisher.Publish<MeterNotesUpdated>(new MeterNotesUpdated()
          {
            UpdatedNode = this._selectedStructureNode,
            NewNotesList = this.NotesList.Select<NoteWithTranslation, Note>((Func<NoteWithTranslation, Note>) (item => item.Note)).ToList<Note>()
          }, (IViewModel) this);
          this.OnRequestClose(true);
        }));
      }
    }
  }
}
