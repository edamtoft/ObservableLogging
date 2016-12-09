using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObservableLogging
{
  class DefaultLogObserver : ILogObserver
  {
    public readonly LogLevel Level;

    public DefaultLogObserver(LogLevel level = LogLevel.Debug)
    {
      Level = level;
    }

    public void OnCompleted() { }
    public void OnError(Exception error) { }
    public void OnNext(MethodCallExpression call)
    {
      var options = call.Method.GetCustomAttribute<LogAttribute>();
      if (options == null || string.IsNullOrEmpty(options.Format) || (int)options.Level < (int)Level)
      {
        return;
      }
      var values = from arg in call.Arguments.Skip(1)
                   let constant = arg as ConstantExpression
                   where constant != null
                   select constant.Value;
      var formattedMessage = string.Format(options.Format, values.ToArray());
      var message = $"{DateTime.Now.ToShortTimeString()} ({options.Level}) [{call.Method.DeclaringType.Name}] {formattedMessage}";
      var previousColor = Console.ForegroundColor;
      Console.ForegroundColor = GetConsoleColor(options.Level);
      Console.WriteLine(message);
      Console.ForegroundColor = previousColor;
    }

    private ConsoleColor GetConsoleColor(LogLevel level)
    {
      switch (level)
      {
        case LogLevel.Verbose:
          return ConsoleColor.DarkGray;
        case LogLevel.Debug:
          return ConsoleColor.Gray;
        case LogLevel.Info:
          return ConsoleColor.White;
        case LogLevel.Warning:
          return ConsoleColor.Yellow;
        case LogLevel.Error:
        default:
          return ConsoleColor.Red;
      }
    }
  }
}
