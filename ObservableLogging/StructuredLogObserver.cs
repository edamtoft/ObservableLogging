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

    public void When(Action<ILog> when, Action then) =>
      Subscribe(when.Method, then.Method);
    public void When(Action<ILog> when, Action<ILog> then) =>
      Subscribe(when.Method, then.Method);
    public void When<T1>(Action<ILog, T1> when, Action<T1> then) =>
      Subscribe(when.Method, then.Method);
    public void When<T1>(Action<ILog, T1> when, Action<ILog,T1> then) =>
      Subscribe(when.Method, then.Method);
    public void When<T1, T2>(Action<ILog, T1, T2> when, Action<T1, T2> then) =>
      Subscribe(when.Method, then.Method);
    public void When<T1, T2>(Action<ILog, T1, T2> when, Action<ILog,T1, T2> then) =>
      Subscribe(when.Method, then.Method);
    public void When<T1, T2, T3>(Action<ILog, T1, T2, T3> when, Action<T1, T2, T3> then) =>
      Subscribe(when.Method, then.Method);
    public void When<T1, T2, T3>(Action<ILog, T1, T2, T3> when, Action<ILog, T1, T2, T3> then) =>
      Subscribe(when.Method, then.Method);
    public void When<T1, T2, T3, T4>(Action<ILog, T1, T2, T3, T4> when, Action<T1, T2, T3, T4> then) =>
      Subscribe(when.Method, then.Method);
    public void When<T1, T2, T3, T4>(Action<ILog, T1, T2, T3, T4> when, Action<ILog, T1, T2, T3, T4> then) =>
      Subscribe(when.Method, then.Method);

    private void Subscribe(MethodInfo when, MethodInfo then)
    {
      // destructure and compile lambda of type (Action<Expression>), then subscribe it to _subject

      var e = Expression.Parameter(typeof(MethodCallExpression), "e"); // Yo dawg, I heard you like expressions
      var args = Expression.Property(e, nameof(MethodCallExpression.Arguments));
      var thenParameters = then.GetParameters();
      var includesLog = thenParameters.Length > 0 && thenParameters[0].ParameterType == typeof(ILog);
      var offset = includesLog ? 0 : 1;
      var destructured = when.GetParameters().Skip(offset)
        .Select((parameter, paramIndex) => {
          var i = Expression.Constant(paramIndex + offset, typeof(int));
          var paramExp = Expression.Call(args, "get_Item", new Type[0], i);
          var paramConstExp = Expression.Convert(paramExp, typeof(ConstantExpression));
          var value = Expression.Property(paramConstExp, nameof(ConstantExpression.Value));
          return Expression.Convert(value, parameter.ParameterType);
        });
      var callThen = Expression.Call(Expression.Constant(this, GetType()), then, destructured);
      var lambda = Expression.Lambda<Action<MethodCallExpression>>(callThen, e);
      _subject
        .Where(call => call.Method == when)
        .Subscribe(lambda.Compile());
    }
  }
}
