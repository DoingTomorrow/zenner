// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.PrivateAutoPersistenceModel
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Automapping
{
  [Obsolete("Depreciated in favour of supplying your own IAutomappingConfiguration instance to AutoMap: AutoMap.AssemblyOf<T>(your_configuration_instance)")]
  public class PrivateAutoPersistenceModel : AutoPersistenceModel
  {
    public PrivateAutoPersistenceModel()
    {
      this.Setup((Action<AutoMappingExpressions>) (s =>
      {
        s.FindIdentity = new Func<Member, bool>(PrivateAutoPersistenceModel.findIdentity);
        s.FindMembers = new Func<Member, bool>(PrivateAutoPersistenceModel.findMembers);
      }));
    }

    private static bool findMembers(Member member) => member.IsField && member.IsPrivate;

    private static bool findIdentity(Member member)
    {
      return member.IsField && member.IsPrivate && member.Name == "id";
    }
  }
}
