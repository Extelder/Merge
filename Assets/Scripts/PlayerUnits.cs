using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnits : MonoBehaviour
{
   public List<Transform> _units = new List<Transform>();
   public Material[] _clothesMaterials;
   
   public static PlayerUnits _singleton { get; private set; }

   private void Awake()
   {
      _singleton = this;
   }

   public void AddUnit(Transform _unitPosition)
   {
      _units.Add(_unitPosition);
   }

   public void RemoveUnit(Transform _unitPosition)
   {
      _units.Remove(_unitPosition);
   }
}
