using BCnEncoder.Shared.ImageFiles;
using OEngineResourceReader.Utils;
using System.Diagnostics;
using System.Text;

namespace OEngineResourceReader.Texture
{
    public class TextureProcessor
    {
        public class TextureInfo
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public int MipMapCount { get; set; }
            public DxgiFormat DxgiFormat { get; set; }
            public int SurfaceSize { get; set; }
            public int Unknown { get; set; }
            public byte[] PixelData { get; set; } = Array.Empty<byte>();
        }

        public TextureInfo? StartParse(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"File not found at {filePath}");
            }
            using var reader = new BinaryReader(File.OpenRead(filePath));
            var version = reader.ReadInt32();
            Debug.WriteLine("Version: {version}");
            TextureInfo? textureInfo = null;
            // old version
            // 0A 00 00 00
            if (version == 0)
            {
                textureInfo = ParseCookedTextureFile(reader, 0xC8);
            }
            else if (version == 10)
            {
                // OEngine Old Texture Format
                // 0x0 - 0xC7 - header 200 bytes
                // 0xC8 - 0xDF meadata 24 bytes
                // 0xE0 - ... data
                textureInfo = ParseCookedTextureFile(reader, 0xC8);
            }
            else if (version == 12)
            {
                // OEngine Old Texture Format
                // 0x0 - 0xC7 - header 200 bytes
                // 0xC8 - 0xDF meadata 24 bytes
                // 0xE0 - ... data
                textureInfo = ParseCookedTextureFile(reader, 0xC8);
            }
            // new version 
            // 10 00 00 00
            else if (version == 16)
            {
                reader.BaseStream.Seek(0xC, SeekOrigin.Begin);
                var serialaiserType = reader.ReadInt32();
                Debug.WriteLine($"Sertialiser Type: {serialaiserType}");
                var seekPosition = 0x0;
               
                if (serialaiserType == 3)
                {
                    seekPosition = 0x89;
                }
                else if (serialaiserType == 4)
                {
                    seekPosition = 0xA8;

                }
                else if (serialaiserType == 5)
                {
                    seekPosition = 0xC8;
                }
                else
                {
                    throw new Exception($"Unsupported serialaiser type: {serialaiserType}");
                }
                // OEngine new Texture Format
                //  header 88 bytes
                //  meadata 12 bytes
                //  data
                textureInfo = ParseCookedTextureFile(reader, seekPosition);
            }
            else
            {
                return null;
            }

            reader.Close();
            return textureInfo;

        }

        public TextureInfo? ParseCookedTextureFile(BinaryReader reader, int seekDataSegment)
        {
           /* if (!ValidateHeaderOEngine(reader))
            {
                return null;
                //throw new Exception("File header does not match the expected format.");
            }
            Debug.WriteLine("Header validation successful!");*/

            var textureInfo = new TextureInfo();

            // header 88 byte
            reader.BaseStream.Seek(seekDataSegment, SeekOrigin.Begin);
            // [width: 4 bytes]
            // [height: 4 bytes]
            // [unknown: 4 bytes]
            // [format: 4 bytes]
            // [data size: 4 bytes]
            // [unknown: 4 bytes]
            // Debug.WriteLine(reader.BaseStream.Position);
            // metadata 
            textureInfo.Width = reader.ReadInt32();
            textureInfo.Height = reader.ReadInt32();
            int unknown = reader.ReadInt32(); /* always 0 */
            Debug.WriteLine($"Unknown1: {unknown}");
            
            textureInfo.MipMapCount = 1;
            int textureFormatId = reader.ReadInt32();
            // Debug.WriteLine($"Texture Format ID: {textureFormatId}");
            textureInfo.DxgiFormat = Helpers.MapEngineFormatToDxgi(textureFormatId);

            if (textureInfo.DxgiFormat == DxgiFormat.DxgiFormatUnknown)
            {
                throw new Exception($"Unknown or unsupported texture format ID: {textureFormatId}");
            }

            // texture size in bytes
            int dataSize = reader.ReadInt32();
            int unknown2 = reader.ReadInt32(); /* always same as dataSize */
            textureInfo.SurfaceSize = dataSize;
            textureInfo.Unknown = unknown2;

            Debug.WriteLine($"Texture Info: {textureInfo.Width}x{textureInfo.Height}, Format: {Helpers.MapDxgiToReadtable(textureInfo.DxgiFormat)}" +
               $"\nSurfaceSize: {textureInfo.SurfaceSize} bytes, Unknown2: {textureInfo.Unknown}");
            if (reader.BaseStream.Position + dataSize > reader.BaseStream.Length)
            {
                throw new Exception("Not enough data in file for pixel information.");
            }
            Debug.WriteLine($"Reading pixel data at position: {reader.BaseStream.Position}");
            textureInfo.PixelData = reader.ReadBytes(dataSize);

            return textureInfo;
        }

      

        public DdsFile CreateDDs(TextureInfo? info)
        {
            if (info == null || info.PixelData == null || info.PixelData.Length == 0)
            {
                throw new ArgumentException("Invalid texture information provided.");
            }
            var (header, dx10Header) = DdsHeader.InitializeCompressed(info.Width, info.Height, info.DxgiFormat, true);
            var ddsFile = new DdsFile(header, dx10Header);
            if (info.MipMapCount > 1)
            {
                ddsFile.header.dwFlags |= HeaderFlags.DdsdMipmapcount;
                ddsFile.header.dwCaps |= HeaderCaps.DdscapsComplex | HeaderCaps.DdscapsMipmap;
                ddsFile.header.dwPitchOrLinearSize = (uint)info.MipMapCount;
            }
            var mainSurface = new DdsMipMap(info.PixelData, (uint)info.Width, (uint)info.Height);
            ddsFile.Faces.Add(new DdsFace((uint)info.Width, (uint)info.Height, (uint)info.PixelData.Length, info.MipMapCount));
            ddsFile.Faces[0].MipMaps[0] = mainSurface;
            return ddsFile;
        }

        public void SaveAsDds(DdsFile ddsFile, string outputPath)
        {
            using (var fs = new FileStream(outputPath, FileMode.Create))
            {
                ddsFile.Write(fs);
            }
        }

        public TextureInfo? GenereateNewTextureInfo(string ddsFilePath)
        {
            var newDds = DdsFile.Load(File.OpenRead(ddsFilePath));
            if (newDds.Faces.Count == 0 || newDds.Faces[0].MipMaps.Length == 0)
            {
                throw new ArgumentException("The source DDS file is empty or invalid.");
            }

            var newTextureSurface = newDds.Faces[0].MipMaps[0];
            int newWidth = (int)newTextureSurface.Width;
            int newHeight = (int)newTextureSurface.Height;
            DxgiFormat newDxgiFormat = newDds.header.ddsPixelFormat.IsDxt10Format
                ? newDds.dx10Header.dxgiFormat
                : newDds.header.ddsPixelFormat.DxgiFormat;

            byte[] newPixelData = newTextureSurface.Data;
            int newDataSize = newPixelData.Length;
            int newMipMapCount = newDds.Faces[0].MipMaps.Length;
            var newTextureInfo = new TextureInfo
            {
                Width = newWidth,
                Height = newHeight,
                DxgiFormat = newDxgiFormat,
                SurfaceSize = newDataSize,
                PixelData = newPixelData,
                MipMapCount = newMipMapCount,
            };
            return newTextureInfo;
        }

        public bool ReplaceTextureInCookedFile(string cookedFilePath, string ddsFilePath)
        {
            return ReplaceTextureInCookedFile(cookedFilePath, ddsFilePath, cookedFilePath);
        }

        public bool ReplaceTextureInCookedFile(TextureInfo newTextureInfo, string cookedFilePath, string newCookedFilePath)
        {
            try
            {
                Debug.WriteLine($"New texture info: {newTextureInfo.Width}x{newTextureInfo.Height}, Format: {Helpers.MapDxgiToReadtable(newTextureInfo.DxgiFormat)}, Size: {newTextureInfo.SurfaceSize} bytes");
               
                byte[] originalCookedBytes = File.ReadAllBytes(cookedFilePath);
               
                int version = BitConverter.ToInt32(originalCookedBytes, 0);
                int metadataOffset;

                // old Oengine files
                if (version == 0)
                {
                    metadataOffset = 0xC8;
                }
                else if (version == 10)
                {
                    metadataOffset = 0xC8;
                }
                else if (version == 12)
                {
                    metadataOffset = 0xC8;
                }
                // new files
                else if (version == 16)
                {
                    int serialiserType = BitConverter.ToInt32(originalCookedBytes, 0xC);
                    if (serialiserType == 3)
                    {
                        metadataOffset = 0x89;
                    }
                    else if (serialiserType == 4)
                    {
                        metadataOffset = 0xA8;
                    }
                    else if (serialiserType == 5)
                    {
                        metadataOffset = 0xC8;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }


                // Offset to field with data size = metadataOffset + offset within metadata block (16 bytes)
                int oldDataSizeOffset = metadataOffset + 16;
                int oldDataSize = BitConverter.ToInt32(originalCookedBytes, oldDataSizeOffset);
                Debug.WriteLine($"Old data size was: {oldDataSize} bytes");

                int pixelDataOffset = metadataOffset + 24;
                int tailOffset = pixelDataOffset + oldDataSize;

                if (tailOffset > originalCookedBytes.Length)
                {
                    throw new ArgumentException("The old data size points beyond the end of the file. File may be corrupt.");
                }

                int tailSize = originalCookedBytes.Length - tailOffset;
                Debug.WriteLine($"Tail section found. Offset: 0x{tailOffset:X}, Size: {tailSize} bytes.");

                //  string tempFilePath = cookedFilePath + ".tmp";
                using (var fs = new FileStream(newCookedFilePath, FileMode.Create, FileAccess.ReadWrite))
                using (var writer = new BinaryWriter(fs))
                {
                    writer.Write(originalCookedBytes, 0, metadataOffset);
                    writer.Write(newTextureInfo.Width);
                    writer.Write(newTextureInfo.Height);
                    writer.Write(0); // Unknown
                    writer.Write(Helpers.MapDxgiToEngineFormat(newTextureInfo.DxgiFormat));
                    writer.Write(newTextureInfo.SurfaceSize);

                    int unknown2Value = BitConverter.ToInt32(originalCookedBytes, metadataOffset + 20);
                    if (unknown2Value == oldDataSize)
                    {
                        writer.Write(newTextureInfo.SurfaceSize);
                    }
                    else
                    {
                        writer.Write(unknown2Value);
                    }

                    Debug.WriteLine("Header and new metadata written.");

                    writer.Write(newTextureInfo.PixelData);
                    Debug.WriteLine("New pixel data written.");

                    if (tailSize > 0)
                    {
                        writer.Write(originalCookedBytes, tailOffset, tailSize);
                        Debug.WriteLine("Tail section written.");
                    }
                }

                Debug.WriteLine("New cooked file has been created successfully.");
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"An error occurred during replacement: {ex.Message}");
            }
        }

        public bool ReplaceTextureInCookedFile(string cookedFilePath, string ddsFilePath, string newCookedFilePath)
        {
            Debug.WriteLine($"--- Starting Texture Replacement ---");
            Debug.WriteLine($"Target: {cookedFilePath}");
            Debug.WriteLine($"Source: {ddsFilePath}");
            Debug.WriteLine($"New: {newCookedFilePath}");

            if (!File.Exists(cookedFilePath) || !File.Exists(ddsFilePath))
            {
                throw new ArgumentException("One of the files does not exist.");
            }
            var newTextureInfo = GenereateNewTextureInfo(ddsFilePath);
            if (newTextureInfo == null)
            {
                throw new ArgumentException("Failed to parse the new DDS file.");
            }

            return ReplaceTextureInCookedFile(newTextureInfo, cookedFilePath, newCookedFilePath);
        }
    }
}
