// Decompiled with JetBrains decompiler
// Type: MSS.Interfaces.IUserRepository
// Assembly: MSS.Interfaces, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 178808BA-C10E-4054-B175-D79F79744EFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Interfaces.dll

using MSS.Core.Model.UsersManagement;
using System.Collections.Generic;

#nullable disable
namespace MSS.Interfaces
{
  public interface IUserRepository : IRepository<User>
  {
    List<User> GetAllUsers();

    List<Operation> GetOperations(User user);
  }
}
