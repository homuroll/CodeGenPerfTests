using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BenchmarkDotNet.Attributes;

using GrobExp.Compiler;

namespace Linq
{
    public class Test2
    {
        public Test2()
        {
            list = new List<TestData>();
            for(int i = 0; i < 1000000; ++i)
                list.Add(new TestData {X = i});
            linq = x => x.Aggregate(0, (s, o) => s + o.X);
            loopsSharp = x =>
                {
                    int res = 0;
                    for(int i = 0; i < x.Count; i++)
                        res = res + x[i].X;
                    return res;
                };
            Expression<Func<List<TestData>, int>> exp = x => x.Aggregate(0, (s, o) => s + o.X);
            loopsExpression = LambdaCompiler.Compile(exp.EliminateLinq(), CompilerOptions.None);
        }

        [Benchmark(Baseline = true)]
        public int LoopsSharp()
        {
            return loopsSharp(list);
        }

        [Benchmark]
        public int LoopsExpression()
        {
            return loopsExpression(list);
        }

        [Benchmark]
        public int Linq()
        {
            return linq(list);
        }

        private readonly List<TestData> list;
        private readonly Func<List<TestData>, int> linq;
        private readonly Func<List<TestData>, int> loopsSharp;
        private readonly Func<List<TestData>, int> loopsExpression;

        private class TestData
        {
            public int X { get; set; }
        }
    }
}