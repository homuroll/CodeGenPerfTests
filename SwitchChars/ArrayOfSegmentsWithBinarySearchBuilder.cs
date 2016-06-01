using System;
using System.Collections.Generic;
using System.Reflection.Emit;

using GrEmit;
using GrEmit.Utils;

namespace SwitchChars
{
    public static class ArrayOfSegmentsWithBinarySearchBuilder
    {
        public static TryGetValueDelegate<T> Build<T>(char[] keys, T[] values, int numberOfSegments, int numberOfKeysPerSegment)
        {
            // Assuming keys are sorted
            var method = new DynamicMethod(Guid.NewGuid().ToString(), typeof(bool),
                                           new[] {typeof(Closure<T>), typeof(char), typeof(T).MakeByRefType()}, typeof(string), true);
            var indices = new List<int>();
            using(var il = new GroboIL(method))
            {
                var idx = il.DeclareLocal(typeof(int), "idx");
                var retFalseLabel = il.DefineLabel("retFalse");
                var segments = new Segment[numberOfSegments];
                for(int i = 0; i < numberOfSegments; ++i)
                {
                    var firstKeyInSegment = keys[i * numberOfKeysPerSegment];
                    var lastKeyInSegment = keys[numberOfKeysPerSegment - 1 + i * numberOfKeysPerSegment];
                    segments[i] = new Segment
                        {
                            FirstKey = firstKeyInSegment,
                            LastKey = lastKeyInSegment,
                            Diff = firstKeyInSegment - indices.Count
                        };
                    var segmentLength = lastKeyInSegment - firstKeyInSegment + 1;
                    int start = indices.Count;
                    for(int j = 0; j < segmentLength; ++j)
                        indices.Add(-1);
                    for(int j = 0; j < numberOfKeysPerSegment; ++j)
                        indices[start + keys[i * numberOfKeysPerSegment + j] - firstKeyInSegment] = i * numberOfKeysPerSegment + j;
                }
                var context = new EmittingContext
                    {
                        Il = il,
                        Segments = segments,
                        RetFalseLabel = retFalseLabel,
                        Idx = idx
                    };
                DoBinarySearch<T>(context, 0, numberOfSegments - 1);
                il.MarkLabel(retFalseLabel);
                il.Ldarg(2); // stack: [ref value]
                if(typeof(T).IsValueType)
                    il.Initobj(typeof(T)); // value = default(T); stack: []
                else
                {
                    il.Ldnull(); // stack: [ref value, null]
                    il.Stind(typeof(T)); // value = null; stack: []
                }
                il.Ldc_I4(0); // stack: [false]
                il.Ret();
            }
            var closure = new Closure<T>
                {
                    indices = indices.ToArray(),
                    values = values
                };
            return (TryGetValueDelegate<T>)method.CreateDelegate(typeof(TryGetValueDelegate<T>), closure);
        }

        private static void DoBinarySearch<T>(EmittingContext context, int l, int r)
        {
            var il = context.Il;
            if(l > r)
            {
                il.Br(context.RetFalseLabel);
                return;
            }

            int m = (l + r) / 2;

            il.Ldarg(1); // stack: [key]
            il.Ldc_I4(context.Segments[m].FirstKey); // stack: [key, segments[m].First]
            var goLeftLabel = il.DefineLabel("goLeft");
            var goRightLabel = il.DefineLabel("goRight");
            il.Blt(goLeftLabel, false); // if(key < segments[m].First) goto goLeft; stack: []
            il.Ldarg(1); // stack: [key]
            il.Ldc_I4(context.Segments[m].LastKey); // stack: [key, segments[m].Last]
            il.Bgt(goRightLabel, false); // if(key > segments[m].Last) goto goRight; stack: []

            il.Ldarg(0); // stack: [closure]
            il.Ldfld(HackHelpers.GetField<Closure<T>>(x => x.indices)); // stack: [closure.indices]
            il.Ldarg(1); // stack: [closure.indices, key]
            il.Ldc_I4(context.Segments[m].Diff); // stack: [closure.indices, key, segments[m].Diff]
            il.Sub(); // stack: [closure.indices, key - segments[m].Diff]
            il.Ldelem(typeof(int)); // stack: [closure.indices[key - segments[m].Diff]]
            il.Dup();
            il.Stloc(context.Idx); // idx = closure.indices[key - segments[m].Diff]; stack: [idx]
            il.Ldc_I4(0); // stack: [idx, 0]
            il.Blt(context.RetFalseLabel, false); // if(idx < 0) goto retFalse; stack: []
            il.Ldarg(2); // stack: [ref value]
            il.Ldarg(0); // stack: [ref value, closure]
            il.Ldfld(HackHelpers.GetField<Closure<T>>(x => x.values)); // stack: [ref value, closure.values]
            il.Ldloc(context.Idx); // stack: [ref value, closure.values, idx]
            il.Ldelem(typeof(T)); // stack: [ref value, closure.values[idx]]
            il.Stind(typeof(T)); // value = closure.values[idx]; stack: []
            il.Ldc_I4(1); // stack: [true]
            il.Ret();

            il.MarkLabel(goLeftLabel);
            DoBinarySearch<T>(context, l, m - 1);

            il.MarkLabel(goRightLabel);
            DoBinarySearch<T>(context, m + 1, r);
        }

        private class Closure<T>
        {
            public int[] indices;
            public T[] values;
        }

        private class EmittingContext
        {
            public GroboIL Il { get; set; }
            public Segment[] Segments { get; set; }
            public GroboIL.Label RetFalseLabel { get; set; }
            public GroboIL.Local Idx { get; set; }
        }

        private class Segment
        {
            public char FirstKey { get; set; }
            public char LastKey { get; set; }
            public int Diff { get; set; }
        }
    }
}