// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Archiving.SearchStructuresViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Errors;
using MSS.Business.Modules.Archiving;
using MSS.DIConfiguration;
using MSS.DTO.Archive;
using MSS.DTO.MessageHandler;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Model.DataCollectors;
using MSSArchive.Core.Model.Meters;
using MSSArchive.Core.Model.Structures;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate;
using NHibernate.Linq;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Archiving
{
  internal class SearchStructuresViewModel : ViewModelBase
  {
    private readonly IWindowFactory _windowFactory;
    private string _pageSize = string.Empty;
    private List<ArchiveStructureNodeDTO> _archiveStructureNodeCollection;
    private ArchiveStructureNodeDTO _selectedItem;
    private string _viewButtonText;

    [Inject]
    public SearchStructuresViewModel(
      ArchiveInformation archiveInformation,
      IWindowFactory windowFactory)
    {
      this.ArchiveInformation = archiveInformation;
      this.PageSize = MSS.Business.Utils.AppContext.Current.GetParameterValue<string>(nameof (PageSize));
      this.ViewButtonText = CultureResources.GetValue("MSS_Client_Button_ViewEntity");
      this._windowFactory = windowFactory;
    }

    private ArchiveInformation ArchiveInformation { get; set; }

    public string PageSize
    {
      get => this._pageSize;
      set
      {
        this._pageSize = value;
        this.OnPropertyChanged(nameof (PageSize));
      }
    }

    public List<ArchiveStructureNodeDTO> ArchiveStructureNodeCollection
    {
      get => this._archiveStructureNodeCollection;
      set
      {
        this._archiveStructureNodeCollection = value;
        this.OnPropertyChanged(nameof (ArchiveStructureNodeCollection));
      }
    }

    public ArchiveStructureNodeDTO SelectedItem
    {
      get => this._selectedItem;
      set
      {
        this._selectedItem = value;
        this.OnPropertyChanged(nameof (SelectedItem));
      }
    }

    public string ViewButtonText
    {
      get => this._viewButtonText;
      set
      {
        this._viewButtonText = value;
        this.OnPropertyChanged(nameof (ViewButtonText));
      }
    }

    public ICommand SearchStrByNameCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          string searchText = parameter as string;
          this.ArchiveStructureNodeCollection = this.GetArchiveStructureDTO(ArchiveManagerNHibernate.GetSessionFactoryMSSArchive(), searchText, this.ArchiveInformation.Id);
          if (this.ArchiveStructureNodeCollection.Count != 0)
            return;
          MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
          {
            MessageType = MessageTypeEnum.Warning,
            MessageText = MessageCodes.No_Item_found.GetStringValue()
          };
        }));
      }
    }

    public ICommand ViewElementCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          switch (this.SelectedItem.Entity.GetType().Name)
          {
            case "ArchiveTenantDTO":
              this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ViewArchiveTenantViewModel>((IParameter) new ConstructorArgument("entity", this.SelectedItem.Entity)));
              break;
            case "ArchiveLocationDTO":
              this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ViewArchiveLocationViewModel>((IParameter) new ConstructorArgument("entity", this.SelectedItem.Entity)));
              break;
            case "ArchiveMeterDTO":
              this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ViewArchiveMeterViewModel>((IParameter) new ConstructorArgument("entity", this.SelectedItem.Entity)));
              break;
            case "ArchiveMinomatDTO":
              this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ViewArchiveMinomatViewModel>((IParameter) new ConstructorArgument("entity", this.SelectedItem.Entity)));
              break;
          }
        }));
      }
    }

    public List<ArchiveStructureNodeDTO> GetArchiveStructureDTO(
      ISessionFactory sessionFactoryMSSArchive,
      string searchText,
      Guid archiveInformationId)
    {
      List<ArchiveStructureNodeDTO> archiveStructureDto = new List<ArchiveStructureNodeDTO>();
      using (IStatelessSession nhSession = sessionFactoryMSSArchive.OpenStatelessSession())
      {
        List<ArchiveStructureNodeDTO> structureCollection = ArchivingHelper.CreateArchiveStructureCollection(LinqExtensionMethods.Query<ArchiveStructureNodeLinks>(nhSession).Where<ArchiveStructureNodeLinks>((Expression<Func<ArchiveStructureNodeLinks, bool>>) (l => l.ArchiveInformation.Id == archiveInformationId && l.EndDate == new DateTime?())).ToList<ArchiveStructureNodeLinks>().OrderBy<ArchiveStructureNodeLinks, int>((Func<ArchiveStructureNodeLinks, int>) (l => l.OrderNr)), LinqExtensionMethods.Query<ArchiveStructureNode>(nhSession).Where<ArchiveStructureNode>((Expression<Func<ArchiveStructureNode, bool>>) (n => n.ArchiveInformation.Id == archiveInformationId && n.EndDate == new DateTime?())).ToList<ArchiveStructureNode>(), this.GetArchiveEntitiesDictionary(nhSession, archiveInformationId));
        archiveStructureDto = ArchivingHelper.FilterArchiveStructureCollectionByName(searchText, structureCollection);
      }
      return archiveStructureDto;
    }

    public Dictionary<Guid, object> GetArchiveEntitiesDictionary(
      IStatelessSession nhSession,
      Guid archiveInformationId)
    {
      Dictionary<Guid, object> entitiesDictionary = new Dictionary<Guid, object>();
      IList<ArchiveMeter> list1 = (IList<ArchiveMeter>) LinqExtensionMethods.Query<ArchiveMeter>(nhSession).Where<ArchiveMeter>((Expression<Func<ArchiveMeter, bool>>) (m => m.ArchiveInformation.Id == archiveInformationId)).ToList<ArchiveMeter>();
      IList<ArchiveLocation> list2 = (IList<ArchiveLocation>) LinqExtensionMethods.Query<ArchiveLocation>(nhSession).Where<ArchiveLocation>((Expression<Func<ArchiveLocation, bool>>) (l => l.ArchiveInformation.Id == archiveInformationId)).ToList<ArchiveLocation>();
      IList<ArchiveTenant> list3 = (IList<ArchiveTenant>) LinqExtensionMethods.Query<ArchiveTenant>(nhSession).Where<ArchiveTenant>((Expression<Func<ArchiveTenant, bool>>) (t => t.ArchiveInformation.Id == archiveInformationId)).ToList<ArchiveTenant>();
      IList<ArchiveMinomat> list4 = (IList<ArchiveMinomat>) LinqExtensionMethods.Query<ArchiveMinomat>(nhSession).Where<ArchiveMinomat>((Expression<Func<ArchiveMinomat, bool>>) (m => m.ArchiveInformation.Id == archiveInformationId)).ToList<ArchiveMinomat>();
      TypeHelperExtensionMethods.ForEach<ArchiveMeter>((IEnumerable<ArchiveMeter>) list1, (Action<ArchiveMeter>) (m => entitiesDictionary.Add(m.Id, (object) m)));
      TypeHelperExtensionMethods.ForEach<ArchiveLocation>((IEnumerable<ArchiveLocation>) list2, (Action<ArchiveLocation>) (l => entitiesDictionary.Add(l.Id, (object) l)));
      TypeHelperExtensionMethods.ForEach<ArchiveTenant>((IEnumerable<ArchiveTenant>) list3, (Action<ArchiveTenant>) (t => entitiesDictionary.Add(t.Id, (object) t)));
      TypeHelperExtensionMethods.ForEach<ArchiveMinomat>((IEnumerable<ArchiveMinomat>) list4, (Action<ArchiveMinomat>) (m => entitiesDictionary.Add(m.Id, (object) m)));
      return entitiesDictionary;
    }
  }
}
