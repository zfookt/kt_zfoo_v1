using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToastManager : Singleton<ToastManager>
{
    private const int DEFAULT_ORDER = UIOrderConst.Toast;

    //<SortingOrder, 对象>
    private readonly Dictionary<int, ToastUI> ToastUIStack = new();

    private int TopSortingOrder = DEFAULT_ORDER;

    // ui根节点
    private GameObject uiRoot;

    public void Init(GameObject uiRootParam)
    {
        this.uiRoot = uiRootParam;
    }

    public void Show(string content)
    {
        ResourceLoader.Instance.LoadAndInstantiateAsync<GameObject>(typeof(ToastUI).Name, go =>
        {
            go.transform.SetParent(this.uiRoot.transform);
            int sortingOrder = ++TopSortingOrder;

            ToastUI scriptObject = go.AddComponent<ToastUI>();
            ToastUIStack[sortingOrder] = scriptObject;
            go.GetComponent<Canvas>().sortingOrder = sortingOrder;

            scriptObject.Init(content, 1f, 150f, 1f);
        });
    }

    public void DestroyToastUI(ToastUI toastUI)
    {
        foreach (var item in ToastUIStack)
        {
            if (item.Value == toastUI)
            {
                ToastUIStack.Remove(item.Key);
                UnityEngine.Object.Destroy(toastUI.gameObject);
                break;
            }
        }

        if (ToastUIStack.Count > 0)
        {
            TopSortingOrder = ToastUIStack.Keys.Max();
        }
        else
        {
            TopSortingOrder = DEFAULT_ORDER;
        }
    }
}