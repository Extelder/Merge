using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsCharacteristics : MonoBehaviour
{
   [SerializeField] private List<Slot> _slots = new List<Slot>();
   

   public static SlotsCharacteristics _singleton { get; private set; }

   private void Awake() =>  _singleton = this;

   public void AddSlot(Slot _slot) => _slots.Add(_slot);

   public List<Slot> GetSlot() => _slots;


   private void OnEnable()
   {
    Fight.OnFightBegin += FightBegam;     
   }

   private void OnDisable()
   {
      Fight.OnFightBegin -= FightBegam;
   }

   private void FightBegam()
   {
      for (int i = 0; i < _slots.Count; i++)
      {
         _slots[i].enabled = false;
      }
   }
}
