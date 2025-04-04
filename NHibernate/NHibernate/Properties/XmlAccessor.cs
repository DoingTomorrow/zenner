// Decompiled with JetBrains decompiler
// Type: NHibernate.Properties.XmlAccessor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using System;
using System.Collections;
using System.Reflection;
using System.Xml;

#nullable disable
namespace NHibernate.Properties
{
  [Serializable]
  public class XmlAccessor : IPropertyAccessor
  {
    private readonly ISessionFactoryImplementor factory;
    private readonly string nodeName;
    private readonly IType propertyType;

    public XmlAccessor(string nodeName, IType propertyType, ISessionFactoryImplementor factory)
    {
      this.factory = factory;
      this.nodeName = nodeName;
      this.propertyType = propertyType;
    }

    public IGetter GetGetter(System.Type theClass, string propertyName)
    {
      if (this.nodeName == null)
        throw new MappingException("no node name for property: " + propertyName);
      if (".".Equals(this.nodeName))
        return (IGetter) new XmlAccessor.TextGetter(this.propertyType, this.factory);
      if (this.nodeName.IndexOf('/') > -1)
        return (IGetter) new XmlAccessor.ElementAttributeGetter(this.nodeName, this.propertyType, this.factory);
      return this.nodeName.IndexOf('@') > -1 ? (IGetter) new XmlAccessor.AttributeGetter(this.nodeName, this.propertyType, this.factory) : (IGetter) new XmlAccessor.ElementGetter(this.nodeName, this.propertyType, this.factory);
    }

    public ISetter GetSetter(System.Type theClass, string propertyName)
    {
      if (this.nodeName == null)
        throw new MappingException("no node name for property: " + propertyName);
      if (".".Equals(this.nodeName))
        return (ISetter) new XmlAccessor.TextSetter(this.propertyType);
      if (this.nodeName.IndexOf('/') > -1)
        return (ISetter) new XmlAccessor.ElementAttributeSetter(this.nodeName, this.propertyType);
      return this.nodeName.IndexOf('@') > -1 ? (ISetter) new XmlAccessor.AttributeSetter(this.nodeName, this.propertyType) : (ISetter) new XmlAccessor.ElementSetter(this.nodeName, this.propertyType);
    }

    public bool CanAccessThroughReflectionOptimizer => true;

    [Serializable]
    public abstract class XmlGetter : IGetter
    {
      protected ISessionFactoryImplementor factory;
      protected IType propertyType;

      internal XmlGetter(IType propertyType, ISessionFactoryImplementor factory)
      {
        this.propertyType = propertyType;
        this.factory = factory;
      }

      public virtual System.Type ReturnType => typeof (object);

      public virtual string PropertyName => (string) null;

      public virtual MethodInfo Method => (MethodInfo) null;

      public virtual object GetForInsert(
        object owner,
        IDictionary mergeMap,
        ISessionImplementor session)
      {
        return this.Get(owner);
      }

      public abstract object Get(object target);
    }

    [Serializable]
    public class AttributeGetter : XmlAccessor.XmlGetter
    {
      private readonly string attributeName;

      public AttributeGetter(string name, IType propertyType, ISessionFactoryImplementor factory)
        : base(propertyType, factory)
      {
        this.attributeName = name.Substring(1);
      }

      public override object Get(object owner)
      {
        XmlNode attribute = (XmlNode) ((XmlNode) owner).Attributes[this.attributeName];
        return attribute != null ? this.propertyType.FromXMLNode(attribute, (IMapping) this.factory) : (object) null;
      }
    }

    [Serializable]
    public abstract class XmlSetter : ISetter
    {
      protected internal IType propertyType;

      internal XmlSetter(IType propertyType) => this.propertyType = propertyType;

      public virtual string PropertyName => (string) null;

      public virtual MethodInfo Method => (MethodInfo) null;

      public abstract void Set(object target, object value);
    }

    [Serializable]
    public class AttributeSetter : XmlAccessor.XmlSetter
    {
      private readonly string attributeName;

      public AttributeSetter(string name, IType propertyType)
        : base(propertyType)
      {
        this.attributeName = name.Substring(1);
      }

      public override void Set(object target, object value)
      {
        XmlElement xmlElement = (XmlElement) target;
        XmlAttribute attribute = xmlElement.Attributes[this.attributeName];
        if (value == null)
        {
          if (attribute == null)
            return;
          xmlElement.Attributes.Remove(attribute);
        }
        else
        {
          if (attribute == null)
          {
            xmlElement.SetAttribute(this.attributeName, "null");
            attribute = xmlElement.Attributes[this.attributeName];
          }
          this.propertyType.SetToXMLNode((XmlNode) attribute, value, (ISessionFactoryImplementor) null);
        }
      }
    }

