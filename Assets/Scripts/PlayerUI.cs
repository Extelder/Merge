using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Text _moneyValueText;
    [SerializeField] private GameObject _winCanvas;
    [SerializeField] private GameObject _loseCanvas;

    private void OnEnable()
    {
        Wallet.MoneyValueEdited += UpdateMoneyText;
        WinOrLoseChecker.OnMatchEnded += EnableCanvas;
    }

    private void OnDisable()
    {
        Wallet.MoneyValueEdited -= UpdateMoneyText;
        WinOrLoseChecker.OnMatchEnded -= EnableCanvas;
    }

    public void UpdateMoneyText(float currentMoneyValue)
    {
        _moneyValueText.text = currentMoneyValue.ToString();
    }

    public void EnableCanvas(bool _win)
    {
        if (_win == true)
        {
            _winCanvas.SetActive(true);
        }
        else
        {
            _loseCanvas.SetActive(true);
        }
    }

}