using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
   public List<Transform> _allEnemies = new List<Transform>();

   public static Enemies _singleton;

   private void Awake()
   {
      _singleton = this;
   }

   public void AddEnemy(Transform _enemy)
   {
      _allEnemies.Add(_enemy);
   }
}
