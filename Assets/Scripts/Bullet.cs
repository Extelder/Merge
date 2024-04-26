using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _speed;
    public int _damage;
    public Type _parentType;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.position += transform.forward * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<UnitHealth>(out UnitHealth _unitHealth) && (_unitHealth._unitType != _parentType))
        {
            _unitHealth.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
