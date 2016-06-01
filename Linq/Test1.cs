using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BenchmarkDotNet.Attributes;

using GrobExp.Compiler;

namespace Linq
{
    public class Test1
    {
        public Test1()
        {
            list = new List<int>();
            for(int i = 0; i < 1000000; ++i)
                list.Add(i);
            linq = x => x.Count(z => z > 10000);

            cyclesSharp = x =>
                {
                    int res = 0;
                    for(int i = 0; i < x.Count; i++)
                    {
                        if(x[i] > 10000)
                            res++;
                    }
                    return res;
                };

            Expression<Func<List<int>, int>> exp = x => x.Count(z => z > 10000);

            cyclesExpression = LambdaCompiler.Compile(exp.EliminateLinq(), CompilerOptions.None);
        }

        [Benchmark(Baseline = true)]
        public int Run_CyclesSharp()
        {
            return cyclesSharp(list);
        }

        [Benchmark]
        public int Run_CyclesExpression()
        {
            return cyclesExpression(list);
        }

        [Benchmark]
        public int Run_Linq()
        {
            return linq(list);
        }

        private readonly List<int> list;
        private readonly Func<List<int>, int> linq;
        private readonly Func<List<int>, int> cyclesSharp;
        private readonly Func<List<int>, int> cyclesExpression;
    }
}