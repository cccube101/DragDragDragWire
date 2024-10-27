using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class ShooterEnemyController : EnemyBase
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�I�u�W�F�N�g")] private GameObject _muzzle;
    [SerializeField, Required, BoxGroup("�I�u�W�F�N�g")] private GameObject _bulletObj;

    [SerializeField, Required, BoxGroup("�e�p�����[�^")] private float _generationVolume;
    [SerializeField, Required, BoxGroup("�e�p�����[�^")] private float _generationRate;
    [SerializeField, Required, BoxGroup("�e�p�����[�^")] private float _generationInterval;

    // ---------------------------- Field
    //  �L���b�V��
    private Transform _muzzleTr = null;

    //  �Ǐ]
    private readonly float LOOK = 0.8f;




    // ---------------------------- UnityMessage
    public override async void Start()
    {
        StartEvent();
        _muzzleTr = _muzzle.transform;

        //  �ˌ��T�C�N���J�n
        await Helper.Tasks.Canceled(ShooterCycle(destroyCancellationToken));
    }

    private void Update()
    {
        //  �ړ�����
        Move();
    }



    // ---------------------------- PrivateMethod
    /// <summary>
    /// �ˌ��T�C�N��
    /// </summary>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�ˌ��T�C�N��</returns>
    private async UniTask ShooterCycle(CancellationToken ct)
    {
        while (true)
        {
            //  ��ʓ��ɃI�u�W�F�N�g�����邩�ǂ���
            if (_sr.isVisible)
            {
                //  �w�萔�ˌ�
                for (int i = 0; i < _generationVolume; i++)
                {
                    //  �p�����[�^����
                    var bullet = Instantiate(_bulletObj, _tr.position, Quaternion.identity);
                    var dir = _muzzleTr.position - _tr.position;

                    //  �e�����w��
                    bullet.GetComponent<BulletController>().Dir = (dir, _tr.rotation);

                    //  �ҋ@
                    await Helper.Tasks.DelayTime(_generationRate, ct);
                }
            }
            //  �ҋ@
            await Helper.Tasks.DelayTime(_generationInterval, ct);
            await UniTask.Yield(cancellationToken: ct);
        }
    }

    /// <summary>
    /// �ړ�����
    /// </summary>
    private void Move()
    {
        //  ��ʓ��ɃI�u�W�F�N�g�����邩�ǂ���
        if (_sr.isVisible)
        {
            var playerPos = PlayerController.Instance.Tr.position;   //  �v���C���[�ʒu�擾

            //  �v���C���[�����։�]
            var dir = Vector3.Lerp(playerPos, _tr.position, LOOK);
            var diff = (playerPos - dir).normalized;
            _tr.rotation = Quaternion.FromToRotation(Vector3.up, diff);
        }
        else
        {
            //  ��~
            _rb2d.Sleep();
        }
    }
}
