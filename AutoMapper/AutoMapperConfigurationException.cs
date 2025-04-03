// Decompiled with JetBrains decompiler
// Type: AutoMapper.AutoMapperConfigurationException
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace AutoMapper
{
  public class AutoMapperConfigurationException : Exception
  {
    public AutoMapperConfigurationException.TypeMapConfigErrors[] Errors { get; private set; }

    public ResolutionContext Context { get; private set; }

    public AutoMapperConfigurationException(string message)
      : base(message)
    {
    }

    protected AutoMapperConfigurationException(string message, Exception inner)
      : base(message, inner)
    {
    }

    public AutoMapperConfigurationException(
      AutoMapperConfigurationException.TypeMapConfigErrors[] errors)
    {
      this.Errors = errors;
    }

    public AutoMapperConfigurationException(ResolutionContext context) => this.Context = context;

    public override string Message
    {
      get
      {
        if (this.Context != null)
        {
          ResolutionContext resolutionContext = this.Context;
          string str1 = string.Format("The following property on {0} cannot be mapped: \n\t{2}\nAdd a custom mapping expression, ignore, add a custom resolver, or modify the destination type {1}.", new object[3]
          {
            (object) resolutionContext.DestinationType.FullName,
            (object) resolutionContext.DestinationType.FullName,
            (object) resolutionContext.GetContextPropertyMap().DestinationProperty.Name
          }) + "\nContext:";
          for (; resolutionContext != null; resolutionContext = resolutionContext.Parent)
          {
            string str2 = str1;
            string str3;
            if (resolutionContext.GetContextPropertyMap() != null)
              str3 = string.Format("\n\tMapping to property {0} from {2} to {1}", new object[3]
              {
                (object) resolutionContext.GetContextPropertyMap().DestinationProperty.Name,
                (object) resolutionContext.DestinationType.FullName,
                (object) resolutionContext.SourceType.FullName
              });
            else
              str3 = string.Format("\n\tMapping from type {1} to {0}", new object[2]
              {
                (object) resolutionContext.DestinationType.FullName,
                (object) resolutionContext.SourceType.FullName
              });
            str1 = str2 + str3;
          }
          return str1 + "\n" + base.Message;
        }
        if (this.Errors == null)
          return base.Message;
        StringBuilder stringBuilder = new StringBuilder("\nUnmapped members were found. Review the types and members below.\nAdd a custom mapping expression, ignore, add a custom resolver, or modify the source/destination type\n");
        foreach (AutoMapperConfigurationException.TypeMapConfigErrors error in this.Errors)
        {
          int count = error.TypeMap.SourceType.FullName.Length + error.TypeMap.DestinationType.FullName.Length + 5;
          stringBuilder.AppendLine(new string('=', count));
          stringBuilder.AppendLine(error.TypeMap.SourceType.Name + " -> " + error.TypeMap.DestinationType.Name + " (" + (object) error.TypeMap.ConfiguredMemberList + " member list)");
          stringBuilder.AppendLine(error.TypeMap.SourceType.FullName + " -> " + error.TypeMap.DestinationType.FullName + " (" + (object) error.TypeMap.ConfiguredMemberList + " member list)");
          stringBuilder.AppendLine(new string('-', count));
          foreach (string unmappedPropertyName in error.UnmappedPropertyNames)
            stringBuilder.AppendLine(unmappedPropertyName);
        }
        return stringBuilder.ToString();
      }
    }

    public override string StackTrace
    {
      get
      {
        if (this.Errors == null)
          return base.StackTrace;
        return string.Join(Environment.NewLine, ((IEnumerable<string>) base.StackTrace.Split(new string[1]
        {
          Environment.NewLine
        }, StringSplitOptions.None)).Where<string>((Func<string, bool>) (str => !str.TrimStart().StartsWith("at AutoMapper."))).ToArray<string>());
      }
    }

    public class TypeMapConfigErrors
    {
      public TypeMap TypeMap { get; private set; }

      public string[] UnmappedPropertyNames { get; private set; }

      public TypeMapConfigErrors(TypeMap typeMap, string[] unmappedPropertyNames)
      {
        this.TypeMap = typeMap;
        this.UnmappedPropertyNames = unmappedPropertyNames;
      }
    }
  }
}
