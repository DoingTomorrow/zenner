// Decompiled with JetBrains decompiler
// Type: NHibernate.Metadata.ICollectionMetadata
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;

#nullable disable
namespace NHibernate.Metadata
{
  public interface ICollectionMetadata
  {
    IType KeyType { get; }

    IType ElementType { get; }

    IType IndexType { get; }

    bool HasIndex { get; }

    string Role { get; }

    bool IsArray { get; }

    bool IsPrimitiveArray { get; }

    bool IsLazy { get; }
  }
}
