using Google.Protobuf;

public interface IProcessNet
{
    void ProcessServerResponse(MsgIds msgId, IMessage message);
}