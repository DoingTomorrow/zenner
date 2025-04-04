// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.BasicLoader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Loader
{
  public abstract class BasicLoader : NHibernate.Loader.Loader
  {
    protected static readonly string[] NoSuffix = new string[1]
    {
      string.Empty
    };
    private IEntityAliases[] descriptors;
    private ICollectionAliases[] collectionDescriptors;

    public BasicLoader(ISessionFactoryImplementor factory)
      : base(factory)
    {
    }

    protected override sealed IEntityAliases[] EntityAliases => this.descriptors;

    protected override sealed ICollectionAliases[] CollectionAliases => this.collectionDescriptors;

    protected abstract string[] Suffixes { get; }

    protected abstract string[] CollectionSuffixes { get; }

    protected override void PostInstantiate()
    {
      ILoadable[] entityPersisters = this.EntityPersisters;
      string[] suffixes = this.Suffixes;
      this.descriptors = new IEntityAliases[entityPersisters.Length];
      for (int index = 0; index < this.descriptors.Length; ++index)
        this.descriptors[index] = (IEntityAliases) new DefaultEntityAliases(entityPersisters[index], suffixes[index]);
      ICollectionPersister[] collectionPersisters = this.CollectionPersisters;
      int num = 0;
      if (collectionPersisters != null)
      {
        string[] collectionSuffixes = this.CollectionSuffixes;
        this.collectionDescriptors = new ICollectionAliases[collectionPersisters.Length];
        for (int index = 0; index < collectionPersisters.Length; ++index)
        {
          if (BasicLoader.IsBag(collectionPersisters[index]))
            ++num;
          this.collectionDescriptors[index] = (ICollectionAliases) new GeneratedCollectionAliases(collectionPersisters[index], collectionSuffixes[index]);
        }
      }
      else
        this.collectionDescriptors = (ICollectionAliases[]) null;
      if (num > 1)
        throw new QueryException("Cannot simultaneously fetch multiple bags.");
    }

    private static bool IsBag(ICollectionPersister collectionPersister)
    {
      return collectionPersister.CollectionType.GetType().IsAssignableFrom(typeof (BagType));
    }

    public static string[] GenerateSuffixes(int length) => BasicLoader.GenerateSuffixes(0, length);

    public static string[] GenerateSuffixes(int seed, int length)
    {
      if (length == 0)
        return BasicLoader.NoSuffix;
      string[] suffixes = new string[length];
      for (int index = 0; index < length; ++index)
        suffixes[index] = (index + seed).ToString() + (object) '_';
      return suffixes;
    }
  }
}
