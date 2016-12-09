using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObservableLogging.Sample;

namespace ObservableLogging
{
  class Program
  {
    static void Main(string[] args)
    {
      var log = new SimpleLog();
      log.AsObservable().Subscribe(new DefaultLogObserver());
      log.AsObservable().Subscribe(new InventoryMetrics());
      Run(log).Wait();
    }

    static async Task Run(ILog log)
    {
      var random = new Random();
      while (true)
      {
        log.ImportStarted();
        await Task.Delay(TimeSpan.FromSeconds(random.Next(1,10)));
        var succeeded = random.NextDouble() > 0.5;;
        if (succeeded)
        {
          log.ImportFinished();
        }
        else
        {
          log.ImportFailed(new Exception("An Error Happened"));
        }
        await Task.Delay(TimeSpan.FromSeconds(random.Next(1,10)));
      }
    }
  }
}
