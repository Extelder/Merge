using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.AI;

public enum Type
{
    Enemy,
    Player
}

public enum FightStyle
{
    Rush,
    Stay
}

[RequireComponent(typeof(Rigidbody), typeof(UnitAnimation))]
public class FightUnit : MonoBehaviour
{
    private List<Transform> _enemies = new List<Transform>();
    private Transform _nearestEnemy;
    private Rigidbody _rigidbody;
    private NavMeshAgent _navMeshAgent;
    private UnitAnimation _unitAnimation;
    [SerializeField] private Type _unitType;
    [SerializeField] private int _givenDamage;
    [SerializeField] private GameObject _ammoPrefab;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private FightStyle _fightStyle;

    private bool _attacking = false;

    private void Start()
    {
        _unitAnimation = GetComponent<UnitAnimation>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();

        _navMeshAgent.updateRotation = false;
        if (_unitType == Type.Enemy)
            _enemies = PlayerUnits._singleton._units;
        else if (_unitType == Type.Player)
            _enemies = Enemies._singleton._allEnemies;
    }

    private void OnEnable() => Fight.OnFightBegin += Attack;

    private void OnDisable() => Fight.OnFightBegin -= Attack;

    private void Update()
    {
        if (_attacking)
        {
            if (_nearestEnemy == null)
            {
                StopAllCoroutines();
                _unitAnimation.SetAttackTrigger(false);
                FindNearestEnemy();
            }
            else
            {
                Vector3 newDir = Vector3.RotateTowards(transform.forward, (_nearestEnemy.position - transform.position),
                    180f, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
                if (_navMeshAgent.enabled == true)
                {
                    _navMeshAgent.SetDestination(_nearestEnemy.position);
                }
            }
        }
    }

    private void Attack()
    {
        FindNearestEnemy();
    }

    private IEnumerator Damaging(UnitHealth _unit)
    {
        while (true)
        {
            _unit.TakeDamage(_givenDamage);
            _navMeshAgent.enabled = false;
            Debug.Log("Damaging");
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator Shooting()
    {
        while (true)
        {
            Bullet _bullet = Instantiate(_ammoPrefab, _muzzle.position, transform.rotation).GetComponent<Bullet>();
            _bullet._damage = _givenDamage;
            _bullet._parentType = _unitType;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void FindNearestEnemy()
    {
        float _nearest = Single.PositiveInfinity;
        Transform _nearestEnemyPosition = transform;
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i] != null)
            {
                Debug.Log((transform.position - _enemies[i].position).sqrMagnitude);
                if ((transform.position - _enemies[i].position).sqrMagnitude < _nearest)
                {
                    _nearest = (transform.position - _enemies[i].position).sqrMagnitude;
                    _nearestEnemyPosition = _enemies[i];
                }
            }
        }

        if (_nearestEnemyPosition != transform)
        {
            _nearestEnemy = _nearestEnemyPosition;
            _attacking = true;
            if (_fightStyle == FightStyle.Rush)
            {
                _navMeshAgent.enabled = true;
            }
            else if (_fightStyle == FightStyle.Stay)
            {
                _unitAnimation.SetAttackTrigger(true);
                StartCoroutine(Shooting());
            }

            Debug.Log(_nearestEnemyPosition.gameObject.name);
        }

        if (_nearestEnemyPosition == transform)
        {
            if (_unitType == Type.Player)
            {
                WinOrLoseChecker.Singleton.CheckWin(true);
            }
            else
            {
                WinOrLoseChecker.Singleton.CheckWin(false);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<UnitHealth>(out UnitHealth _unitHealth) && _attacking)
        {
            if (_unitHealth._unitType != _unitType && _nearestEnemy.GetComponent<UnitHealth>() == _unitHealth &&
                _fightStyle == FightStyle.Rush)
            {
                _unitAnimation.SetAttackTrigger(true);
                StartCoroutine(Damaging(_unitHealth));
            }
        }
    }

    public int GetGiveDamage() => _givenDamage;

    public void SetGivenDamage(int value)
    {
        _givenDamage = Mathf.Clamp(value, 20, 150);
    }
}