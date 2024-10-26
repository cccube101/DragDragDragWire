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

    protected SpriteRenderer _sr = null;
    protected Rigidbody2D _rb = null;

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
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// �v���C���[�ւ̃_���[�W
    /// </summary>
    /// <param name="player">�v���C���[�I�u�W�F�N�g</param>
    /// <returns></returns>
    public virtual int Damage(GameObject player)
    {
        //  �m�b�N�o�b�N
        var dir = (player.transform.position - transform.position).normalized;
        player.GetComponent<Rigidbody2D>().AddForce(dir * _knockBackForce);

        return _damage;
    }

    /// <summary>
    /// �G���ŃC�x���g
    /// </summary>
    public virtual void Die()
    {
        //  �G�t�F�N�g����
        Instantiate(_knockEffect, transform.position, Quaternion.identity);

        //  �폜
        Destroy(gameObject);
    }
}
