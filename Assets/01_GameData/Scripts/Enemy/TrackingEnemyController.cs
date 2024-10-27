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
            //  �v���C���[�ʒu�擾
            var playerPos = PlayerController.Instance.Tr.position;
            //  �Ǐ]�ړ�
            if (_move == Helper.Switch.ON)
            {
                _agent.SetDestination(playerPos);
            }
            //  �v���C���[�����։�]
            var dir = Vector3.Lerp(playerPos, _tr.position, LOOK);    //  ��������
            var diff = (playerPos - dir).normalized;    //  �m�[�}���C�Y����
            _tr.rotation = Quaternion.FromToRotation(Vector3.up, diff);
        }
        else
        {
            //  ��~
            _rb2d.Sleep();
        }
    }
}
