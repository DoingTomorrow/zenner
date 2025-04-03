// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.UsersManagement.UsersManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using CA.Cryptography;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.Users;
using MSS.Interfaces;
using MSS.Localisation;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Windows.Data;

#nullable disable
namespace MSS.Business.Modules.UsersManagement
{
  public class UsersManager
  {
    private readonly ISession _nhSession;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IRepository<RoleOperation> _roleOperationRepository;
    private readonly IRepository<Role> _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IRepository<Country> _countryRepository;

    public UsersManager(IRepositoryFactory repositoryFactory)
    {
      this._nhSession = repositoryFactory.GetSession();
      this._repositoryFactory = repositoryFactory;
      this._userRepository = repositoryFactory.GetUserRepository();
      this._userRoleRepository = repositoryFactory.GetRepository<UserRole>();
      this._roleOperationRepository = repositoryFactory.GetRepository<RoleOperation>();
      this._roleRepository = repositoryFactory.GetRepository<Role>();
      this._countryRepository = repositoryFactory.GetRepository<Country>();
    }

    public bool IsUsernameInUse(string username)
    {
      return this._userRepository.FirstOrDefault((Expression<Func<User, bool>>) (u => u.Username == username && !u.IsDeactivated)) != null;
    }

    public bool IsLastSuperuser(UserDTO user)
    {
      if ("Superuser" != user.Role)
        return false;
      return this._userRoleRepository.Where((Expression<Func<UserRole, bool>>) (item => item.Role.Name == "Superuser" && !item.User.IsDeactivated)).Count<UserRole>() <= 1;
    }

    private VirtualQueryableCollectionView<UserDTO> GetUsersDTOVirtualCollection(
      int startIdex,
      int pageSize,
      Expression<Func<User, bool>> condition,
      VirtualQueryableCollectionView<UserDTO> collection)
    {
      IList<User> userList = this._userRepository.SearchFor_ByPage(condition, startIdex, pageSize);
      VirtualQueryableCollectionView<UserDTO> userDtos = new VirtualQueryableCollectionView<UserDTO>();
      TypeHelperExtensionMethods.ForEach<User>((IEnumerable<User>) userList, (Action<User>) (u => userDtos.Add((object) new UserDTO()
      {
        FirstName = u.FirstName,
        Id = u.Id,
        LastName = u.LastName,
        Role = this.GetRoles(u.UserRoles),
        Username = u.Username,
        Office = u.Office,
        Country = this.GetCountry(u.Country),
        Language = u.Language,
        CountryId = (u.Country != null ? u.Country.Id : Guid.Empty)
      })));
      return userDtos;
    }

