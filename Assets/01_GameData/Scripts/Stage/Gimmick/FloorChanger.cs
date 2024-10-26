using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class FloorChanger : GimmickBase
{
    // ---------------------------- SerializeField
    [SerializeField] private GameObject[] _floorsObjects;
    [SerializeField] private float _waitTime;
    [SerializeField] private float _duration;
    [SerializeField] private int _loopTime;
    [SerializeField] private Color _toColor;
    [SerializeField] private Color _endColor;
    [SerializeField] private UnityEvent _alertClip;

    // ---------------------------- Field
    private readonly Dictionary<GameObject, Tilemap> _floors = new();


    // ---------------------------- UnityMessage
    private async void Start()
    {
        //  �I�u�W�F�N�g�ɕR�Â����^�C���}�b�v�̃L���b�V��
        foreach (var obj in _floorsObjects)
        {
            _floors.Add(obj, obj.GetComponent<Tilemap>());
            obj.SetActive(false);
        }
        //  ���X�g�̏��߂̕����̂݃A�N�e�B�u��
        _floorsObjects[0].SetActive(true);

        //  �t���A�؊����J�n
        await Helper.Tasks.Canceled(StartEvent(destroyCancellationToken));
    }

    // ---------------------------- PrivateMethod
    /// <summary>
    /// �J�n
    /// </summary>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�J�n����</returns>
    private async UniTask StartEvent(CancellationToken ct)
    {
        while (true)
        {
            //  �����t���A�̐؊���
            foreach (var floor in _floors)
            {
                floor.Key.SetActive(true);

                //  ������悤�ɃX�v���C�g�̐F��߂�
                await DOVirtual.Color(_endColor, _toColor, _duration, (color) =>
                {
                    floor.Value.color = color;
                })
                .SetEase(Ease.Linear)
                .SetLink(floor.Key)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);

                //  �؊����܂őҋ@
                await Helper.Tasks.DelayTime(_waitTime, ct);

                //  �؊�������
                var tasks = new List<UniTask>()
                {
                    Fade(),
                    PlayClip(),
                };
                async UniTask Fade()
                {
                    //  �w��񐔃A���[�g�ɍ��킹�F���t�F�[�h
                    await DOVirtual.Color(_toColor, _endColor, _duration, (color) =>
                        {
                            floor.Value.color = color;
                        })
                        .SetEase(Ease.Linear)
                        .SetLoops(_loopTime, LoopType.Yoyo)
                        .SetLink(floor.Key)
                        .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
                }
                async UniTask PlayClip()
                {
                    //  �w��񐔃A���[�g���Đ�
                    for (var i = 0; i < _loopTime / 2 + 1; i++)
                    {
                        _alertClip?.Invoke();
                        await Helper.Tasks.DelayTime(_duration * 2, ct);
                    }
                }
                await UniTask.WhenAll(tasks);

                //  �������v���C���[�̃t�b�N������L�����Z��
                PlayerController.Instance.ShotPhase = UnityEngine.InputSystem.InputActionPhase.Canceled;
                //  �t���A���A�N�e�B�u��
                floor.Key.SetActive(false);
            }

            await UniTask.Yield(cancellationToken: ct);
        }
    }
}
