using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.AI;

public class TrackingEnemyController : MonoBehaviour, IEnemyDamageable
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�X�C�b�`")] private Helper.Switch _move;

    [SerializeField, Required, BoxGroup("�p�����[�^")] private float _speed;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private int _damage;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private float _knockBackForce;

    [SerializeField, Required, BoxGroup("�G�t�F�N�g")] private GameObject _knockEffect;

    // ---------------------------- Field
    //  ������
    private SpriteRenderer _sr = null;
    private Rigidbody2D _rb = null;
    private NavMeshAgent _agent = null;

    //  �Ǐ]
    private const float LOOK = 0.8f;




    // ---------------------------- UnityMessage
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Move();
    }





    // ---------------------------- PublicMethod
    /// <summary>
    /// �v���C���[�ւ̃_���[�W
    /// </summary>
    public int Damage(GameObject obj)
    {
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
        Instantiate(_knockEffect, transform.position, Quaternion.identity); //  �G�t�F�N�g
        Destroy(gameObject);    //  �폜
    }




    // ---------------------------- PrivateMethod
    /// <summary>
    /// �ړ�����
    /// </summary>
    private void Move()
    {
        if (_sr.isVisible)
        {
            //  �Ǐ]�ړ�
            if (_move == Helper.Switch.ON)
            {
                _agent.SetDestination(PlayerController.Instance.transform.position);
            }
            //  �v���C���[�����։�]
            var playerPos = PlayerController.Instance.transform.position;
            var dir = Vector3.Lerp(playerPos, transform.position, LOOK);
            var diff = (playerPos - dir).normalized;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
        }
        else
        {
            _rb.Sleep();
        }
    }
}
