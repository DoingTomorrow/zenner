// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.Virtualization.VirtualizationManager
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MSS.Business.Modules.UsersManagement;
using MSS.Core.Model.Orders;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.Orders;
using MSS.DTO.Users;
using MSS.Interfaces;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.Utils.Virtualization
{
  public class VirtualizationManager
  {
    private readonly ISession _nhSession;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IUserRepository _userRepository;

    public VirtualizationManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._nhSession = repositoryFactory.GetSession();
      this._userRepository = repositoryFactory.GetUserRepository();
      repositoryFactory.GetRepository<UserRole>();
      repositoryFactory.GetRepository<Country>();
      Mapper.CreateMap<Role, RoleDTO>();
      Mapper.CreateMap<Order, OrderDTO>();
    }

    public VirtualQueryableCollectionView<TV> LoadItems<TD, TV>(
      int pageSize,
      Expression<Func<TD, bool>> condition)
      where TD : class
    {
      VirtualQueryableCollectionView<TV> queryableCollectionView = new VirtualQueryableCollectionView<TV>();
      queryableCollectionView.LoadSize = pageSize;
      queryableCollectionView.VirtualItemCount = this._repositoryFactory.GetRepository<TD>().SearchFor_RecordsCount(condition);
      VirtualQueryableCollectionView<TV> collection = queryableCollectionView;
      collection.ItemsLoading += (EventHandler<VirtualQueryableCollectionViewItemsLoadingEventArgs>) ((s, args) => collection.Load(args.StartIndex, (IEnumerable) this.GetVirtualCollection<TD, TV>(args.StartIndex, pageSize, condition)));
      return collection;
    }

    private VirtualQueryableCollectionView<TV> GetVirtualCollection<TD, TV>(
      int startIdex,
      int pageSize,
      Expression<Func<TD, bool>> condition)
      where TD : class
    {
      IList<TD> source = this._repositoryFactory.GetRepository<TD>().SearchFor_ByPage(condition, startIdex, pageSize);
      VirtualQueryableCollectionView<TV> destination = new VirtualQueryableCollectionView<TV>();
      Mapper.Map<IList<TD>, VirtualQueryableCollectionView<TV>>(source, destination);
      return destination;
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
      VirtualizationManager.\u003C\u003Ec__DisplayClass7_0 cDisplayClass70 = new VirtualizationManager.\u003C\u003Ec__DisplayClass7_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass70.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass70.search = search;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass70.pageSize = pageSize;
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass70.search == string.Empty)
      {
        Expression<Func<User, bool>> expression = (Expression<Func<User, bool>>) (u => !u.IsDeactivated && !u.Username.StartsWith("default", StringComparison.CurrentCulture));
        // ISSUE: reference to a compiler-generated field
        cDisplayClass70.condition = expression;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Expression<Func<User, bool>> expression = (Expression<Func<User, bool>>) (s => s.FirstName.Contains(cDisplayClass70.search) || s.LastName.Contains(cDisplayClass70.search) || s.UserRoles.First<UserRole>((Func<UserRole, bool>) (ur => ur.Role.Name.Contains(cDisplayClass70.search))) != default (object) || s.Username.Contains(cDisplayClass70.search) && !s.IsDeactivated && !s.Username.StartsWith("default", StringComparison.CurrentCulture));
        // ISSUE: reference to a compiler-generated field
        cDisplayClass70.condition = expression;
      }
      VirtualQueryableCollectionView<UserDTO> queryableCollectionView1 = new VirtualQueryableCollectionView<UserDTO>();
      // ISSUE: reference to a compiler-generated field
      queryableCollectionView1.LoadSize = cDisplayClass70.pageSize;
      // ISSUE: reference to a compiler-generated field
      queryableCollectionView1.VirtualItemCount = this._userRepository.SearchFor_RecordsCount(cDisplayClass70.condition);
      VirtualQueryableCollectionView<UserDTO> queryableCollectionView2 = queryableCollectionView1;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass70.collection = queryableCollectionView2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      cDisplayClass70.collection.ItemsLoading += new EventHandler<VirtualQueryableCollectionViewItemsLoadingEventArgs>(cDisplayClass70.\u003CLoadUsers\u003Eb__0);
      // ISSUE: reference to a compiler-generated field
      return cDisplayClass70.collection;
    }

    private UsersManager GetUserManagerInstance() => new UsersManager(this._repositoryFactory);
  }
}
