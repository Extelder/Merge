using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(UnitShopUI))]
public class UnitShop : MonoBehaviour
{
    private SlotsCharacteristics _slotsCharacteristics;
    private int _cost = 150;
    private bool _canBuy = true;

    private void Start()
    {
        Fight.OnFightBegin += DestroyObject;
        _slotsCharacteristics = SlotsCharacteristics._singleton;
        GetComponent<UnitShopUI>().UpdateUnitCostText(_cost);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    
    public void BuyUnit(Unit _unitPrefab)
    {
        if ((Wallet.Singleton.GetCurrentMoney() - _cost) >= 0 && _canBuy == true)
        {
            for (int i = 0; i < _slotsCharacteristics.GetSlot().Count; i++)
            {
                if (_slotsCharacteristics.GetSlot()[i].GetBlockedState() == false)
                {
                    Wallet.Singleton.SpendMoney(_cost);
                    _cost += new Random().Next(15, 100);
                    _unitPrefab = Instantiate(_unitPrefab, _slotsCharacteristics.GetSlot()[i].transform.position,
                        Quaternion.identity);

                    _unitPrefab._start = false;
                    _slotsCharacteristics.GetSlot()[i].SetBlockedState(_unitPrefab, true);
                    _unitPrefab._currentSlot = _slotsCharacteristics.GetSlot()[i];
                    GetComponent<UnitShopUI>().UpdateUnitCostText(_cost);
                    break;
                }
            }
        }
    }

    public void OnDisable()
    {
        Fight.OnFightBegin -= DestroyObject;
    }
}