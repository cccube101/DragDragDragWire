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
        //  �L���b�V��
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //  �ړ�����
        Move();
    }





    // ---------------------------- PublicMethod
    /// <summary>
    /// �v���C���[�ւ̃_���[�W
    /// </summary>
    /// <param name="player">�v���C���[�I�u�W�F�N�g</param>
    /// <returns>�_���[�W��</returns>
    public int Damage(GameObject player)
    {
        //  �m�b�N�o�b�N
        var dir = (player.transform.position - transform.position).normalized;
        player.GetComponent<Rigidbody2D>().AddForce(dir * _knockBackForce);

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




    // ---------------------------- PrivateMethod
    /// <summary>
    /// �ړ�����
    /// </summary>
    private void Move()
    {
        //  ��ʓ��ɃI�u�W�F�N�g�����邩�ǂ���
        if (_sr.isVisible)
        {
            //  �Ǐ]�ړ�
            if (_move == Helper.Switch.ON)
            {
                _agent.SetDestination(PlayerController.Instance.transform.position);
            }
            //  �v���C���[�����։�]
            var playerPos = PlayerController.Instance.transform.position;   //  �v���C���[�ʒu�擾
            var dir = Vector3.Lerp(playerPos, transform.position, LOOK);    //  ��������
            var diff = (playerPos - dir).normalized;    //  �m�[�}���C�Y����
            transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
        }
        else
        {
            //  ��~
            _rb.Sleep();
        }
    }
}
