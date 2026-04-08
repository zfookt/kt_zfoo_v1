using UnityEngine;

public class Main : MonoBehaviour
{
    private void Awake()
    {
        UIManager.Instance.Init(this.gameObject);
        ResponseManager.Instance.Init();
        ToastManager.Instance.Init(this.gameObject);
    }

    private void Start()
    {
        // 先进入登录界面
        UIManager.Instance.ShowWindow<LoginUI>();
    }
}