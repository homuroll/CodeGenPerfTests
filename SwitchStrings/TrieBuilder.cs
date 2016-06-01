using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using GrEmit;

namespace SwitchStrings
{
    public static class TrieBuilder
    {
        public static TryGetValueDelegate<T> Build<T>(string[] keys, T[] values)
        {
            var trie = BuildTrie(keys);
            var method = new DynamicMethod(Guid.NewGuid().ToString(), typeof(bool), new[] {typeof(T[]), typeof(string), typeof(T).MakeByRefType()},
                                           typeof(string), true);
            using(var il = new GroboIL(method))
            {
                il.Ldarg(2); // stack: [ref value]
                il.Initobj(typeof(T)); // value = default(value); stack: []
                var index = il.DeclareLocal(typeof(int), "index");
                il.Ldc_I4(0); // stack: [0]
                il.Stloc(index); // index = 0; stack: []
                var length = il.DeclareLocal(typeof(int), "length");
                il.Ldarg(1); // stack: [key]
                il.Call(stringLengthGetter); // stack: [key.Length]
                il.Stloc(length); // length = key.Length; stack: []
                var context = new EmittingContext
                    {
                        Il = il,
                        Index = index,
                        Length = length,
                        CurChar = il.DeclareLocal(typeof(char))
                    };
                InlineTrie<T>(trie, context);
            }
            return (TryGetValueDelegate<T>)method.CreateDelegate(typeof(TryGetValueDelegate<T>), values);
        }

        private static void InlineTrie<T>(TrieNode node, EmittingContext context)
        {
            var il = context.Il;
            var index = context.Index;
            il.Ldloc(index); // stack: [index]
            il.Ldloc(context.Length); // stack: [index, length]
            var retLabel = il.DefineLabel("ret");
            il.Bge(retLabel, false); // if(index >= length) goto ret; stack: []
            il.Ldarg(1); // stack: [key]
            il.Ldloc(index); // stack: [key, index]
            il.Call(stringCharsGetter); // stack: [key[index]]
            il.Stloc(context.CurChar); // curChar = key[index]; stack: []
            il.Ldloc(index); // stack: [index]
            il.Ldc_I4(1); // stack: [index, 1]
            il.Add(); // stack: [index + 1]
            il.Stloc(index); // index = index + 1; stack: []

            if(node.children.Count <= 7)
                EmitIfs<T>(node, context);
            else
                EmitSwitch<T>(node, context);

            il.MarkLabel(retLabel);
            if(node.id < 0)
                il.Ldc_I4(0); // stack: [false]
            else
            {
                il.Ldarg(2); // stack: [ref value]
                il.Ldarg(0); // stack: [ref value, values]
                il.Ldc_I4(node.id); // stack: [ref value, values, id]
                il.Ldelem(typeof(T)); // stack: [ref value, values[id]]
                il.Stind(typeof(T)); // value = values[id]; stack: []
                il.Ldc_I4(1); // stack: [true]
            }
            il.Ret();
        }

        private static void EmitIfs<T>(TrieNode node, EmittingContext context)
        {
            var il = context.Il;
            var curChar = context.CurChar;
            foreach(var pair in node.children)
            {
                il.Ldloc(curChar); // stack: [curChar]
                il.Ldc_I4(pair.Key); // stack: [curChar, pair.Key]
                var notEqualLabel = il.DefineLabel("notEqual");
                il.Bne_Un(notEqualLabel); // if(curChar != pair.Key) goto notEqual; stack: []
                InlineTrie<T>(pair.Value, context);

                il.MarkLabel(notEqualLabel);
            }
            il.Ldc_I4(0); // stack: [false]
            il.Ret();
        }

        private static int CalcSize(char[] values)
        {
            var hashSet = new HashSet<int>();
            for(var n = Math.Max(values.Length, 1);; ++n)
            {
                hashSet.Clear();
                bool ok = true;
                foreach(var x in values)
                {
                    var item = x % n;
                    if(hashSet.Contains(item))
                    {
                        ok = false;
                        break;
                    }
                    hashSet.Add(item);
                }
                if(ok) return n;
            }
        }

        private static void EmitSwitch<T>(TrieNode node, EmittingContext context)
        {
            var il = context.Il;
            var curChar = context.CurChar;
            int n = CalcSize(node.children.Keys.ToArray());
            var defaultLabel = il.DefineLabel("default");
            var labels = new GroboIL.Label[n];
            for(int i = 0; i < labels.Length; ++i)
                labels[i] = defaultLabel;
            foreach(var key in node.children.Keys)
                labels[key % n] = il.DefineLabel("case_" + key);

            il.Ldloc(curChar); // stack: [curChar]
            il.Ldc_I4(n); // stack: [curChar, n]
            il.Rem(false); // stack: [curChar % n]
            il.Switch(labels); // goto labels[curChar % n]; stack: []
            il.Br(defaultLabel); // default;
            foreach(var pair in node.children)
            {
                il.MarkLabel(labels[pair.Key % n]); // case_{key}; stack: []
                InlineTrie<T>(pair.Value, context);
            }
            il.MarkLabel(defaultLabel);
            il.Ldc_I4(0); // stack: [false]
            il.Ret();
        }

        private static TrieNode BuildTrie(string[] keys)
        {
            var root = new TrieNode();
            for(int i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                AddString(root, key, i);
            }
            return root;
        }

        private static void AddString(TrieNode node, string str, int id)
        {
            foreach(var c in str)
            {
                TrieNode child;
                if(!node.children.TryGetValue(c, out child))
                    node.children.Add(c, child = new TrieNode());
                node = child;
            }
            node.id = id;
        }

        private static readonly MethodInfo stringLengthGetter = typeof(string).GetProperty("Length", BindingFlags.Instance | BindingFlags.Public).GetGetMethod();
        private static readonly MethodInfo stringCharsGetter = typeof(string).GetProperty("Chars", BindingFlags.Instance | BindingFlags.Public).GetGetMethod();

        private class EmittingContext
        {
            public GroboIL Il { get; set; }
            public GroboIL.Local Index { get; set; }
            public GroboIL.Local Length { get; set; }
            public GroboIL.Local CurChar { get; set; }
        }

        private class TrieNode
        {
            public int id = -1;
            public readonly Dictionary<char, TrieNode> children = new Dictionary<char, TrieNode>();
        }
    }
}