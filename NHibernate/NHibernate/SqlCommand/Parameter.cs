// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.Parameter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.SqlCommand
{
  [Serializable]
  public class Parameter
  {
    private int? parameterPosition;

    private Parameter()
    {
    }

    public int? ParameterPosition
    {
      get => this.parameterPosition;
      set => this.parameterPosition = value;
    }

    public object BackTrack { get; set; }

    public static Parameter Placeholder => new Parameter();

    public static Parameter WithIndex(int position)
    {
      return new Parameter()
      {
        ParameterPosition = new int?(position)
      };
    }

    public Parameter Clone()
    {
      return new Parameter() { BackTrack = this.BackTrack };
    }

    public static Parameter[] GenerateParameters(int count)
    {
      Parameter[] parameters = new Parameter[count];
      for (int index = 0; index < count; ++index)
        parameters[index] = Parameter.Placeholder;
      return parameters;
    }

    public override bool Equals(object obj) => obj is Parameter;

    public override int GetHashCode() => 1337;

    public override string ToString() => "?";

    public static bool operator ==(Parameter a, Parameter b)
    {
      return object.Equals((object) a, (object) b);
    }

    public static bool operator ==(object a, Parameter b) => object.Equals(a, (object) b);

    public static bool operator ==(Parameter a, object b) => object.Equals((object) a, b);

    public static bool operator !=(Parameter a, object b) => !(a == b);

    public static bool operator !=(object a, Parameter b) => !(a == b);

    public static bool operator !=(Parameter a, Parameter b) => !(a == b);
  }
}
