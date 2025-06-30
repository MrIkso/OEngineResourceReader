namespace OEngineResourceReader.FontParser
{
    public class FontCalculator
    {
        public static (int spacingHoriz, int spacingVert) CalculateSpacing(FontData fontData)
        {
            if (fontData.Glyphs == null || fontData.Glyphs.Count < 2)
            {
                return (1, 1);
            }

            var glyphs = fontData.Glyphs;

            int minHorizontalSpacing = int.MaxValue;
            int minVerticalSpacing = int.MaxValue;

            for (int i = 0; i < glyphs.Count; i++)
            {
                var g1 = glyphs[i];
                var g1Right = g1.X + g1.Width;
                var g1Bottom = g1.Y + g1.Height;

                for (int j = 0; j < glyphs.Count; j++)
                {
                    if (i == j) continue;

                    var g2 = glyphs[j];

                    // check if g2 is to the left of g1
                    // if g2 is to the left of g1, we can check for horizontal spacing
                    if (g2.X >= g1Right && (g1.Y < g2.Y + g2.Height && g1Bottom > g2.Y))
                    {
                        int horizontalGap = (int)g2.X - (int)g1Right;
                        if (horizontalGap < minHorizontalSpacing)
                        {
                            minHorizontalSpacing = horizontalGap;
                        }
                    }

                    if (g2.Y >= g1Bottom && (g1.X < g2.X + g2.Width && g1Right > g2.X))
                    {
                        int verticalGap = (int)g2.Y - (int)g1Bottom;
                        if (verticalGap < minVerticalSpacing)
                        {
                            minVerticalSpacing = verticalGap;
                        }
                    }
                }
            }

           
            int finalHorizSpacing = (minHorizontalSpacing == int.MaxValue) ? 1 : minHorizontalSpacing;
            int finalVertSpacing = (minVerticalSpacing == int.MaxValue) ? 1 : minVerticalSpacing;

            return (finalHorizSpacing, finalVertSpacing);
        }

        // decompiled method from sub_7FF7785B1500
        public static void CalculateYOffsets(FontData font)
        {
            if (font.Glyphs == null || font.Glyphs.Count == 0)
            {
                font.MaxYOffset = -1.0f; // default values
                font.MinYOffset = 1.0f;
                return;
            }

            float maxYOffset = float.MinValue;
            float minYOffset = float.MaxValue;
            unsafe
            {
                foreach (var glyph in font.Glyphs)
                {
                    // top point of the glyph (XAdvance + XOffset)
                    float top = glyph.XAdvance + glyph.XOffset;
                    if (maxYOffset < top)
                    {
                        maxYOffset = top;
                    }

                    // buttom point of the glyph (XAdvance)
                    float bottom = glyph.XAdvance;
                    if (minYOffset > bottom)
                    {
                        minYOffset = bottom;
                    }
                }
            }

            font.MaxYOffset = maxYOffset;
            font.MinYOffset = minYOffset;
        }
    }
}
