﻿using EnchCoreApi.Common.IO.LibDeflate.Imports;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EnchCoreApi.Common.IO.LibDeflate;

using static Decompression;

public sealed class ZlibDecompressor : Decompressor
{
    public ZlibDecompressor() : base()
    {
    }

    protected override OperationStatus DecompressCore(ReadOnlySpan<byte> input, Span<byte> output, nuint uncompressedSize)
        => StatusFromResult(libdeflate_zlib_decompress(decompressor, MemoryMarshal.GetReference(input),
            (nuint)input.Length, ref MemoryMarshal.GetReference(output), uncompressedSize, out Unsafe.NullRef<nuint>()));

    protected override OperationStatus DecompressCore(ReadOnlySpan<byte> input, Span<byte> output, out nuint bytesWritten)
        => StatusFromResult(libdeflate_zlib_decompress(decompressor, MemoryMarshal.GetReference(input),
            (nuint)input.Length, ref MemoryMarshal.GetReference(output), (nuint)output.Length, out bytesWritten));

    protected override OperationStatus DecompressCore(ReadOnlySpan<byte> input, Span<byte> output, nuint uncompressedSize, out nuint bytesRead)
        => StatusFromResult(libdeflate_zlib_decompress_ex(decompressor, MemoryMarshal.GetReference(input),
            (nuint)input.Length, ref MemoryMarshal.GetReference(output), uncompressedSize, out bytesRead, out Unsafe.NullRef<nuint>()));

    protected override OperationStatus DecompressCore(ReadOnlySpan<byte> input, Span<byte> output, out nuint bytesWritten, out nuint bytesRead)
        => StatusFromResult(libdeflate_zlib_decompress_ex(decompressor, MemoryMarshal.GetReference(input),
            (nuint)input.Length, ref MemoryMarshal.GetReference(output), (nuint)output.Length, out bytesRead, out bytesWritten));
}
