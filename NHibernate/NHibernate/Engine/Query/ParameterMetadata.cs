// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.ParameterMetadata
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Engine.Query
{
  [Serializable]
  public class ParameterMetadata
  {
    private readonly Dictionary<string, NamedParameterDescriptor> namedDescriptorMap;
    private readonly OrdinalParameterDescriptor[] ordinalDescriptors;

    public ParameterMetadata(
      IEnumerable<OrdinalParameterDescriptor> ordinalDescriptors,
      IDictionary<string, NamedParameterDescriptor> namedDescriptorMap)
    {
      this.ordinalDescriptors = ordinalDescriptors == null ? Enumerable.Empty<OrdinalParameterDescriptor>().ToArray<OrdinalParameterDescriptor>() : ordinalDescriptors.ToArray<OrdinalParameterDescriptor>();
      this.namedDescriptorMap = namedDescriptorMap == null ? new Dictionary<string, NamedParameterDescriptor>(1) : new Dictionary<string, NamedParameterDescriptor>(namedDescriptorMap);
    }

    public int OrdinalParameterCount => this.ordinalDescriptors.Length;

    public ICollection<string> NamedParameterNames
    {
      get => (ICollection<string>) this.namedDescriptorMap.Keys;
    }

    public OrdinalParameterDescriptor GetOrdinalParameterDescriptor(int position)
    {
      if (position < 1 || position > this.ordinalDescriptors.Length)
        throw new IndexOutOfRangeException("Remember that ordinal parameters are 1-based!");
      return this.ordinalDescriptors[position - 1];
    }

    public IType GetOrdinalParameterExpectedType(int position)
    {
      return this.GetOrdinalParameterDescriptor(position).ExpectedType;
    }

    public NamedParameterDescriptor GetNamedParameterDescriptor(string name)
    {
      NamedParameterDescriptor parameterDescriptor;
      this.namedDescriptorMap.TryGetValue(name, out parameterDescriptor);
      return parameterDescriptor != null ? parameterDescriptor : throw new QueryParameterException("could not locate named parameter [" + name + "]");
    }

    public IType GetNamedParameterExpectedType(string name)
    {
      return this.GetNamedParameterDescriptor(name).ExpectedType;
    }
  }
}
