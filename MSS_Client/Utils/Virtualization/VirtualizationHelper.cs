// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.Virtualization.VirtualizationHelper
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Modules.UsersManagement;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.Users;
using MSS.Interfaces;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.Utils.Virtualization
{
  public class VirtualizationHelper
  {
    private readonly ISession _nhSession;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IUserRepository _userRepository;

    public VirtualizationHelper(IRepositoryFactory repositoryFactory)
    {
      this._nhSession = repositoryFactory.GetSession();
      this._repositoryFactory = repositoryFactory;
      this._userRepository = repositoryFactory.GetUserRepository();
      repositoryFactory.GetRepository<UserRole>();
      repositoryFactory.GetRepository<Country>();
    }

    private VirtualQueryableCollectionView<UserDTO> GetUsersDTOVirtualCollection(
      int startIdex,
      int pageSize,
      Expression<Func<User, bool>> condition,
      VirtualQueryableCollectionView<UserDTO> collection)
    {
      VirtualQueryableCollectionView<UserDTO> userDtos = new VirtualQueryableCollectionView<UserDTO>();
      int totalCount;
      IEnumerable<User> users = ServerOperationProcessor<User>.ProcessQueryByCommand((IRepository<User>) this._userRepository, startIdex, pageSize, collection, condition, out totalCount);
      collection.VirtualItemCount = totalCount;
      TypeHelperExtensionMethods.ForEach<User>(users, (Action<User>) (u =>
      {
        VirtualQueryableCollectionView<UserDTO> queryableCollectionView = userDtos;
        UserDTO userDto1 = new UserDTO();
        userDto1.FirstName = u.FirstName;
        userDto1.Id = u.Id;
        userDto1.LastName = u.LastName;
        userDto1.Role = this.GetUserManagerInstance().GetRoles(u.UserRoles);
        userDto1.Username = u.Username;
        userDto1.Office = u.Office;
        userDto1.Country = this.GetUserManagerInstance().GetCountry(u.Country);
        userDto1.Language = u.Language;
        UserDTO userDto2 = userDto1;
        Country country = u.Country;
        Guid guid = country != null ? country.Id : Guid.Empty;
        userDto2.CountryId = guid;
        UserDTO userDto3 = userDto1;
        queryableCollectionView.Add((object) userDto3);
      }));
      return userDtos;
    }

    public VirtualQueryableCollectionView<UserDTO> LoadUsers(string search, int pageSize)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      VirtualizationHelper.\u003C\u003Ec__DisplayClass5_0 cDisplayClass50 = new VirtualizationHelper.\u003C\u003Ec__DisplayClass5_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass50.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass50.search = search;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass50.pageSize = pageSize;
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass50.search == string.Empty)
      {
        Expression<Func<User, bool>> expression = (Expression<Func<User, bool>>) (u => !u.IsDeactivated && !u.Username.StartsWith("default"));
        // ISSUE: reference to a compiler-generated field
        cDisplayClass50.condition = expression;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Expression<Func<User, bool>> expression = (Expression<Func<User, bool>>) (s => s.FirstName.Contains(cDisplayClass50.search) || s.LastName.Contains(cDisplayClass50.search) || s.UserRoles.First<UserRole>((Func<UserRole, bool>) (ur => ur.Role.Name.Contains(cDisplayClass50.search))) != default (object) || s.Username.Contains(cDisplayClass50.search) && !s.IsDeactivated && !s.Username.StartsWith("default"));
        // ISSUE: reference to a compiler-generated field
        cDisplayClass50.condition = expression;
      }
      VirtualQueryableCollectionView<UserDTO> queryableCollectionView1 = new VirtualQueryableCollectionView<UserDTO>();
      // ISSUE: reference to a compiler-generated field
      queryableCollectionView1.LoadSize = cDisplayClass50.pageSize;
      // ISSUE: reference to a compiler-generated field
      queryableCollectionView1.VirtualItemCount = this._userRepository.SearchFor_RecordsCount(cDisplayClass50.condition);
      VirtualQueryableCollectionView<UserDTO> queryableCollectionView2 = queryableCollectionView1;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass50.collection = queryableCollectionView2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      cDisplayClass50.collection.ItemsLoading += new EventHandler<VirtualQueryableCollectionViewItemsLoadingEventArgs>(cDisplayClass50.\u003CLoadUsers\u003Eb__0);
      // ISSUE: reference to a compiler-generated field
      return cDisplayClass50.collection;
    }

    private UsersManager GetUserManagerInstance() => new UsersManager(this._repositoryFactory);
  }
}
