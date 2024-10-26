using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class ShooterEnemyController : MonoBehaviour, IEnemyDamageable
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�I�u�W�F�N�g")] private GameObject _muzzle;
    [SerializeField, Required, BoxGroup("�I�u�W�F�N�g")] private GameObject _bulletObj;

    [SerializeField, Required, BoxGroup("�e�p�����[�^")] private float _generationVolume;
    [SerializeField, Required, BoxGroup("�e�p�����[�^")] private float _generationRate;
    [SerializeField, Required, BoxGroup("�e�p�����[�^")] private float _generationInterval;

    [SerializeField, Required, BoxGroup("�_���[�W")] private int _damage;
    [SerializeField, Required, BoxGroup("�_���[�W")] private float _knockBackForce;

    [SerializeField, Required, BoxGroup("�G�t�F�N�g")] private GameObject _knockEffect;

    // ---------------------------- Field
    //  ������
    private SpriteRenderer _sr = null;
    private Rigidbody2D _rb = null;

    //  �Ǐ]
    private readonly float LOOK = 0.8f;




    // ---------------------------- UnityMessage
    private async void Start()
    {
        //  �L���b�V��
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();

        //  �ˌ��T�C�N���J�n
        await Helper.Tasks.Canceled(ShooterCycle(destroyCancellationToken));
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
        var dir = (player.transform.position - transform.position).normalized;
        player.GetComponent<Rigidbody2D>().AddForce(dir * _knockBackForce);
        return _damage;
    }

    /// <summary>
    /// �G����
    /// </summary>
    public void Die()
    {
        Instantiate(_knockEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
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
                    var bullet = Instantiate(_bulletObj, transform.position, Quaternion.identity);
                    var dir = _muzzle.transform.position - transform.position;

                    //  �e�����w��
                    bullet.GetComponent<BulletController>().Dir = (dir, transform.rotation);

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
            var playerPos = PlayerController.Instance.transform.position;   //  �v���C���[�ʒu�擾

            //  �v���C���[�����։�]
            var dir = Vector3.Lerp(playerPos, transform.position, LOOK);
            var diff = (playerPos - dir).normalized;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
        }
        else
        {
            //  ��~
            _rb.Sleep();
        }
    }
}
