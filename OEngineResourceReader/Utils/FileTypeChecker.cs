using System.Diagnostics;
using System.Net;
using System.Text;

namespace OEngineResourceReader.Utils
{
    public class FileTypeChecker
    {
        public enum FileType
        {
            Texture = 1, // "oCTexture"
            Materials = 2, // "oCMaterial"
            Animation = 3, // oCAnimation
            Level = 4, // oCGameStream
            Geometry = 5, // oCGeometry
            Collision = 6, // oCCollision
            Object = 7, // oCGameObject
            Shader = 8, // oCShader2Reflection
            SettingsVfx = 9, // oCSheduledVfcSettings
            SettingsGlobalEntityValue = 10, // oCGlobalEntityValueSettings
            Font = 11,
            Enemies = 12, // oCDtEnemyDefinition
            DreamShards = 13, // oCDtDreamShardDefinition
            Challenges = 14,
            AchievementDefinition = 15, //AchievementDefinition
            EntitySettingsResource = 16, // oCEntitySettingsResource
            Cooked = 17,
            Unknown
        }

        public FileType Type { get; set; } = FileType.Unknown;

        public FileTypeChecker(string filePath)
        {
            ValidateHeaderOEngine(filePath);
        }

        public bool ValidateHeaderOEngine(string filePath)
        {
            using BinaryReader reader = new BinaryReader(File.OpenRead(filePath));

            Debug.WriteLine("Validating file header...");

            // Ensure the file has at least 0x30 bytes
            if (reader.BaseStream.Length < 0x30)
            {
                return false;
            }

            reader.BaseStream.Seek(0xC, SeekOrigin.Begin);
            float val = reader.ReadSingle();
            if (val == 1f)
            {
                Type = FileType.Font;
                return true;
            }

            reader.BaseStream.Seek(0x8, SeekOrigin.Begin); // Skip to the serializer section
            string cookedVal = FileReader.ReadLengthPrefixedString(reader);
            if (cookedVal.Equals("Cooked")){
                Type = FileType.Cooked;
                return true;
            }
            reader.BaseStream.Seek(0x0, SeekOrigin.Begin);

            // Read only the first 0x30 bytes
            byte[] headerBytes = reader.ReadBytes(0x30);
            // Use a MemoryStream to process the headerBytes for finding the serializer
            using MemoryStream memoryStream = new MemoryStream(headerBytes);
            using BinaryReader headerReader = new BinaryReader(memoryStream);

            // Validate the magic number at offset 8
            if (!headerBytes.Skip(8).Take(4).SequenceEqual(new byte[] { 0x11, 0x11, 0xBB, 0xAA }))
            {
                Debug.WriteLine("Initial magic number mismatch.");
                return false;
            }

            memoryStream.Seek(0x10, SeekOrigin.Begin); // Skip to the serializer section
            string serializer = FileReader.ReadLengthPrefixedString(headerReader);
            Debug.WriteLine($"File type serializer: {serializer}");
            // Determine file type based on serializer string
            Type = serializer switch
            {
                "oCTexture" => FileType.Texture,
                "oCMaterial" => FileType.Materials,
                "oCAnimation" => FileType.Animation,
                "oCGameStream" => FileType.Level,
                "oCGeometry" => FileType.Geometry,
                "oCCollision" => FileType.Collision,
                "oCGoComponent" => FileType.Geometry,
                "oCGameObject" => FileType.Object,
                "oCShader2Reflection" => FileType.Shader,
                "oCScheduledVfxSettings" => FileType.SettingsVfx,
                "oCGlobalEntityValueSettings" => FileType.SettingsGlobalEntityValue,
                "oCDtEnemyDefinition" => FileType.Enemies,
                "oCDtDreamShardDefinition" => FileType.DreamShards,
                "ChallengeDefinition" => FileType.Challenges,
                "AchievementDefinition" => FileType.AchievementDefinition,
                "oCEntitySettingsResource" => FileType.EntitySettingsResource,
               
                _ => FileType.Unknown
            };

            Debug.WriteLine($"File type determined: {Type}");

            return Type != FileType.Unknown;
        }

    }
}
