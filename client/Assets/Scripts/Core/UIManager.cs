using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    // 分配到的序列
    private int seqIndex = 0;

    // <弹框名字, 对应的GameObject>
    private readonly Dictionary<string, BaseUI> UIPanelDic = new();

    // ui根节点
    private GameObject uiRoot;

    public void Init(GameObject uiRoot)
    {
        this.uiRoot = uiRoot;
    }

    /// <summary>
    /// 同步弹框
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T ShowWindow<T>(int order = -1) where T : BaseUI
    {
        string resPath = typeof(T).Name;

        if (UIPanelDic.ContainsKey(resPath))
        {
            BaseUI scriptObject = UIPanelDic[resPath];
            if (scriptObject.gameObject != null)
            {
                // 设置下层级
                scriptObject.gameObject.GetComponent<Canvas>().sortingOrder = (order > 0 ? order : ++seqIndex);
                return scriptObject as T;
            }
        }

        GameObject go = ResourceLoader.Instance.LoadAndInstantiate(resPath);

        if (go != null)
        {
            return AddWindow<T>(uiRoot, go, order);
        }

        return null;
    }

    /// <summary>
    /// 主要用于加载大资源时，展示一个进度条时使用
    /// </summary>
    /// <param name="onFinished"></param>
    /// <typeparam name="T"></typeparam>
    public void ShowWindowAsync<T>(Action<T> onFinished = null) where T : BaseUI
    {
        string resPath = typeof(T).Name;

        if (UIPanelDic.ContainsKey(resPath))
        {
            BaseUI scriptObject = UIPanelDic[resPath];
            if (scriptObject.gameObject != null)
            {
                // 设置下层级
                scriptObject.gameObject.GetComponent<Canvas>().sortingOrder = ++seqIndex;
                return;
            }
        }

        ResourceLoader.Instance.LoadAndInstantiateAsync<GameObject>(resPath, go =>
        {
            T scriptObject = null;
            if (go != null)
            {
                scriptObject = AddWindow<T>(uiRoot, go);
            }

            onFinished?.Invoke(scriptObject);
        });
    }

    /// <summary>
    /// 隐藏单个界面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void HideWindow<T>() where T : MonoBehaviour
    {
        if (UIPanelDic.Count == 0)
        {
            return;
        }

        HideWindow(typeof(T).Name);
    }

    /// <summary>
    /// 关闭弹出的自己
    /// </summary>
    /// <param name="baseUI"></param>
    public void HideSelfWindow(BaseUI baseUI)
    {
        HideWindow(baseUI.gameObject.name);
    }

    /// <summary>
    /// 关闭所有的界面
    /// </summary>
    public void HideAllWindow()
    {
        foreach (var windowName in UIPanelDic.Keys.ToArray())
        {
            HideWindow(windowName);
        }
    }

    private void HideWindow(string windowName)
    {
        if (UIPanelDic.ContainsKey(windowName))
        {
            BaseUI scriptObject = UIPanelDic[windowName];

            UIPanelDic.Remove(windowName);

            if (scriptObject != null && scriptObject.gameObject != null)
            {
                UnityEngine.Object.Destroy(scriptObject.gameObject);
            }
        }
    }

    private T AddWindow<T>(GameObject uiParent, GameObject go, int order = -1) where T : BaseUI
    {
        go.transform.SetParent(uiParent.transform);

        T scriptObject = go.AddComponent<T>();
        UIPanelDic[go.name] = scriptObject;

        // 只有1个的时候，就相当于重置一下
        if (UIPanelDic.Count == 1)
        {
            seqIndex = 0;
        }

        go.GetComponent<Canvas>().sortingOrder = (order > 0 ? order : ++seqIndex);
        return scriptObject;
    }
}