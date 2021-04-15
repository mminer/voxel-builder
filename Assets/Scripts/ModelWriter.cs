using System;
using System.IO;

public class ModelWriter : ModelIO
{
    readonly string path;

    public ModelWriter(string path) => this.path = path;

    public void Write(Voxel[] model)
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
