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
        _sr = GetComponent<SpriteRenderer>();
        await Helper.Tasks.Canceled(EnemyGeneration(destroyCancellationToken));
    }





    // ---------------------------- PrivateMethod
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask EnemyGeneration(CancellationToken ct)
    {
        while (true)
        {
            //  �����Ԋu
            var time = Random.Range(_generateTime.x, _generateTime.y);
            await Helper.Tasks.DelayTime(time, ct);

            //  ��������
            if (_generate == Helper.Switch.OFF && !_sr.isVisible)
            {
                continue;
            }

            //  ����
            var enemy = Instantiate(_enemy, transform.position, Quaternion.identity);
            _totalCount.Add(enemy);

            //  ����������
            if (_totalCount.Count > _spawnLimit)
            {
                _totalCount[0]?.GetComponent<IEnemyDamageable>().Die();
                _totalCount.RemoveAt(0);
            }

            await UniTask.Yield(cancellationToken: ct);
        }
    }
}
