using Alchemy.Inspector;
using UnityEngine;

public class BulletController : MonoBehaviour, IEnemyDamageable
{
    // ---------------------------- SerializeField

    [SerializeField, Required, BoxGroup("��b")] private float _moveSpeed;
    [SerializeField, Required, BoxGroup("��b")] private int _damage;
    [SerializeField, Required, BoxGroup("��b")] private float _knockBackForce;

    [SerializeField, Required, BoxGroup("�G�t�F�N�g")] private GameObject _knockEffect;
    [SerializeField, Required, BoxGroup("�G�t�F�N�g")] private GameObject _shootClip;
    [SerializeField, Required, BoxGroup("�G�t�F�N�g")] private GameObject _hitClip;

    // ---------------------------- Field
    private Vector3 _addDir;

    // ---------------------------- Property
    /// <summary>
    /// �����X�V
    /// </summary>
    public (Vector3 Dir, Quaternion Rotation) Dir
    {
        set
        {
            _addDir = value.Dir;
            transform.rotation = value.Rotation;
        }
    }


    // ---------------------------- UnityMessage
    private void Start()
    {
        Instantiate(_shootClip, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        transform.position += _addDir * _moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var obj = collision.gameObject;
        if (obj.CompareTag(TagName.Ground))
        {
            Die();
        }
        else if (obj.CompareTag(TagName.Belt))
        {
            Die();
        }
    }




    // ---------------------------- PublicMethod
    /// <summary>
    /// �v���C���[�ւ̃_���[�W
    /// </summary>
    public int Damage(GameObject obj)
    {
        var dir = (obj.transform.position - transform.position).normalized;
        obj.GetComponent<Rigidbody2D>().AddForce(dir * _knockBackForce);

        return _damage;
    }

    /// <summary>
    /// �G����
    /// </summary>
    public void Die()
    {
        //  �G�t�F�N�g
        Instantiate(_hitClip, transform.position, Quaternion.identity);
        Instantiate(_knockEffect, transform.position, Quaternion.identity);

        //  �폜
        Destroy(gameObject);
    }
}
