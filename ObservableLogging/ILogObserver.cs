using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ObservableLogging
{
  public interface ILogObserver : IObserver<MethodCallExpression> { };
}
