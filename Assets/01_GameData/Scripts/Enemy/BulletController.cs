using Alchemy.Inspector;
using UnityEngine;

public class BulletController : EnemyBase
{
    // ---------------------------- SerializeField

    [SerializeField, Required, BoxGroup("�p�����[�^")] private float _moveSpeed;
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
            _tr.rotation = value.Rotation;
        }
    }


    // ---------------------------- UnityMessage
    public override void Start()
    {
        StartEvent();

        //  �ˌ����Đ�
        Instantiate(_shootClip, _tr.position, Quaternion.identity);
    }

    private void Update()
    {
        //  �ړ�����
        _tr.position += _addDir * _moveSpeed * Time.deltaTime;
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
    /// �G����
    /// </summary>
    public override void Die()
    {
        //  �G�t�F�N�g����
        Instantiate(_hitClip, _tr.position, Quaternion.identity);
        Instantiate(_knockEffect, _tr.position, Quaternion.identity);

        //  �폜
        Destroy(_obj);
    }
}
