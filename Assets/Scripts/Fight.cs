using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
   public delegate void OnFight();

   public static event OnFight OnFightBegin;

   public void Begin()
   {
      //BattleAISurface.Singleton.BakeSurface();
      OnFightBegin.Invoke();
   }
}
