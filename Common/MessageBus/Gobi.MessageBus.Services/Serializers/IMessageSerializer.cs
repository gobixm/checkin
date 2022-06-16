namespace Gobi.MessageBus.Services.Serializers;

public interface IMessageSerializer
{
    byte[] Serialize<T>(Stream stream, T body) where T : class;
    T? Deserialize<T>(Stream stream, byte[] discriminator) where T : class;
}