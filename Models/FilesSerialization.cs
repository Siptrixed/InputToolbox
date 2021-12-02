using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputToolbox.Models
{
    internal static class FilesSerialization
    {
        private static readonly MessagePackSerializerOptions Options =
       MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

        public static async Task SaveObjToFile<T>(string filename,T obj)
        {
            await using Stream stream = File.Create(filename);
            await MessagePackSerializer.SerializeAsync(stream, obj, Options);
        }

        public static bool TryReadObjFromFile<T>(string filename,out T obj)
        {
            try
            {
                using Stream stream = File.OpenRead(filename);
                obj = MessagePackSerializer.Deserialize<T>(stream, Options);
                return true;
            }
            catch (MessagePackSerializationException)
            {
                obj = default;
                return false;
            }
        }
    }
}
