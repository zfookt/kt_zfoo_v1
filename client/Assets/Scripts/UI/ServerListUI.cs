using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class ServerListUI : BaseUI, IProcessNet
{
    private Transform content;

    private void Awake()
    {
        NetManager.Instance.Register(this);
        content = cover.Find("ScrServerList/Viewport/Content");
    }

    private void OnDestroy()
    {
        NetManager.Instance.UnRegister(this);
    }

    public void Init(List<ServerInfo> serverInfoList)
    {
        foreach (ServerInfo serverInfo in serverInfoList)
        {
            GameObject go = ResourceLoader.Instance.LoadAndInstantiate(typeof(ServerInfoComp).Name);
            ServerInfoComp scriptObject = go.AddComponent<ServerInfoComp>();
            scriptObject.Init(serverInfo);

            go.transform.SetParent(content.transform);
        }
    }

    public void ProcessServerResponse(MsgIds msgId, IMessage message)
    {
        if (msgId == MsgIds.LoginGameResp)
        {
            LoginGameResp loginGameResp = message as LoginGameResp;
            
            // 登录失败就不处理了，因为token失败,客户端会直接被断开连接
            if (loginGameResp.ErrorCode == ErrorCode.Ok)
            {
                UIManager.Instance.HideAllWindow();

                // 进入大厅
                HallUI hallUI = UIManager.Instance.ShowWindow<HallUI>();
                hallUI.Init(ServerDataManager.Instance.serverInfo.name + "对战大厅");
            }
        }
    }
}