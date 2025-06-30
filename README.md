# OEngineResourceReader

![License](https://img.shields.io/badge/License-MIT-blue.svg)
![Language](https://img.shields.io/badge/Language-C%23-purple.svg)
![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey.svg)

An unofficial tool for viewing, inspecting, and modifying textures, fonts, and text files from OEngine game files.

---

## Features

-   **File & Directory Browser:** Navigate through game directories with a familiar tree view structure.
-   **Dynamic File Filtering:** Show only the files you need by filtering by extension (e.g., `*.fnt.Font.fnb`).
-   **Texture Viewer:**
    -  View texture files with transparency support.
    -  Smooth zoom and pan controls for detailed inspection.
    -  Export textures to the standard `.dds` format.
    -  Replace existing textures with custom ones.
-   **Font Inspector:**
    -  View font texture atlases.
    -  Inspect properties like glyph count, kerning pairs, and line height.
    -  Live preview rendering of any text string using the game's font.
-   **Text/Localization Viewer:**
    -  Open and view text resource files.
    -  Display text entries in a clear, editable grid.
    -  Edit and save translated text back to the file.
-   **User-Friendly Interface:** All tools are organized in a clean, tabbed interface with keyboard navigation support.

## System Requirements

- Windows 10 or Windows 11
- .NET 8+

## How to Use

1. Go to the [**Releases**](https://github.com/MrIkso/OEngineResourceReader/releases) page of this repository.
2. Download the latest `.zip` file.
3. Extract the contents to a folder on your computer.
4. Run `OEngineResourceReader.exe`.

**Getting Started:**
-  Use **File > Open Directory...** to select a folder with game resources. The file tree will populate.
-  Double-click a file in the tree to open it in the appropriate viewer tab.
-  Right-click on files or folders for more options, like "Open in Explorer".
- Or we can Drag Drop files\folder
---

## ⚠️ Disclaimer

**This is the most important section. Please read it carefully.**

-   **Unofficial Software:** This application ("OEngineResourceReader") is an independent, unofficial modification tool and is not affiliated with, authorized, sponsored, or otherwise approved by the original game developers or publishers.
-   **Use At Your Own Risk:** The Tool is designed to modify game files. Any modifications to game data carry an inherent risk. You are solely responsible for any potential damage, corruption, or loss of game files, save data, or system instability that may result from using this Tool.
-   **NO WARRANTY:** THE TOOL IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES, OR OTHER LIABILITY.
-   **Back Up Your Data:** It is **strongly recommended** that you create a full backup of your game installation folder and save files before using this Tool.

By downloading and using this Tool, you agree to these terms.

---

## For Developers (Building from Source)

If you want to contribute or build the project yourself:

1.  Clone this repository: `git clone https://github.com/MrIkso/OEngineResourceReader`
2.  Open the `.sln` file in Visual Studio 2022.
3.  Make sure you have the .NET 8 SDK installed.
4.  Build the solution (Ctrl+Shift+B). The executable will be in the `bin/Debug` or `bin/Release` folder.

## License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

## Acknowledgments

Special thanks to the developers of any third-party libraries used in this project:
- [ImageView.PictureBox](https://github.com/tonyp7/ImageView)
- [BCnEncoder.Net](https://github.com/Nominom/BCnEncoder.NET)
- etc.
