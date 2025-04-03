// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.UsersManagement.RoleManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.UsersManagement;
using MSS.DTO.Users;
using MSS.Interfaces;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;

#nullable disable
namespace MSS.Business.Modules.UsersManagement
{
  public class RoleManager
  {
    private readonly IRepository<Operation> _operationRepository;
    private readonly IRepository<RoleOperation> _roleOperationRepository;
    private readonly IRepository<Role> _roleRepository;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IRepositoryFactory _repositoryFactory;

    public RoleManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._userRoleRepository = repositoryFactory.GetRepository<UserRole>();
      this._operationRepository = repositoryFactory.GetRepository<Operation>();
      this._roleOperationRepository = repositoryFactory.GetRepository<RoleOperation>();
      this._roleRepository = repositoryFactory.GetRepository<Role>();
    }

    public IEnumerable<RoleDTO> SearchRoleDTO(string searchText)
    {
      IList<Role> source = this._roleRepository.SearchFor((Expression<Func<Role, bool>>) (s => s.Name.Contains(searchText) && !s.IsDeactivated));
      List<RoleDTO> roleCollection = new List<RoleDTO>();
      TypeHelperExtensionMethods.ForEach<Role>(source.Where<Role>((Func<Role, bool>) (r => !r.IsDeactivated)), (Action<Role>) (r => roleCollection.Add(new RoleDTO()
      {
        Id = r.Id,
        IsStandard = r.IsStandard,
        Name = r.Name
      })));
      roleCollection = roleCollection.OrderBy<RoleDTO, string>((Func<RoleDTO, string>) (x => x.Name)).ToList<RoleDTO>();
      return (IEnumerable<RoleDTO>) roleCollection;
    }

    public IEnumerable<RoleDTO> GetRolesDTO()
    {
      List<RoleDTO> roleCollection = new List<RoleDTO>();
      TypeHelperExtensionMethods.ForEach<Role>(this._roleRepository.GetAll().Where<Role>((Func<Role, bool>) (r => !r.IsDeactivated)), (Action<Role>) (r => roleCollection.Add(new RoleDTO()
      {
        Id = r.Id,
        IsStandard = r.IsStandard,
        Name = r.Name
      })));
      roleCollection = roleCollection.OrderBy<RoleDTO, string>((Func<RoleDTO, string>) (x => x.Name)).ToList<RoleDTO>();
      return (IEnumerable<RoleDTO>) roleCollection;
    }

    public IEnumerable<OperationDTO> GetOperationsDTO()
    {
      List<OperationDTO> operationCollection = new List<OperationDTO>();
      TypeHelperExtensionMethods.ForEach<Operation>((IEnumerable<Operation>) this._operationRepository.GetAll(), (Action<Operation>) (o => operationCollection.Add(new OperationDTO()
      {
        Id = o.Id,
        Name = o.Name
      })));
      return (IEnumerable<OperationDTO>) operationCollection;
    }

    public IEnumerable<Role> GetRole() => (IEnumerable<Role>) this._roleRepository.GetAll();

    public IEnumerable<Operation> GetOperations()
    {
      return (IEnumerable<Operation>) this._operationRepository.GetAll();
    }

    public IEnumerable<RoleOperation> GetRoleOperations()
    {
      return (IEnumerable<RoleOperation>) this._roleOperationRepository.GetAll();
    }

    public void CreateRole(RoleDTO roleDto, IEnumerable<OperationDTO> roleOperationList)
    {
      try
      {
        this._repositoryFactory.GetSession().BeginTransaction();
        Role role = new Role()
        {
          IsStandard = roleDto.IsStandard,
          Name = roleDto.Name,
          IsDeactivated = false
        };
        this._roleRepository.TransactionalInsert(role);
        foreach (RoleOperation entity in roleOperationList.Select<OperationDTO, Operation>((Func<OperationDTO, Operation>) (operationModel => this._operationRepository.GetById((object) operationModel.Id))).Select<Operation, RoleOperation>((Func<Operation, RoleOperation>) (operationValue => new RoleOperation()
        {
          Role = role,
          Operation = operationValue
        })))
          this._roleOperationRepository.TransactionalInsert(entity);
        this._repositoryFactory.GetSession().Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._repositoryFactory.GetSession().Transaction.Rollback();
        throw;
      }
    }

    public void EditRole(RoleDTO roleDto, IEnumerable<OperationDTO> operationsList)
    {
      try
      {
        this._repositoryFactory.GetSession().BeginTransaction();
        Role role = this._roleRepository.GetById((object) roleDto.Id);
        if (role != null)
        {
          role.Name = roleDto.Name;
          this._roleRepository.TransactionalUpdate(role);
          foreach (RoleOperation entity in role.RoleOperations.Where<RoleOperation>((Func<RoleOperation, bool>) (ro => operationsList.All<OperationDTO>((Func<OperationDTO, bool>) (r => r.Id != ro.Operation.Id)))).ToList<RoleOperation>())
            this._roleOperationRepository.TransactionalDelete(entity);
          foreach (OperationDTO operationDto in operationsList.Where<OperationDTO>((Func<OperationDTO, bool>) (o => role.RoleOperations.All<RoleOperation>((Func<RoleOperation, bool>) (ro => ro.Operation.Id != o.Id)))))
          {
            Operation byId = this._operationRepository.GetById((object) operationDto.Id);
            this._roleOperationRepository.TransactionalInsert(new RoleOperation()
            {
              Role = role,
              Operation = byId
            });
          }
        }
        this._repositoryFactory.GetSession().Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._repositoryFactory.GetSession().Transaction.Rollback();
        throw;
      }
    }

    public bool DeleteRole(Guid roleId)
    {
      bool flag1 = false;
      bool flag2 = false;
      try
      {
        this._repositoryFactory.GetSession().BeginTransaction();
        Role role = this._roleRepository.GetById((object) roleId);
        if (this._userRoleRepository.GetAll().ToList<UserRole>().Where<UserRole>((Func<UserRole, bool>) (userRole => userRole.Role != null && userRole.Role.Id == role.Id && userRole.User != null && !userRole.User.IsDeactivated)).Count<UserRole>() != 0)
        {
          flag1 = true;
          throw new Exception();
        }
        role.IsDeactivated = true;
        this._roleRepository.TransactionalUpdate(role);
        this._repositoryFactory.GetSession().Transaction.Commit();
        flag2 = true;
      }
      catch (Exception ex)
      {
        if (flag1)
        {
          int num = (int) MessageBox.Show("Error. There are users assigned to this role");
        }
        this._repositoryFactory.GetSession().Transaction.Rollback();
      }
      return flag2;
    }
  }
}
