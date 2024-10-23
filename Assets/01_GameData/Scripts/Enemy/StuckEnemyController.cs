using Alchemy.Inspector;
using UnityEngine;

public class StuckEnemyController : MonoBehaviour, IEnemyDamageable
{
    // ---------------------------- SerializeField

    [SerializeField, Required, BoxGroup("�p�����[�^")] private float _turnSpeed;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private int _damage;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private float _knockBackForce;

    [SerializeField, Required, BoxGroup("�G�t�F�N�g")] private GameObject _knockEffect;

    // ---------------------------- Field
    private SpriteRenderer _sr = null;


    // ---------------------------- UnityMessage
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_sr.isVisible)
        {
            transform.eulerAngles += new Vector3(0, 0, _turnSpeed * Time.deltaTime);
        }
    }




    // ---------------------------- PublicMethod
    /// <summary>
    /// �v���C���[�ւ̃_���[�W
    /// </summary>
    public int Damage(GameObject obj)
    {
        //  �G�t�F�N�g
        Instantiate(_knockEffect, transform.position, Quaternion.identity);

        //  �m�b�N�o�b�N
        var dir = (obj.transform.position - transform.position).normalized;
        obj.GetComponent<Rigidbody2D>().AddForce(dir * _knockBackForce);

        //  �_���[�W
        return _damage;
    }

    /// <summary>
    /// �G����
    /// </summary>
    public void Die()
    {
        //  �G�t�F�N�g
        Instantiate(_knockEffect, transform.position, Quaternion.identity);

        //  �폜
        Destroy(gameObject);
    }
}
