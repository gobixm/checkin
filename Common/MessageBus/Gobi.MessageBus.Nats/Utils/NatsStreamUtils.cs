using NATS.Client.JetStream;

namespace Gobi.MessageBus.Nats.Utils;

public static class NatsStreamUtils
{
    public static StreamInfo? GetStreamInfoOrNullWhenNotExist(IJetStreamManagement jsm, string streamName)
    {
        try
        {
            return jsm.GetStreamInfo(streamName);
        }
        catch (NATSJetStreamException e)
        {
            if (e.ErrorCode == 404) return null;
            throw;
        }
    }

    public static bool StreamExists(IJetStreamManagement jsm, string streamName)
    {
        return GetStreamInfoOrNullWhenNotExist(jsm, streamName) != null;
    }

    public static StreamInfo CreateStream(IJetStreamManagement jsm, string streamName, StorageType storageType,
        params string[] subjects)
    {
        var sc = StreamConfiguration.Builder()
            .WithName(streamName)
            .WithStorageType(storageType)
            .WithSubjects(subjects)
            .Build();

        return jsm.AddStream(sc);
    }

    public static void DeleteStream(IJetStreamManagement jsm, string stream)
    {
        jsm.DeleteStream(stream);
    }

    public static void CreateStreamWhenDoesNotExist(IJetStreamManagement jsm, string stream, params string[] subjects)
    {
        try
        {
            jsm.GetStreamInfo(stream); // this throws if the stream does not exist
            return;
        }
        catch (NATSJetStreamException)
        {
            /* stream does not exist */
        }

        var sc = StreamConfiguration.Builder()
            .WithName(stream)
            .WithStorageType(StorageType.Memory)
            .WithSubjects(subjects)
            .Build();
        jsm.AddStream(sc);
    }

    public static StreamInfo CreateStreamOrUpdateSubjects(IJetStreamManagement jsm, string streamName,
        StorageType storageType, params string[] subjects)
    {
        var si = GetStreamInfoOrNullWhenNotExist(jsm, streamName);
        if (si == null) return CreateStream(jsm, streamName, storageType, subjects);

        var sc = si.Config;
        var needToUpdate = false;
        foreach (var sub in subjects)
            if (!sc.Subjects.Contains(sub))
            {
                needToUpdate = true;
                sc.Subjects.Add(sub);
            }

        if (needToUpdate) si = jsm.UpdateStream(sc);

        return si;
    }
}