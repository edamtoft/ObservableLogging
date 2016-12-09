using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObservableLogging
{
  public static class Log
  {


    [Log("{0}",LogLevel.Verbose)]
    public static void Verbose(this ILog log, string message) => log.Post(message);

    [Log("{0}", LogLevel.Debug)]
    public static void Debug(this ILog log, string message) => log.Post(message);

    [Log("{0}", LogLevel.Info)]
    public static void Info(this ILog log, string message) => log.Post(message);

    [Log("{0}", LogLevel.Warning)]
    public static void Warn(this ILog log, string message) => log.Post(message);

    [Log("{0}\n{1}", LogLevel.Warning)]
    public static void Warn(this ILog log, string message, Exception err) => log.Post(message, err);

    [Log("{0}", LogLevel.Error)]
    public static void Error(this ILog log, string message) => log.Post(message);

    [Log("{0}\n{1}", LogLevel.Error)]
    public static void Error(this ILog log, string message, Exception err) => log.Post(message, err);






    public static void Post(this ILog log, params object[] args)
    {
      var frame = new StackFrame(1);
      var callingMethod = frame.GetMethod() as MethodInfo;
      var firstArg = new Expression[] { Expression.Constant(log, typeof(ILog)) };
      var restArgs = callingMethod.GetParameters()
        .Skip(1)
        .Select((param, i) => Expression.Constant(args[i], param.ParameterType));
      log.Push(Expression.Call(callingMethod, firstArg.Concat(restArgs)));
    }

    public static void Post<T1>(this ILog log, T1 arg1)
    {
      var frame = new StackFrame(1);
      var callingMethod = frame.GetMethod() as MethodInfo;
      var args = new Expression[] {
        Expression.Constant(log, typeof(ILog)),
        Expression.Constant(arg1, typeof(T1))
      };
      log.Push(Expression.Call(callingMethod, args));
    }

    public static void Post<T1, T2>(this ILog log, T1 arg1, T2 arg2)
    {
      var frame = new StackFrame(1);
      var callingMethod = frame.GetMethod() as MethodInfo;
      var args = new Expression[] {
        Expression.Constant(log, typeof(ILog)),
        Expression.Constant(arg1, typeof(T1)),
        Expression.Constant(arg2, typeof(T2))
      };
      log.Push(Expression.Call(callingMethod, args));
    }

    public static void Post<T1, T2, T3>(this ILog log, T1 arg1, T2 arg2, T3 arg3)
    {
      var frame = new StackFrame(1);
      var callingMethod = frame.GetMethod() as MethodInfo;
      var args = new Expression[] {
        Expression.Constant(log, typeof(ILog)),
        Expression.Constant(arg1, typeof(T1)),
        Expression.Constant(arg2, typeof(T2)),
        Expression.Constant(arg3, typeof(T3))
      };
      log.Push(Expression.Call(callingMethod, args));
    }
  }
}
