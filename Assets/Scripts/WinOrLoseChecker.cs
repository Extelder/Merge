using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinOrLoseChecker : MonoBehaviour
{
   public static event Action<bool> OnMatchEnded;
   
   public static WinOrLoseChecker Singleton { get; private set; }
   

   private void Awake()
   {
       Singleton = this;
   }

   public void CheckWin(bool _win)
   {
       OnMatchEnded.Invoke(_win);   
   }
}
