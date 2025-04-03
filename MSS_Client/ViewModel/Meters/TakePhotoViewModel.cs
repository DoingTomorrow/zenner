// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Meters.TakePhotoViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AForge.Video;
using AForge.Video.DirectShow;
using MSS.Localisation;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace MSS_Client.ViewModel.Meters
{
  public class TakePhotoViewModel : ViewModelBase
  {
    private ImageSource _frameHolder;
    private ImageSource _resultImage;
    private string _camCollection;

    public FilterInfoCollection LocalWebCamsCollection { get; set; }

    public VideoCaptureDevice localWebCam { get; set; }

    public ObservableCollection<ImageSource> PhotosCollection { get; set; }

    public ImageSource FrameHolder
    {
      get => this._frameHolder;
      set
      {
        this._frameHolder = value;
        this.OnPropertyChanged(nameof (FrameHolder));
      }
    }

    public string Title { get; set; }

    public TakePhotoViewModel(ObservableCollection<ImageSource> photoCollection)
    {
      this.Title = Resources.MSS_Client_Add_Meter_Photos;
    }

    public ImageSource ResultImage
    {
      get => this._resultImage;
      set
      {
        this._resultImage = value;
        this.OnPropertyChanged(nameof (ResultImage));
      }
    }

    public string CamCollection
    {
      get => this._camCollection;
      set
      {
        this._camCollection = value;
        this.OnPropertyChanged(nameof (CamCollection));
      }
    }

    public ICommand SetVideoStream
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (((System.Windows.Controls.Image) _).Source != null)
            return;
          this.LocalWebCamsCollection = new FilterInfoCollection((Guid) FilterCategory.VideoInputDevice);
          this.CamCollection = ((CollectionBase) this.LocalWebCamsCollection).Count.ToString();
          this.localWebCam = ((CollectionBase) this.LocalWebCamsCollection).Count <= 1 ? new VideoCaptureDevice(this.LocalWebCamsCollection[0].MonikerString) : new VideoCaptureDevice(this.LocalWebCamsCollection[int.Parse(ConfigurationManager.AppSettings["CamsCollectionCount"]) - 1].MonikerString);
          this.localWebCam.NewFrame += new NewFrameEventHandler(this.Cam_NewFrame);
          this.localWebCam.DesiredFrameSize = new System.Drawing.Size(640, 360);
          System.Windows.Controls.Image image = (System.Windows.Controls.Image) _;
          image.Width = 640.0;
          image.Height = 360.0;
          this.localWebCam.Start();
        }));
      }
    }

    public ICommand CapturePictureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ => this.ResultImage = this.FrameHolder));
      }
    }

    public ICommand DeletePictureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ => this.ResultImage = (ImageSource) null));
      }
    }

    public ICommand SavePhotoCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (_ => this.OnRequestClose(true)));
    }

    private void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
    {
      System.Drawing.Image image = (System.Drawing.Image) eventArgs.Frame.Clone();
      MemoryStream memoryStream = new MemoryStream();
      image.Save((Stream) memoryStream, ImageFormat.Bmp);
      memoryStream.Seek(0L, SeekOrigin.Begin);
      BitmapImage bi = new BitmapImage();
      bi.BeginInit();
      bi.StreamSource = (Stream) memoryStream;
      bi.EndInit();
      bi.Freeze();
      Application.Current.Dispatcher.InvokeAsync((Action) (() => this.FrameHolder = (ImageSource) bi));
    }

    public void DettachWebCam()
    {
      this.localWebCam.SignalToStop();
      this.localWebCam.WaitForStop();
    }
  }
}
