// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.MetaType
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Collections.Generic;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class MetaType
  {
    private readonly ICollection<MetaProperty> properties = (ICollection<MetaProperty>) new TypeElementCollection<MetaProperty>();
    private readonly ICollection<MetaEvent> events = (ICollection<MetaEvent>) new TypeElementCollection<MetaEvent>();
    private readonly ICollection<MetaMethod> methods = (ICollection<MetaMethod>) new TypeElementCollection<MetaMethod>();

    public IEnumerable<MetaMethod> Methods => (IEnumerable<MetaMethod>) this.methods;

    public IEnumerable<MetaProperty> Properties => (IEnumerable<MetaProperty>) this.properties;

    public IEnumerable<MetaEvent> Events => (IEnumerable<MetaEvent>) this.events;

    public void AddMethod(MetaMethod method) => this.methods.Add(method);

    public void AddEvent(MetaEvent @event) => this.events.Add(@event);

    public void AddProperty(MetaProperty property) => this.properties.Add(property);
  }
}
