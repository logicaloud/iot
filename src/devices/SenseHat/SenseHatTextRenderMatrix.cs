// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing;

namespace Iot.Device.SenseHat
{
    /// <summary>
    /// Helper class holding text rendering parameters
    /// </summary>
    public class SenseHatTextRenderMatrix
    {
        private const int LedMatrixWidth = 8;

        /// <summary>
        /// The x position within the PixelMatrix where rendering to the 8x8 LEDs should begin.
        /// </summary>
        private int _horizontalScrollPosition = 0;

        /// <summary>
        /// Construct initial render state/
        /// </summary>
        /// <param name="text">Rendered text.</param>
        /// <param name="pixelMatrix">Render h.LedMatrix.SetText containting text of size pixelMatrixWith * CharWidth.</param>
        /// <param name="pixelMatrixWidth">Width of the bitmap</param>
        public SenseHatTextRenderMatrix(string text, byte[] pixelMatrix, int pixelMatrixWidth)
        {
            Text = text;
            PixelMatrix = pixelMatrix;
            PixelMatrixWidth = pixelMatrixWidth;
        }

        /// <summary>
        /// Rendered text
        /// </summary>
        public readonly string Text;

        /// <summary>
        /// Matrix containing the bitmap of size 'PixelMatrixWidth * 8'. Values not equal zero indicate that a pixel should be set.
        /// </summary>
        public readonly byte[] PixelMatrix;

        /// <summary>
        /// The width of the rendered bitmap.
        /// </summary>
        public readonly int PixelMatrixWidth;

        /// <summary>
        /// Determine whether a pixel is "on"
        /// </summary>
        /// <param name="x">x position within the 8x8 matrix</param>
        /// <param name="y">y position within the 8x8 matrix</param>
        /// <returns></returns>
        public bool IsPixelSet(int x, int y)
        {
            if (PixelMatrixWidth == 0)
            {
                return false;
            }

            int effectiveX;
            if (PixelMatrixWidth < LedMatrixWidth)
            {
                // Center letter within LED matrix
                effectiveX = x - (LedMatrixWidth - PixelMatrixWidth) / 2;
                if (effectiveX < 0 || effectiveX >= PixelMatrixWidth)
                {
                    return false;
                }
            }
            else
            {
                // _horizontalScrollPosition may be not-zero if text is scrolled.
                effectiveX = (x + _horizontalScrollPosition) % PixelMatrixWidth;
            }

            return PixelMatrix[effectiveX + y * PixelMatrixWidth] != 0;
        }

        /// <summary>
        /// Move the text by one pixel
        /// </summary>
        public void ScrollByOnePixel()
        {
            if (PixelMatrixWidth > LedMatrixWidth)
            {
                _horizontalScrollPosition = (_horizontalScrollPosition + 1) % PixelMatrixWidth;
            }
        }
    }
}
