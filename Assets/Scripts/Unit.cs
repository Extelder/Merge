using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeSkin
{
    Boobs,
    Nails,
    Ass
}

[RequireComponent(typeof(Rigidbody), typeof(FightUnit), typeof(UnitHealth))]
public class Unit : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private Vector3 _currentSlotTransform;
    private SlotsCharacteristics _slotsCharacteristics;
    private FightUnit _fightUnit;
    private UnitHealth _unitHealth;
    private PlayerUnits _playerUnits;
    public Slot _currentSlot;
    private Camera _camera;
    [SerializeField] private TypeSkin _skinType;
    [SerializeField] private int _skinId;
    [SerializeField] private Transform _increasePartAfterCombine;
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private int _materialTOChangeId;
    [SerializeField] private int _shapeMeshId;
    [SerializeField] private Material[] _materialsAfterCombine;

    private Vector3 _pointScreen;
    private bool _drag;
    private bool _fighting = false;
    public bool _start = true;
    private Vector3 Offset;

    private void Awake()
    {
        _currentSlotTransform = this.transform.position;
        _rigidBody = GetComponent<Rigidbody>();
        _fightUnit = GetComponent<FightUnit>();
        _unitHealth = GetComponent<UnitHealth>();
    }

    private void Start()
    {
        _camera = Camera.main;
        _slotsCharacteristics = SlotsCharacteristics._singleton;
        _playerUnits = PlayerUnits._singleton;
        if (_start == true)
            Invoke("FoundNearestSlot", 0.05f);


        _playerUnits.AddUnit(transform);
    }

    private void Disactivate()
    {
        enabled = false;
        _fighting = true;
        _rigidBody.isKinematic = true;
    }

    private void OnMouseDown()
    {
        _currentSlotTransform = this.transform.position;
        if (_fighting == false)
        {
            _pointScreen = _camera.WorldToScreenPoint(transform.position);

            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (groundPlane.Raycast(ray, out float Position))
            {
                Vector3 world = ray.GetPoint(Position);
                Offset = transform.position - world;
            }
        }
    }

    private void OnMouseDrag()
    {
        if (_fighting == false)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.up * 0.2f);
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (groundPlane.Raycast(ray, out float Position))
            {
                Vector3 world = ray.GetPoint(Position);
                transform.position = new Vector3(world.x + Offset.x, (world.y) + 0.08f, (world.z + Offset.z));
            }
        }
    }

    private void OnMouseUp()
    {
        if (_fighting == false)
            FoundNearestSlot();
    }

    public void FoundNearestSlot()
    {
        bool _finded = false;
        ResearchAllSlots(0.47f);

        if (_finded == false) ResearchAllSlots(1.3f);

        if (_finded == false)
        {
            Debug.Log(_currentSlotTransform);
            transform.position = _currentSlotTransform;
        }


        void ResearchAllSlots(float _minimumRange)
        {
            for (int i = 0; i < _slotsCharacteristics.GetSlot().Count; i++)
            {
                Transform _slotPosition = _slotsCharacteristics.GetSlot()[i].transform;
                if ((transform.position - _slotPosition.position).sqrMagnitude < _minimumRange)
                {
                    if (_slotsCharacteristics.GetSlot()[i].GetBlockedState() == false)
                    {
                        Debug.Log("AAAAAA");
                        if (_currentSlot != null) _currentSlot.SetBlockedState(this, false);

                        _currentSlotTransform = _slotPosition.position;
                        transform.position = _slotPosition.position;

                        _slotsCharacteristics.GetSlot()[i].SetBlockedState(this, true);
                        _currentSlot = _slotsCharacteristics.GetSlot()[i];

                        _finded = true;
                        break;
                    }
                    else if (_slotsCharacteristics.GetSlot()[i].GetBlockedState() == true)
                    {
                        if (_skinType == _slotsCharacteristics.GetSlot()[i]._CurrentUnit._skinType &&
                            _skinId == _slotsCharacteristics.GetSlot()[i]._CurrentUnit._skinId &&
                            _currentSlot._CurrentUnit != _slotsCharacteristics.GetSlot()[i]._CurrentUnit)
                        {
                            if (_currentSlot != null) _currentSlot.SetBlockedState(this, false);

                            _slotsCharacteristics.GetSlot()[i]._CurrentUnit.MergeUnits();
                            _finded = true;
                            _playerUnits.RemoveUnit(transform);

                            Destroy(gameObject);
                            break;
                        }
                    }
                }
            }
        }
    }

    public void MergeUnits()
    {
        _skinId++;
        switch (_skinId)
        {
            case 1:
            {
                ChangeAnyUnitCharacteristics(0, 50f);
                gameObject.GetComponent<MeshRenderer>().material = _playerUnits._clothesMaterials[0];
                break;
            }
            case 2:
            {
                ChangeAnyUnitCharacteristics(1, 100f);
                gameObject.GetComponent<MeshRenderer>().material = _playerUnits._clothesMaterials[1];
                break;
            }
        }

        void ChangeAnyUnitCharacteristics(int id, float value)
        {
            _skinnedMeshRenderer.materials[_materialTOChangeId].color = _materialsAfterCombine[id].color;
            _skinnedMeshRenderer.SetBlendShapeWeight(_shapeMeshId, value);
            _unitHealth.Heal(20f);
            _fightUnit.SetGivenDamage(_fightUnit.GetGiveDamage() + 20);
        }
    }

    public void OnEnable()
    {
        Fight.OnFightBegin += Disactivate;
    }

    private void OnDisable()
    {
        Fight.OnFightBegin -= Disactivate;
    }
}