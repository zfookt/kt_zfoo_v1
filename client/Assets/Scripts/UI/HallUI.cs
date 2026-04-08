using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HallUI : BaseUI
{
    private Text title;
    private Button enterGame;

    private void Awake()
    {
        title = this.cover.Find("Title").GetComponent<Text>();
        enterGame = this.cover.Find("EnterGame").GetComponent<Button>();
        enterGame.onClick.AddListener(OnEnterGameClick);
    }

    public void Init(string title)
    {
        this.title.text = title;
    }

    private void OnEnterGameClick()
    {
        Debug.Log("OnEnterGameClick");
    }
}