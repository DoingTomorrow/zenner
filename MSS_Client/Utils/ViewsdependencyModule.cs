// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.ViewsdependencyModule
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS_Client.ViewModel.Users;
using Ninject.Modules;

#nullable disable
namespace MSS_Client.Utils
{
  public class ViewsdependencyModule : NinjectModule
  {
    public override void Load()
    {
      this.Bind<UsersViewModel>().ToSelf();
      this.Bind<CreateUserViewModel>().ToSelf();
      this.Bind<EditUserViewModel>().ToSelf();
      this.Bind<DeleteUserViewModel>().ToSelf();
    }
  }
}
