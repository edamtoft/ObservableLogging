using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableLogging
{
  [AttributeUsage(AttributeTargets.Method)]
  public class LogAttribute : Attribute
  {
    public readonly string Format;
    public readonly LogLevel Level;

    public LogAttribute(string format, LogLevel level = LogLevel.Debug)
    {
      Format = format;
      Level = level;
    }

    public LogAttribute(LogLevel level = LogLevel.Debug)
    {
      Format = null;
      Level = level;
    }
  }
}
