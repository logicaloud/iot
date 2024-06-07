// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;

namespace Iot.Device.SenseHat
{
    /// <summary>
    /// Implementation of a proportional pixel font for a 8x8 bit display matrix
    /// </summary>
    public class SenseHatTextFont
    {
        // Defining Char width/height as constants for clarity, other values do not
        // work though because bit patterns are stored in bytes.
        private const int CharWidth = 8;
        private const int CharHeight = 8;
        // Limit length of text when rendering
        private const int MaxTextLength = 128;
        // Using Font5x8
        private Graphics.Font5x8 _font = new Graphics.Font5x8();

        /// <summary>
        /// Generates a byte matrix containing the bit pattern for the rendered text.
        /// </summary>
        /// <param name="text">The text to render</param>
        /// <returns>The initial renderstate including bitmap dimension</returns>
        public SenseHatTextRenderState RenderText(string text)
        {
            if (text.Length > MaxTextLength)
            {
                text = "Text is too long";
            }

            // Calculate "bitmap" width for mono space font
            var renderWidth = text.Length * (_font.Width + 1) - 1;

            // Reserve space for the rendered bitmap
            var matrix = new byte[renderWidth * CharHeight];

            var x = 0;
            foreach (var c in text)
            {
                for (var cx = 0; cx < _font.Width; cx++)
                {
                    _font.GetCharData(c, out var glyph);
                    var bitPattern = glyph[cx];
                    var y = 0;
                    byte flag = 1;
                    for (var cy = 0; cy < _font.Height; cy++, y++)
                    {
                        if ((bitPattern & flag) != 0)
                        {
                            // Set value to 1 to indicate that a pixel should be "on".
                            matrix[x + cx + (cy + _font.YDisplacement) * renderWidth] = 1;
                        }

                        flag <<= 1;
                    }
                }

                x += _font.XDisplacement;
            }

            return new SenseHatTextRenderState(text, matrix, renderWidth);
        }
    }
}
