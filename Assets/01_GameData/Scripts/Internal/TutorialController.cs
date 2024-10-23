using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Helper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�h���b�O")] private Transform _arrowTr;
    [SerializeField, Required, BoxGroup("�h���b�O")] private Transform[] _dragPathTr;
    [SerializeField, Required, BoxGroup("�h���b�O")] private Transform _dragMouseTr;
    [SerializeField, Required, BoxGroup("�h���b�O")] private int _dragLoops;
    [SerializeField, Required, BoxGroup("�h���b�O")] private float _dragAnimeDuration;
    [SerializeField, Required, BoxGroup("�h���b�O")] private LineRenderer _dragLine;

    [SerializeField, Required, BoxGroup("�V���b�g")] private GameObject _shotMouseNormal;
    [SerializeField, Required, BoxGroup("�V���b�g")] private GameObject _shotMouseClick;
    [SerializeField, Required, BoxGroup("�V���b�g")] private Transform _shotHeadTr;
    [SerializeField, Required, BoxGroup("�V���b�g")] private Transform _shotPlayerTr;
    [SerializeField, Required, BoxGroup("�V���b�g")] private Vector2 _endScale;
    [SerializeField, Required, BoxGroup("�V���b�g")] private float _scaleDuration;
    [SerializeField, Required, BoxGroup("�V���b�g")] private float _shotDuration;
    [SerializeField, Required, BoxGroup("�V���b�g")] private float _moveDuration;
    [SerializeField, Required, BoxGroup("�V���b�g")] private float _waitTime;
    [SerializeField, Required, BoxGroup("�V���b�g")] private LineRenderer _shotRopeLine;

    // ---------------------------- Field
    private readonly float LOOK = 0.8f;

    // ---------------------------- UnityMessage
    private async void Start()
    {
        await Tasks.Canceled(StartEvent(destroyCancellationToken));
    }

    private void Update()
    {
        //  �h���b�O
        LookDragMouse();
        DrawDragLine();

        //  �V���b�g
        DrawShotLine();
    }

    // ---------------------------- PublicMethod





    // ---------------------------- PrivateMethod
    /// <summary>
    /// �J�n�C�x���g
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask StartEvent(CancellationToken ct)
    {
        await UniTask.WaitUntil(() => Decision());
        var tasks = new List<UniTask>()
        {
            DragLoop(ct),
            ShotLoop(ct),
        };
        await UniTask.WhenAll(tasks);
    }


    /// <summary>
    /// �h���b�O�A�j���[�V�������[�v
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask DragLoop(CancellationToken ct)
    {
        var path = _dragPathTr.Select(tr => tr.position).ToArray();
        var reversePath = path.Reverse().ToArray();

        while (true)
        {
            await PathMove(path);
            await PathMove(reversePath);

            await UniTask.Yield(cancellationToken: ct);
        }

        async UniTask PathMove(Vector3[] path)
        {
            await _dragMouseTr.DOPath(path, _dragAnimeDuration, PathType.CatmullRom, PathMode.Sidescroller2D)
                .SetEase(Ease.Linear)
                .SetLoops(_dragLoops, LoopType.Restart)
                .SetOptions(true)
                .SetLink(_dragMouseTr.gameObject)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
        }
    }

    /// <summary>
    /// �����}�E�X�֕����]��
    /// </summary>
    private void LookDragMouse()
    {
        var mousePos = _dragMouseTr.position;
        var dir = Vector3.Lerp(mousePos, _arrowTr.position, LOOK);
        var diff = (mousePos - dir).normalized;
        _arrowTr.rotation = Quaternion.FromToRotation(Vector3.down, diff);
    }

    /// <summary>
    /// ���`��
    /// </summary>
    private void DrawDragLine()
    {
        var headPos = Decision() ? _dragMouseTr.position : _arrowTr.position;
        _dragLine.SetPositions(new Vector3[] { _arrowTr.position, headPos });
    }

    /// <summary>
    /// �V���b�g�A�j���[�V�������[�v
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask ShotLoop(CancellationToken ct)
    {
        var originPlayerPos = _shotPlayerTr.position;
        var originAnchorPos = _shotRopeLine.transform.position;
        while (true)
        {
            await Tasks.DelayTime(_waitTime, ct);
            await Anime(originAnchorPos);
            await Tasks.DelayTime(_waitTime, ct);
            await Anime(originPlayerPos);
        }

        async UniTask Anime(Vector3 endPos)
        {
            var tasks = new List<UniTask>()
            {
                Move(_shotHeadTr,_shotDuration,endPos),
                Move(_shotPlayerTr,_moveDuration,endPos),
                MouseAnime(),
            };
            await UniTask.WhenAll(tasks);
        }
        async UniTask Move(Transform tr, float duration, Vector3 endPos)
        {
            await tr.DOMove(endPos, duration)
                .SetEase(Ease.Linear)
                .SetLink(tr.gameObject)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
        }
        async UniTask MouseAnime()
        {
            _shotMouseNormal.SetActive(false);
            _shotMouseClick.SetActive(true);
            await Scale(_endScale.x);

            await Tasks.DelayTime(_moveDuration - _scaleDuration, ct);

            await Scale(_endScale.y);
            _shotMouseNormal.SetActive(true);
            _shotMouseClick.SetActive(false);
        }
        async UniTask Scale(float endScale)
        {
            await _shotMouseClick.transform.DOScale(endScale, _scaleDuration)
              .SetEase(Ease.OutBack)
              .SetLink(_shotMouseClick)
              .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
        }
    }

    /// <summary>
    /// ���`��
    /// </summary>
    private void DrawShotLine()
    {
        var headPos = Decision() ? _shotHeadTr.position : _shotPlayerTr.position;
        _shotRopeLine.SetPositions(new Vector3[] { _shotPlayerTr.position, headPos });
    }

    /// <summary>
    /// �`�ʉ�
    /// </summary>
    /// <returns></returns>
    private bool Decision()
    {
        var isDefault = GameManager.Instance.State.CurrentValue == GameState.DEFAULT;
        _dragLine.gameObject.SetActive(isDefault);
        _shotRopeLine.gameObject.SetActive(isDefault);
        return isDefault && !Tasks.IsFade;
    }
}
