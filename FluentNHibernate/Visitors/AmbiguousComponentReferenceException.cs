// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Visitors.AmbiguousComponentReferenceException
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Visitors
{
  [Serializable]
  public class AmbiguousComponentReferenceException : Exception
  {
    public AmbiguousComponentReferenceException(
      Type referencedComponentType,
      Type sourceType,
      Member sourceMember)
      : base(string.Format("Multiple external components for '{0}', referenced from property '{1}' of '{2}', unable to continue.", (object) referencedComponentType.Name, (object) sourceMember.Name, (object) sourceType.Name))
    {
      this.ReferencedComponentType = referencedComponentType;
      this.SourceType = sourceType;
      this.SourceMember = sourceMember;
    }

    public Type ReferencedComponentType { get; private set; }

    public Type SourceType { get; private set; }

    public Member SourceMember { get; private set; }
  }
}
