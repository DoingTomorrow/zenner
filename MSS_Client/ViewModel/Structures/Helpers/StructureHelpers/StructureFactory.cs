// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.Helpers.StructureHelpers.StructureFactory
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Core.Model.Structures;

#nullable disable
namespace MSS_Client.ViewModel.Structures.Helpers.StructureHelpers
{
  public class StructureFactory
  {
    public static StructureBehaviour GetStructureBehaviour(
      StructureTypeEnum? structureType,
      bool isEdit)
    {
      StructureTypeEnum? nullable = structureType;
      StructureBehaviour structureBehaviour;
      if (nullable.HasValue)
      {
        switch (nullable.GetValueOrDefault())
        {
          case StructureTypeEnum.Physical:
            structureBehaviour = (StructureBehaviour) PhysicalStructureBehaviour.GetPhysicalStructureBehaviour(isEdit);
            goto label_6;
          case StructureTypeEnum.Logical:
            structureBehaviour = (StructureBehaviour) LogicalStructureBehaviour.GetLogicalStructureBehaviour(isEdit);
            goto label_6;
          case StructureTypeEnum.Fixed:
            structureBehaviour = (StructureBehaviour) FixedStructureBehavior.GetFixedStructureBehaviour(isEdit);
            goto label_6;
        }
      }
      structureBehaviour = (StructureBehaviour) null;
label_6:
      return structureBehaviour;
    }

    public static StructureBehaviour GetStructureBehaviour(StructureTypeEnum? structureType)
    {
      StructureTypeEnum? nullable = structureType;
      StructureBehaviour structureBehaviour;
      if (nullable.HasValue)
      {
        switch (nullable.GetValueOrDefault())
        {
          case StructureTypeEnum.Physical:
            structureBehaviour = (StructureBehaviour) new PhysicalStructureBehaviour();
            goto label_6;
          case StructureTypeEnum.Logical:
            structureBehaviour = (StructureBehaviour) new LogicalStructureBehaviour();
            goto label_6;
          case StructureTypeEnum.Fixed:
            structureBehaviour = (StructureBehaviour) new FixedStructureBehavior();
            goto label_6;
        }
      }
      structureBehaviour = (StructureBehaviour) null;
label_6:
      return structureBehaviour;
    }
  }
}
