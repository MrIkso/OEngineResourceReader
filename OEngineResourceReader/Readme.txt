OEngine Resource Reader v%version%

Main Features:
- View graphic files (textures).
- View fonts.
- View and edit text files.
- Replace textures.
- Replace fonts.

Working with Files:
- Opening a single text document: Drag file into program window or use menu File -> Load Text.
- Opening a single texture: Drag file into program window or use menu File -> Open Texture File.
- Opening a single font file: Drag file into program window or use menu File -> Load Font.
- Opening an entire folder: For convenient project navigation, drag a folder (usually `_Cooking` folder in game's resource directory) into program window or use menu File -> Open Resource Directory.
- Exporting a texture as a ready-to-edit .dds graphic file: File -> Export To Texture File.
- Replacing current texture with another .dds graphic file: File -> Replace Texture. Afterwards, you must save changes via File -> Save Modified Texture.
- Exporting font configuration to create a new font in BMFont: File -> Export Font Configuration to BMFont.
- Generating a new font from a .fnt file created in BMFont: File -> Generate new font from .fnt.

Interface:
- Folders:
    - Navigate file tree with mouse or keyboard arrows. Open a file by clicking it or pressing Enter.
    - context menu for a tree item is opened with a right-click (RMB).
    - Below file tree is a filter where names or extensions can be entered, separated by commas (e.g., *.dxt, Text).
    - Tree has context menu allows you to open file in Windows Explorer or copy path to clipboard.

- Files:
    - Different file types (text, texture, font) open in their own tabs. name of currently open file from selected tab is displayed at top of program window.
    - Text:
        - Text files can be edited directly within program. number of lines in "Original Text" column is displayed at top. translation is entered into "Translation" column.
        - `Save` button saves changes to a file location specified in dialog box.
        - `Export` button exports `current view of table` (not original file content) as a .json file, which can be uploaded to third-party services like Crowdin or SmartCat.
        - `Import` button imports a previously exported .json file back into program, populating "Translation" column if its content differs from "Original Text".
        - `File Version` is a technical parameter and is not relevant for end-user.
        - search field works for both columns.

    - Texture:
        - Zooming is supported with mouse wheel while holding `Ctrl` key and with touchpad gestures. A left-click zooms in, while `Ctrl` + left-click zooms out.
        - `B` (or `W`) key toggles viewer background between black and white.
        - `Texture Format` is technical information that will be needed to generate and replace a texture in an external program. new texture must have same format.

    - Font:
        - There is a preview window, font information, and data about glyph currently under mouse cursor.
        - `Live Preview Text` is a field where you can enter any characters to see how they will be displayed with selected font and to check which characters are supported.

Workflow:
- Replacing Text:
    1. Edit text directly in program (only rows where "Translation" field is not empty are saved; others remain unchanged).
    2. Alternatively, export file to .json, edit it in an external service, export it from there in same .json format, and import it back into program. Then, save file.
- Replacing a Texture:
    1. Open texture file in program.
    2. Export it as a .dds image.
    3. Edit it in an external editor (e.g., GIMP).
    4. Replace image in program (File -> Replace Texture) and save it (File -> Save Modified Texture).
- Replacing a Font:
    1. Open font file (not texture).
    2. Export font configuration file.
    3. Create a new font and its texture using BMFont (https://www.angelcode.com/products/bmfont/).
       Important: font description file exported from BMFont must be in text-based .fnt format, and both .fnt file and font texture file must be in same folder.
    4. Generate new font from created .fnt file via File -> Generate new font from .fnt. font texture will be automatically replaced and saved.
       Important! When saving font, you must save it with same name as original file so that game can recognize it and font texture is replaced correctly.
- Replacing fonts for old version:
    1. Open font file.
    2. Export font configuration for BMFont.
    3. Create a new font in BMFont and save it in text format (.fnt).
    4. Copy text of new .fnt file and replace it in original .fnt font file.
    Remember that font file name must remain unchanged and font texture name that is in .fnt file must also match original one so that game can recognize it.
    5. Replace font texture with new texture created in BMFont.

- Previewing fonts:
    1. Open font file.
    2. View glyphs, their sizes and other parameters.
    3. Use Live Preview Text field to check character support.
    Also, in font information block you can see which texture font uses, its size.

Developers: MrIkso, Sent_DeZ

Disclaimer
    This tool is an unofficial, third-party free utility created for community use. 
    It is not affiliated with, supported by the original game developers.
    Use this software at your own risk. The author is not responsible for any damage to your game files, save data, or system.
    Always back up your files before making any modifications.

Program uses following libraries:
- BCnEncoder.Net (https://github.com/Nominom/BCnEncoder.NET)
- PictureBox (https://github.com/tonyp7/ImageView)

Ukraine, 2025