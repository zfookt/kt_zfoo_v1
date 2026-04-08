using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WaitingUI : BaseUI
{
    private void Start()
    {
        RectTransform rectTransform = cover.Find("Circle").GetComponent<RectTransform>();

        DOTween.To((value) =>
            {
                if (rectTransform != null)
                {
                    rectTransform.localEulerAngles = new Vector3(0, 0, value);
                }
            }, 0, -360, 1.0f).SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }
}