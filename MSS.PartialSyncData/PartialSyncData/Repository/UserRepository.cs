// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Repository.UserRepository
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using MSS.Core.Model.UsersManagement;
using MSS.Interfaces;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Repository
{
  public class UserRepository(ISession session) : 
    MSS.PartialSyncData.Repository.Repository<User>(session),
    IUserRepository,
    IRepository<User>
  {
    public List<User> GetAllUsers()
    {
      return this._session.Query<User>().FetchMany<User, UserRole>((Expression<Func<User, IEnumerable<UserRole>>>) (u => u.UserRoles)).ToList<User>();
    }

    public List<Operation> GetOperations(User user)
    {
      if (user == null)
        return (List<Operation>) null;
      List<Operation> mergedOperations = new List<Operation>();
      user.UserRoles.ForEach<UserRole>((Action<UserRole>) (ur => this._session.Query<RoleOperation>().Fetch<RoleOperation, Operation>((Expression<Func<RoleOperation, Operation>>) (ro => ro.Operation)).Where<RoleOperation>((Expression<Func<RoleOperation, bool>>) (ro => ro.Role.Id == ur.Role.Id)).Select<RoleOperation, Operation>((Expression<Func<RoleOperation, Operation>>) (ro => ro.Operation)).Distinct<Operation>().ToList<Operation>().ForEach((Action<Operation>) (o =>
      {
        if (mergedOperations.Contains(o))
          return;
        mergedOperations.Add(o);
      }))));
      return mergedOperations;
    }
  }
}
