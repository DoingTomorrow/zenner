// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.ParamLocationRecognizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine.Query
{
  public class ParamLocationRecognizer : ParameterParser.IRecognizer
  {
    private readonly Dictionary<string, ParamLocationRecognizer.NamedParameterDescription> namedParameterDescriptions = new Dictionary<string, ParamLocationRecognizer.NamedParameterDescription>();
    private readonly List<int> ordinalParameterLocationList = new List<int>();

    public static ParamLocationRecognizer ParseLocations(string query)
    {
      ParamLocationRecognizer locations = new ParamLocationRecognizer();
      ParameterParser.Parse(query, (ParameterParser.IRecognizer) locations);
      return locations;
    }

    public IDictionary<string, ParamLocationRecognizer.NamedParameterDescription> NamedParameterDescriptionMap
    {
      get
      {
        return (IDictionary<string, ParamLocationRecognizer.NamedParameterDescription>) this.namedParameterDescriptions;
      }
    }

    public List<int> OrdinalParameterLocationList => this.ordinalParameterLocationList;

    public void OutParameter(int position)
    {
    }

    public void OrdinalParameter(int position) => this.ordinalParameterLocationList.Add(position);

    public void NamedParameter(string name, int position)
    {
      this.GetOrBuildNamedParameterDescription(name, false).Add(position);
    }

    public void JpaPositionalParameter(string name, int position)
    {
      this.GetOrBuildNamedParameterDescription(name, true).Add(position);
    }

    public void Other(char character)
    {
    }

    public void Other(string sqlPart)
    {
    }

    private ParamLocationRecognizer.NamedParameterDescription GetOrBuildNamedParameterDescription(
      string name,
      bool jpa)
    {
      ParamLocationRecognizer.NamedParameterDescription parameterDescription;
      this.namedParameterDescriptions.TryGetValue(name, out parameterDescription);
      if (parameterDescription == null)
      {
        parameterDescription = new ParamLocationRecognizer.NamedParameterDescription(jpa);
        this.namedParameterDescriptions[name] = parameterDescription;
      }
      return parameterDescription;
    }

    public class NamedParameterDescription
    {
      private readonly bool jpaStyle;
      private readonly List<int> positions = new List<int>();

      public NamedParameterDescription(bool jpaStyle) => this.jpaStyle = jpaStyle;

      internal void Add(int position) => this.positions.Add(position);

      public int[] BuildPositionsArray() => this.positions.ToArray();

      public bool JpaStyle => this.jpaStyle;
    }
  }
}
