// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing;

namespace Iot.Device.SenseHat
{
    /// <summary>
    /// Helper class holding text rendering parameters
    /// </summary>
    public class SenseHatTextRenderState
    {
        /// <summary>
        /// The x position within the PixelMatrix where rendering to the 8x8 LEDs should begin.
        /// </summary>
        private int _horizontalScrollPosition = 0;

        /// <summary>
        /// Construct initial render state/
        /// </summary>
        /// <param name="pixelMatrix">Render bitmap containting text of size pixelMatrixWith * CharWidth.</param>
        /// <param name="pixelMatrixWidth">Width of the bitmap</param>
        public SenseHatTextRenderState(byte[] pixelMatrix, int pixelMatrixWidth)
        {
            PixelMatrix = pixelMatrix;
            PixelMatrixWidth = pixelMatrixWidth;
            TextColor = Color.Green;
            TextBackgroundColor = null; // no background color
        }

        /// <summary>
        /// Matrix containing the bitmap of size 'PixelMatrixWidth * 8'. Values not equal zero indicate that a pixel should be set.
        /// </summary>
        public readonly byte[] PixelMatrix;

        /// <summary>
        /// The width of the rendered bitmap.
        /// </summary>
        public readonly int PixelMatrixWidth;

        /// <summary>
        /// Text color.
        /// </summary>
        public Color TextColor { get; set; }

        /// <summary>
        /// Text background color.
        /// </summary>
        public Color? TextBackgroundColor { get; set; }

        /// <summary>
        /// Determine whether a pixel is "on"
        /// </summary>
        /// <param name="x">x position within the 8x8 matrix</param>
        /// <param name="y">y position within the 8x8 matrix</param>
        /// <returns></returns>
        public bool IsPixelSet(int x, int y)
        {
            // WindoxX may be not-zero if text is scrolled.
            var effectiveX = (x + _horizontalScrollPosition) % PixelMatrixWidth;
            return PixelMatrix[effectiveX + y * PixelMatrixWidth] != 0;
        }

        /// <summary>
        /// Move the text by one pixel
        /// </summary>
        public void ScrollByOnePixel()
        {
            _horizontalScrollPosition = (_horizontalScrollPosition + 1) % PixelMatrixWidth;
        }
    }
}
