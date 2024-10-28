using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeeloewenCraft
{
    public enum MultiplayerPacketType
    {
        CREATE_CHUNK,
        INITIAL_LOAD,
        SET_BLOCK,
        CREATE_ENTITY,
        REMOVE_ENTITY,
        PRESSED_CHANGE,
        SYNC_POS,
        ADD_TO_INV,
        REMOVE_FROM_INV,
        DAMAGE_ENTITY,
        HEAL_ENTITY,
        PING_REQUEST,
        PING_RESPONSE
    }

    public enum MultiplayerType
    {
        OFFLINE, //No multiplayer
        CLIENT,
        SERVER
    }

    public class NetworkPacket
    {
        public MultiplayerPacketType type;
        public string[] content;
        public bool isValid = true;

        public NetworkPacket(MultiplayerPacketType type, string[] data)
        {
            this.type = type;
            content = data;

            NetworkHandler.packetTypeDictionary.TryGetValue(type, out var entry);
            if (entry.length != content.Length)
            {
                isValid = false;
                Log.Write($"Length of arguments for packet {type} was invalid. Expected {entry.length} arguments, got {content.Length}.", "Error");
            }
        }

        public byte[] GetBytes()
        {

            using (var memStream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStream))
                {
                    List<byte> byteList = new List<byte>();

                    foreach (string str in content)
                    {
                        byte[] stringBytes = Encoding.UTF8.GetBytes(str);
                        byteList.AddRange(BitConverter.GetBytes(stringBytes.Length));
                        byteList.AddRange(stringBytes);
                    }

                    byte[] contentBytes = byteList.ToArray();
                    byte[] typeBytes = Encoding.UTF8.GetBytes(type.ToString());

                    writer.Write(typeBytes.Length); //4 Bytes
                    writer.Write(typeBytes);
                    writer.Write(contentBytes);
                    return memStream.ToArray();
                }
            }
        }
    }
}