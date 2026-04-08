using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using UnityEngine;

public class NetManager : Singleton<NetManager>
{
    private HashSet<IProcessNet> set = new();


    public void Register(IProcessNet processNet)
    {
        bool ok = set.Add(processNet);
        if (!ok)
        {
            Debug.LogError("NetManager Register error!!!!");
        }
    }

    public void UnRegister(IProcessNet processNet)
    {
        bool ok = set.Remove(processNet);
        if (!ok)
        {
            Debug.LogError("NetManager UnRegister error!!!!");
        }
    }

    /// <summary>
    /// 暂时是在ResponseController中调用
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="message"></param>
    public void ProcessServerResponse(MsgIds msgId, IMessage message)
    {
        foreach (IProcessNet handler in set.ToArray())
        {
            handler.ProcessServerResponse(msgId, message);
        }
    }
}