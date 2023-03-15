using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System.Text;

namespace BasicAPICosmosDb.Services
{
    public class CosmosJsonNetSerializer : CosmosSerializer
    {
        private static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);
        private readonly JsonSerializer Serializer;
        private readonly JsonSerializerSettings serializerSettings;

        public CosmosJsonNetSerializer()
            : this(new JsonSerializerSettings())
        {
        }

        public CosmosJsonNetSerializer(
            JsonSerializerSettings serializerSettings
        )
        {
            this.serializerSettings = serializerSettings;
            this.Serializer = JsonSerializer.Create(this.serializerSettings);
        }

        public override T FromStream<T>(Stream stream)
        {
            using (stream)
            {
                if (typeof(Stream).IsAssignableFrom(typeof(T)))
                {
                    return (T)(object)(stream);
                }

                using StreamReader sr = new StreamReader(stream);
                using JsonTextReader jsonTextReader = new JsonTextReader(sr);
                return Serializer.Deserialize<T>(jsonTextReader);
            }
        }

        public override Stream ToStream<T>(T input)
        {
            MemoryStream streamPayload = new MemoryStream();
            using (StreamWriter streamWriter = new StreamWriter(streamPayload, encoding: DefaultEncoding, bufferSize: Constants.DefaultBufferSize, leaveOpen: true))
            {
                using JsonWriter writer = new JsonTextWriter(streamWriter)
                {
                    Formatting = Formatting.None
                };
                Serializer.Serialize(writer, input);
                writer.Flush();
                streamWriter.Flush();
            }

            streamPayload.Position = Constants.Zero;
            return streamPayload;
        }
    }
}
