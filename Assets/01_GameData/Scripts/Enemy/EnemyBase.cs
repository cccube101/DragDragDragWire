using Alchemy.Inspector;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IEnemyDamageable
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("��b")] protected int _damage;
    [SerializeField, Required, BoxGroup("��b")] protected float _knockBackForce;
    [SerializeField, Required, BoxGroup("��b")] protected GameObject _knockEffect;


    // ---------------------------- Field
    protected IEnemyDamageable _enemyDamageable;

    protected GameObject _obj = null;
    protected Transform _tr = null;
    protected SpriteRenderer _sr = null;
    protected Rigidbody2D _rb2d = null;

    // ---------------------------- UnityMessage
    public virtual void Start()
    {
        StartEvent();
    }



    // ---------------------------- PublicMethod
    /// <summary>
    /// �J�n�C�x���g
    /// </summary>
    public void StartEvent()
    {
        _obj = gameObject;
        _tr = transform;
        _sr = GetComponent<SpriteRenderer>();
        _rb2d = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// �v���C���[�ւ̃_���[�W
    /// </summary>
    /// <returns>�_���[�W��</returns>
    public virtual int Damage()
    {
        KnockBackPlayer();

        return _damage;
    }

    /// <summary>
    /// �m�b�N�o�b�N
    /// </summary>
    public void KnockBackPlayer()
    {
        //  �m�b�N�o�b�N
        var player = PlayerController.Instance;
        var dir = (player.Tr.position - _tr.position).normalized;
        player.RB2D.AddForce(dir * _knockBackForce);
    }

    /// <summary>
    /// �G���ŃC�x���g
    /// </summary>
    public virtual void Die()
    {
        //  �G�t�F�N�g����
        Instantiate(_knockEffect, _tr.position, Quaternion.identity);

        //  �폜
        Destroy(_obj);
    }
}
