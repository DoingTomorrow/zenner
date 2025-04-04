// Decompiled with JetBrains decompiler
// Type: System.Reactive.Constants_Core
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

#nullable disable
namespace System.Reactive
{
  internal class Constants_Core
  {
    private const string OBSOLETE_REFACTORING = "This property is no longer supported due to refactoring of the API surface and elimination of platform-specific dependencies.";
    public const string OBSOLETE_SCHEDULER_NEWTHREAD = "This property is no longer supported due to refactoring of the API surface and elimination of platform-specific dependencies. Please add a reference to the System.Reactive.PlatformServices assembly for your target platform and use NewThreadScheduler.Default to obtain an instance of this scheduler type.";
    public const string OBSOLETE_SCHEDULER_TASKPOOL = "This property is no longer supported due to refactoring of the API surface and elimination of platform-specific dependencies. Please add a reference to the System.Reactive.PlatformServices assembly for your target platform and use TaskPoolScheduler.Default to obtain an instance of this scheduler type.";
    public const string OBSOLETE_SCHEDULER_THREADPOOL = "This property is no longer supported due to refactoring of the API surface and elimination of platform-specific dependencies. Consider using Scheduler.Default to obtain the platform's most appropriate pool-based scheduler. In order to access a specific pool-based scheduler, please add a reference to the System.Reactive.PlatformServices assembly for your target platform and use the appropriate scheduler in the System.Reactive.Concurrency namespace.";
    public const string OBSOLETE_SCHEDULEREQUIRED = "This instance property is no longer supported. Use CurrentThreadScheduler.IsScheduleRequired instead.";
  }
}
