using UT.Data.Modlet;

namespace Shared.Modules
{
    public static class ModletStream
    {
        public static (TKey? key, TValue? value)? ReadPacket<TKey, TValue>(byte[]? stream)
        {
            Packet<TKey, TValue>? packet = Packet<TKey, TValue>.Decode(stream);
            if (packet == null)
            {
                return null;
            }
            return (packet.Description, packet.Data);
        }

        public static byte[] CreatePacket<TKey, TValue>(TKey key, TValue value)
        {
            return Packet<TKey, TValue>.Encode(key, value);
        }

        public static T? GetInputType<T>(byte[]? stream)
            where T : struct
        {
            var packet = ReadPacket<T, object>(stream);
            if (packet == null)
            {
                return null;
            }

            return packet.Value.key;
        }

        public static TKey? GetKey<TKey, TData>(byte[]? stream)
        {
            if (stream == null)
            {
                return default;
            }

            var packet = ReadPacket<TKey, TData>(stream);
            if (packet == null)
            {
                return default;
            }

            return packet.Value.key;
        }

        public static TData? GetContent<TKey, TData>(byte[]? stream)
            where TKey : struct
        {
            if (stream == null)
            {
                return default;
            }
            var packet = ReadPacket<TKey, TData>(stream);
            if (packet == null)
            {
                return default;
            }

            return packet.Value.value;
        }
    }
}
