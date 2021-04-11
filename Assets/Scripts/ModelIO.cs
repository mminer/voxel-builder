using System;
using System.IO;

public static class ModelIO
{
    const string FileType = "VM"; // "Voxel Model"?
    const byte Version = 1;

    public static Voxel[] ReadModel(string path)
    {
        using var fileStream = File.OpenRead(path);
        using var reader = new BinaryReader(fileStream);

        var fileType = reader.ReadChars(FileType.Length);
        var version = reader.ReadByte();

        // TODO: verify that file type and version are expected

        var voxelCount = reader.ReadUInt16();
        var voxels = new Voxel[voxelCount];

        for (var i = 0; i < voxelCount; i++)
        {
            var x = reader.ReadSByte();
            var y = reader.ReadSByte();
            var z = reader.ReadSByte();
            var colorIndex = reader.ReadByte();
            var flags = reader.ReadUInt64();
            voxels[i] = new Voxel(x, y, z, colorIndex, flags);
        }

        return voxels;
    }

    public static void WriteModel(string path, Voxel[] model)
    {
        using var fileStream = File.OpenWrite(path);
        using var writer = new BinaryWriter(fileStream);

        foreach (var c in FileType)
        {
            writer.Write(c);
        }

        writer.Write(Version);

        var voxelCount = Convert.ToUInt16(model.Length);
        writer.Write(voxelCount);

        for (var i = 0; i < voxelCount; i++)
        {
            var voxel = model[i];
            writer.Write(voxel.X);
            writer.Write(voxel.Y);
            writer.Write(voxel.Z);
            writer.Write(voxel.ColorIndex);
            writer.Write(voxel.Flags);
        }
    }
}
