using System.Net.Http;

namespace MIGA.Telemetry
{
  public interface IProcessTelemetryEvent
  {
    void ProcessEvent(IConfiguration context);
    TelemetryEvent Event { get;}
  }
}