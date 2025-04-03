// Decompiled with JetBrains decompiler
// Type: Castle.Core.Logging.AbstractLoggerFactory
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.IO;

#nullable disable
namespace Castle.Core.Logging
{
  [Serializable]
  public abstract class AbstractLoggerFactory : MarshalByRefObject, ILoggerFactory
  {
    public virtual ILogger Create(Type type)
    {
      return type != null ? this.Create(type.FullName) : throw new ArgumentNullException(nameof (type));
    }

    public virtual ILogger Create(Type type, LoggerLevel level)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      return this.Create(type.FullName, level);
    }

    public abstract ILogger Create(string name);

    public abstract ILogger Create(string name, LoggerLevel level);

    protected static FileInfo GetConfigFile(string fileName)
    {
      return !Path.IsPathRooted(fileName) ? new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)) : new FileInfo(fileName);
    }
  }
}
