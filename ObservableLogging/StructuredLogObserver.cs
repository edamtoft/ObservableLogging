using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObservableLogging
{
  public abstract class StructuredLogObserver : ILogObserver
  {
    private readonly Subject<MethodCallExpression> _subject;

    public StructuredLogObserver()
    {
      _subject = new Subject<MethodCallExpression>();
    }

    public void OnCompleted() => _subject.OnCompleted();
    public void OnError(Exception error) => _subject.OnError(error);
    public void OnNext(MethodCallExpression value) => _subject.OnNext(value);

    public void When(Action<ILog> whenMethodCalled, Expression<Action<ILog>> then) =>
      Subscribe(whenMethodCalled.Method, then);

    public void When<T1>(Action<ILog, T1> whenMethodCalled, Expression<Action<ILog,T1>> then) =>
      Subscribe(whenMethodCalled.Method, then);

    public void When<T1, T2>(Action<ILog, T1, T2> whenMethodCalled, Expression<Action<ILog,T1, T2>> then) =>
      Subscribe(whenMethodCalled.Method, then);

    // etc...

    private void Subscribe(MethodInfo when, Expression then)
    {
      // destructure and compile lambda of type (Action<Expression>), then subscribe it to _subject

      var e = Expression.Parameter(typeof(MethodCallExpression), "e"); // Yo dawg, I heard you like expressions
      var args = Expression.Property(e, nameof(MethodCallExpression.Arguments));
      var destructured = when.GetParameters()
        .Select((parameter, paramIndex) => {
          var i = Expression.Constant(paramIndex, typeof(int));
          var paramExp = Expression.Call(args, "get_Item", new Type[0], i);
          var paramConstExp = Expression.Convert(paramExp, typeof(ConstantExpression));
          var value = Expression.Property(paramConstExp, nameof(ConstantExpression.Value));
          return Expression.Convert(value, parameter.ParameterType);
        });
      var callThen = Expression.Invoke(then, destructured);
      var lambda = Expression.Lambda<Action<MethodCallExpression>>(callThen, e);
      var compiled = lambda.Compile();
      _subject
        .Where(call => call.Method == when)
        .Subscribe(compiled);
    }
  }
}
