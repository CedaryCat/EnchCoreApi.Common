using System;
using System.Runtime.InteropServices;

namespace EnchCoreApi.Common.IO.LibDeflate.Imports;

using size_t = nuint;

internal static class CustomMemoryAllocator
{
    //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate nint malloc_func(size_t size);
    //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void free_func(nint alloc);

    ///<summary>
    /// Install a custom memory allocator which libdeflate will use for all memory
    /// allocations.  'malloc_func' is a function that must behave like malloc(), and
    /// 'free_func' is a function that must behave like free().
    ///
    /// There must not be any libdeflate_compressor or libdeflate_decompressor
    /// structures in existence when calling this function.
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void libdeflate_set_memory_allocator(malloc_func malloc, free_func free);
}
