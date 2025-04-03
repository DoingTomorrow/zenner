// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.DataCollectors.StructureMinomatViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Business.Modules.OrdersManagement;
using MSS.Core.Model.Structures;
using MSS.DTO.Minomat;
using MSS.Interfaces;
using MVVM.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Media;

#nullable disable
namespace MSS_Client.ViewModel.DataCollectors
{
  public class StructureMinomatViewModel : ValidationViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;

    [Inject]
    public StructureMinomatViewModel(MinomatDTO minomat, IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      StructureNode structureNode = this._repositoryFactory.GetRepository<StructureNode>().FirstOrDefault((Expression<Func<StructureNode, bool>>) (x => x.EntityId == minomat.Id));
      if (structureNode == null)
        return;
      this.StructureNodeCollection = OrdersHelper.GetStructureNodeDTOForRootNode(this._repositoryFactory.GetRepository<StructureNodeLinks>().FirstOrDefault((Expression<Func<StructureNodeLinks, bool>>) (x => x.Node.Id == structureNode.Id)).RootNode.Id, this._repositoryFactory.GetRepository<StructureNodeLinks>(), this._repositoryFactory.GetRepository<StructureNode>(), this._repositoryFactory.GetRepository<StructureNodeType>(), this._repositoryFactory.GetSession());
      this.StructureNodeCollection.First<StructureNodeDTO>().SubNodes.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (x => x.Id == structureNode.Id)).BackgroundColor = (Brush) Brushes.LightGreen;
    }

    public IEnumerable<StructureNodeDTO> StructureNodeCollection { get; set; }
  }
}
