// Decompiled with JetBrains decompiler
// Type: MSS.CoreUsers.Model.UsersManagement.User
// Assembly: MSS.CoreUsers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A5E4896B-24D5-47E0-812E-3F88FF96B0B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.CoreUsers.dll

using MLS.Core.Model.Licensing;

#nullable disable
namespace MSS.CoreUsers.Model.UsersManagement
{
  public class User
  {
    public virtual int Id { get; set; }

    public virtual string FirstName { get; set; }

    public virtual string LastName { get; set; }

    public virtual string Username { get; set; }

    public virtual string Password { get; set; }

    public virtual Customer Customer { get; set; }
  }
}
