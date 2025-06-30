using System.Diagnostics;
using System.Text;

namespace OEngineResourceReader.Utils
{
    internal class FileReader
    {

        // Serialized header magic for OEngine files
        public static bool ValidateHeaderOEngine(string inputFilePath)
        {
            using BinaryReader reader = new BinaryReader(File.OpenRead(inputFilePath));
            Debug.WriteLine("Validating file header...");

            if (reader.BaseStream.Length < 144)
            {
                return false;
            }

            reader.BaseStream.Seek(8, SeekOrigin.Begin);
            byte[] magicStart = reader.ReadBytes(4);
            if (!magicStart.SequenceEqual(new byte[] { 0x11, 0x11, 0xBB, 0xAA }))
            {
                return false;
            }
            return true;
        }

        public static string ReadLengthPrefixedString(BinaryReader reader)
        {
            try
            {
                // read the length of the string (4 bytes)
                int length = reader.ReadInt32();
                if (length > 0)
                {
                    // read the string data based on the length
                    byte[] stringBytes = reader.ReadBytes(length);
                    return Encoding.UTF8.GetString(stringBytes);
                }
            }
            catch (EndOfStreamException)
            {
                Debug.WriteLine("Reached end of stream while reading string.");
                return string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error reading string: {ex.Message}");
                return string.Empty;
            }
            return string.Empty;
        }

        public static byte[]? ReadLastBytes(string filePath, int numberOfBytesToRead)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (fs.Length < numberOfBytesToRead)
                    {
                        Console.WriteLine($"Error: File size ({fs.Length}) is smaller than requested bytes ({numberOfBytesToRead}).");
                    
                        return null;
                    }
                    fs.Seek(-numberOfBytesToRead, SeekOrigin.End);
                    byte[] buffer = new byte[numberOfBytesToRead];
                    int bytesRead = fs.Read(buffer, 0, numberOfBytesToRead);
                    if (bytesRead < numberOfBytesToRead)
                    {
                        Array.Resize(ref buffer, bytesRead);
                    }
                    return buffer;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
    }
}
