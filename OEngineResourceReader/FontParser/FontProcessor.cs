using OEngineResourceReader.Utils;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace OEngineResourceReader.FontParser
{
    public class FontProcessor
    {
        public static FontData? GetFont { get; set; }

        public static bool Load(string filePath)
        {
            try
            {
                // handle .fnt.Font.gen files from old OEngine versions
                if (filePath.EndsWith("fnt.Font.gen"))
                {
                    FontData? oldFont = GenerateFontFromBmFont(filePath);
                    if (oldFont != null)
                    {
                        GetFont = oldFont;
                        return true;
                    }
                }
                else
                {
                    var font = new FontData();
                    using (var reader = new BinaryReader(File.OpenRead(filePath)))
                    {
                        // read header first 36 bytes
                        font.FileVersion = reader.ReadInt32();      // file version 4 bytes
                        reader.ReadInt32();                         // skipped reserved 4 bytes
                        font.FontVersion = reader.ReadInt32();       // font version 4 bytes
                        font.UseUnicode = reader.ReadSingle();            // unknown value 4 bytes, seems to be a boolean in practice

                        Console.WriteLine($"Unknown value {font.UseUnicode} in font file.");

                        font.Base = reader.ReadInt32();              // 0x10
                        font.LineHeight = reader.ReadInt32();          // 0x14
                        font.TextureWidth = reader.ReadInt32();    // 0x18
                        font.TextureHeight = reader.ReadInt32();   // 0x1C
                        font.Pages = reader.ReadInt32();           // 0x20

                        // glyphs size 4 ytes
                        // paddig 4 bytes + data glyphs 48 bytes * glyphs size

                        ReadGlyphs(reader, font);

                        // kernig count 4 байти
                        // 4 byte data * kerning count
                        ReadKerning(reader, font);

                        ReadPostData(reader, font);

                        if (font.FontVersion < 2)
                        {
                            FontCalculator.CalculateYOffsets(font);
                        }
                    }

                    GetFont = font;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "\n" +ex.StackTrace);
                return false;
            }
        }

        private static void ReadGlyphs(BinaryReader reader, FontData font)
        {
            // read glyphs count
            int glyphCount = reader.ReadInt32();
            font.GlyphsCount = glyphCount;

            Console.WriteLine($"Reading Glyphs array. Count from file: {glyphCount}. Stream position: 0x{reader.BaseStream.Position:X}");

            font.Glyphs = new List<Glyph>(glyphCount);

            for (int i = 0; i < glyphCount; i++)
            {
                var padding = reader.ReadInt32(); // padding, 4 bytes, always 0

                var glyph = new Glyph();
                glyph.X = reader.ReadSingle(); // 1
                glyph.Y = reader.ReadSingle(); // 2
                glyph.Width = reader.ReadSingle(); // 3
                glyph.Height = reader.ReadSingle();// 4
                glyph.XOffset = reader.ReadSingle();// 5
                glyph.YOffset = reader.ReadSingle(); //6
                glyph.XAdvance = reader.ReadSingle();// 7
                glyph.Page = reader.ReadSingle();// 8
                glyph.U1 = reader.ReadSingle();// 9
                glyph.V1 = reader.ReadSingle();// 10
                glyph.U2 = reader.ReadSingle(); // 11
                glyph.V2 = reader.ReadSingle(); // 12

                font.Glyphs.Add(glyph);

            }
            Console.WriteLine($"Glyphs end read. Stream position: 0x{reader.BaseStream.Position:X}");
        }

        private static void ReadKerning(BinaryReader reader, FontData font)
        {
            if (reader.BaseStream.Position + 4 > reader.BaseStream.Length)
            {
                Console.WriteLine("No kerning data found.");
                return;
            }

            // Read kerning count
            int kerningCount = reader.ReadInt32();
            font.KerningCount = kerningCount;

            if (kerningCount <= 0 || kerningCount > 1000000)
            {
                Console.WriteLine($"Warning: Invalid kerning count ({kerningCount}).");
                return;
            }
            Console.WriteLine($"Reading Kerning pairs array. Count from file: {kerningCount}.");

            font.LookupTable = new int[kerningCount];
            for (int i = 0; i < kerningCount; i++)
            {
                font.LookupTable[i] = reader.ReadInt32();
            }

            Console.WriteLine($"Kerning end read. Stream position: 0x{reader.BaseStream.Position:X}");
        }

        private static void ReadPostData(BinaryReader reader, FontData font)
        {
            if (font.FontVersion >= 2)
            {
                font.MaxYOffset = reader.ReadSingle();
                font.MinYOffset = reader.ReadSingle();
            }

            if (font.FontVersion >= 3)
            {
                //Console.WriteLine($"TexturePath read, Stream position: 0x{reader.BaseStream.Position:X}");
                font.TexturePath = FileReader.ReadLengthPrefixedString(reader);
            }
        }

        private static bool SaveNewFontFile(FontData oldFontData, string fntFilePath, string outputFontBinPath)
        {
            FontData? newFontData = GenerateFontFromBmFont(fntFilePath);
            if (newFontData == null)
            {
                Console.WriteLine("Unable generate new font data.");
                return false;
            }
            // Cpy old font data properties to new font data
            newFontData.FileVersion = oldFontData.FileVersion;
            newFontData.FontVersion = oldFontData.FontVersion;
            newFontData.UseUnicode = oldFontData.UseUnicode;
            newFontData.TexturePath = oldFontData.TexturePath;
            SaveFontData(newFontData, outputFontBinPath);
            return true;
        }

        public static FontData? GenerateFontFromBmFont(string fntFilePath)
        {
            if (!File.Exists(fntFilePath))
            {
                Console.WriteLine($"Error: File {fntFilePath} not found.");
                return null;
            }

            var fontData = new FontData();
            // set default values
            fontData.FileVersion = 16;
            fontData.FontVersion = 3;
            fontData.UseUnicode = 1.0f; // useUnicode is a float in the original format, but it seems to be a boolean in practice

            var glyphsTemp = new SortedDictionary<int, Glyph>(); // using SortedDictionary to maintain order by character ID

            var lines = File.ReadAllLines(fntFilePath);

            foreach (var line in lines)
            {
                if (line.StartsWith("common"))
                {
                    fontData.LineHeight = GetIntValue(line, "lineHeight");
                    fontData.Base = GetIntValue(line, "base");
                    fontData.TextureWidth = GetIntValue(line, "scaleW");
                    fontData.TextureHeight = GetIntValue(line, "scaleH");
                    fontData.Pages = GetIntValue(line, "pages");
                }
                else if (line.StartsWith("page"))
                {
                    // Get font texture name, need for replace on old font texture
                    // after we replace it to old name from original font
                    if (GetIntValue(line, "id") == 0)
                    {
                        fontData.TexturePath = GetValue(line, "file");
                    }
                }
                else if (line.StartsWith("char ") && !line.StartsWith("chars count"))
                {
                    int id = GetIntValue(line, "id");
                    var glyph = new Glyph
                    {
                        X = GetFloatValue(line, "x"),
                        Y = GetFloatValue(line, "y"),
                        Width = GetFloatValue(line, "width"),
                        Height = GetFloatValue(line, "height"),
                        XOffset = GetFloatValue(line, "xoffset"),
                        YOffset = GetFloatValue(line, "yoffset"),
                        XAdvance = GetFloatValue(line, "xadvance"),
                        Page = GetIntValue(line, "page"),
                    };

                    // Calculate uv coorinates (normalized)
                    glyph.U1 = glyph.X / fontData.TextureWidth;
                    glyph.V1 = glyph.Y / fontData.TextureHeight;
                    glyph.U2 = (glyph.X + glyph.Width) / fontData.TextureWidth;
                    glyph.V2 = (glyph.Y + glyph.Height) / fontData.TextureHeight;

                    glyphsTemp[id] = glyph;
                }
                /*else if (line.StartsWith("kerning "))
                {
                    int first = GetIntValue(line, "first");
                    int second = GetIntValue(line, "second");
                    int amount = GetIntValue(line, "amount");
                    //fontData.KerningPairs[Tuple.Create(first, second)] = amount;
                }*/
            }

            int lookupTableSize = 65536;
            fontData.LookupTable = new int[lookupTableSize];
            for (int i = 0; i < lookupTableSize; i++)
            {
                fontData.LookupTable[i] = -1;
            }

            fontData.Glyphs = new List<Glyph>(glyphsTemp.Count);
            int glyphIndex = 0;
            foreach (var pair in glyphsTemp)
            {
                int charId = pair.Key;
                Glyph glyph = pair.Value;

                fontData.Glyphs.Add(glyph);

                if (charId < lookupTableSize)
                {
                    fontData.LookupTable[charId] = glyphIndex;
                }

                glyphIndex++;
            }

            fontData.GlyphsCount = fontData.Glyphs.Count;
            fontData.KerningCount = fontData.LookupTable.Length;

            FontCalculator.CalculateYOffsets(fontData);

            return fontData;
        }

        private static string? GetValue(string line, string key)
        {
            var match = Regex.Match(line, $@"{key}=""?([^""\s]+)""?");
            return match.Success ? match.Groups[1].Value : null;
        }

        private static int GetIntValue(string line, string key)
        {
            var valueStr = GetValue(line, key);
            return valueStr != null ? int.Parse(valueStr, CultureInfo.InvariantCulture) : 0;
        }

        private static float GetFloatValue(string line, string key)
        {
            var valueStr = GetValue(line, key);
            return valueStr != null ? float.Parse(valueStr, CultureInfo.InvariantCulture) : 0f;
        }

        public static void SaveFontData(FontData font, string savePath)
        {
            if (font == null)
            {
                Console.WriteLine("Font data is null, cannot save.");
                return;
            }
            using (var writer = new BinaryWriter(File.Open(savePath, FileMode.Create)))
            {
                writer.Write(font.FileVersion);
                writer.Write(0); // reserved
                writer.Write(font.FontVersion);
                writer.Write(font.UseUnicode);
                writer.Write(font.Base);
                writer.Write(font.LineHeight);
                writer.Write(font.TextureWidth);
                writer.Write(font.TextureHeight);
                writer.Write(font.Pages);
                // Write glyphs
                writer.Write(font.GlyphsCount);
                foreach (var glyph in font.Glyphs)
                {
                    writer.Write(0); // padding
                    writer.Write(glyph.X);
                    writer.Write(glyph.Y);
                    writer.Write(glyph.Width);
                    writer.Write(glyph.Height);
                    writer.Write(glyph.XOffset);
                    writer.Write(glyph.YOffset);
                    writer.Write(glyph.XAdvance);
                    writer.Write(glyph.Page);
                    writer.Write(glyph.U1);
                    writer.Write(glyph.V1);
                    writer.Write(glyph.U2);
                    writer.Write(glyph.V2);
                }
                // Write kerning
                writer.Write(font.KerningCount);
                foreach (var kerning in font.LookupTable)
                {
                    writer.Write(kerning);
                }
                // Write post data
                if (font.FontVersion >= 2)
                {
                    writer.Write(font.MaxYOffset);
                    writer.Write(font.MinYOffset);
                }
                if (font.FontVersion >= 3)
                {
                    byte[] textBytes = Encoding.UTF8.GetBytes(font.TexturePath ?? string.Empty);
                    writer.Write(textBytes.Length);
                    writer.Write(textBytes);
                }
            }
        }
    }
}
