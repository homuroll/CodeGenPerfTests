using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using GrobExp.Compiler;

namespace Linq
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            new Test3().Run_CyclesExpression();
            BenchmarkRunner.Run<Test3>(
                ManualConfig.Create(DefaultConfig.Instance)
                            .With(Job.LegacyJitX86)
                            .With(Job.LegacyJitX64)
                            .With(Job.RyuJitX64)
                            .With(Job.Mono));
        }
    }

    public static class ExpressionExtensions
    {
        public static bool IsLinkOfChain(this Expression node, bool rootOnlyParameter, bool hard)
        {
            return node != null &&
                   (node.NodeType == ExpressionType.Parameter
                    || (node.NodeType == ExpressionType.Constant && !rootOnlyParameter)
                    || IsLinkOfChain(node as MemberExpression, rootOnlyParameter, hard)
                    || IsLinkOfChain(node as BinaryExpression, rootOnlyParameter, hard)
                    || IsLinkOfChain(node as UnaryExpression, rootOnlyParameter, hard)
                    || IsLinkOfChain(node as MethodCallExpression, rootOnlyParameter, hard));
        }

        public static Expression[] SmashToSmithereens(this Expression exp)
        {
            var result = new List<Expression>();
            while(exp != null)
            {
                var end = false;
                result.Add(exp);
                switch(exp.NodeType)
                {
                case ExpressionType.MemberAccess:
                    exp = ((MemberExpression)exp).Expression;
                    break;
                case ExpressionType.ArrayIndex:
                    exp = ((BinaryExpression)exp).Left;
                    break;
                case ExpressionType.ArrayLength:
                    exp = ((UnaryExpression)exp).Operand;
                    break;
                case ExpressionType.Call:
                    var methodCallExpression = (MethodCallExpression)exp;
                    exp = methodCallExpression.Method.IsExtension() ? methodCallExpression.Arguments[0] : methodCallExpression.Object;
                    break;
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    exp = ((UnaryExpression)exp).Operand;
                    break;
                default:
                    end = true;
                    break;
                }
                if(end) break;
            }
            result.Reverse();
            return result.ToArray();
        }

        private static bool IsLinkOfChain(UnaryExpression node, bool rootOnlyParameter, bool hard)
        {
            if(hard)
                return node != null && node.NodeType == ExpressionType.Convert && IsLinkOfChain(node.Operand, rootOnlyParameter, true);
            return node != null && node.NodeType == ExpressionType.Convert;
        }

        private static bool IsLinkOfChain(MemberExpression node, bool rootOnlyParameter, bool hard)
        {
            if(hard)
                return node != null && IsLinkOfChain(node.Expression, rootOnlyParameter, true);
            return node != null && node.Expression != null;
        }

        private static bool IsExtension(MethodInfo method)
        {
            return method.IsExtension(); // && !(method.DeclaringType == typeof(Enumerable) && (method.Name == "ToArray" || method.Name == "ToList"));
        }

        private static bool IsLinkOfChain(MethodCallExpression node, bool rootOnlyParameter, bool hard)
        {
            if(node == null || !(node.Method.DeclaringType == typeof(Enumerable)))
                return false;
            if(hard)
                return (node.Object != null && IsLinkOfChain(node.Object, rootOnlyParameter, true)) || (IsExtension(node.Method) && IsLinkOfChain(node.Arguments[0], rootOnlyParameter, true));
            return node.Object != null || IsExtension(node.Method);
        }

        private static bool IsLinkOfChain(BinaryExpression node, bool rootOnlyParameter, bool hard)
        {
            if(hard)
                return node != null && node.NodeType == ExpressionType.ArrayIndex && IsLinkOfChain(node.Left, rootOnlyParameter, true);
            return node != null && node.NodeType == ExpressionType.ArrayIndex;
        }

        public static Type TryGetItemType(this Type type)
        {
            if(type.IsArray) return type.GetElementType();
            if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GetGenericArguments().Single();
            foreach(var t in type.GetInterfaces())
            {
                if(t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    return t.GetGenericArguments().Single();
            }
            if(type == typeof(IEnumerable))
                return typeof(object);
            if(type.GetInterfaces().Any(t => t == typeof(IEnumerable)))
                return typeof(object);
            return null;
        }

        public static Type GetItemType(this Type type)
        {
            var result = type.TryGetItemType();
            if(result == null)
                throw new ArgumentException("Unable to extract item type of '" + type + "'");
            return result;
        }

        public static Expression<TDelegate> EliminateLinq<TDelegate>(this Expression<TDelegate> expression)
        {
            return Expression.Lambda<TDelegate>(new LinqEliminator().Eliminate(expression.Body), expression.Parameters);
        }
    }

    public class ParameterReplacer : ExpressionVisitor
    {
        public ParameterReplacer(ParameterExpression from, ParameterExpression to)
        {
            this.to = to;
            this.from = from;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == from ? to : base.VisitParameter(node);
        }

        private readonly ParameterExpression to;
        private readonly ParameterExpression from;
    }
}