using System;
using UnityEngine;

public readonly struct Voxel
{
    public readonly byte ColorIndex;
    public readonly ulong Flags;
    public readonly Vector3Int Position;
    public readonly sbyte X;
    public readonly sbyte Y;
    public readonly sbyte Z;

    public Voxel(sbyte x, sbyte y, sbyte z, byte colorIndex, ulong flags)
    {
        ColorIndex = colorIndex;
        Flags = flags;
        Position = new Vector3Int(x, y, z);
        X = x;
        Y = y;
        Z = z;
    }

    public Voxel(Vector3 position, byte colorIndex = 0, ulong flags = 0) : this(
        Convert.ToSByte(position.x),
        Convert.ToSByte(position.y),
        Convert.ToSByte(position.z),
        colorIndex,
        flags)
    {
    }

    public override int GetHashCode() => Position.GetHashCode();
}
