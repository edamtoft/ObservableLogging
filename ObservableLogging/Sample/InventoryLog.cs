using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableLogging.Sample
{
  public static class InventoryLog
  {
    public static void ImportFinished(this ILog log) => log.Post();

    [Log("Import started", LogLevel.Debug)]
    public static void ImportStarted(this ILog log) => log.Post();

    public static void ImportFailed(this ILog log, Exception error) => log.Post(error);


    [Log("Import finished in {0}", LogLevel.Info)]
    public static void ImportFinished(this ILog log, TimeSpan span) => log.Post(span);

    [Log("Import finished in {0} (average: {1})", LogLevel.Info)]
    public static void ImportFinished(this ILog log, TimeSpan span, TimeSpan avg) => log.Post(span, avg);


    [Log("Import failed after {0}\n{1}", LogLevel.Error)]
    public static void ImportFailed(this ILog log, TimeSpan span, Exception err) => log.Post(span, err);
  }
}
