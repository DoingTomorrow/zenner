// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.AttributeValue
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Web.WebPages.Instrumentation;

#nullable disable
namespace System.Web.WebPages
{
  public class AttributeValue
  {
    public AttributeValue(
      PositionTagged<string> prefix,
      PositionTagged<object> value,
      bool literal)
    {
      this.Prefix = prefix;
      this.Value = value;
      this.Literal = literal;
    }

    public PositionTagged<string> Prefix { get; private set; }

    public PositionTagged<object> Value { get; private set; }

    public bool Literal { get; private set; }

    public static AttributeValue FromTuple(
      Tuple<Tuple<string, int>, Tuple<object, int>, bool> value)
    {
      return new AttributeValue((PositionTagged<string>) value.Item1, (PositionTagged<object>) value.Item2, value.Item3);
    }

    public static AttributeValue FromTuple(
      Tuple<Tuple<string, int>, Tuple<string, int>, bool> value)
    {
      return new AttributeValue((PositionTagged<string>) value.Item1, new PositionTagged<object>((object) value.Item2.Item1, value.Item2.Item2), value.Item3);
    }

    public static implicit operator AttributeValue(
      Tuple<Tuple<string, int>, Tuple<object, int>, bool> value)
    {
      return AttributeValue.FromTuple(value);
    }

    public static implicit operator AttributeValue(
      Tuple<Tuple<string, int>, Tuple<string, int>, bool> value)
    {
      return AttributeValue.FromTuple(value);
    }
  }
}