    public VirtualQueryableCollectionView<UserDTO> LoadUsers(string search, int pageSize)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UsersManager.\u003C\u003Ec__DisplayClass11_0 cDisplayClass110 = new UsersManager.\u003C\u003Ec__DisplayClass11_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass110.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass110.search = search;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass110.pageSize = pageSize;
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass110.search == string.Empty)
      {
        Expression<Func<User, bool>> expression = (Expression<Func<User, bool>>) (u => !u.IsDeactivated && !u.Username.StartsWith("default"));
        // ISSUE: reference to a compiler-generated field
        cDisplayClass110.condition = expression;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Expression<Func<User, bool>> expression = (Expression<Func<User, bool>>) (s => s.FirstName.Contains(cDisplayClass110.search) || s.LastName.Contains(cDisplayClass110.search) || s.UserRoles.First<UserRole>((Func<UserRole, bool>) (ur => ur.Role.Name.Contains(cDisplayClass110.search))) != default (object) || s.Username.Contains(cDisplayClass110.search) && !s.IsDeactivated && !s.Username.StartsWith("default"));
        // ISSUE: reference to a compiler-generated field
        cDisplayClass110.condition = expression;
      }
      VirtualQueryableCollectionView<UserDTO> queryableCollectionView1 = new VirtualQueryableCollectionView<UserDTO>();
      // ISSUE: reference to a compiler-generated field
      queryableCollectionView1.LoadSize = cDisplayClass110.pageSize;
      // ISSUE: reference to a compiler-generated field
      queryableCollectionView1.VirtualItemCount = this._userRepository.SearchFor_RecordsCount(cDisplayClass110.condition);
      VirtualQueryableCollectionView<UserDTO> queryableCollectionView2 = queryableCollectionView1;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass110.collection = queryableCollectionView2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      cDisplayClass110.collection.ItemsLoading += new EventHandler<VirtualQueryableCollectionViewItemsLoadingEventArgs>(cDisplayClass110.\u003CLoadUsers\u003Eb__0);
      // ISSUE: reference to a compiler-generated field
      return cDisplayClass110.collection;
    }

    public string GetCountry(Country country) => country != null ? country.Name : string.Empty;

    public RadObservableCollection<Country> GetCountries()
    {
      RadObservableCollection<Country> countries = new RadObservableCollection<Country>();
      IList<Country> all = this._countryRepository.GetAll();
      if (all.Any<Country>())
        countries = new RadObservableCollection<Country>((IEnumerable<Country>) all);
      return countries;
    }

    public void UpdateCountries(RadObservableCollection<CountryDTO> countryList)
    {
      try
      {
        this._nhSession.FlushMode = FlushMode.Commit;
        this._nhSession.BeginTransaction();
        foreach (CountryDTO country in (Collection<CountryDTO>) countryList)
        {
          Country byId = this._countryRepository.GetById((object) country.Id);
          byId.DefaultScenarioId = country.DefaultScenarioId;
          this._countryRepository.TransactionalUpdate(byId);
        }
        this._nhSession.Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public string GetUserRoles(Guid userId)
    {
      string empty = string.Empty;
      string userRoles = this._userRoleRepository.SearchFor((Expression<Func<UserRole, bool>>) (ur => ur.User.Id == userId)).Aggregate<UserRole, string>(empty, (Func<string, UserRole, string>) ((current, ur) => current + ur.Role.Name + ", "));
      if (userRoles != string.Empty)
        userRoles = userRoles.Remove(userRoles.LastIndexOf(','));
      return userRoles;
    }

    public string GetRoles(IList<UserRole> userRoles)
    {
      string roles = (string) null;
      if (userRoles != null && userRoles.Count > 0)
        roles = userRoles.Aggregate<UserRole, string>((string) null, (Func<string, UserRole, string>) ((current, userRole) => current + userRole.Role.Name + "; ")).Trim().TrimEnd(';');
      return roles;
    }

    public IEnumerable<User> GetUsers() => (IEnumerable<User>) this._userRepository.GetAll();

    public IEnumerable<UserRole> GetUserRoles()
    {
      return (IEnumerable<UserRole>) this._userRoleRepository.GetAll();
    }

    public void CreateUser(UserEditDTO userDto, string password)
    {
      try
      {
        this._nhSession.BeginTransaction();
        string hashAndSalt;
        new PasswordManager().GetPasswordHashAndSaltConcatenatedString(password, out hashAndSalt);
        User entity = new User()
        {
          FirstName = userDto.FirstName,
          LastName = userDto.LastName,
          Username = userDto.Username,
          Password = hashAndSalt,
          Office = userDto.Office,
          Country = this._countryRepository.GetById((object) userDto.CountryId),
          IsDeactivated = false
        };
        this._userRepository.TransactionalInsert(entity);
        Role byId = this._roleRepository.GetById((object) userDto.RoleId);
        this._userRoleRepository.TransactionalInsert(new UserRole()
        {
          User = entity,
          Role = byId
        });
        this._nhSession.Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public void CreateUser(
      UserEditDTO userDto,
      ObservableCollection<RoleDTO> roleList,
      string password)
    {
      try
      {
        this._nhSession.BeginTransaction();
        string hashAndSalt;
        new PasswordManager().GetPasswordHashAndSaltConcatenatedString(password, out hashAndSalt);
        User entity = new User()
        {
          FirstName = userDto.FirstName,
          LastName = userDto.LastName,
          Username = userDto.Username,
          Password = hashAndSalt,
          Office = userDto.Office,
          Country = this._countryRepository.GetById((object) userDto.CountryId),
          IsDeactivated = false,
          Language = userDto.Language
        };
        this._userRepository.TransactionalInsert(entity);
        foreach (RoleDTO role in (Collection<RoleDTO>) roleList)
        {
          Role byId = this._roleRepository.GetById((object) role.Id);
          this._userRoleRepository.TransactionalInsert(new UserRole()
          {
            User = entity,
            Role = byId
          });
        }
        this._nhSession.Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public void EditUser(
      UserEditDTO userDto,
      ObservableCollection<RoleDTO> roleList,
      string password)
    {
      try
      {
        this._nhSession.FlushMode = FlushMode.Commit;
        this._nhSession.BeginTransaction();
        User user = this._userRepository.GetById((object) userDto.Id);
        if (user != null)
        {
          user.FirstName = userDto.FirstName;
          user.LastName = userDto.LastName;
          user.Office = userDto.Office;
          user.Language = userDto.Language;
          user.Country = this._countryRepository.GetById((object) userDto.CountryId);
          if (!string.IsNullOrEmpty(password))
          {
            string hashAndSalt;
            new PasswordManager().GetPasswordHashAndSaltConcatenatedString(password, out hashAndSalt);
            user.Password = hashAndSalt;
          }
          this._userRepository.TransactionalUpdate(user);
          foreach (UserRole entity in user.UserRoles.Where<UserRole>((Func<UserRole, bool>) (ur => roleList.All<RoleDTO>((Func<RoleDTO, bool>) (r => r.Id != ur.Role.Id)))).ToList<UserRole>())
            this._userRoleRepository.TransactionalDelete(entity);
          foreach (RoleDTO roleDto in roleList.Where<RoleDTO>((Func<RoleDTO, bool>) (r => user.UserRoles.All<UserRole>((Func<UserRole, bool>) (ur => ur.Role.Id != r.Id)))))
          {
            Role byId = this._roleRepository.GetById((object) roleDto.Id);
            this._userRoleRepository.TransactionalInsert(new UserRole()
            {
              User = user,
              Role = byId
            });
          }
        }
        this._nhSession.Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public void EditUser(UserEditDTO userDto, string password)
    {
      try
      {
        this._nhSession.FlushMode = FlushMode.Commit;
        this._nhSession.BeginTransaction();
        User byId1 = this._userRepository.GetById((object) userDto.Id);
        if (byId1 != null)
        {
          byId1.FirstName = userDto.FirstName;
          byId1.LastName = userDto.LastName;
          byId1.Office = userDto.Office;
          byId1.Country = this._countryRepository.GetById((object) userDto.CountryId);
          if (!string.IsNullOrEmpty(password))
          {
            string hashAndSalt;
            new PasswordManager().GetPasswordHashAndSaltConcatenatedString(password, out hashAndSalt);
            byId1.Password = hashAndSalt;
          }
          this._userRepository.TransactionalUpdate(byId1);
          UserRole entity1 = this._userRoleRepository.SearchFor((Expression<Func<UserRole, bool>>) (ur => ur.User.Id == userDto.Id)).FirstOrDefault<UserRole>();
          if (entity1 != null)
          {
            this._userRoleRepository.TransactionalDelete(entity1);
            Role byId2 = this._roleRepository.GetById((object) userDto.RoleId);
            this._userRoleRepository.TransactionalInsert(new UserRole()
            {
              User = byId1,
              Role = byId2
            });
          }
          else
          {
            UserRole entity2 = new UserRole();
            entity2.User = byId1;
            entity2.Role = this._roleRepository.FirstOrDefault((Expression<Func<Role, bool>>) (r => r.Id == userDto.RoleId));
            this._userRoleRepository.TransactionalInsert(entity2);
          }
        }
        this._nhSession.Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public void DeleteUser(Guid userId)
    {
      User byId = this._userRepository.GetById((object) userId);
      byId.IsDeactivated = true;
      this._userRepository.Update(byId);
    }

    public void SetLanguage(Guid userId, string language)
    {
      User byId = this._userRepository.GetById((object) userId);
      byId.Language = language;
      this._userRepository.Update(byId);
    }

    public bool HasRight(string operation)
    {
      return MSS.Business.Utils.AppContext.Current.LoggedUser != null && MSS.Business.Utils.AppContext.Current.Operations.Count<string>((Func<string, bool>) (op => op == operation)) > 0;
    }

    public bool ValidateUsername(string username, out ICollection<string> validationErrors)
    {
      validationErrors = (ICollection<string>) new List<string>();
      if (this.IsUsernameInUse(username))
        validationErrors.Add(Resources.MSS_Client_UserControl_Dialog_UsernameExisting);
      if (username.StartsWith("default"))
        validationErrors.Add(Resources.MSS_Client_UserControl_Dialog_Username_Default);
      return validationErrors.Count == 0;
    }

    public IEnumerable<UserDTO> GetUsersDTO()
    {
      List<UserDTO> users = new List<UserDTO>();
      TypeHelperExtensionMethods.ForEach<User>(this._userRepository.GetAllUsers().Where<User>((Func<User, bool>) (u => !u.IsDeactivated)), (Action<User>) (u => users.Add(new UserDTO()
      {
        FirstName = u.FirstName,
        Id = u.Id,
        LastName = u.LastName,
        Role = this.GetRoles(u.UserRoles),
        Username = u.Username,
        Office = u.Office,
        Country = this.GetCountry(u.Country),
        Language = u.Language,
        CountryId = u.Country != null ? u.Country.Id : Guid.Empty
      })));
      return (IEnumerable<UserDTO>) users;
    }

    public IEnumerable<UserDTO> SearchUserDTO(string searchText)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UsersManager.\u003C\u003Ec__DisplayClass28_0 cDisplayClass280 = new UsersManager.\u003C\u003Ec__DisplayClass28_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass280.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass280.searchText = searchText;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      IList<User> userList = this._userRepository.SearchFor((Expression<Func<User, bool>>) (s => s.FirstName.Contains(cDisplayClass280.searchText) || s.LastName.Contains(cDisplayClass280.searchText) || s.UserRoles.First<UserRole>((Func<UserRole, bool>) (ur => ur.Role.Name.Contains(cDisplayClass280.searchText))) != default (object) || s.Username.Contains(cDisplayClass280.searchText) && !s.IsDeactivated));
      // ISSUE: reference to a compiler-generated field
      cDisplayClass280.userDtos = new List<UserDTO>();
      // ISSUE: reference to a compiler-generated method
      TypeHelperExtensionMethods.ForEach<User>((IEnumerable<User>) userList, new Action<User>(cDisplayClass280.\u003CSearchUserDTO\u003Eb__0));
      // ISSUE: reference to a compiler-generated field
      return (IEnumerable<UserDTO>) cDisplayClass280.userDtos;
    }
  }
}
