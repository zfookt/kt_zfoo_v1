using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingBoxUIManager : Singleton<WaitingBoxUIManager>
{
    public void Show()
    {
        UIManager.Instance.ShowWindow<WaitingBoxUI>(UIOrderConst.WaitingBox);
    }

    public void Hide()
    {
        UIManager.Instance.HideWindow<WaitingBoxUI>();
    }
}