// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Meters.MeterPhotosViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Business.Events;
using MSS.DIConfiguration;
using MSS.Interfaces;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace MSS_Client.ViewModel.Meters
{
  public class MeterPhotosViewModel : ViewModelBase
  {
    private readonly IWindowFactory _windowFactory;
    private readonly StructureNodeDTO SelectedStructureNodeDTO;
    private bool _isBusy;
    private bool _isDeleteButtonActive;

    public MeterPhotosViewModel(
      StructureNodeDTO selectedStructureNode,
      IWindowFactory windowFactory)
    {
      this.SelectedStructureNodeDTO = selectedStructureNode;
      List<byte[]> assignedPicture = this.SelectedStructureNodeDTO.AssignedPicture;
      this.PhotosCollection = new ObservableCollection<ImageSource>((assignedPicture != null ? (IEnumerable<ImageSource>) assignedPicture.Select<byte[], BitmapImage>((Func<byte[], BitmapImage>) (_ => _.ToImageSource())) : (IEnumerable<ImageSource>) null) ?? new List<ImageSource>().AsEnumerable<ImageSource>());
      this.SelectedStructureNodeDTO = selectedStructureNode;
      this._windowFactory = windowFactory;
    }

    public ObservableCollection<ImageSource> PhotosCollection { get; set; }

    public ImageSource SelectedPhoto { get; set; }

    public bool SimulateData { get; set; }

    public string Title { get; set; }

    public bool IsBusy
    {
      get => this._isBusy;
      set
      {
        this._isBusy = value;
        this.OnPropertyChanged(nameof (IsBusy));
      }
    }

    public int SelectedPhotoIndex { get; set; }

    public bool IsDeleteButtonActive
    {
      get => this._isDeleteButtonActive;
      set
      {
        this._isDeleteButtonActive = value;
        this.OnPropertyChanged(nameof (IsDeleteButtonActive));
      }
    }

    public ICommand DeletePhotoCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ => this.PhotosCollection.Remove(this.SelectedPhoto)));
      }
    }

    public ICommand TakePhotoCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          TakePhotoViewModel takePhotoViewModel = DIConfigurator.GetConfigurator().Get<TakePhotoViewModel>((IParameter) new ConstructorArgument("photoCollection", (object) this.PhotosCollection));
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) takePhotoViewModel);
          if (!newModalDialog.HasValue || !newModalDialog.Value)
            return;
          this.PhotosCollection.Add(takePhotoViewModel.ResultImage);
        }));
      }
    }

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          EventPublisher.Publish<MeterPhotosUpdated>(new MeterPhotosUpdated()
          {
            UpdatedNode = this.SelectedStructureNodeDTO,
            NewPhotos = this.PhotosCollection.Select<ImageSource, byte[]>((Func<ImageSource, byte[]>) (v => v == null ? (byte[]) null : v.ToByteArray((BitmapEncoder) new PngBitmapEncoder()))).ToList<byte[]>()
          }, (IViewModel) this);
          this.IsBusy = true;
          this.OnRequestClose(true);
        }));
      }
    }
  }
}
