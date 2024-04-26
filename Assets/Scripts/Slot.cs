using System;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Unit _CurrentUnit; 
    [SerializeField] private bool _blocked = false;

    public void SetBlockedState(Unit _who, bool value)
    {
        _CurrentUnit = _who;
        _blocked = value;
        if (value == false)
        {
            _CurrentUnit = null;
        }
    }

    public bool GetBlockedState() => _blocked;
}
