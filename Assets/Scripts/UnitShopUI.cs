using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitShopUI : MonoBehaviour
{
    private Text _unitCostText;

    private void Awake()
    {
        _unitCostText = GetComponentInChildren<Text>();
        _unitCostText.fontSize = _unitCostText.fontSize += 20;
    }

    public void UpdateUnitCostText(int currentCost)
    {
        _unitCostText.text = currentCost.ToString();
    }
}
