// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.RadioTest.RadioTestViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Core.Model.RadioTest;
using MSS.Interfaces;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.RadioTest
{
  internal class RadioTestViewModel : ViewModelBase
  {
    private IRepositoryFactory _repositoryFactory;

    public RadioTestViewModel(IRepositoryFactory repositoryFactory, StructureNodeDTO structureNode)
    {
      this._repositoryFactory = repositoryFactory;
      this.TestOrderList = (IEnumerable<TestOrder>) this._repositoryFactory.GetRepository<TestOrder>().SearchFor((Expression<Func<TestOrder, bool>>) (x => x.StructureNode != default (object) && x.StructureNode.Id == structureNode.Id));
    }

    public IEnumerable<RadioTestRun> RadioTestRunList { get; set; }

    public IEnumerable<TestOrder> TestOrderList { get; set; }

    public ICommand CancelCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (Delegate => this.OnRequestClose(false)));
    }
  }
}
