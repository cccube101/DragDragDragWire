using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using static PlayerController;

public class GuideController : MonoBehaviour
{
    private enum Tasks
    {
        MOUSE_LEFT,
        MOUSE_RIGHT
    }
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�}�E�X")] private GameObject _mouse;
    [SerializeField, Required, BoxGroup("�}�E�X")] private Transform[] _mouseMoveRightPos;
    [SerializeField, Required, BoxGroup("�}�E�X")] private Transform[] _mouseMoveLeftPos;
    [SerializeField, Required, BoxGroup("�}�E�X")] private int _mouseMoveCycle;
    [SerializeField, Required, BoxGroup("�}�E�X")] private float _mouseMoveDuration;

    [SerializeField, Required, BoxGroup("���")] private GameObject _mouseArrow;
    [SerializeField, Required, BoxGroup("���")] private float _endAngle;

    [SerializeField, Required, BoxGroup("���C��")] private LineRenderer[] _lines;
    [SerializeField, Required, BoxGroup("���C��")] private Transform[] _lineEndPos;
    [SerializeField, Required, BoxGroup("���C��")] private float _lineDuration;

    [SerializeField, Required, BoxGroup("���C���}�E�X")] private GameObject _lineMouse;
    [SerializeField, Required, BoxGroup("���C���}�E�X")] private float _lienMouseSize;
    [SerializeField, Required, BoxGroup("���C���}�E�X")] private float _lienMouseDuration;
    [SerializeField, Required, BoxGroup("���C���}�E�X")] private Sprite[] _lineMouseSprite;

    // ---------------------------- Field
    private CancellationTokenSource cts;

    private Vector3[] _mouseMoveRightPosValue;
    private Vector3[] _mouseMoveLeftPosValue;
    private readonly float HALF_ANGLE = 180;

    private Vector3[] _edge;
    private Vector3[] _currentEdge;
    private float _mouseSize;


    // ---------------------------- UnityMessage
    private async void Start()
    {
        cts = new();

        //  �ʒu�l�ϊ�
        _mouseMoveRightPosValue = InitPos(_mouseMoveRightPos);
        _mouseMoveLeftPosValue = InitPos(_mouseMoveLeftPos);

        //  �ʒu�l�ϊ�
        _edge = InitPos(_lineEndPos);
        _currentEdge = new Vector3[]
        {
            _lineEndPos[(int)LinePos.ORIGIN].position,
            _lineEndPos[(int)LinePos.ORIGIN].position
        };
        _mouseSize = _lineMouse.transform.localScale.x;


        var tasks = new List<UniTask>()
        {
            Canceled(MouseAnime()),
            Canceled(LineAnime())
        };
        await Canceled(UniTask.WhenAll(tasks));
    }

    private void Update()
    {
        //  ���C���`��
        _lines[(int)LineType.ROPE].SetPositions(_currentEdge);
        _lines[(int)LineType.INDICATOR].SetPositions(_edge);
    }

    public UnityEvent OnDestroyed = new();
    private void OnDestroy()
    {
        cts.Cancel();
        OnDestroyed.Invoke();
    }


    // ---------------------------- PrivateMethod
    /// <summary>
    /// �ʒu�z�񏉊���
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private Vector3[] InitPos(Transform[] pos)
    {
        var value = new Vector3[pos.Length];
        for (int i = 0; i < pos.Length; i++)
        {
            value[i] = pos[i].position;
        }
        return value;
    }


    #region ------ MouseAnime
    /// <summary>
    /// �}�E�X�A�j���[�V����
    /// </summary>
    /// <returns></returns>
    private async UniTask MouseAnime()
    {
        while (true)
        {
            var leftTasks = new List<UniTask>
            {
                Canceled(LoopProcess(_mouseMoveCycle, Tasks.MOUSE_LEFT)),
                Canceled(AngleChange(_mouseArrow, HALF_ANGLE + _endAngle, _mouseMoveDuration))
            };
            await Canceled(UniTask.WhenAll(leftTasks));

            var rightTasks = new List<UniTask>
            {
                Canceled(LoopProcess(_mouseMoveCycle, Tasks.MOUSE_RIGHT)),
                Canceled(AngleChange(_mouseArrow, HALF_ANGLE - _endAngle, _mouseMoveDuration))
            };
            await Canceled(UniTask.WhenAll(rightTasks));

            await UniTask.Yield(cancellationToken: cts.Token);
        }
    }


    /// <summary>
    /// ���[�v����
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    private async UniTask LoopProcess(int count, Tasks type)
    {
        while (count > 0)
        {
            count--;
            switch (type)
            {
                case Tasks.MOUSE_LEFT:
                    await Canceled(PathMove
                        (_mouse
                        , _mouseMoveLeftPosValue
                        , _mouseMoveDuration));
                    break;

                case Tasks.MOUSE_RIGHT:
                    await Canceled(PathMove
                        (_mouse
                        , _mouseMoveRightPosValue
                        , _mouseMoveDuration));
                    break;
            }
            await UniTask.Yield(cancellationToken: cts.Token);
        }
    }

    /// <summary>
    /// �p�X�ړ�
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="pos"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private async UniTask PathMove
        (GameObject obj
        , Vector3[] pos
        , float duration)
    {
        await obj.transform.DOPath
        (pos
        , duration
        , PathType.CatmullRom)
        .SetEase(Ease.Linear)
        .SetOptions(true)
        .SetLink(obj)
        .ToUniTask(cancellationToken: cts.Token);
    }

    /// <summary>
    /// �p�x�ύX
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="endValue"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private async UniTask AngleChange
        (GameObject obj
        , float endValue
        , float duration)
    {
        var angle = obj.transform.eulerAngles;
        await DOVirtual.Float
            (angle.z, endValue, duration
            , (value) =>
            {
                obj.transform.eulerAngles = new Vector3(angle.x, angle.y, value);
            })
            .SetEase(Ease.Linear)
            .SetLink(obj)
            .ToUniTask(cancellationToken: cts.Token);
    }

    #endregion


    #region ------ LineAnime
    /// <summary>
    /// ���C���A�j���[�V����
    /// </summary>
    /// <returns></returns>
    private async UniTask LineAnime()
    {
        while (true)
        {
            _currentEdge[(int)LinePos.HEAD] = _edge[(int)LinePos.HEAD];
            if (_lineMouse != null)
            {
                _lineMouse.GetComponent<SpriteRenderer>().sprite = _lineMouseSprite[1];
            }

            var useTasks = new List<UniTask>
            {
                Canceled(LineMouseControl(_mouseSize )),
                Canceled(UniTask.Delay(TimeSpan.FromSeconds(_lineDuration / 2),cancellationToken: cts.Token))
            };
            await Canceled(UniTask.WhenAll(useTasks));

            _currentEdge[(int)LinePos.HEAD] = _edge[(int)LinePos.ORIGIN];
            if (_lineMouse != null)
            {
                _lineMouse.GetComponent<SpriteRenderer>().sprite = _lineMouseSprite[0];
            }

            var unUseTasks = new List<UniTask>
            {
                Canceled(LineMouseControl(_mouseSize* _lienMouseSize)),
                Canceled(UniTask.Delay(TimeSpan.FromSeconds(_lineDuration / 2), cancellationToken : cts.Token))
            };
            await Canceled(UniTask.WhenAll(unUseTasks));

            await UniTask.Yield(cancellationToken: cts.Token);
        }
    }

    /// <summary>
    /// �}�E�X�A�j���[�V����
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    private async UniTask LineMouseControl(float size)
    {
        if (_lineMouse != null)
        {
            await _lineMouse.transform.DOScale
                    (size, _lienMouseDuration)
                    .SetEase(Ease.Linear)
                    .SetLink(_lineMouse)
                    .ToUniTask(cancellationToken: cts.Token);
        }
    }

    #endregion

    /// <summary>
    /// UniTask�L�����Z������
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    private async UniTask Canceled(UniTask task)
    {
        if (await task.SuppressCancellationThrow()) { return; }
    }
}
