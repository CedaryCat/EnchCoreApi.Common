﻿using EnchCoreApi.Common.IO.LibDeflate.Imports;
using System;
using System.Runtime.InteropServices;

namespace EnchCoreApi.Common.IO.LibDeflate;

public sealed class ZlibCompressor : Compressor
{
    public ZlibCompressor(int compressionLevel) : base(compressionLevel)
    {
    }

    protected override nuint CompressCore(ReadOnlySpan<byte> input, Span<byte> output)
        => Compression.libdeflate_zlib_compress(compressor, MemoryMarshal.GetReference(input), (nuint)input.Length, ref MemoryMarshal.GetReference(output), (nuint)output.Length);

    protected override nuint GetBoundCore(nuint inputLength)
        => Compression.libdeflate_zlib_compress_bound(compressor, inputLength);
}
