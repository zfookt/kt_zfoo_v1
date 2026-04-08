using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    private Transform _cover;

    protected Transform cover
    {
        get
        {
            if (_cover == null)
            {
                _cover = transform.Find("Cover");
            }

            return _cover;
        }
    }
}