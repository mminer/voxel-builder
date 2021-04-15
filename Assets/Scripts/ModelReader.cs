using System.IO;

public class ModelReader : ModelIO
{
    readonly string path;

    public ModelReader(string path) => this.path = path;

    public Voxel[] Read()
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
}
