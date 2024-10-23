using UnityEngine;
using DG.Tweening;
using Alchemy.Inspector;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

public class MoveFloorController : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�ړ��p�����[�^")] private Transform[] _pos;
    [SerializeField, Required, BoxGroup("�ړ��p�����[�^")] private float _time;
    [SerializeField, Required, BoxGroup("�ړ��p�����[�^")] private float _waitTime;
    [SerializeField, Required, BoxGroup("�ړ��p�����[�^")] private LoopType _loopType;
    [SerializeField, Required, BoxGroup("�ړ��p�����[�^")] private PathType _pathType;
    [SerializeField, Required, BoxGroup("�ړ��p�����[�^")] private bool _setOption;







    // ---------------------------- UnityMessage
    private void Start()
    {
        Tasks(destroyCancellationToken).Forget();
    }

    private async UniTask Tasks(CancellationToken ct)
    {
        //  �ړ��o�R�n������
        Vector3[] positions = new Vector3[_pos.Length];
        for (int i = 0; i < _pos.Length; i++)
        {
            positions[i] = _pos[i].position;
        }

        //  �ҋ@
        await Helper.Tasks.DelayTime(_waitTime, ct);

        //  �ړ�����
        await transform.DOPath
                (positions
                , _time
                , _pathType
                , PathMode.Sidescroller2D)
                .SetEase(Ease.Linear)
                .SetLoops(-1, _loopType)
                .SetOptions(_setOption)
                .SetLink(gameObject)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
    }
}
