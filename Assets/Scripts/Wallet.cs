using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    private float _moneyAmount;

    public delegate void OnValueEdited(float _cuurentValue);

    public static event OnValueEdited MoneyValueEdited;

    public static Wallet Singleton { get; private set; }

    private void Start()
    {
        Singleton = this;
        _moneyAmount = PlayerPrefs.GetFloat("Money");
        AddMoney(10000f);
        SaveMoney();
    }

    public void AddMoney(float value)
    {
        _moneyAmount += value;
        SaveMoney();
    }

    public void SpendMoney(float value)
    {
        if (_moneyAmount > 0)
        {
            _moneyAmount -= value;
            SaveMoney();
        }
    }

    public float GetCurrentMoney() => _moneyAmount;

    public void SaveMoney()
    {
        MoneyValueEdited.Invoke(_moneyAmount);
        PlayerPrefs.SetFloat("Money", _moneyAmount);
    }
}