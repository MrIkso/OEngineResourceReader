using OEngineResourceReader.Utils;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace OEngineResourceReader.Language
{
    // Text file format description:
    // [Header]
    //  [File Version: 4 bytes]
    //  [Reserved 1: 4 bytes] // currently set to 0
    //  [Reserved 2: 4 bytes] // currently set to 0
    //  [Entry Count: 4 bytes]
    //  [Entry Count Duplicate: 4 bytes]
    // [Data Body]
    //  [Text Entry 1]
    //    [String 1 Length: 4 bytes]
    //    [String 1 Data: N bytes]
    //  [Text Entry N]
    //    [String 2 Length: 4 bytes]
    //    [String 2 Data: M bytes]
    // ...

    public class TextProcessor
    {
        public Dictionary<int, string> TextDictionary { get; set; } = [];
        public int FileVersion { get; set; }

        public bool ProcessText(string inputTextPath)
        {
            TextDictionary.Clear();

            using FileStream fs = File.OpenRead(inputTextPath);
            using BinaryReader reader = new BinaryReader(fs);
            long fileLength = fs.Length;

            if (fileLength < 20)
            {
                Debug.WriteLine("File is too small to contain a valid header.");
                return false;
            }

            FileVersion = reader.ReadInt32();
            if (FileVersion > 100)
            {
                Debug.WriteLine("Unsupported file version. Expected version >= 1.");
                return false;
            }
            reader.BaseStream.Seek(8, SeekOrigin.Current); // skip reserved bytes
            int entryCount = reader.ReadInt32();
            int sieze2 = reader.ReadInt32();

            Debug.WriteLine($"File Version: {FileVersion}");
            Debug.WriteLine($"Size1: {entryCount}, Size2: {sieze2}");
            Debug.WriteLine("Reading strings...");

            for (int i = 0; i < entryCount; i++)
            {
                if (reader.BaseStream.Position >= fileLength)
                {
                    Debug.WriteLine($"Warning: Reached end of stream prematurely after reading {i} of {entryCount} entries.");
                    break; 
                }

               string value = FileReader.ReadLengthPrefixedString(reader);
               TextDictionary.Add(i, value);
            }
            return TextDictionary.Count != 0;
        }

        public void GenerateTextFile(Dictionary<int, string> textDictionary, string savePath)
        {
            if (File.Exists(savePath)) {
                File.Delete(savePath);
            }

            using BinaryWriter writer = new BinaryWriter(File.Open(savePath, FileMode.OpenOrCreate));
            writer.Write(FileVersion);
            writer.Write(0);
            writer.Write(0);
            int size = textDictionary.Count;
            writer.Write(size);
            writer.Write(size);

            foreach (var item in textDictionary)
            {
                string textContent = item.Value;
                byte[] textBytes = Encoding.UTF8.GetBytes(textContent);
                writer.Write(textBytes.Length);
                writer.Write(textBytes);
            }
        }

        public void ExportTextToJson(string savePath)
        {
            if (TextDictionary.Count == 0)
            {
                Debug.WriteLine("No text entries to export.");
                return;
            }

            var exportData = TextDictionary.ToDictionary(
                entry => entry.Key.ToString(),
                entry => entry.Value
            );

            string jsonString = JsonSerializer.Serialize(exportData, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            File.WriteAllText(savePath, jsonString);
        }

        public static Dictionary<int, string>? ImportTextFromJson(string jsonPath)
        {
            if (!File.Exists(jsonPath))
            {
                Debug.WriteLine($"JSON file not found: {jsonPath}");
                return null;
            }
            string jsonString = File.ReadAllText(jsonPath);
            try
            {
                var importedData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
                if (importedData != null)
                {
                    var dictionary = importedData.ToDictionary(
                        kvp => int.Parse(kvp.Key),
                        kvp => kvp.Value
                    );
                    return dictionary;
                }
            }
            catch (JsonException ex)
            {
               throw new Exception($"Error deserializing JSON: {ex.Message}");
            }
            return null;
        }
    }
}
