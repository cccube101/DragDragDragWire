using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.AI;

public class TrackingEnemyController : EnemyBase
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�X�C�b�`")] private Helper.Switch _move;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private float _speed;

    // ---------------------------- Field
    //  ������
    private NavMeshAgent _agent = null;

    //  �Ǐ]
    private const float LOOK = 0.8f;




    // ---------------------------- UnityMessage
    public override void Start()
    {
        StartEvent();

        //  �L���b�V��
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //  �ړ�����
        Move();
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
