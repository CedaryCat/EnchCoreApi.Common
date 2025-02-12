using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class MemoryExtension
    {
        public unsafe static int Sum<T>(this ReadOnlyMemory<T> memory, delegate*<T, int> getNumber) {
            var span = memory.Span;
            int sum = 0;
            for (int i = 0; i < span.Length; i++) {
                sum += getNumber(span[i]);
            }
            return sum;
        }
        public unsafe static int Sum_Int32Member<T>(this ReadOnlySpan<T> span, int memberOffset) where T : struct {
            int sum = 0;
            for (int i = 0; i < span.Length; i++) {
                void* ptr = Unsafe.AsPointer(ref Unsafe.AsRef(in span[i]));
                sum += *(int*)Unsafe.Add<byte>(ptr, memberOffset);
            }
            return sum;
        }
        public unsafe static int Sum_Int32Member<T>(this ReadOnlyMemory<T> memory, int memberOffset) where T : struct {
            return memory.Span.Sum_Int32Member(memberOffset);
        }
    }
}
