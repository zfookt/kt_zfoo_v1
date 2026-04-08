using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

/// <summary>
///  全局消息处理
/// </summary>
public class ResponseManager : Singleton<ResponseManager>
{
    //<消息返回, 消息体>
    private readonly Dictionary<MsgIds, Func<byte[], IMessage>> protoCode2FuncDic = new();

    public void Init()
    {
        RegisterProto();
    }

    public void AddHandler(MsgIds msgId, Func<byte[], IMessage> action)
    {
        if (!protoCode2FuncDic.TryAdd(msgId, action))
        {
            Debug.LogError($"AddHandler失败 {msgId} 重复注册");
            return;
        }
    }

    public Func<byte[], IMessage> GetHandler(MsgIds msgId)
    {
        if (protoCode2FuncDic.TryGetValue(msgId, out Func<byte[], IMessage> func))
        {
            return func;
        }

        Debug.LogWarning($"GetHandler失败 {msgId} 没有对应的处理函数");

        return null;
    }

    public void HandleMsg(MsgIds msgId, byte[] protoBody)
    {
        Func<byte[], IMessage> handler = GetHandler(msgId);
        if (handler != null)
        {
            IMessage message = handler.Invoke(protoBody);

            // 客户端处理服务端返回
            NetManager.Instance.ProcessServerResponse(msgId, message);
        }
        else
        {
            ToastManager.Instance.Show($"{msgId} not register in ResponseController!!!");
        }
    }

    public void RegisterProto()
    {
        AddHandler(MsgIds.LoginGameResp, msg =>
        {
            LoginGameResp loginGameResp = LoginGameResp.Parser.ParseFrom(msg);

            return loginGameResp;
        });
    }
}