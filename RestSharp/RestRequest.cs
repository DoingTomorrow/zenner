// Decompiled with JetBrains decompiler
// Type: RestSharp.RestRequest
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

#nullable disable
namespace RestSharp
{
  public class RestRequest : IRestRequest
  {
    private Method _method;
    private DataFormat _requestFormat = DataFormat.Xml;
    private int _attempts;

    public ISerializer JsonSerializer { get; set; }

    public ISerializer XmlSerializer { get; set; }

    public RestRequest()
    {
      this.Parameters = new List<Parameter>();
      this.Files = new List<FileParameter>();
      this.XmlSerializer = (ISerializer) new RestSharp.Serializers.XmlSerializer();
      this.JsonSerializer = (ISerializer) new RestSharp.Serializers.JsonSerializer();
      this.OnBeforeDeserialization = (Action<IRestResponse>) (r => { });
    }

    public RestRequest(Method method)
      : this()
    {
      this.Method = method;
    }

    public RestRequest(string resource)
      : this(resource, Method.GET)
    {
    }

    public RestRequest(string resource, Method method)
      : this()
    {
      this.Resource = resource;
      this.Method = method;
    }

    public RestRequest(Uri resource)
      : this(resource, Method.GET)
    {
    }

    public RestRequest(Uri resource, Method method)
      : this(resource.IsAbsoluteUri ? resource.AbsolutePath + resource.Query : resource.OriginalString, method)
    {
    }

    public IRestRequest AddFile(string name, string path)
    {
      return this.AddFile(new FileParameter()
      {
        Name = name,
        FileName = Path.GetFileName(path),
        Writer = (Action<Stream>) (s =>
        {
          using (StreamReader streamReader = new StreamReader(path))
            streamReader.BaseStream.CopyTo(s);
        })
      });
    }

    public IRestRequest AddFile(string name, byte[] bytes, string fileName)
    {
      return this.AddFile(FileParameter.Create(name, bytes, fileName));
    }

    public IRestRequest AddFile(string name, byte[] bytes, string fileName, string contentType)
    {
      return this.AddFile(FileParameter.Create(name, bytes, fileName, contentType));
    }

    public IRestRequest AddFile(string name, Action<Stream> writer, string fileName)
    {
      return this.AddFile(name, writer, fileName, (string) null);
    }

    public IRestRequest AddFile(
      string name,
      Action<Stream> writer,
      string fileName,
      string contentType)
    {
      return this.AddFile(new FileParameter()
      {
        Name = name,
        Writer = writer,
        FileName = fileName,
        ContentType = contentType
      });
    }

    private IRestRequest AddFile(FileParameter file)
    {
      this.Files.Add(file);
      return (IRestRequest) this;
    }

    public IRestRequest AddBody(object obj, string xmlNamespace)
    {
      string str;
      string name;
      switch (this.RequestFormat)
      {
        case DataFormat.Json:
          str = this.JsonSerializer.Serialize(obj);
          name = this.JsonSerializer.ContentType;
          break;
        case DataFormat.Xml:
          this.XmlSerializer.Namespace = xmlNamespace;
          str = this.XmlSerializer.Serialize(obj);
          name = this.XmlSerializer.ContentType;
          break;
        default:
          str = "";
          name = "";
          break;
      }
      return this.AddParameter(name, (object) str, ParameterType.RequestBody);
    }

    public IRestRequest AddBody(object obj) => this.AddBody(obj, "");

    public IRestRequest AddObject(object obj, params string[] whitelist)
    {
      foreach (PropertyInfo property in obj.GetType().GetProperties())
      {
        if (whitelist.Length == 0 || whitelist.Length > 0 && ((IEnumerable<string>) whitelist).Contains<string>(property.Name))
        {
          Type propertyType = property.PropertyType;
          object obj1 = property.GetValue(obj, (object[]) null);
          if (obj1 != null)
          {
            if (propertyType.IsArray)
              obj1 = (object) string.Join(",", (string[]) obj1);
            this.AddParameter(property.Name, obj1);
          }
        }
      }
      return (IRestRequest) this;
    }

    public IRestRequest AddObject(object obj)
    {
      this.AddObject(obj, new string[0]);
      return (IRestRequest) this;
    }

    public IRestRequest AddParameter(Parameter p)
    {
      this.Parameters.Add(p);
      return (IRestRequest) this;
    }

    public IRestRequest AddParameter(string name, object value)
    {
      return this.AddParameter(new Parameter()
      {
        Name = name,
        Value = value,
        Type = ParameterType.GetOrPost
      });
    }

    public IRestRequest AddParameter(string name, object value, ParameterType type)
    {
      return this.AddParameter(new Parameter()
      {
        Name = name,
        Value = value,
        Type = type
      });
    }

    public IRestRequest AddHeader(string name, string value)
    {
      return this.AddParameter(name, (object) value, ParameterType.HttpHeader);
    }

    public IRestRequest AddCookie(string name, string value)
    {
      return this.AddParameter(name, (object) value, ParameterType.Cookie);
    }

    public IRestRequest AddUrlSegment(string name, string value)
    {
      return this.AddParameter(name, (object) value, ParameterType.UrlSegment);
    }

    public List<Parameter> Parameters { get; private set; }

    public List<FileParameter> Files { get; private set; }

    public Method Method
    {
      get => this._method;
      set => this._method = value;
    }

    public string Resource { get; set; }

    public DataFormat RequestFormat
    {
      get => this._requestFormat;
      set => this._requestFormat = value;
    }

    public string RootElement { get; set; }

    public Action<IRestResponse> OnBeforeDeserialization { get; set; }

    public string DateFormat { get; set; }

    public string XmlNamespace { get; set; }

    public ICredentials Credentials { get; set; }

    public object UserState { get; set; }

    public int Timeout { get; set; }

    public void IncreaseNumAttempts() => ++this._attempts;

    public int Attempts => this._attempts;
  }
}
