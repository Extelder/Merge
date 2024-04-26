using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleAISurface : MonoBehaviour
{
  [SerializeField] private NavMeshSurface _navMeshSurface;

  public static BattleAISurface Singleton { get; private set; }

  private void Awake()
  {
    Singleton = this;
  }

  public void BakeSurface()
  {
    _navMeshSurface.BuildNavMesh();
  }
}
