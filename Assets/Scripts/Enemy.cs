using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    void Start()
    {
        Enemies._singleton.AddEnemy(this.transform);
    }
}
