using System;
using System.Collections;
using BestHTTP.WebSocket;
using Google.Protobuf;
using UnityEngine;

/// <summary>
/// 因为用到协程，所以需要Mono
/// </summary>
public class WebSocketManager : MonoSingleton<WebSocketManager>
{
    private WebSocket webSocket;
    private Coroutine heartBeatCoroutine;

    private float lastReceiveTime;

    private Action okAction;
    private Action failedAction;

    /// <summary>
    /// 主要是心跳检测
    /// </summary>
    private void Update()
    {
        // 通过心跳检测
        if (IsOpen && Time.time - lastReceiveTime > 1500)
        {
            Close();
        }
    }

    public void Init(Action okAction, Action failedAction)
    {
        // 连接时增加弹框
        WaitingUIManager.Instance.Show();

        this.okAction = okAction;
        this.failedAction = failedAction;

        if (heartBeatCoroutine != null)
        {
            StopCoroutine(heartBeatCoroutine);
            heartBeatCoroutine = null;
        }

        heartBeatCoroutine = StartCoroutine(SendHeartbeatMsgCoroutine());

        webSocket = new WebSocket(new Uri(GameConfig.serverUrl));

        // 4个监听
        webSocket.OnOpen += OnOpen;
        webSocket.OnBinary += OnBinary;
        webSocket.OnError += OnError;
        webSocket.OnClosed += OnClosed;

        // 开始连接
        webSocket.Open();
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="message"></param>
    public void SendBinaryMessage(MsgIds msgId, IMessage message)
    {
        if (!IsOpen)
        {
            ToastManager.Instance.Show("与服务器没有连接");

            return;
        }

        webSocket.Send(MsgHelper.Encode(msgId, message));
    }

    private void OnOpen(WebSocket ws)
    {
        WaitingUIManager.Instance.Hide();

        lastReceiveTime = Time.time;

        okAction?.Invoke();
        okAction = null;
        failedAction = null;

        ToastManager.Instance.Show("服务器连接成功");
    }

    private void OnBinary(WebSocket ws, byte[] data)
    {
        lastReceiveTime = Time.time;

        (MsgIds msgId, byte[] msgBody) msg = MsgHelper.Decode(data);
        ResponseManager.Instance.HandleMsg(msg.msgId, msg.msgBody);
    }

    private void OnError(WebSocket ws, string reason)
    {
        Close();
    }

    private void OnClosed(WebSocket ws, UInt16 code, string message)
    {
        Close();
    }

    private void Close()
    {
        WaitingUIManager.Instance.Hide();
        WaitingBoxUIManager.Instance.Show();

        failedAction?.Invoke();
        okAction = null;
        failedAction = null;

        if (heartBeatCoroutine != null)
        {
            StopCoroutine(heartBeatCoroutine);
            heartBeatCoroutine = null;
        }

        webSocket?.Close();
        webSocket = null;
    }

    /// <summary>
    ///  发送心跳协议
    /// </summary>
    /// <returns></returns>
    private IEnumerator SendHeartbeatMsgCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);

            if (IsOpen)
            {
                //SendBinaryMessage(ProtoCode.EHeartBeat, new HeartBeat());
            }
        }
    }

    public bool IsOpen
    {
        get { return webSocket != null && webSocket.IsOpen; }
    }
}