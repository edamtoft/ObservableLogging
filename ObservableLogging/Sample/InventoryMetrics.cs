using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableLogging.Sample
{
  /// <summary>
  /// Tracks inventory metrics and then dispatches those metrics along with new messages
  /// </summary>
  class InventoryMetrics : StructuredLogObserver
  {
    public InventoryMetrics()
    {
      When(InventoryLog.ImportStarted, log => Started(log));
      When(InventoryLog.ImportFinished, log => Finished(log));
      When(InventoryLog.ImportFailed, (ILog log, Exception err) => Error(log, err));
    }

    void Started(ILog log)
    {
      WhenStarted = DateTime.Now;
    }

    void Finished(ILog log)
    {
      var span = DateTime.Now - WhenStarted;
      log.ImportFinished(span);
    }

    void Error(ILog log, Exception err)
    {
      if (err is InvalidOperationException)
      {
        log.Error("Invalid Operation Exception");
        return;
      }
      var span = DateTime.Now - WhenStarted;
      log.ImportFailed(span, err);
    }


    private DateTime WhenStarted;
  }
}
