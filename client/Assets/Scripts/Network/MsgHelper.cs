using System;
using System.Net;
using Google.Protobuf;

public static class MsgHelper
{
    /// <summary>
    /// 发送数据前编码
    /// </summary>
    /// <param name="msgId"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static byte[] Encode(MsgIds msgId, IMessage message)
    {
        byte[] msgIdBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((int)msgId));
        byte[] msgBodyBytes = message.ToByteArray();


        byte[] mergeArray = new byte[msgIdBytes.Length + msgBodyBytes.Length];

        Buffer.BlockCopy(msgIdBytes, 0, mergeArray, 0, msgIdBytes.Length);
        Buffer.BlockCopy(msgBodyBytes, 0, mergeArray, msgIdBytes.Length, msgBodyBytes.Length);

        return mergeArray;
    }

    /// <summary>
    /// 收到数据后,解码
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static (MsgIds msgId, byte[] msgBody) Decode(byte[] data)
    {
        // 丢弃掉header
        int value = BitConverter.ToInt32(data, 0);
        int msgId = IPAddress.NetworkToHostOrder(value);

        byte[] msgBody = new byte[data.Length - 4];
        Buffer.BlockCopy(data, 4, msgBody, 0, msgBody.Length);

        return ((MsgIds)msgId, msgBody);
    }
}