// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.IdentifierProperty
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Tuple
{
  [Serializable]
  public class IdentifierProperty : Property
  {
    private readonly bool isVirtual;
    private readonly bool embedded;
    private readonly IdentifierValue unsavedValue;
    private readonly IIdentifierGenerator identifierGenerator;
    private readonly bool identifierAssignedByInsert;
    private readonly bool hasIdentifierMapper;

    public IdentifierProperty(
      string name,
      string node,
      IType type,
      bool embedded,
      IdentifierValue unsavedValue,
      IIdentifierGenerator identifierGenerator)
      : base(name, node, type)
    {
      this.isVirtual = false;
      this.embedded = embedded;
      this.hasIdentifierMapper = false;
      this.unsavedValue = unsavedValue;
      this.identifierGenerator = identifierGenerator;
      this.identifierAssignedByInsert = identifierGenerator is IPostInsertIdentifierGenerator;
    }

    public IdentifierProperty(
      IType type,
      bool embedded,
      bool hasIdentifierMapper,
      IdentifierValue unsavedValue,
      IIdentifierGenerator identifierGenerator)
      : base((string) null, (string) null, type)
    {
      this.isVirtual = true;
      this.embedded = embedded;
      this.hasIdentifierMapper = hasIdentifierMapper;
      this.unsavedValue = unsavedValue;
      this.identifierGenerator = identifierGenerator;
      this.identifierAssignedByInsert = identifierGenerator is IPostInsertIdentifierGenerator;
    }

    public bool IsVirtual => this.isVirtual;

    public bool IsEmbedded => this.embedded;

    public IdentifierValue UnsavedValue => this.unsavedValue;

    public IIdentifierGenerator IdentifierGenerator => this.identifierGenerator;

    public bool IsIdentifierAssignedByInsert => this.identifierAssignedByInsert;

    public bool HasIdentifierMapper => this.hasIdentifierMapper;
  }
}
