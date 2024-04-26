using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void SetAttackTrigger(bool value)
    {
        if(_animator != null)
         _animator.SetBool("Attacking", value);
    }
}
