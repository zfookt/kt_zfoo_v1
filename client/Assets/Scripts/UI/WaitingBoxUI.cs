using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingBoxUI : BaseUI
{
    private Button Left;
    private Button Right;

    private void Awake()
    {
        Left = cover.Find("Bg/Left").GetComponent<Button>();
        Left.onClick.AddListener(OnLeftButton);

        Right = cover.Find("Bg/Right").GetComponent<Button>();
        Right.onClick.AddListener(OnRightButton);
    }

    private void OnLeftButton()
    {
        Debug.Log("OnLeftButton");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnRightButton()
    {
        UIManager.Instance.HideAllWindow();

        UIManager.Instance.ShowWindow<LoginUI>();
    }
}