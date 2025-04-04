// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.ServiceRecord
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public sealed class ServiceRecord : IEnumerable<ServiceAttribute>, IEnumerable
  {
    public const string ErrorMsgNotSeq = "LanguageBaseList attribute not of type ElementSequence.";
    public const string ErrorMsgNoAttributeWithId = "No Service Attribute with that ID.";
    public const string ErrorMsgListContainsNotAttribute = "The list contains a element which is not a ServiceAttribute.";
    private IList<ServiceAttribute> m_attributes;
    private byte[] m_srcBytes;

    public ServiceRecord()
    {
      this.m_attributes = (IList<ServiceAttribute>) new List<ServiceAttribute>();
    }

    public ServiceRecord(IList<ServiceAttribute> attributesList)
    {
      this.m_attributes = attributesList != null ? attributesList : throw new ArgumentNullException(nameof (attributesList));
    }

    public ServiceRecord(params ServiceAttribute[] attributesList)
      : this((IList<ServiceAttribute>) attributesList)
    {
    }

    public static ServiceRecord CreateServiceRecordFromBytes(byte[] recordBytes)
    {
      return recordBytes != null ? new ServiceRecordParser().Parse(recordBytes) : throw new ArgumentNullException(nameof (recordBytes));
    }

    public int Count
    {
      [DebuggerStepThrough] get => this.m_attributes.Count;
    }

    public ServiceAttribute this[int index] => this.GetAttributeByIndex(index);

    public ServiceAttribute GetAttributeByIndex(int index) => this.m_attributes[index];

    public bool Contains(ServiceAttributeId id)
    {
      return this.TryGetAttributeById(id, out ServiceAttribute _);
    }

    public ServiceAttribute GetAttributeById(ServiceAttributeId id)
    {
      ServiceAttribute attribute;
      if (!this.TryGetAttributeById(id, out attribute))
        throw new KeyNotFoundException("No Service Attribute with that ID.");
      return attribute;
    }

    private bool TryGetAttributeById(ServiceAttributeId id, out ServiceAttribute attribute)
    {
      foreach (ServiceAttribute attribute1 in (IEnumerable<ServiceAttribute>) this.m_attributes)
      {
        if (attribute1.Id == id)
        {
          attribute = attribute1;
          return true;
        }
      }
      attribute = (ServiceAttribute) null;
      return false;
    }

    public IList<ServiceAttributeId> AttributeIds
    {
      get
      {
        ServiceAttributeId[] attributeIds = new ServiceAttributeId[this.Count];
        int index = 0;
        foreach (ServiceAttribute attribute in (IEnumerable<ServiceAttribute>) this.m_attributes)
        {
          attributeIds[index] = attribute.Id;
          ++index;
        }
        return (IList<ServiceAttributeId>) attributeIds;
      }
    }

    public bool Contains(ServiceAttributeId id, LanguageBaseItem language)
    {
      if (language == null)
        throw new ArgumentNullException(nameof (language));
      ServiceAttribute attribute;
      return this.TryGetAttributeById(ServiceRecord.CreateLanguageBasedAttributeId(id, language.AttributeIdBase), out attribute) && attribute.Value.ElementType == ElementType.TextString;
    }

    public ServiceAttribute GetAttributeById(ServiceAttributeId id, LanguageBaseItem language)
    {
      if (language == null)
        throw new ArgumentNullException(nameof (language));
      return this.GetAttributeById(ServiceRecord.CreateLanguageBasedAttributeId(id, language.AttributeIdBase));
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static ServiceAttributeId CreateLanguageBasedAttributeId(
      ServiceAttributeId id,
      ServiceAttributeId baseId)
    {
      short num = (short) baseId;
      ServiceAttributeId basedAttributeId = id + num;
      if (((basedAttributeId < (ServiceAttributeId) 0 ? 1 : 0) ^ (id < (ServiceAttributeId) 0 ? 1 : (baseId < (ServiceAttributeId) 0 ? 1 : 0))) != 0)
        throw new OverflowException();
      return basedAttributeId;
    }

    public string GetMultiLanguageStringAttributeById(
      ServiceAttributeId id,
      LanguageBaseItem language)
    {
      if (language == null)
        throw new ArgumentNullException(nameof (language));
      return this.GetAttributeById(ServiceRecord.CreateLanguageBasedAttributeId(id, language.AttributeIdBase)).Value.GetValueAsString(language);
    }

    public string GetPrimaryMultiLanguageStringAttributeById(ServiceAttributeId id)
    {
      LanguageBaseItem language = this.GetPrimaryLanguageBaseItem() ?? LanguageBaseItem.CreateEnglishUtf8PrimaryLanguageItem();
      return this.GetMultiLanguageStringAttributeById(id, language);
    }

    public LanguageBaseItem[] GetLanguageBaseList()
    {
      if (!this.Contains((ServiceAttributeId) 6))
        return new LanguageBaseItem[0];
      ServiceAttribute attributeById = this.GetAttributeById((ServiceAttributeId) 6);
      if (attributeById.Value.ElementType != ElementType.ElementSequence)
        return new LanguageBaseItem[0];
      try
      {
        return LanguageBaseItem.ParseListFromElementSequence(attributeById.Value);
      }
      catch (ProtocolViolationException ex)
      {
        return new LanguageBaseItem[0];
      }
    }

    public LanguageBaseItem GetPrimaryLanguageBaseItem()
    {
      foreach (LanguageBaseItem languageBase in this.GetLanguageBaseList())
      {
        if (languageBase.AttributeIdBase == (ServiceAttributeId) 256)
          return languageBase;
      }
      return (LanguageBaseItem) null;
    }

    public IEnumerator<ServiceAttribute> GetEnumerator()
    {
      return (IEnumerator<ServiceAttribute>) new ServiceRecord.ServiceRecordEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) new ServiceRecord.ServiceRecordEnumerator(this);
    }

    internal void SetSourceBytes(byte[] recordBytes)
    {
      this.m_srcBytes = (byte[]) recordBytes.Clone();
    }

    public byte[] SourceBytes => this.m_srcBytes;

    public byte[] ToByteArray() => new ServiceRecordCreator().CreateServiceRecord(this);

    internal sealed class ServiceRecordEnumerator : 
      IEnumerator<ServiceAttribute>,
      IDisposable,
      IEnumerator
    {
      private ServiceRecord m_record;
      private int m_currentIndex = -1;

      internal ServiceRecordEnumerator(ServiceRecord record) => this.m_record = record;

      public ServiceAttribute Current
      {
        get
        {
          if (this.m_record == null)
            throw new ObjectDisposedException(this.GetType().Name);
          if (this.m_currentIndex < 0)
            throw new InvalidOperationException();
          if (this.m_currentIndex >= this.m_record.Count)
            throw new InvalidOperationException();
          return this.m_record[this.m_currentIndex];
        }
      }

      public void Dispose() => this.m_record = (ServiceRecord) null;

      object IEnumerator.Current => (object) this.Current;

      public bool MoveNext()
      {
        if (this.m_record == null)
          throw new ObjectDisposedException(this.GetType().Name);
        ++this.m_currentIndex;
        if (this.m_currentIndex >= 0 && this.m_currentIndex < this.m_record.Count)
          return true;
        this.m_currentIndex = this.m_record.Count;
        return false;
      }

      public void Reset()
      {
        if (this.m_record == null)
          throw new ObjectDisposedException(this.GetType().Name);
        this.m_currentIndex = -1;
      }
    }
  }
}
