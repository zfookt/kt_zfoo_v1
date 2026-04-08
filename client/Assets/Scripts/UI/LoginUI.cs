using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : BaseUI
{
    private InputField mInput_AccountId;
    private Button mBtn_Login;

    private void Awake()
    {
        mInput_AccountId = cover.Find("Bg/Input_AccountId").GetComponent<InputField>();

        mBtn_Login = cover.Find("Bg/Btn_Login").GetComponent<Button>();
        mBtn_Login.onClick.AddListener(OnLoginClick);
    }

    private void OnLoginClick()
    {
        string openId = this.mInput_AccountId.text;
        if (string.IsNullOrEmpty(openId))
        {
            ToastManager.Instance.Show("请输入账号");
            return;
        }

        HttpManager.Instance.SendPostRequest(GameConfig.gatewayUrl, new LoginRequest()
        {
            openId = openId
        }, (json) =>
        {
            if (string.IsNullOrEmpty(json))
            {
                ToastManager.Instance.Show("获取服务器列表失败");
                return;
            }

            Root root = JsonConvert.DeserializeObject<Root>(json);

            if (root.code == 200)
            {
                ServerDataManager.Instance.token = root.data.token;
                List<ServerInfo> serverInfoList = root.data.serverInfoList;

                UIManager.Instance.HideAllWindow();

                // 展示服务器列表界面
                ServerListUI serverListUI = UIManager.Instance.ShowWindow<ServerListUI>();
                serverListUI.Init(serverInfoList);
            }
            else
            {
                Debug.LogError("error");
            }
        });
    }
}