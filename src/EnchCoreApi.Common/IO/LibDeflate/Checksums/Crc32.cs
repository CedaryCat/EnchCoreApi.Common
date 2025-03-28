﻿using System;
using System.Runtime.InteropServices;

namespace EnchCoreApi.Common.IO.LibDeflate.Checksums;

using static Imports.Checksums;

public struct Crc32
{
    private uint _currentCrc;

    public uint Hash => _currentCrc;

    public void Append(ReadOnlySpan<byte> input)
        => _currentCrc = AppendCore(_currentCrc, input);

    public uint Compute(ReadOnlySpan<byte> input)
        => _currentCrc = AppendCore(0, input);

    private static uint AppendCore(uint crc, ReadOnlySpan<byte> input)
        => libdeflate_crc32(crc, MemoryMarshal.GetReference(input), (nuint)input.Length);
}
