namespace Gobi.Encodings.Encoders.Numbers;

public static class VariableLengthEncoder
{
    public static int EncodeUint(Span<byte> buffer, uint value)
        {
            if (buffer.Length < 5) throw new ArgumentException($"{nameof(buffer)} must be at least 5 bytes length");

            var position = 0;

            while (true)
            {
                if ((value & 0b11111111111111111111111110000000) == 0)
                {
                    buffer[position++] = (byte) value;
                    break;
                }

                buffer[position++] = (byte) ((value & 0b01111111) | 0b10000000);
                value >>= 7;
            }

            return position;
        }

        public static (uint value, int size) DecodeUint(ReadOnlySpan<byte> buffer)
        {
            uint result = 0;
            var shift = 0;
            var size = 0;
            foreach (var b in buffer)
            {
                ++size;
                result |= (uint) (b & 0b1111111) << shift;
                shift += 7;
                if ((b & 0b10000000) == 0) return (result, size);
            }

            return (result, size);
        }


        public static int EncodeUlong(Span<byte> buffer, ulong value)
        {
            if (buffer.Length < 10) throw new ArgumentException($"{nameof(buffer)} must be at least 10 bytes length");

            var position = 0;

            while (true)
            {
                if ((value & 0b1111111111111111111111111111111111111111111111111111111110000000) == 0)
                {
                    buffer[position++] = (byte) value;
                    break;
                }

                buffer[position++] = (byte) ((value & 0b01111111) | 0b10000000);
                value >>= 7;
            }

            return position;
        }

        public static (ulong value, int size) DecodeULong(ReadOnlySpan<byte> buffer)
        {
            ulong result = 0;
            var shift = 0;
            var size = 0;
            foreach (var b in buffer)
            {
                ++size;
                result |= (ulong) (b & 0b1111111) << shift;
                shift += 7;
                if ((b & 0b10000000) == 0) return (result, size);
            }

            return (result, size);
        }

}