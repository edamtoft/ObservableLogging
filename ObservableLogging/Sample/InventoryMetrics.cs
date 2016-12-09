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

    private int _count;
    private double _totalSeconds;
    private DateTime _whenStarted;

    public InventoryMetrics()
    {
      When(InventoryLog.ImportStarted, () => _whenStarted = DateTime.Now);

      When(InventoryLog.ImportFinished, log =>
      {
        var span = DateTime.Now - _whenStarted;
        _count++;
        _totalSeconds += span.TotalSeconds;
        var avg = TimeSpan.FromSeconds(_totalSeconds / _count);
        log.ImportFinished(span, avg);
      });

      When(InventoryLog.ImportFailed, (ILog log, Exception err) =>
      {
        var span = DateTime.Now - _whenStarted;
        log.ImportFailed(span, err);
      });
    }
  }
}
