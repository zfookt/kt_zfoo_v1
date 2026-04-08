using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerInfoComp : MonoBehaviour
{
    private ServerInfo serverInfo { get; set; }

    private Text text;

    private Button btnServer;

    private void Awake()
    {
        this.text = this.transform.Find("BtnServer/Text").GetComponent<Text>();

        this.btnServer = this.transform.Find("BtnServer").GetComponent<Button>();
        this.btnServer.onClick.AddListener(OnBtnServerClick);
    }

    public void Init(ServerInfo serverInfo)
    {
        this.serverInfo = serverInfo;

        this.text.text = this.serverInfo.name;
    }

    private void OnBtnServerClick()
    {
        // string gameServerUrl = $"ws://{serverInfo.ip}:{serverInfo.port}/ws";
        //
        // WebSocketManager.Instance.Init(gameServerUrl, () =>
        // {
        //     ServerDataManager.Instance.serverInfo = serverInfo;
        //
        //     WebSocketManager.Instance.SendBinaryMessage(MsgIds.LoginGame, new LoginGame()
        //     {
        //         Token = ServerDataManager.Instance.token
        //     });
        // }, () => { });
    }
}