﻿using EnchCoreApi.Common.IO.LibDeflate.Imports;
using System;
using System.Runtime.InteropServices;

namespace EnchCoreApi.Common.IO.LibDeflate;

public sealed class GzipCompressor : Compressor
{
    public GzipCompressor(int compressionLevel) : base(compressionLevel)
    {
    }

    protected override nuint CompressCore(ReadOnlySpan<byte> input, Span<byte> output)
        => Compression.libdeflate_gzip_compress(compressor, MemoryMarshal.GetReference(input), (nuint)input.Length, ref MemoryMarshal.GetReference(output), (nuint)output.Length);

    protected override nuint GetBoundCore(nuint inputLength)
        => Compression.libdeflate_gzip_compress_bound(compressor, inputLength);
}
