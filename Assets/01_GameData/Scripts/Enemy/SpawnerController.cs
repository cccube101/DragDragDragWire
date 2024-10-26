using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�X�C�b�`")] private Helper.Switch _generate;

    [SerializeField, Required, BoxGroup("�p�����[�^")] private int _spawnLimit;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private Vector2 _generateTime;
    [SerializeField, Required, BoxGroup("�p�����[�^/�G�I�u�W�F�N�g")] private GameObject _enemy;

    // ---------------------------- Field
    private SpriteRenderer _sr = null;
    private readonly List<GameObject> _totalCount = new();




    // ---------------------------- UnityMessage
    private async void Start()
    {
        //  �L���b�V��
        _sr = GetComponent<SpriteRenderer>();

        //  �����J�n
        await Helper.Tasks.Canceled(EnemyGeneration(destroyCancellationToken));
    }





    // ---------------------------- PrivateMethod
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�^�X�N����</returns>
    private async UniTask EnemyGeneration(CancellationToken ct)
    {
        while (true)
        {
            //  �����Ԋu
            var time = Random.Range(_generateTime.x, _generateTime.y);
            await Helper.Tasks.DelayTime(time, ct);

            //  ��������
            //  �X�C�b�` �I�� ���� ��ʓ��ɃI�u�W�F�N�g�����邩�ǂ���
            if (_generate == Helper.Switch.OFF && !_sr.isVisible)
            {
                continue;
            }

            //  ����
            var enemy = Instantiate(_enemy, transform.position, Quaternion.identity);
            _totalCount.Add(enemy);

            //  ����������
            //  �ő吔�𒴂����Ƃ������폜
            if (_totalCount.Count > _spawnLimit)
            {
                _totalCount[0]?.GetComponent<IEnemyDamageable>().Die();
                _totalCount.RemoveAt(0);
            }

            await UniTask.Yield(cancellationToken: ct);
        }
    }
}
