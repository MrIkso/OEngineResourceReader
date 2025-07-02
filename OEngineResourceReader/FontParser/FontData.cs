using System.Drawing.Imaging;

namespace OEngineResourceReader.FontParser
{
    public class Glyph
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float XOffset { get; set; }
        public float YOffset { get; set; }
        public float XAdvance { get; set; }
        public float Page { get; set; }

        // texture coordinates (uv)
        // x / textureWidth
        public float U1 { get; set; }
        // y / textureHeight
        public float V1 { get; set; }
        // (x + width) / textureWidth
        public float U2 { get; set; }
        // (y + height) / textureHeight
        public float V2 { get; set; }
    }

    public class FontData
    {
        public int FileVersion { get; set; }
        public int FontVersion { get; set; }
        public float UseUnicode { get; set; }
        public int LineHeight { get; set; }
        public int Base { get; set; }
        public int TextureWidth { get; set; }
        public int TextureHeight { get; set; }
        public int Pages { get; set; }

        public int GlyphsCount { get; set; }
        public int KerningCount { get; set; }
        public string TexturePath { get; set; } = string.Empty;

        public float MaxYOffset { get; set; }
        public float MinYOffset { get; set; }

        // data arrays
        public List<Glyph> Glyphs { get; set; } = [];
        public int[] LookupTable { get; set; } = Array.Empty<int>();

        public Glyph? GetGlyph(char character)
        {

            int charId = character;
            if (LookupTable == null || charId >= LookupTable.Length)
            {
                return null;
            }

            int glyphIndex = LookupTable[charId];
            if (glyphIndex < 0 || glyphIndex >= Glyphs.Count)
            {
                return null;
            }
            return Glyphs[glyphIndex];
        }

        public Bitmap RenderText(string text, Bitmap fontTexture, Color? tintColor = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new Bitmap(1, 1);
            }

            List<Tuple<Glyph, PointF>> glyphPositions = new List<Tuple<Glyph, PointF>>();
            float currentX = 0;

            foreach (char c in text)
            {
                Glyph glyph = GetGlyph(c);
                if (glyph == null)
                {
                    continue;
                }

                float drawX = currentX + glyph.XOffset;
                float drawY = this.Base + glyph.YOffset;

                glyphPositions.Add(new Tuple<Glyph, PointF>(glyph, new PointF(drawX, drawY)));

                float advance = Math.Max(glyph.XAdvance, (glyph.Width + 0.5f) / 2);
                currentX += advance;
            }

            if (glyphPositions.Count == 0)
            {
                return new Bitmap(1, 1);
            }

            float minX = glyphPositions.Min(p => p.Item2.X);
            float maxX = glyphPositions.Max(p => p.Item2.X + p.Item1.Width);
            float minY = glyphPositions.Min(p => p.Item2.Y);
            float maxY = glyphPositions.Max(p => p.Item2.Y + p.Item1.Height);

            int totalWidth = (int)Math.Ceiling(maxX - minX);
            int totalHeight = (int)Math.Ceiling(maxY - minY);

            if (totalWidth <= 0 || totalHeight <= 0) return new Bitmap(1, 1);

            Bitmap resultBitmap = new Bitmap(totalWidth, totalHeight, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(resultBitmap))
            {
                g.Clear(Color.Transparent);

                foreach (var posInfo in glyphPositions)
                {
                    Glyph glyph = posInfo.Item1;
                    PointF position = posInfo.Item2;

                    RectangleF sourceRect = new RectangleF(glyph.X, glyph.Y, glyph.Width, glyph.Height);
                    RectangleF destRect = new RectangleF(
                        position.X - minX,
                        position.Y - minY,
                        glyph.Width,
                        glyph.Height);

                    g.DrawImage(fontTexture, destRect, sourceRect, GraphicsUnit.Pixel);
                }
            }

            if (tintColor.HasValue && tintColor.Value != Color.Transparent)
            {
                for (int y = 0; y < resultBitmap.Height; y++)
                {
                    for (int x = 0; x < resultBitmap.Width; x++)
                    {
                        Color pixel = resultBitmap.GetPixel(x, y);
                        if (pixel.A > 0)
                        {
                            Color newColor = Color.FromArgb(pixel.A, tintColor.Value.R, tintColor.Value.G, tintColor.Value.B);
                            resultBitmap.SetPixel(x, y, newColor);
                        }
                    }
                }
            }

            return resultBitmap;
        }

        public Glyph? GetGlyphAtPixel(float x, float y)
        {
            foreach (Glyph glyph in this.Glyphs)
            {
                float left = glyph.X;
                float top = glyph.Y;
                float right = glyph.X + glyph.Width;
                float bottom = glyph.Y + glyph.Height;
                if (x >= left && x <= right &&
                    y >= top && y <= bottom)
                {
                    return glyph;
                }
            }
            return null;
        }

        public char FindCharacterForGlyphIndex(int glyphIndex)
        {

            if (LookupTable == null || glyphIndex < 0 || glyphIndex >= Glyphs.Count)
            {
                return char.MaxValue;
            }

            for (int charId = 0; charId < LookupTable.Length; charId++)
            {
                if (LookupTable[charId] == glyphIndex)
                {
                    // Return the character corresponding to the glyph index
                    return Convert.ToChar(charId);
                }
            }

            return char.MaxValue;
        }

        public Bitmap? GetGlyphBitmap(Glyph glyph, Bitmap fontTexture)
        {
            if (glyph == null || fontTexture == null)
            {
                return null;
            }
            if (glyph.Width <= 0 || glyph.Height <= 0)
            {
                return null;
            }
            Rectangle sourceRect = new Rectangle(
                (int)glyph.X,
                (int)glyph.Y,
                (int)glyph.Width,
                (int)glyph.Height
            );

            if (sourceRect.Right > fontTexture.Width || sourceRect.Bottom > fontTexture.Height)
            {
                Console.WriteLine($"Warning: Glyph coordinates [{sourceRect}] are out of texture bounds [{fontTexture.Width}x{fontTexture.Height}].");
                return null;
            }

            Bitmap glyphBitmap = new Bitmap(sourceRect.Width, sourceRect.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(glyphBitmap))
            {
                g.DrawImage(
                    fontTexture,
                    new Rectangle(0, 0, sourceRect.Width, sourceRect.Height),
                    sourceRect,
                    GraphicsUnit.Pixel
                );
            }

            return glyphBitmap;
        }
    }
}
