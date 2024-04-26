using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealth : MonoBehaviour
{
   [SerializeField] private Type _type;
   [SerializeField] private float _currentValue;
   [SerializeField] private Image _healthBar;
   [SerializeField] private float _cost;

   public Type _unitType;

   public void TakeDamage(int _damage)
   {
      if (_currentValue > 0)
      {
         _currentValue -= _damage;
         UpdateHealthBarValue();
      }
      else if (_currentValue <= 0)
      {
         if (_type == Type.Enemy)
         {
            Wallet.Singleton.AddMoney(_cost);
         }
         else if (_type == Type.Player)
         {
            Wallet.Singleton.SpendMoney(_cost);
         }
         Destroy(gameObject);
      }
   }

   public void Heal(float value)
   {
      _currentValue += value;
   }

   private void UpdateHealthBarValue()
   {
      _healthBar.fillAmount = _currentValue / 100;
   }
}