    [Serializable]
    public class ElementAttributeGetter : XmlAccessor.XmlGetter
    {
      private readonly string attributeName;
      private readonly string elementName;

      public ElementAttributeGetter(
        string name,
        IType propertyType,
        ISessionFactoryImplementor factory)
        : base(propertyType, factory)
      {
        this.elementName = name.Substring(0, name.IndexOf('/'));
        this.attributeName = name.Substring(name.IndexOf('/') + 2);
      }

      public override object Get(object owner)
      {
        XmlNode xmlNode = (XmlNode) ((XmlNode) owner)[this.elementName];
        if (xmlNode == null)
          return (object) null;
        XmlAttribute attribute = xmlNode.Attributes[this.attributeName];
        return attribute == null ? (object) null : this.propertyType.FromXMLNode((XmlNode) attribute, (IMapping) this.factory);
      }
    }

    [Serializable]
    public class ElementAttributeSetter : XmlAccessor.XmlSetter
    {
      private readonly string attributeName;
      private readonly string elementName;

      public ElementAttributeSetter(string name, IType propertyType)
        : base(propertyType)
      {
        this.elementName = name.Substring(0, name.IndexOf('/'));
        this.attributeName = name.Substring(name.IndexOf('/') + 2);
      }

      public override void Set(object target, object value)
      {
        XmlElement xmlElement = (XmlElement) target;
        XmlElement element = xmlElement[this.elementName];
        if (value == null)
        {
          if (element == null)
            return;
          xmlElement.RemoveChild((XmlNode) element);
        }
        else
        {
          XmlAttribute node;
          if (element == null)
          {
            element = xmlElement.OwnerDocument.CreateElement(this.elementName);
            xmlElement.AppendChild((XmlNode) element);
            node = (XmlAttribute) null;
          }
          else
            node = element.Attributes[this.attributeName];
          if (node == null)
          {
            element.SetAttribute(this.attributeName, "null");
            node = element.Attributes[this.attributeName];
          }
          this.propertyType.SetToXMLNode((XmlNode) node, value, (ISessionFactoryImplementor) null);
        }
      }
    }

    [Serializable]
    public class ElementGetter : XmlAccessor.XmlGetter
    {
      private readonly string elementName;

      public ElementGetter(string name, IType propertyType, ISessionFactoryImplementor factory)
        : base(propertyType, factory)
      {
        this.elementName = name;
      }

      public override object Get(object owner)
      {
        XmlNode xml = (XmlNode) ((XmlNode) owner)[this.elementName];
        return xml != null ? this.propertyType.FromXMLNode(xml, (IMapping) this.factory) : (object) null;
      }
    }

    [Serializable]
    public class ElementSetter : XmlAccessor.XmlSetter
    {
      private readonly string elementName;

      public ElementSetter(string name, IType propertyType)
        : base(propertyType)
      {
        this.elementName = name;
      }

      public override void Set(object target, object value)
      {
        if (value == CollectionType.UnfetchedCollection)
          return;
        XmlElement xmlElement = (XmlElement) target;
        XmlElement oldChild = xmlElement[this.elementName];
        if (oldChild != null)
          xmlElement.RemoveChild((XmlNode) oldChild);
        if (value == null)
          return;
        XmlElement element = xmlElement.OwnerDocument.CreateElement(this.elementName);
        xmlElement.AppendChild((XmlNode) element);
        this.propertyType.SetToXMLNode((XmlNode) element, value, (ISessionFactoryImplementor) null);
      }
    }

    [Serializable]
    public class TextGetter(IType propertyType, ISessionFactoryImplementor factory) : 
      XmlAccessor.XmlGetter(propertyType, factory)
    {
      public override object Get(object owner)
      {
        return this.propertyType.FromXMLNode((XmlNode) owner, (IMapping) this.factory);
      }
    }

    [Serializable]
    public class TextSetter(IType propertyType) : XmlAccessor.XmlSetter(propertyType)
    {
      public override void Set(object target, object value)
      {
        XmlNode node = (XmlNode) target;
        if (this.propertyType.IsXMLElement)
          return;
        if (value == null)
          node.InnerText = "";
        else
          this.propertyType.SetToXMLNode(node, value, (ISessionFactoryImplementor) null);
      }
    }
  }
}
