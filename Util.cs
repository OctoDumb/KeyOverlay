using System;
using System.Globalization;
using SFML.Graphics;

namespace KeyOverlay
{
    static class Util
    {
        // Copied from osu-framework
        // Too lazy to write this myself
        public static Color ColorFromHex(string hex)
        {
            var hexSpan = hex[0] == '#' ? hex.AsSpan().Slice(1) : hex.AsSpan();

            switch(hexSpan.Length)
            {
                case 3:
                    return new Color(
                        (byte)(byte.Parse(hexSpan.Slice(0, 1), NumberStyles.HexNumber) * 17),
                        (byte)(byte.Parse(hexSpan.Slice(1, 1), NumberStyles.HexNumber) * 17),
                        (byte)(byte.Parse(hexSpan.Slice(2, 1), NumberStyles.HexNumber) * 17),
                        255
                    );

                case 6:
                    return new Color(
                        byte.Parse(hexSpan.Slice(0, 2), NumberStyles.HexNumber),
                        byte.Parse(hexSpan.Slice(2, 2), NumberStyles.HexNumber),
                        byte.Parse(hexSpan.Slice(4, 2), NumberStyles.HexNumber),
                        255
                    );

                case 4:
                    return new Color(
                        (byte)(byte.Parse(hexSpan.Slice(0, 1), NumberStyles.HexNumber) * 17),
                        (byte)(byte.Parse(hexSpan.Slice(1, 1), NumberStyles.HexNumber) * 17),
                        (byte)(byte.Parse(hexSpan.Slice(2, 1), NumberStyles.HexNumber) * 17),
                        (byte)(byte.Parse(hexSpan.Slice(3, 1), NumberStyles.HexNumber) * 17)
                    );

                case 8:
                    return new Color(
                        byte.Parse(hexSpan.Slice(0, 2), NumberStyles.HexNumber),
                        byte.Parse(hexSpan.Slice(2, 2), NumberStyles.HexNumber),
                        byte.Parse(hexSpan.Slice(4, 2), NumberStyles.HexNumber),
                        byte.Parse(hexSpan.Slice(6, 2), NumberStyles.HexNumber)
                    );

                default:
                    throw new ArgumentException(@"Invalid hex string length!");
            }
        }

        public static long GetMillisecondsNow()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}