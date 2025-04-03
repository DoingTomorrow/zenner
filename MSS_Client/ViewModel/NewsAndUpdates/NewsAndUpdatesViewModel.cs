// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.NewsAndUpdates.NewsAndUpdatesViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.DTO.MessageHandler;
using MSS.Interfaces;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.NewsAndUpdates
{
  public class NewsAndUpdatesViewModel : ViewModelBase
  {
    private IRepositoryFactory _repositoryFactory;
    private ViewModelBase _messageUserControl;
    private string _subject;
    private string _startDate;
    private string _description;
    private bool _isEnabledLeftButton;
    private bool _isEnabledRightButton;

    public NewsAndUpdatesViewModel(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      EventPublisher.Register<ActionSyncFinished>(new Action<ActionSyncFinished>(this.CreateMessage));
      IList<MSS.Core.Model.News.News> all = this._repositoryFactory.GetRepository<MSS.Core.Model.News.News>().GetAll();
      this.IsEnabledLeftButton = false;
      this.IsEnabledRightButton = true;
      if (all.Any<MSS.Core.Model.News.News>())
      {
        this.GetFirstNews(all);
      }
      else
      {
        this.IsEnabledLeftButton = false;
        this.IsEnabledRightButton = false;
      }
    }

    private void GetFirstNews(IList<MSS.Core.Model.News.News> news)
    {
      IEnumerable<MSS.Core.Model.News.News> source = news.Reverse<MSS.Core.Model.News.News>();
      this.AllNews = source.ToList<MSS.Core.Model.News.News>();
      this.CurrentNews = source.Any<MSS.Core.Model.News.News>() ? source.FirstOrDefault<MSS.Core.Model.News.News>() : new MSS.Core.Model.News.News();
      this.HtmlString = this.CurrentNews.Description;
      this.Subject = this.CurrentNews.Subject;
      this.StartDate = this.CurrentNews.StartDate.ToString("d");
      this.CurrentNews.IsNew = false;
      this._repositoryFactory.GetRepository<MSS.Core.Model.News.News>().Update(this.CurrentNews);
      this.CheckIfAllNewsAreRead();
    }

    public List<MSS.Core.Model.News.News> AllNews { get; set; }

    public MSS.Core.Model.News.News CurrentNews { get; set; }

    public ViewModelBase MessageUserControl
    {
      get => this._messageUserControl;
      set
      {
        this._messageUserControl = value;
        this.OnPropertyChanged(nameof (MessageUserControl));
      }
    }

    private void CreateMessage(ActionSyncFinished messageFinished)
    {
      switch (messageFinished.Message.MessageType)
      {
        case MessageTypeEnum.Success:
          this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(messageFinished.Message.MessageText);
          break;
        case MessageTypeEnum.Warning:
          this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(messageFinished.Message.MessageText);
          break;
      }
    }

    public IEnumerable<MSS.Core.Model.News.News> GetAllNews
    {
      get => (IEnumerable<MSS.Core.Model.News.News>) this._repositoryFactory.GetRepository<MSS.Core.Model.News.News>().GetAll();
    }

    public ICommand LeftClickCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          this.IsEnabledRightButton = true;
          int num = this.AllNews.IndexOf(this.CurrentNews);
          this.IsEnabledLeftButton = num != 1;
          if (num < 1)
            return;
          this.CurrentNews = this.AllNews[num - 1];
          this.CurrentNews.IsNew = false;
          this._repositoryFactory.GetRepository<MSS.Core.Model.News.News>().Update(this.CurrentNews);
          this.CheckIfAllNewsAreRead();
          this.HtmlString = this.CurrentNews.Description;
          this.Subject = this.CurrentNews.Subject;
          this.StartDate = this.CurrentNews.StartDate.ToString("d");
        }));
      }
    }

    private void CheckIfAllNewsAreRead()
    {
      if (this.GetAllNews.Any<MSS.Core.Model.News.News>((Func<MSS.Core.Model.News.News, bool>) (x => x.StartDate < DateTime.Today && x.EndDate > DateTime.Today && x.IsNew)))
        return;
      EventPublisher.Publish<CleanNews>(new CleanNews()
      {
        IsRead = true
      }, (IViewModel) this);
    }

    public ICommand RightClickCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          this.IsEnabledLeftButton = true;
          int num = this.AllNews.IndexOf(this.CurrentNews);
          this.IsEnabledRightButton = num != this.AllNews.Count - 2;
          if (num >= this.AllNews.Count - 1)
            return;
          this.CurrentNews = this.AllNews[num + 1];
          this.CurrentNews.IsNew = false;
          this._repositoryFactory.GetRepository<MSS.Core.Model.News.News>().Update(this.CurrentNews);
          this.CheckIfAllNewsAreRead();
          this.HtmlString = this.CurrentNews.Description;
          this.Subject = this.CurrentNews.Subject;
          this.StartDate = this.CurrentNews.StartDate.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }));
      }
    }

    public string Subject
    {
      get => this._subject;
      set
      {
        this._subject = value;
        this.OnPropertyChanged(nameof (Subject));
      }
    }

    public string StartDate
    {
      get => this._startDate;
      set
      {
        this._startDate = value;
        this.OnPropertyChanged(nameof (StartDate));
      }
    }

    public string HtmlString
    {
      get => this._description;
      set
      {
        this._description = value;
        this.OnPropertyChanged(nameof (HtmlString));
      }
    }

    public bool IsEnabledLeftButton
    {
      get => this._isEnabledLeftButton;
      set
      {
        this._isEnabledLeftButton = value;
        this.OnPropertyChanged(nameof (IsEnabledLeftButton));
      }
    }

    public bool IsEnabledRightButton
    {
      get => this._isEnabledRightButton;
      set
      {
        this._isEnabledRightButton = value;
        this.OnPropertyChanged(nameof (IsEnabledRightButton));
      }
    }
  }
}
