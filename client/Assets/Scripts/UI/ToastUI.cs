using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ToastUI : MonoBehaviour
{
    private Image Bg;
    private Text Text;

    public void Awake()
    {
        Bg = transform.Find("Bg").GetComponent<Image>();
        Text = Bg.transform.Find("Text").GetComponent<Text>();
    }

    public void Init(string content, float moveTime, float moveHeight, float fadeTime)
    {
        Text.text = content;

        Tweener tweener = DOTween.To((value) =>
        {
            if (Bg != null && Bg.transform != null)
            {
                Bg.transform.localPosition = new Vector3(Bg.transform.localPosition.x, value, 0);
            }
        }, 0, moveHeight, moveTime).SetEase(Ease.OutCubic);

        tweener.OnComplete(() =>
        {
            if (Bg != null)
            {
                Bg.DOFade(0, fadeTime).OnComplete(() =>
                {
                    // 销毁
                    ToastManager.Instance.DestroyToastUI(this);
                });
            }
        });
    }
}