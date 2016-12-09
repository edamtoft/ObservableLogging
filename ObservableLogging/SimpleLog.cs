using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace ObservableLogging
{
  public class SimpleLog : ILog
  {
    private readonly Subject<MethodCallExpression> _subject;

    public SimpleLog()
    {
      _subject = new Subject<MethodCallExpression>();
    }

    public void Push(MethodCallExpression e) => _subject.OnNext(e);

    public IObservable<MethodCallExpression> AsObservable() => _subject;
  }
}
