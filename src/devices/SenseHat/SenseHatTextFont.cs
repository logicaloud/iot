// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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

        private class Glyph
        {
            public Glyph(int id, byte width, byte height, sbyte xoffset, byte yoffset, byte xadvance, byte[] pixels)
            {
                Id = id;

                Width = width;
                Height = height;
                Xoffset = xoffset;
                Yoffset = yoffset;
                Xadvance = xadvance;
                Pixels = pixels;
            }

            // Char code
            public readonly int Id;
            // Char width, may be less than 8
            public readonly byte Width;
            // Char height, may be less than 8
            public readonly byte Height;
            // X offset to position the character correctly within an 8x8 matrix
            public readonly sbyte Xoffset;
            // Y offset to position the character correctly within an 8x8 matrix
            public readonly byte Yoffset;
            // X advancement to place the next character in a sequence of characters
            public readonly byte Xadvance;
            // Pixel code; each value represents a vertical bit pattern within the character
            public readonly byte[] Pixels;
        }

        private readonly Dictionary<int, Glyph> _definitions = new Dictionary<int, Glyph>()
        {
            /*blank*/{ 32, new Glyph(32, 3, 1, -1, 7, 4, new byte[] { 0x00, 0x00, 0x00 }) },
            /*!*/{ 33, new Glyph(33, 1, 7, 2, 0, 5, new byte[] { 0x5F }) },
            /*"*/{ 34, new Glyph(34, 3, 3, 2, 0, 7, new byte[] { 0x07, 0x00, 0x07 }) },
            /*#*/{ 35, new Glyph(35, 6, 6, 1, 1, 8, new byte[] { 0x12, 0x3F, 0x12, 0x12, 0x3F, 0x12 }) },
            /*$*/{ 36, new Glyph(36, 5, 7, 1, 0, 7, new byte[] { 0x24, 0x2A, 0x7F, 0x2A, 0x10 }) },
            /*%*/{ 37, new Glyph(37, 7, 7, 1, 0, 9, new byte[] { 0x06, 0x29, 0x16, 0x08, 0x34, 0x4A, 0x30 }) },
            /*&*/{ 38, new Glyph(38, 5, 7, 1, 0, 7, new byte[] { 0x36, 0x49, 0x49, 0x49, 0x72 }) },
            /*'*/{ 39, new Glyph(39, 1, 3, 2, 0, 5, new byte[] { 0x07 }) },
            /*(*/{ 40, new Glyph(40, 3, 7, 3, 0, 7, new byte[] { 0x1C, 0x22, 0x41 }) },
            /*)*/{ 41, new Glyph(41, 3, 7, 1, 0, 7, new byte[] { 0x41, 0x22, 0x1C }) },
            /***/{ 42, new Glyph(42, 5, 5, 1, 0, 7, new byte[] { 0x0A, 0x04, 0x1F, 0x04, 0x0A }) },
            /*+*/{ 43, new Glyph(43, 5, 5, 1, 1, 7, new byte[] { 0x04, 0x04, 0x1F, 0x04, 0x04 }) },
            /*,*/{ 44, new Glyph(44, 2, 3, 1, 5, 5, new byte[] { 0x04, 0x03 }) },
            /*-*/{ 45, new Glyph(45, 4, 1, 1, 3, 6, new byte[] { 0x01, 0x01, 0x01, 0x01 }) },
            /*.*/{ 46, new Glyph(46, 1, 1, 2, 6, 5, new byte[] { 0x01 }) },
            /*/*/{ 47, new Glyph(47, 3, 7, 1, 0, 5, new byte[] { 0x60, 0x1C, 0x03 }) },
            /*0*/{ 48, new Glyph(48, 5, 7, 1, 0, 7, new byte[] { 0x3E, 0x51, 0x49, 0x45, 0x3E }) },
            /*1*/{ 49, new Glyph(49, 3, 7, 2, 0, 7, new byte[] { 0x04, 0x02, 0x7F }) },
            /*2*/{ 50, new Glyph(50, 5, 7, 1, 0, 7, new byte[] { 0x42, 0x61, 0x51, 0x49, 0x46 }) },
            /*3*/{ 51, new Glyph(51, 5, 7, 1, 0, 7, new byte[] { 0x22, 0x41, 0x49, 0x49, 0x36 }) },
            /*4*/{ 52, new Glyph(52, 5, 7, 1, 0, 7, new byte[] { 0x18, 0x14, 0x12, 0x11, 0x7F }) },
            /*5*/{ 53, new Glyph(53, 5, 7, 1, 0, 7, new byte[] { 0x27, 0x45, 0x45, 0x45, 0x39 }) },
            /*6*/{ 54, new Glyph(54, 5, 7, 1, 0, 7, new byte[] { 0x3E, 0x49, 0x49, 0x49, 0x32 }) },
            /*7*/{ 55, new Glyph(55, 5, 7, 1, 0, 7, new byte[] { 0x61, 0x11, 0x09, 0x05, 0x03 }) },
            /*8*/{ 56, new Glyph(56, 5, 7, 1, 0, 7, new byte[] { 0x36, 0x49, 0x49, 0x49, 0x36 }) },
            /*9*/{ 57, new Glyph(57, 5, 7, 1, 0, 7, new byte[] { 0x26, 0x49, 0x49, 0x49, 0x3E }) },
            /*:*/{ 58, new Glyph(58, 1, 5, 2, 2, 5, new byte[] { 0x11 }) },
            /*;*/{ 59, new Glyph(59, 2, 6, 1, 2, 5, new byte[] { 0x20, 0x19 }) },
            /*<*/{ 60, new Glyph(60, 3, 5, 1, 1, 5, new byte[] { 0x04, 0x0A, 0x11 }) },
            /*=*/{ 61, new Glyph(61, 4, 3, 1, 2, 6, new byte[] { 0x05, 0x05, 0x05, 0x05 }) },
            /*>*/{ 62, new Glyph(62, 3, 5, 1, 1, 5, new byte[] { 0x11, 0x0A, 0x04 }) },
            /*?*/{ 63, new Glyph(63, 5, 7, 1, 0, 7, new byte[] { 0x02, 0x01, 0x51, 0x09, 0x06 }) },
            /*@*/{ 64, new Glyph(64, 7, 8, 1, 0, 9, new byte[] { 0x7E, 0x81, 0x99, 0xA5, 0xBD, 0xA1, 0x1E }) },
            /*A*/{ 65, new Glyph(65, 5, 7, 1, 0, 7, new byte[] { 0x7E, 0x11, 0x11, 0x11, 0x7E }) },
            /*B*/{ 66, new Glyph(66, 5, 7, 1, 0, 7, new byte[] { 0x7F, 0x49, 0x49, 0x49, 0x36 }) },
            /*C*/{ 67, new Glyph(67, 5, 7, 1, 0, 7, new byte[] { 0x3E, 0x41, 0x41, 0x41, 0x22 }) },
            /*D*/{ 68, new Glyph(68, 5, 7, 1, 0, 7, new byte[] { 0x7F, 0x41, 0x41, 0x41, 0x3E }) },
            /*E*/{ 69, new Glyph(69, 5, 7, 1, 0, 7, new byte[] { 0x7F, 0x49, 0x49, 0x41, 0x41 }) },
            /*F*/{ 70, new Glyph(70, 5, 7, 1, 0, 7, new byte[] { 0x7F, 0x09, 0x09, 0x01, 0x01 }) },
            /*G*/{ 71, new Glyph(71, 5, 7, 1, 0, 7, new byte[] { 0x3E, 0x41, 0x41, 0x49, 0x7A }) },
            /*H*/{ 72, new Glyph(72, 5, 7, 1, 0, 7, new byte[] { 0x7F, 0x08, 0x08, 0x08, 0x7F }) },
            /*I*/{ 73, new Glyph(73, 1, 7, 2, 0, 5, new byte[] { 0x7F }) },
            /*J*/{ 74, new Glyph(74, 5, 7, 1, 0, 7, new byte[] { 0x20, 0x40, 0x40, 0x40, 0x3F }) },
            /*K*/{ 75, new Glyph(75, 5, 7, 1, 0, 7, new byte[] { 0x7F, 0x08, 0x14, 0x22, 0x41 }) },
            /*L*/{ 76, new Glyph(76, 5, 7, 1, 0, 7, new byte[] { 0x7F, 0x40, 0x40, 0x40, 0x40 }) },
            /*M*/{ 77, new Glyph(77, 7, 7, 1, 0, 9, new byte[] { 0x7F, 0x04, 0x08, 0x10, 0x08, 0x04, 0x7F }) },
            /*N*/{ 78, new Glyph(78, 5, 7, 1, 0, 7, new byte[] { 0x7F, 0x04, 0x08, 0x10, 0x7F }) },
            /*O*/{ 79, new Glyph(79, 5, 7, 1, 0, 7, new byte[] { 0x3E, 0x41, 0x41, 0x41, 0x3E }) },
            /*P*/{ 80, new Glyph(80, 5, 7, 1, 0, 7, new byte[] { 0x7F, 0x11, 0x11, 0x11, 0x0E }) },
            /*Q*/{ 81, new Glyph(81, 5, 7, 1, 0, 7, new byte[] { 0x3E, 0x41, 0x51, 0x21, 0x5E }) },
            /*R*/{ 82, new Glyph(82, 5, 7, 1, 0, 7, new byte[] { 0x7F, 0x11, 0x11, 0x31, 0x4E }) },
            /*S*/{ 83, new Glyph(83, 5, 7, 1, 0, 7, new byte[] { 0x26, 0x49, 0x49, 0x49, 0x32 }) },
            /*T*/{ 84, new Glyph(84, 5, 7, 1, 0, 7, new byte[] { 0x01, 0x01, 0x7F, 0x01, 0x01 }) },
            /*U*/{ 85, new Glyph(85, 5, 7, 1, 0, 7, new byte[] { 0x3F, 0x40, 0x40, 0x40, 0x3F }) },
            /*V*/{ 86, new Glyph(86, 5, 7, 1, 0, 7, new byte[] { 0x1F, 0x20, 0x40, 0x20, 0x1F }) },
            /*W*/{ 87, new Glyph(87, 7, 7, 1, 0, 9, new byte[] { 0x3F, 0x40, 0x40, 0x3C, 0x40, 0x40, 0x3F }) },
            /*X*/{ 88, new Glyph(88, 5, 7, 1, 0, 7, new byte[] { 0x63, 0x14, 0x08, 0x14, 0x63 }) },
            /*Y*/{ 89, new Glyph(89, 5, 7, 1, 0, 7, new byte[] { 0x03, 0x04, 0x78, 0x04, 0x03 }) },
            /*Z*/{ 90, new Glyph(90, 5, 7, 1, 0, 7, new byte[] { 0x61, 0x51, 0x49, 0x45, 0x43 }) },
            /*[*/{ 91, new Glyph(91, 3, 7, 3, 0, 7, new byte[] { 0x7F, 0x41, 0x41 }) },
            /*\*/{ 92, new Glyph(92, 3, 7, 1, 0, 5, new byte[] { 0x03, 0x1C, 0x60 }) },
            /*]*/{ 93, new Glyph(93, 3, 7, 1, 0, 7, new byte[] { 0x41, 0x41, 0x7F }) },
            /*^*/{ 94, new Glyph(94, 5, 3, 1, 0, 7, new byte[] { 0x04, 0x02, 0x01, 0x02, 0x04 }) },
            /*_*/{ 95, new Glyph(95, 5, 1, 0, 7, 5, new byte[] { 0x01, 0x01, 0x01, 0x01, 0x01 }) },
            /*`*/{ 96, new Glyph(96, 2, 2, 1, 0, 5, new byte[] { 0x01, 0x02 }) },
            /*a*/{ 97, new Glyph(97, 5, 5, 1, 2, 7, new byte[] { 0x08, 0x15, 0x15, 0x15, 0x1E }) },
            /*b*/{ 98, new Glyph(98, 5, 7, 1, 0, 7, new byte[] { 0x7F, 0x44, 0x44, 0x44, 0x38 }) },
            /*c*/{ 99, new Glyph(99, 5, 5, 1, 2, 7, new byte[] { 0x0E, 0x11, 0x11, 0x11, 0x0A }) },
            /*d*/{ 100, new Glyph(100, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x44, 0x44, 0x44, 0x7F }) },
            /*e*/{ 101, new Glyph(101, 5, 5, 1, 2, 7, new byte[] { 0x0E, 0x15, 0x15, 0x15, 0x06 }) },
            /*f*/{ 102, new Glyph(102, 4, 7, 1, 0, 6, new byte[] { 0x04, 0x7E, 0x05, 0x05 }) },
            /*g*/{ 103, new Glyph(103, 5, 6, 1, 2, 7, new byte[] { 0x06, 0x29, 0x29, 0x29, 0x1F }) },
            /*h*/{ 104, new Glyph(104, 5, 7, 1, 0, 7, new byte[] { 0x7F, 0x04, 0x04, 0x04, 0x78 }) },
            /*i*/{ 105, new Glyph(105, 1, 7, 2, 0, 5, new byte[] { 0x7D }) },
            /*j*/{ 106, new Glyph(106, 5, 8, 1, 0, 7, new byte[] { 0x40, 0x80, 0x80, 0x80, 0x7D }) },
            /*k*/{ 107, new Glyph(107, 5, 7, 1, 0, 7, new byte[] { 0x7F, 0x10, 0x18, 0x24, 0x40 }) },
            /*l*/{ 108, new Glyph(108, 2, 7, 2, 0, 5, new byte[] { 0x3F, 0x40 }) },
            /*m*/{ 109, new Glyph(109, 7, 5, 1, 2, 9, new byte[] { 0x1F, 0x01, 0x01, 0x06, 0x01, 0x01, 0x1E }) },
            /*n*/{ 110, new Glyph(110, 5, 5, 1, 2, 7, new byte[] { 0x1F, 0x01, 0x01, 0x01, 0x1E }) },
            /*o*/{ 111, new Glyph(111, 5, 5, 1, 2, 7, new byte[] { 0x0E, 0x11, 0x11, 0x11, 0x0E }) },
            /*p*/{ 112, new Glyph(112, 5, 6, 1, 2, 7, new byte[] { 0x3F, 0x09, 0x09, 0x09, 0x06 }) },
            /*q*/{ 113, new Glyph(113, 5, 6, 1, 2, 7, new byte[] { 0x06, 0x09, 0x09, 0x09, 0x3F }) },
            /*r*/{ 114, new Glyph(114, 5, 5, 1, 2, 7, new byte[] { 0x1F, 0x04, 0x02, 0x01, 0x01 }) },
            /*s*/{ 115, new Glyph(115, 5, 5, 1, 2, 7, new byte[] { 0x12, 0x15, 0x15, 0x15, 0x08 }) },
            /*t*/{ 116, new Glyph(116, 4, 6, 1, 1, 6, new byte[] { 0x02, 0x1F, 0x22, 0x22 }) },
            /*u*/{ 117, new Glyph(117, 5, 5, 1, 2, 7, new byte[] { 0x0F, 0x10, 0x10, 0x10, 0x0F }) },
            /*v*/{ 118, new Glyph(118, 5, 5, 1, 2, 7, new byte[] { 0x07, 0x08, 0x10, 0x08, 0x07 }) },
            /*w*/{ 119, new Glyph(119, 7, 5, 1, 2, 9, new byte[] { 0x0F, 0x10, 0x10, 0x0C, 0x10, 0x10, 0x0F }) },
            /*x*/{ 120, new Glyph(120, 5, 5, 1, 2, 7, new byte[] { 0x11, 0x0A, 0x04, 0x0A, 0x11 }) },
            /*y*/{ 121, new Glyph(121, 5, 6, 1, 2, 7, new byte[] { 0x07, 0x28, 0x28, 0x28, 0x1F }) },
            /*z*/{ 122, new Glyph(122, 5, 5, 1, 2, 7, new byte[] { 0x11, 0x19, 0x15, 0x13, 0x11 }) },
            /*{*/{ 123, new Glyph(123, 4, 7, 2, 0, 7, new byte[] { 0x08, 0x36, 0x41, 0x41 }) },
            /*|*/{ 124, new Glyph(124, 1, 7, 2, 0, 5, new byte[] { 0x7F }) },
            /*}*/{ 125, new Glyph(125, 4, 7, 1, 0, 7, new byte[] { 0x41, 0x41, 0x36, 0x08 }) },
            /*~*/{ 126, new Glyph(126, 6, 2, 1, 0, 8, new byte[] { 0x02, 0x01, 0x01, 0x02, 0x02, 0x01 }) },
            /*blank*/{ 160, new Glyph(160, 3, 1, -1, 7, 4, new byte[] { 0x00, 0x00, 0x00 }) },
            /*¡*/{ 161, new Glyph(161, 1, 7, 2, 1, 5, new byte[] { 0x7D }) },
            /*¢*/{ 162, new Glyph(162, 5, 7, 1, 0, 7, new byte[] { 0x1C, 0x22, 0x7F, 0x22, 0x14 }) },
            /*£*/{ 163, new Glyph(163, 6, 7, 1, 0, 8, new byte[] { 0x48, 0x7E, 0x49, 0x49, 0x41, 0x22 }) },
            /*¥*/{ 165, new Glyph(165, 5, 7, 1, 0, 7, new byte[] { 0x2B, 0x2C, 0x78, 0x2C, 0x2B }) },
            /*¦*/{ 166, new Glyph(166, 1, 7, 2, 0, 5, new byte[] { 0x77 }) },
            /*¨*/{ 168, new Glyph(168, 3, 1, 2, 0, 7, new byte[] { 0x01, 0x00, 0x01 }) },
            /*©*/{ 169, new Glyph(169, 7, 8, 1, 0, 9, new byte[] { 0x7E, 0x81, 0x99, 0xA5, 0xA5, 0x81, 0x7E }) },
            /*«*/{ 171, new Glyph(171, 6, 5, 1, 1, 8, new byte[] { 0x04, 0x0A, 0x11, 0x04, 0x0A, 0x11 }) },
            /*¬*/{ 172, new Glyph(172, 5, 3, 1, 3, 7, new byte[] { 0x01, 0x01, 0x01, 0x01, 0x07 }) },
            /*®*/{ 174, new Glyph(174, 7, 8, 1, 0, 9, new byte[] { 0x7E, 0x81, 0xBD, 0x95, 0xA9, 0x81, 0x7E }) },
            /*°*/{ 176, new Glyph(176, 4, 4, 1, 0, 6, new byte[] { 0x06, 0x09, 0x09, 0x06 }) },
            /*±*/{ 177, new Glyph(177, 5, 7, 1, 0, 7, new byte[] { 0x44, 0x44, 0x5F, 0x44, 0x44 }) },
            /*´*/{ 180, new Glyph(180, 2, 2, 2, 0, 5, new byte[] { 0x02, 0x01 }) },
            /*µ*/{ 181, new Glyph(181, 5, 6, 1, 2, 7, new byte[] { 0x3F, 0x08, 0x08, 0x08, 0x07 }) },
            /*¶*/{ 182, new Glyph(182, 7, 7, 1, 0, 9, new byte[] { 0x06, 0x0F, 0x0F, 0x7F, 0x01, 0x01, 0x7F }) },
            /*·*/{ 183, new Glyph(183, 1, 1, 2, 3, 5, new byte[] { 0x01 }) },
            /*¸*/{ 184, new Glyph(184, 3, 3, 1, 5, 5, new byte[] { 0x04, 0x05, 0x02 }) },
            /*»*/{ 187, new Glyph(187, 6, 5, 1, 1, 8, new byte[] { 0x11, 0x0A, 0x04, 0x11, 0x0A, 0x04 }) },
            /*¿*/{ 191, new Glyph(191, 5, 7, 1, 1, 7, new byte[] { 0x30, 0x48, 0x45, 0x40, 0x20 }) },
            /*À*/{ 192, new Glyph(192, 5, 7, 1, 0, 7, new byte[] { 0x78, 0x25, 0x26, 0x24, 0x78 }) },
            /*Á*/{ 193, new Glyph(193, 5, 7, 1, 0, 7, new byte[] { 0x78, 0x24, 0x26, 0x25, 0x78 }) },
            /*Â*/{ 194, new Glyph(194, 5, 7, 1, 0, 7, new byte[] { 0x78, 0x26, 0x25, 0x26, 0x78 }) },
            /*Ã*/{ 195, new Glyph(195, 6, 7, 1, 0, 7, new byte[] { 0x7A, 0x25, 0x25, 0x26, 0x7A, 0x01 }) },
            /*Ä*/{ 196, new Glyph(196, 5, 7, 1, 0, 7, new byte[] { 0x78, 0x25, 0x24, 0x25, 0x78 }) },
            /*Å*/{ 197, new Glyph(197, 5, 7, 1, 0, 7, new byte[] { 0x7A, 0x25, 0x25, 0x25, 0x7A }) },
            /*Æ*/{ 198, new Glyph(198, 9, 7, 1, 0, 11, new byte[] { 0x7E, 0x11, 0x11, 0x11, 0x7F, 0x49, 0x49, 0x41, 0x41 }) },
            /*Ç*/{ 199, new Glyph(199, 5, 8, 1, 0, 7, new byte[] { 0x1E, 0xA1, 0xA1, 0x61, 0x12 }) },
            /*È*/{ 200, new Glyph(200, 5, 7, 1, 0, 7, new byte[] { 0x7C, 0x55, 0x56, 0x44, 0x44 }) },
            /*É*/{ 201, new Glyph(201, 5, 7, 1, 0, 7, new byte[] { 0x7C, 0x54, 0x56, 0x45, 0x44 }) },
            /*Ê*/{ 202, new Glyph(202, 5, 7, 1, 0, 7, new byte[] { 0x7C, 0x56, 0x55, 0x46, 0x44 }) },
            /*Ë*/{ 203, new Glyph(203, 5, 7, 1, 0, 7, new byte[] { 0x7C, 0x55, 0x54, 0x45, 0x44 }) },
            /*Ì*/{ 204, new Glyph(204, 2, 7, 1, 0, 5, new byte[] { 0x01, 0x7A }) },
            /*Í*/{ 205, new Glyph(205, 2, 7, 2, 0, 5, new byte[] { 0x7A, 0x01 }) },
            /*Î*/{ 206, new Glyph(206, 3, 7, 1, 0, 5, new byte[] { 0x02, 0x79, 0x02 }) },
            /*Ï*/{ 207, new Glyph(207, 3, 7, 1, 0, 5, new byte[] { 0x01, 0x7C, 0x01 }) },
            /*Ð*/{ 208, new Glyph(208, 6, 7, 0, 0, 7, new byte[] { 0x08, 0x7F, 0x49, 0x49, 0x41, 0x3E }) },
            /*Ñ*/{ 209, new Glyph(209, 6, 7, 1, 0, 7, new byte[] { 0x7E, 0x09, 0x11, 0x22, 0x7E, 0x01 }) },
            /*Ò*/{ 210, new Glyph(210, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x45, 0x46, 0x44, 0x38 }) },
            /*Ó*/{ 211, new Glyph(211, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x44, 0x46, 0x45, 0x38 }) },
            /*Ô*/{ 212, new Glyph(212, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x46, 0x45, 0x46, 0x38 }) },
            /*Õ*/{ 213, new Glyph(213, 6, 7, 1, 0, 7, new byte[] { 0x3A, 0x45, 0x45, 0x46, 0x3A, 0x01 }) },
            /*Ö*/{ 214, new Glyph(214, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x45, 0x44, 0x45, 0x38 }) },
            /*×*/{ 215, new Glyph(215, 5, 5, 1, 1, 7, new byte[] { 0x11, 0x0A, 0x04, 0x0A, 0x11 }) },
            /*Ø*/{ 216, new Glyph(216, 7, 7, 0, 0, 7, new byte[] { 0x40, 0x3E, 0x51, 0x49, 0x45, 0x3E, 0x01 }) },
            /*Ù*/{ 217, new Glyph(217, 5, 7, 1, 0, 7, new byte[] { 0x3C, 0x41, 0x42, 0x40, 0x3C }) },
            /*Ú*/{ 218, new Glyph(218, 5, 7, 1, 0, 7, new byte[] { 0x3C, 0x40, 0x42, 0x41, 0x3C }) },
            /*Û*/{ 219, new Glyph(219, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x42, 0x41, 0x42, 0x38 }) },
            /*Ü*/{ 220, new Glyph(220, 5, 7, 1, 0, 7, new byte[] { 0x3C, 0x41, 0x40, 0x41, 0x3C }) },
            /*Ý*/{ 221, new Glyph(221, 5, 7, 1, 0, 7, new byte[] { 0x0C, 0x10, 0x62, 0x11, 0x0C }) },
            /*Þ*/{ 222, new Glyph(222, 5, 7, 1, 0, 7, new byte[] { 0x7F, 0x12, 0x12, 0x12, 0x0C }) },
            /*ß*/{ 223, new Glyph(223, 5, 7, 1, 0, 7, new byte[] { 0x7E, 0x01, 0x49, 0x49, 0x36 }) },
            /*à*/{ 224, new Glyph(224, 5, 7, 1, 0, 7, new byte[] { 0x20, 0x55, 0x56, 0x54, 0x78 }) },
            /*á*/{ 225, new Glyph(225, 5, 7, 1, 0, 7, new byte[] { 0x20, 0x54, 0x56, 0x55, 0x78 }) },
            /*â*/{ 226, new Glyph(226, 5, 7, 1, 0, 7, new byte[] { 0x20, 0x56, 0x55, 0x56, 0x78 }) },
            /*ã*/{ 227, new Glyph(227, 6, 7, 1, 0, 7, new byte[] { 0x22, 0x55, 0x55, 0x56, 0x7A, 0x01 }) },
            /*ä*/{ 228, new Glyph(228, 5, 7, 1, 0, 7, new byte[] { 0x20, 0x55, 0x54, 0x55, 0x78 }) },
            /*å*/{ 229, new Glyph(229, 5, 7, 1, 0, 7, new byte[] { 0x22, 0x55, 0x55, 0x55, 0x7A }) },
            /*æ*/{ 230, new Glyph(230, 9, 5, 1, 2, 11, new byte[] { 0x08, 0x15, 0x15, 0x15, 0x0E, 0x15, 0x15, 0x15, 0x06 }) },
            /*ç*/{ 231, new Glyph(231, 5, 8, 1, 0, 7, new byte[] { 0x0E, 0x91, 0xB1, 0x51, 0x0A }) },
            /*è*/{ 232, new Glyph(232, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x55, 0x56, 0x54, 0x18 }) },
            /*é*/{ 233, new Glyph(233, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x54, 0x56, 0x55, 0x18 }) },
            /*ê*/{ 234, new Glyph(234, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x56, 0x55, 0x56, 0x18 }) },
            /*ë*/{ 235, new Glyph(235, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x55, 0x54, 0x55, 0x18 }) },
            /*ì*/{ 236, new Glyph(236, 2, 7, 1, 0, 5, new byte[] { 0x01, 0x7A }) },
            /*í*/{ 237, new Glyph(237, 2, 7, 2, 0, 5, new byte[] { 0x7A, 0x01 }) },
            /*î*/{ 238, new Glyph(238, 3, 7, 1, 0, 5, new byte[] { 0x02, 0x79, 0x02 }) },
            /*ï*/{ 239, new Glyph(239, 3, 7, 1, 0, 5, new byte[] { 0x01, 0x7C, 0x01 }) },
            /*ð*/{ 240, new Glyph(240, 6, 7, 1, 0, 7, new byte[] { 0x30, 0x48, 0x4A, 0x4A, 0x7F, 0x02 }) },
            /*ñ*/{ 241, new Glyph(241, 6, 7, 1, 0, 7, new byte[] { 0x7E, 0x05, 0x05, 0x06, 0x7A, 0x01 }) },
            /*ò*/{ 242, new Glyph(242, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x45, 0x46, 0x44, 0x38 }) },
            /*ó*/{ 243, new Glyph(243, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x44, 0x46, 0x45, 0x38 }) },
            /*ô*/{ 244, new Glyph(244, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x46, 0x45, 0x46, 0x38 }) },
            /*õ*/{ 245, new Glyph(245, 6, 7, 1, 0, 7, new byte[] { 0x3A, 0x45, 0x45, 0x46, 0x3A, 0x01 }) },
            /*ö*/{ 246, new Glyph(246, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x45, 0x44, 0x45, 0x38 }) },
            /*÷*/{ 247, new Glyph(247, 5, 5, 1, 1, 7, new byte[] { 0x04, 0x04, 0x15, 0x04, 0x04 }) },
            /*ø*/{ 248, new Glyph(248, 5, 7, 1, 1, 7, new byte[] { 0x5C, 0x32, 0x2A, 0x26, 0x1D }) },
            /*ù*/{ 249, new Glyph(249, 5, 7, 1, 0, 7, new byte[] { 0x3C, 0x41, 0x42, 0x40, 0x3C }) },
            /*ú*/{ 250, new Glyph(250, 5, 7, 1, 0, 7, new byte[] { 0x3C, 0x40, 0x42, 0x41, 0x3C }) },
            /*û*/{ 251, new Glyph(251, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x42, 0x41, 0x42, 0x38 }) },
            /*ü*/{ 252, new Glyph(252, 5, 7, 1, 0, 7, new byte[] { 0x3C, 0x41, 0x40, 0x41, 0x3C }) },
            /*ý*/{ 253, new Glyph(253, 5, 8, 1, 0, 7, new byte[] { 0x1C, 0xA0, 0xA2, 0xA1, 0x7C }) },
            /*þ*/{ 254, new Glyph(254, 5, 8, 1, 0, 7, new byte[] { 0xFF, 0x24, 0x24, 0x24, 0x18 }) },
            /*ÿ*/{ 255, new Glyph(255, 5, 8, 1, 0, 7, new byte[] { 0x1C, 0xA1, 0xA0, 0xA1, 0x7C }) },
            /*Ĉ*/{ 264, new Glyph(264, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x46, 0x45, 0x46, 0x28 }) },
            /*ĉ*/{ 265, new Glyph(265, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x46, 0x45, 0x46, 0x28 }) },
            /*Č*/{ 268, new Glyph(268, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x45, 0x46, 0x45, 0x28 }) },
            /*č*/{ 269, new Glyph(269, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x45, 0x46, 0x45, 0x28 }) },
            /*Ď*/{ 270, new Glyph(270, 5, 7, 1, 0, 7, new byte[] { 0x7C, 0x45, 0x46, 0x45, 0x38 }) },
            /*ď*/{ 271, new Glyph(271, 9, 7, 1, 0, 12, new byte[] { 0x38, 0x44, 0x44, 0x44, 0x7F, 0x00, 0x00, 0x04, 0x03 }) },
            /*Ě*/{ 282, new Glyph(282, 5, 7, 1, 0, 7, new byte[] { 0x7C, 0x55, 0x56, 0x45, 0x44 }) },
            /*ě*/{ 283, new Glyph(283, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x55, 0x56, 0x55, 0x18 }) },
            /*Ĝ*/{ 284, new Glyph(284, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x46, 0x45, 0x56, 0x74 }) },
            /*ĝ*/{ 285, new Glyph(285, 5, 8, 1, 0, 7, new byte[] { 0x18, 0xA6, 0xA5, 0xA6, 0x7C }) },
            /*Ĥ*/{ 292, new Glyph(292, 5, 7, 1, 0, 7, new byte[] { 0x78, 0x22, 0x21, 0x22, 0x78 }) },
            /*ĥ*/{ 293, new Glyph(293, 5, 7, 1, 0, 7, new byte[] { 0x7C, 0x08, 0x0A, 0x09, 0x72 }) },
            /*ı*/{ 305, new Glyph(305, 1, 5, 2, 2, 5, new byte[] { 0x1F }) },
            /*Ĵ*/{ 308, new Glyph(308, 5, 7, 1, 0, 7, new byte[] { 0x22, 0x41, 0x42, 0x40, 0x3C }) },
            /*ĵ*/{ 309, new Glyph(309, 5, 8, 1, 0, 7, new byte[] { 0x42, 0x81, 0x82, 0x80, 0x7C }) },
            /*Ň*/{ 327, new Glyph(327, 5, 7, 1, 0, 7, new byte[] { 0x7C, 0x09, 0x12, 0x21, 0x7C }) },
            /*ň*/{ 328, new Glyph(328, 5, 7, 1, 0, 7, new byte[] { 0x7C, 0x05, 0x06, 0x05, 0x78 }) },
            /*Œ*/{ 338, new Glyph(338, 9, 7, 1, 0, 11, new byte[] { 0x3E, 0x41, 0x41, 0x41, 0x7F, 0x49, 0x49, 0x41, 0x41 }) },
            /*œ*/{ 339, new Glyph(339, 9, 5, 1, 2, 11, new byte[] { 0x0E, 0x11, 0x11, 0x11, 0x0E, 0x15, 0x15, 0x15, 0x06 }) },
            /*Ř*/{ 344, new Glyph(344, 5, 7, 1, 0, 7, new byte[] { 0x7C, 0x25, 0x26, 0x25, 0x58 }) },
            /*ř*/{ 345, new Glyph(345, 5, 7, 1, 0, 7, new byte[] { 0x7C, 0x11, 0x0A, 0x05, 0x04 }) },
            /*Ŝ*/{ 348, new Glyph(348, 5, 7, 1, 0, 7, new byte[] { 0x48, 0x56, 0x55, 0x56, 0x20 }) },
            /*ŝ*/{ 349, new Glyph(349, 5, 7, 1, 0, 7, new byte[] { 0x48, 0x56, 0x55, 0x56, 0x20 }) },
            /*Š*/{ 352, new Glyph(352, 5, 7, 1, 0, 7, new byte[] { 0x48, 0x55, 0x56, 0x55, 0x20 }) },
            /*š*/{ 353, new Glyph(353, 5, 7, 1, 0, 7, new byte[] { 0x48, 0x55, 0x56, 0x55, 0x20 }) },
            /*Ť*/{ 356, new Glyph(356, 5, 7, 1, 0, 7, new byte[] { 0x04, 0x05, 0x7E, 0x05, 0x04 }) },
            /*ť*/{ 357, new Glyph(357, 8, 7, 1, 0, 11, new byte[] { 0x04, 0x3E, 0x44, 0x44, 0x00, 0x00, 0x04, 0x03 }) },
            /*Ŭ*/{ 364, new Glyph(364, 5, 7, 1, 0, 7, new byte[] { 0x39, 0x42, 0x42, 0x42, 0x39 }) },
            /*ŭ*/{ 365, new Glyph(365, 5, 7, 1, 0, 7, new byte[] { 0x39, 0x42, 0x42, 0x42, 0x39 }) },
            /*Ů*/{ 366, new Glyph(366, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x42, 0x45, 0x42, 0x38 }) },
            /*ů*/{ 367, new Glyph(367, 5, 7, 1, 0, 7, new byte[] { 0x38, 0x42, 0x45, 0x42, 0x38 }) },
            /*Ÿ*/{ 376, new Glyph(376, 5, 7, 1, 0, 7, new byte[] { 0x0C, 0x11, 0x60, 0x11, 0x0C }) },
            /*Ž*/{ 381, new Glyph(381, 5, 7, 1, 0, 7, new byte[] { 0x44, 0x65, 0x56, 0x4D, 0x44 }) },
            /*ž*/{ 382, new Glyph(382, 5, 7, 1, 0, 7, new byte[] { 0x44, 0x65, 0x56, 0x4D, 0x44 }) },
            /*ˆ*/{ 710, new Glyph(710, 3, 2, 2, 0, 7, new byte[] { 0x02, 0x01, 0x02 }) },
            /*ˇ*/{ 711, new Glyph(711, 3, 2, 2, 0, 7, new byte[] { 0x01, 0x02, 0x01 }) },
            /*˘*/{ 728, new Glyph(728, 5, 2, 1, 0, 7, new byte[] { 0x01, 0x02, 0x02, 0x02, 0x01 }) },
            /*˚*/{ 730, new Glyph(730, 3, 3, 1, 0, 5, new byte[] { 0x02, 0x05, 0x02 }) },
            /*˜*/{ 732, new Glyph(732, 6, 2, 1, 0, 8, new byte[] { 0x02, 0x01, 0x01, 0x02, 0x02, 0x01 }) },
            /*–*/{ 8211, new Glyph(8211, 4, 1, 1, 3, 6, new byte[] { 0x01, 0x01, 0x01, 0x01 }) },
            /*—*/{ 8212, new Glyph(8212, 6, 1, 1, 3, 8, new byte[] { 0x01, 0x01, 0x01, 0x01, 0x01, 0x01 }) },
            /*‘*/{ 8216, new Glyph(8216, 2, 3, 2, 0, 5, new byte[] { 0x06, 0x01 }) },
            /*’*/{ 8217, new Glyph(8217, 2, 3, 1, 0, 5, new byte[] { 0x04, 0x03 }) },
            /*‚*/{ 8218, new Glyph(8218, 2, 3, 1, 5, 5, new byte[] { 0x04, 0x03 }) },
            /*“*/{ 8220, new Glyph(8220, 4, 3, 2, 0, 7, new byte[] { 0x06, 0x01, 0x06, 0x01 }) },
            /*”*/{ 8221, new Glyph(8221, 4, 3, 1, 0, 7, new byte[] { 0x04, 0x03, 0x04, 0x03 }) },
            /*„*/{ 8222, new Glyph(8222, 4, 3, 1, 5, 7, new byte[] { 0x04, 0x03, 0x04, 0x03 }) },
            /*†*/{ 8224, new Glyph(8224, 5, 8, 1, 0, 7, new byte[] { 0x04, 0x04, 0xFF, 0x04, 0x04 }) },
            /*‡*/{ 8225, new Glyph(8225, 5, 8, 1, 0, 7, new byte[] { 0x24, 0x24, 0xFF, 0x24, 0x24 }) },
            /*•*/{ 8226, new Glyph(8226, 2, 2, 2, 3, 6, new byte[] { 0x03, 0x03 }) },
            /*…*/{ 8230, new Glyph(8230, 7, 1, 1, 6, 9, new byte[] { 0x01, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 }) },
            /*‰*/{ 8240, new Glyph(8240, 11, 7, 1, 0, 13, new byte[] { 0x06, 0x29, 0x16, 0x08, 0x34, 0x4A, 0x30, 0x00, 0x30, 0x48, 0x30 }) },
            /*‹*/{ 8249, new Glyph(8249, 3, 5, 3, 1, 7, new byte[] { 0x04, 0x0A, 0x11 }) },
            /*›*/{ 8250, new Glyph(8250, 3, 5, 1, 1, 7, new byte[] { 0x11, 0x0A, 0x04 }) },
            /*€*/{ 8364, new Glyph(8364, 6, 7, 1, 0, 8, new byte[] { 0x14, 0x3E, 0x55, 0x45, 0x41, 0x22 }) },
            /*₱*/{ 8369, new Glyph(8369, 7, 7, 1, 0, 9, new byte[] { 0x04, 0x7F, 0x15, 0x15, 0x15, 0x0E, 0x04 }) },
            /*₷*/{ 8375, new Glyph(8375, 8, 8, 0, 0, 9, new byte[] { 0x10, 0xE0, 0x50, 0xE6, 0x59, 0xE9, 0x49, 0x32 }) },
            /*℗*/{ 8471, new Glyph(8471, 7, 8, 1, 0, 9, new byte[] { 0x7E, 0x81, 0xBD, 0x95, 0x89, 0x81, 0x7E }) },
            /*™*/{ 8482, new Glyph(8482, 9, 4, 1, 0, 11, new byte[] { 0x01, 0x0F, 0x01, 0x00, 0x0F, 0x02, 0x04, 0x02, 0x0F }) }
        };

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

            // Collect character definitions
            var glyphs = new List<Glyph>(text.Length);
            foreach (var c in text)
            {
                if (_definitions.TryGetValue((int)c, out var glyph))
                {
                    glyphs.Add(glyph);
                }
                else
                {
                    // Render blank
                    glyphs.Add(_definitions[32]);
                }
            }

            // Calculate "bitmap" width
            var renderWidth = 0;
            for (var i = 0; i < glyphs.Count; i++)
            {
                var glyph = glyphs[i];
                if (i < glyphs.Count - 1)
                {
                    renderWidth += glyph.Xadvance;
                }
                else
                {
                    renderWidth += glyph.Width;
                }
            }

            // Reserve space for the rendered bitmap
            var matrix = new byte[renderWidth * CharHeight];

            var x = 0;
            foreach (var c in glyphs)
            {
                for (var cx = 0; cx < c.Width; cx++)
                {
                    byte bitPattern = c.Pixels[cx];
                    var y = 0;
                    byte flag = 1;
                    for (var cy = 0; cy < c.Height; cy++, y++)
                    {
                        if ((bitPattern & flag) != 0)
                        {
                            // Set value to 1 to indicate that a pixel should be "on".
                            matrix[x + cx + (cy + c.Yoffset) * renderWidth] = 1;
                        }

                        flag <<= 1;
                    }
                }

                x += c.Xadvance;
            }

            return new SenseHatTextRenderState(text, matrix, renderWidth);
        }
    }
}
