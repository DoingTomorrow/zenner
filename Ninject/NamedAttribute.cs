// Decompiled with JetBrains decompiler
// Type: Ninject.NamedAttribute
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using Ninject.Planning.Bindings;

#nullable disable
namespace Ninject
{
  public class NamedAttribute : ConstraintAttribute
  {
    public string Name { get; private set; }

    public NamedAttribute(string name)
    {
      Ensure.ArgumentNotNullOrEmpty(name, nameof (name));
      this.Name = name;
    }

    public override bool Matches(IBindingMetadata metadata)
    {
      Ensure.ArgumentNotNull((object) metadata, nameof (metadata));
      return metadata.Name == this.Name;
    }
  }
}
