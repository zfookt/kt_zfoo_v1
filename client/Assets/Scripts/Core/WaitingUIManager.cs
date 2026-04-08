public class WaitingUIManager : Singleton<WaitingUIManager>
{
    public void Show()
    {
        UIManager.Instance.ShowWindow<WaitingUI>(UIOrderConst.Waiting);
    }

    public void Hide()
    {
        UIManager.Instance.HideWindow<WaitingUI>();
    }
}