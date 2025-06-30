
using BCnEncoder.Decoder;
using BCnEncoder.Shared.ImageFiles;
using OEngineResourceReader.FontParser;
using System.Diagnostics;
using System.Text;

namespace OEngineResourceReader.Utils
{
    public class Helpers
    {
        public static Bitmap DdsToBitmap(DdsFile ddsFile)
        {
            var d = new BcDecoder();

            var colors = d.Decode(ddsFile);

            int w = (int)ddsFile.Faces[0].Width;
            int h = (int)ddsFile.Faces[0].Height;
            var bitmap = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    var rgba = colors[y * w + x];
                    var clr = Color.FromArgb(rgba.a, rgba.r, rgba.g, rgba.b);
                    bitmap.SetPixel(x, y, clr);
                }
            }

            return bitmap;
        }

        public static Bitmap DdsToBitmap(byte[] ddsBytes)
        {
            var d = new BcDecoder();
            var dds = DdsFile.Load(new MemoryStream(ddsBytes));
            var colors = d.Decode(dds);

            int w = (int)dds.Faces[0].Width;
            int h = (int)dds.Faces[0].Height;
            var bitmap = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    var rgba = colors[y * w + x];
                    var clr = Color.FromArgb(rgba.a, rgba.r, rgba.g, rgba.b);
                    bitmap.SetPixel(x, y, clr);
                }
            }

            return bitmap;
        }

        public static DxgiFormat MapEngineFormatToDxgi(int engineFormatId)
        {
            return engineFormatId switch
            {
                0 => DxgiFormat.DxgiFormatR8G8B8A8Unorm, // RGBA8
                1 => DxgiFormat.DxgiFormatA8Unorm, // A8
                2 => DxgiFormat.DxgiFormatD32Float, // DepthFloat32
                3 => DxgiFormat.DxgiFormatD24UnormS8Uint, // Depth
                4 => DxgiFormat.DxgiFormatBc1Unorm, // DXT1
                5 => DxgiFormat.DxgiFormatBc3Unorm, // DXT5
                6 => DxgiFormat.DxgiFormatUnknown, // GNF
                _ => DxgiFormat.DxgiFormatUnknown
            };
        }

        public static string MapDxgiToReadtable(DxgiFormat engineFormatId)
        {
            return engineFormatId switch
            {
                DxgiFormat.DxgiFormatR8G8B8A8Unorm => "RGBA8",
                DxgiFormat.DxgiFormatA8Unorm => "A8",
                DxgiFormat.DxgiFormatD32Float => "DepthFloat32",
                DxgiFormat.DxgiFormatD24UnormS8Uint => "Depth",
                DxgiFormat.DxgiFormatBc1Unorm => "DXT1",
                DxgiFormat.DxgiFormatBc3Unorm => "DXT5",
                _ => "Unknown"
            };
        }

        public static int MapDxgiToEngineFormat(DxgiFormat dxgiFormat)
        {
            return dxgiFormat switch
            {
                DxgiFormat.DxgiFormatR8G8B8A8Unorm => 0,
                DxgiFormat.DxgiFormatA8Unorm => 1,
                DxgiFormat.DxgiFormatD32Float => 2,
                DxgiFormat.DxgiFormatD24UnormS8Uint => 3,
                DxgiFormat.DxgiFormatBc1Unorm => 4,
                DxgiFormat.DxgiFormatBc3Unorm => 5,
                DxgiFormat.DxgiFormatUnknown => 6, // GNF
                _ => throw new NotSupportedException($"Cannot map {dxgiFormat} back to an engine format ID.")
            };
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            hex = hex.Replace(" ", "");
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }


        public static int CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }

            char[] delimiters = [' ', '\r', '\n', '.', ',', ';', ':', '!', '?'];
            return text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static (List<string> Directories, List<string> Files)? GetFilteredDirectoryContent(string path, List<string> filters)
        {
            try
            {
                var matchingFiles = new List<string>();
                if (filters.Any())
                {
                    matchingFiles.AddRange(Directory.GetFiles(path)
                        .Where(file => filters.Any(filter => file.Contains(filter, StringComparison.OrdinalIgnoreCase))));
                }
                else
                {
                    matchingFiles.AddRange(Directory.GetFiles(path));
                }

                var nonEmptySubDirs = new List<string>();
                foreach (var subDir in Directory.GetDirectories(path))
                {
                    if (GetFilteredDirectoryContent(subDir, filters) != null)
                    {
                        nonEmptySubDirs.Add(subDir);
                    }
                }

                if (matchingFiles.Any() || nonEmptySubDirs.Any())
                {
                    return (nonEmptySubDirs, matchingFiles);
                }
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        public static void OpenFileInExplorer(string filePath)
        {
            if (File.Exists(filePath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer",
                    Arguments = $"/select,\"{filePath}\"",
                    UseShellExecute = true
                });
            }
            else if (Directory.Exists(filePath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer",
                    Arguments = $"\"{filePath}\"",
                    UseShellExecute = true
                });
            }

        }

        public static string? FindTextureFileForFont(string needTextue, string fontFilePath)
        {
            if (string.IsNullOrEmpty(fontFilePath) || !File.Exists(fontFilePath))
            {
                Debug.WriteLine($"Error: Font file path is invalid or file does not exist: {fontFilePath}");
                return null;
            }

            string? directory = Path.GetDirectoryName(fontFilePath);
            if (string.IsNullOrEmpty(directory))
            {
                Debug.WriteLine("Error: Could not determine the directory.");
                return null;
            }

            string fontFileName = Path.GetFileName(fontFilePath);
            Debug.WriteLine($"Searching for texture files in directory: {directory} for font file: {fontFileName}");
            string[] parts = fontFileName.Split('.');

            if (parts.Length < 2)
            {
                Debug.WriteLine($"Error: Invalid file name format: {fontFileName}");
                return null;
            }

            string baseName = parts[0];
            string searchPattern;
            string? foundFile = null;

            if (parts.Length >= 4)
            {
                string identifier = parts[3];
                searchPattern = $"{baseName}.{identifier}_0*";
                Debug.WriteLine($"Trying pattern 1: {searchPattern}");
                foundFile = Directory.EnumerateFiles(directory, searchPattern).FirstOrDefault();
                if (foundFile != null)
                {
                    Debug.WriteLine($"Found file: {Path.GetFileName(foundFile)}");
                    return foundFile;
                }
            }
          
            searchPattern = $"{baseName}_0*";
            Debug.WriteLine($"Trying pattern 2: {searchPattern}");
            foundFile = Directory.EnumerateFiles(directory, searchPattern).FirstOrDefault();
            if (foundFile != null)
            {
                Debug.WriteLine($"Found file: {Path.GetFileName(foundFile)}");
                return foundFile;
            }

            parts = fontFileName.Split("!");
            if(parts.Length >= 2)
            {
                baseName = parts[0];
               
                searchPattern = $"{baseName}!{needTextue}*";
                Debug.WriteLine($"Trying pattern 3: {searchPattern}");
                foundFile = Directory.EnumerateFiles(directory, searchPattern).FirstOrDefault();
                if (foundFile != null)
                {
                    Debug.WriteLine($"Found file: {Path.GetFileName(foundFile)}");
                    return foundFile;
                }

            }

            parts = fontFileName.Split('~');

            if (parts.Length >= 2)
            {
                baseName = parts[0];
                string[] ids = parts[1].Split(".");
                searchPattern = $"{baseName}_0~{ids[0]}*";
                Debug.WriteLine($"Trying pattern 4: {searchPattern}");
                foundFile = Directory.EnumerateFiles(directory, searchPattern).FirstOrDefault();
                if (foundFile != null)
                {
                    Debug.WriteLine($"Found file: {Path.GetFileName(foundFile)}");
                    return foundFile;
                }

                searchPattern = $"{baseName}_{ids[3]}_0*";
                Debug.WriteLine($"Trying pattern 5: {searchPattern}");
                foundFile = Directory.EnumerateFiles(directory, searchPattern).FirstOrDefault();
                if (foundFile != null)
                {
                    Debug.WriteLine($"Found file: {Path.GetFileName(foundFile)}");
                    return foundFile;
                }
                searchPattern = $"{baseName}_0*";
                Debug.WriteLine($"Trying pattern 6: {searchPattern}");
                foundFile = Directory.EnumerateFiles(directory, searchPattern).FirstOrDefault();
                if (foundFile != null)
                {
                    Debug.WriteLine($"Found file: {Path.GetFileName(foundFile)}");
                    return foundFile;
                }
            }
            Debug.WriteLine("Texture file not found using known patterns.");
            return null;
        }

        public static string GenerateBmtConfigFile(FontData fontData, string fontFaceName)
        {
            string charsString = GenerateCharsString(fontData);
            var (spacingH, spacingV) = FontCalculator.CalculateSpacing(fontData);

            var sb = new StringBuilder();

            sb.AppendLine("# AngelCode Bitmap Font Generator configuration file");
            sb.AppendLine("fileVersion=1");
            sb.AppendLine();

            sb.AppendLine("# font settings");
            sb.AppendLine($"fontName=\"{fontFaceName}\"");
            sb.AppendLine("fontFile=");
            sb.AppendLine("charSet=0");
            sb.AppendLine($"fontSize={fontData.LineHeight}");
            sb.AppendLine("aa=1");
            sb.AppendLine("useSmoothing=0");
            sb.AppendLine("isBold=0");
            sb.AppendLine("isItalic=0");
            sb.AppendLine($"useUnicode={fontData.UseUnicode}");
            sb.AppendLine("disableBoxChars=1");
            sb.AppendLine("outputInvalidCharGlyph=0");
            sb.AppendLine("dontIncludeKerningPairs=0");
            sb.AppendLine("useHinting=0");
            sb.AppendLine("renderFromOutline=0");
            sb.AppendLine("useClearType=1");

            sb.AppendLine();
            sb.AppendLine("# character alignment");
            sb.AppendLine("paddingDown=0");
            sb.AppendLine("paddingUp=0");
            sb.AppendLine("paddingRight=0");
            sb.AppendLine("paddingLeft=0");
            sb.AppendLine($"spacingHoriz={spacingH}");
            sb.AppendLine($"spacingVert={spacingV}");
            sb.AppendLine("useFixedHeight=0");
            sb.AppendLine("forceZero=0");
            sb.AppendLine("widthPaddingFactor=0.00");
            sb.AppendLine();

            sb.AppendLine("# output file");
            sb.AppendLine($"outWidth={fontData.TextureWidth}");
            sb.AppendLine($"outHeight={fontData.TextureHeight}");
            sb.AppendLine("outBitDepth=32");
            sb.AppendLine("fontDescFormat=0");
            sb.AppendLine("fourChnlPacked=0");
            sb.AppendLine("textureFormat=dds");
            sb.AppendLine("textureCompression=3");
            sb.AppendLine("alphaChnl=0");
            sb.AppendLine("redChnl=4");
            sb.AppendLine("greenChnl=4");
            sb.AppendLine("blueChnl=4");
            sb.AppendLine("invA=0");
            sb.AppendLine("invR=0");
            sb.AppendLine("invG=0");
            sb.AppendLine("invB=0");
            sb.AppendLine();

            sb.AppendLine("# outline");
            sb.AppendLine("outlineThickness=0");
            sb.AppendLine();

            sb.AppendLine("# selected chars");
            sb.AppendLine($"chars={charsString}");

            return sb.ToString();
        }

        public static string GenerateCharsString(FontData fontData)
        {
            if (fontData.LookupTable == null || fontData.LookupTable.Length == 0)
            {
                Console.WriteLine("Warning: LookupTable is empty. Cannot generate 'chars' string.");
                return string.Empty;
            }

            var charIds = new List<int>();
            for (int i = 0; i < fontData.LookupTable.Length; i++)
            {
                if (fontData.LookupTable[i] != -1)
                {
                    charIds.Add(i);
                }
            }

            if (charIds.Count == 0)
            {
                return string.Empty;
            }

            var ranges = new List<string>();
            int rangeStart = charIds[0];

            for (int i = 1; i < charIds.Count; i++)
            {

                if (charIds[i] != charIds[i - 1] + 1)
                {
                    int rangeEnd = charIds[i - 1];
                    if (rangeStart == rangeEnd)
                    {
                        ranges.Add(rangeStart.ToString());
                    }
                    else
                    {
                        ranges.Add($"{rangeStart}-{rangeEnd}");
                    }
                    rangeStart = charIds[i];
                }
            }
            int lastRangeEnd = charIds[charIds.Count - 1];
            if (rangeStart == lastRangeEnd)
            {
                ranges.Add(rangeStart.ToString());
            }
            else
            {
                ranges.Add($"{rangeStart}-{lastRangeEnd}");
            }

            return string.Join(",", ranges);
        }

        public static void ShowFormAtTopRight(Form parent, Form formToShow)
        {
            formToShow.StartPosition = FormStartPosition.Manual;

            if (parent.WindowState == FormWindowState.Minimized)
            {
                formToShow.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                int x = parent.Right - formToShow.Width;
                int y = parent.Top;
                formToShow.Location = new Point(x, y);
            }

            formToShow.Show(parent);
        }

        public static string GetApplicationVersion()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            return $"{version.Major}.{version.Minor}.{version.Build}";
        }
    }
}