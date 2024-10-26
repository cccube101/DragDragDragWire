using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Helper;
using Alchemy.Inspector;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class TitleManager : MonoBehaviour
{
    private enum State
    {
        TITLE, STAGESELECT
    }

    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�f�o�b�O")] private Helper.Switch _GUI;

    [SerializeField, Required, BoxGroup("�x�[�X")] private CanvasGroup _baseCanvas;

    [SerializeField, Required, BoxGroup("�I�[�f�B�I")] private UnityEvent _fadeClip;
    [SerializeField, Required, BoxGroup("�I�[�f�B�I")] private UnityEvent _pressAnyClip;
    [SerializeField, Required, BoxGroup("�I�[�f�B�I")] private UnityEvent _countClip;
    [SerializeField, Required, BoxGroup("�I�[�f�B�I")] private AudioSource _bgmSource;

    [SerializeField, Required, BoxGroup("���S�A�j���[�V����")] private ParticleSystem _headParticle;
    [SerializeField, Required, BoxGroup("���S�A�j���[�V����")] private RectTransform _lineRect;
    [SerializeField, Required, BoxGroup("���S�A�j���[�V����")] private Vector2 _lineMinMax;
    [SerializeField, Required, BoxGroup("���S�A�j���[�V����")] private float _animeDuration;
    [SerializeField, Required, BoxGroup("���S�A�j���[�V����")] private float _particleDuration;
    [SerializeField, Required, BoxGroup("���S�A�j���[�V����")] private TMP_Text _titleLogo;
    [SerializeField, Required, BoxGroup("���S�A�j���[�V����")] private Color _startColor;
    [SerializeField, Required, BoxGroup("���S�A�j���[�V����")] private Color _toColor;

    [SerializeField, Required, BoxGroup("�L�����o�X�A�j���[�V����")] private CanvasGroup _stageSelectCanvasGroup;
    [SerializeField, Required, BoxGroup("�L�����o�X�A�j���[�V����")] private GameObject _logoCanvas;
    [SerializeField, Required, BoxGroup("�L�����o�X�A�j���[�V����")] private GameObject _stageCanvas;
    [SerializeField, Required, BoxGroup("�L�����o�X�A�j���[�V����")] private float _canvasMoveDuration;
    [SerializeField, Required, BoxGroup("�L�����o�X�A�j���[�V����")] private RectTransform _pressAny;
    [SerializeField, Required, BoxGroup("�L�����o�X�A�j���[�V����")] private float _pressAnySize;
    [SerializeField, Required, BoxGroup("�L�����o�X�A�j���[�V����")] private float _pressAnyDuration;
    [SerializeField, Required, BoxGroup("�L�����o�X�A�j���[�V����")] private float _pressAnyDelay;
    [SerializeField, Required, BoxGroup("�L�����o�X�A�j���[�V����")] private float _pressAnyPushSize;
    [SerializeField, Required, BoxGroup("�L�����o�X�A�j���[�V����")] private float _pressAnyPushDuration;
    [SerializeField, Required, BoxGroup("�L�����o�X�A�j���[�V����")] private ParticleSystem _pressAnyEffect;
    [SerializeField, Required, BoxGroup("�L�����o�X�A�j���[�V����")] private Ease _canvasMoveEase;



    [SerializeField, Required, BoxGroup("���ʃA�j���[�V����")] private RectTransform _volumeFrame;
    [SerializeField, Required, BoxGroup("���ʃA�j���[�V����")] private Vector2 _volumeFramePos;
    [SerializeField, Required, BoxGroup("���ʃA�j���[�V����")] private float _volumeFrameDuration;
    [SerializeField, Required, BoxGroup("���ʃA�j���[�V����")] private Ease _volumeMoveEase;

    [SerializeField, Required, BoxGroup("�X�R�A�{�[�h")] private RectTransform _scoreBoard;
    [SerializeField, Required, BoxGroup("�X�R�A�{�[�h")] private float _scoreBoardScale;
    [SerializeField, Required, BoxGroup("�X�R�A�{�[�h")] private float _scoreBoardDuration;
    [SerializeField, Required, BoxGroup("�X�R�A�{�[�h")] private TMP_Text _totalScoreText;
    [SerializeField, Required, BoxGroup("�X�R�A�{�[�h")] private TMP_Text _averageTimeText;
    [SerializeField, Required, BoxGroup("�X�R�A�{�[�h")] private float _countDuration;

    [SerializeField, Required, BoxGroup("�{�^��")] private Button _backTitleButton;
    [SerializeField, Required, BoxGroup("�{�^��")] private Button _playButton;
    [SerializeField, Required, BoxGroup("�{�^��")] private Button _volumeButton;
    [SerializeField, Required, BoxGroup("�{�^��")] private Button _quitButton;


    [SerializeField, Required, BoxGroup("�I��")] private CanvasGroup _quitCanvas;
    [SerializeField, Required, BoxGroup("�I��")] private Vector2 _quitFadeValue;
    [SerializeField, Required, BoxGroup("�I��")] private float _quitFadeDuration;




    [SerializeField] private FancyScrollView.Scroller _scroller;

    // ---------------------------- Field
    private readonly ReactiveProperty<State> _state = new(State.TITLE);
    private bool _isMoveCanvas = false;

    private RectTransform _logoRect;
    private RectTransform _stageRect;

    private bool _isVolumeFrameUp = false;

    private Tweener _pressAnyDefaultAnime;

    // ---------------------------- Property
    public Vector2 QuitFadeValue => _quitFadeValue;



    // ---------------------------- UnityMessage

    private void Awake()
    {
        //  �t���[�����[�g�Œ�
        Application.targetFrameRate = 60;

#if UNITY_EDITOR
        if (Data.ScoreList.Count == 0)
        {
            Data.ScoreInit();
        }
#endif
    }

    private async void Start()
    {


        ParamImplement();   //  �p�����[�^�ۑ�
        EventObserve(); //  �C�x���g�Ď�

        await Tasks.Canceled(StartEvent(destroyCancellationToken)); //  �X�^�[�g������
    }

    private void OnGUI()
    {
        if (_GUI == Helper.Switch.ON)
        {
            var pos = new Rect[30];
            for (int i = 0; i < pos.Length; i++)
            {
                pos[i] = new Rect(10, 1080 - i * 30, 300, 30);
            }
            var style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 25;

            GUI.TextField(pos[1], $"Focus", style);
        }
    }

    // ---------------------------- PublicMethod
    /// <summary>
    /// �Q�[���J�n
    /// </summary>
    /// <param name="ctx"></param>
    public async void OnAny(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed
            && _state.Value == State.TITLE
            && !_isMoveCanvas)
        {
            await Tasks.DelayTime(0.1f, destroyCancellationToken);
            _state.Value = State.STAGESELECT;
        }
    }

    /// <summary>
    /// �Q�[���I��
    /// </summary>
    /// <param name="ctx"></param>
    public async void OnGameQuit(InputAction.CallbackContext ctx)
    {
        if (_state.Value == State.STAGESELECT
            && !_isMoveCanvas)
        {
            await FadeQuitCanvas(true, _quitFadeValue.x, _quitFadeValue.y, destroyCancellationToken);
        }
    }

    /// <summary>
    /// �Q�[���I���L�����o�X�t�F�[�h����
    /// </summary>
    /// <param name="isOpen"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask FadeQuitCanvas(bool isOpen, float from, float to, CancellationToken ct)
    {
        _isMoveCanvas = isOpen;
        _stageSelectCanvasGroup.blocksRaycasts = !isOpen;
        _quitCanvas.gameObject.SetActive(true);

        await DOVirtual.Float(from, to, _quitFadeDuration, fade =>
        {
            _quitCanvas.alpha = fade;
        })
        .SetEase(Ease.Linear)
        .SetLink(_quitCanvas.gameObject)
        .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);

        _quitCanvas.gameObject.SetActive(isOpen);
    }

    // ---------------------------- PrivateMethod
    /// <summary>
    /// �p�����[�^�ۑ�
    /// </summary>
    private void ParamImplement()
    {
        Tasks.FadeClip = _fadeClip;   //  �t�F�[�hSE�ݒ�

        //  Rect�ۑ�
        _logoRect = _logoCanvas.GetComponent<RectTransform>();
        _stageRect = _stageCanvas.GetComponent<RectTransform>();

    }

    /// <summary>
    /// �C�x���g�Ď�
    /// </summary>
    private void EventObserve()
    {
        //  �X�e�[�g�Ď�
        _state.SubscribeAwait(async (state, ct) =>
        {
            _isMoveCanvas = true;
            switch (state)
            {
                case State.TITLE:
                    _stageSelectCanvasGroup.blocksRaycasts = false;

                    await MoveCanvas(0, -Base.HEIGHT, ct);
                    _stageCanvas.SetActive(false);

                    _stageSelectCanvasGroup.blocksRaycasts = true;

                    break;

                case State.STAGESELECT:
                    _stageSelectCanvasGroup.blocksRaycasts = false;

                    _stageCanvas.SetActive(true);

                    _pressAnyDefaultAnime.Kill();

                    _pressAnyEffect.Play();
                    _pressAnyClip?.Invoke();

                    await _pressAny.DOScale(_pressAnyPushSize, _pressAnyPushDuration)
                            .SetEase(Ease.OutBack)
                            .SetUpdate(true)
                            .SetLink(_pressAny.gameObject)
                            .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);

                    await Tasks.DelayTime(_pressAnyDelay, ct);

                    await MoveCanvas(Base.HEIGHT, 0, ct);

                    //  �X�R�A�\��
                    await ScoreBoardSize(_scoreBoardScale);
                    await CountTask(_totalScoreText, Data.TotalScore, "0", _countDuration, ct);
                    await CountTask(_averageTimeText, Data.AverageTime, "0.00", _countDuration, ct);
                    await Tasks.DelayTime(_scoreBoardDuration, ct);
                    await ScoreBoardSize(1);


                    // �J�E���g�^�X�N
                    async UniTask CountTask(TMP_Text text, float value, string writing, float duration, CancellationToken ct)
                    {
                        _countClip?.Invoke();
                        await DOVirtual.Float(0, value, duration,
                            (value) =>
                            {
                                text.text = value.ToString(writing);
                            })
                            .SetEase(Ease.OutBack)
                            .SetLink(text.gameObject)
                            .SetUpdate(true)
                            .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
                    }
                    //  �X�R�A�{�[�h�T�C�Y
                    async UniTask ScoreBoardSize(float size)
                    {
                        await _scoreBoard.DOScale(size, _scoreBoardDuration)
                            .SetEase(Ease.OutBack)
                            .SetLink(_scoreBoard.gameObject)
                            .SetUpdate(true)
                            .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
                    }

                    _stageSelectCanvasGroup.blocksRaycasts = true;

                    break;
            }
            _isMoveCanvas = false;

        }, AwaitOperation.Drop)
        .RegisterTo(destroyCancellationToken);

        // �{�^���Ď�
        _backTitleButton.OnClickAsObservable()
            .SubscribeAwait(async (_, ct) =>
            {
                if (_state.Value == State.STAGESELECT)
                {
                    Audio.SaveVolume();
                    await MoveY(_volumeFrame.gameObject, _volumeFrame, _volumeFramePos.x, _volumeFrameDuration, _volumeMoveEase, ct);
                    _pressAnyDefaultAnime = PressAnyAnimation();
                    _state.Value = State.TITLE;
                }
            }, AwaitOperation.Drop)
            .RegisterTo(destroyCancellationToken);

        _playButton.OnClickAsObservable()
            .SubscribeAwait(async (_, ct) =>
            {
                //  �w��V�[���ֈڍs
                Audio.SaveVolume();
                await Tasks.SceneChange(_scroller.Index + 1, _baseCanvas, ct);

            }, AwaitOperation.Drop)
            .RegisterTo(destroyCancellationToken);

        _volumeButton.OnClickAsObservable()
            .SubscribeAwait(async (_, ct) =>
            {
                _isVolumeFrameUp = !_isVolumeFrameUp;
                var pos = _isVolumeFrameUp ? _volumeFramePos.y : _volumeFramePos.x;
                await MoveY(_volumeFrame.gameObject, _volumeFrame, pos, _volumeFrameDuration, _volumeMoveEase, ct);

            }, AwaitOperation.Drop)
            .RegisterTo(destroyCancellationToken);

        _quitButton.OnClickAsObservable()
             .SubscribeAwait(async (_, ct) =>
             {
                 await FadeQuitCanvas(true, _quitFadeValue.x, _quitFadeValue.y, ct);

             }, AwaitOperation.Drop)
             .RegisterTo(destroyCancellationToken);
    }

    /// <summary>
    /// �X�^�[�g������
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask StartEvent(CancellationToken ct)
    {
        //  �I�v�V�����ʒu������
        await MoveY(_stageCanvas, _stageRect, -Base.HEIGHT, 0, Ease.Linear, ct);

        //  �t�F�[�h�A�E�g
        await Tasks.FadeOut(ct);
        //  BGM�Đ�
        if (_bgmSource.isActiveAndEnabled)
        {
            _bgmSource.Play();
        }

        //  ���S�F�ύX
        _ = DOVirtual.Color(_startColor, _toColor, _particleDuration, (color) =>
            {
                _titleLogo.color = color;
            })
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true)
            .SetLink(gameObject);

        _pressAnyDefaultAnime = PressAnyAnimation();

        //  �p�[�e�B�N���Đ����[�v
        ParticleLoop(ct).Forget();
        async UniTask ParticleLoop(CancellationToken ct)
        {
            await Helper.Tasks.DelayTime(_particleDuration, ct);
            while (true)
            {
                await _lineRect.DOAnchorPosX(_lineMinMax.y, _animeDuration)
                    .SetEase(Ease.Linear)
                    .SetLink(_lineRect.gameObject)
                    .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);

                _headParticle.Play();

                await Helper.Tasks.DelayTime(_particleDuration, ct);

                await _lineRect.DOAnchorPosX(_lineMinMax.x, _animeDuration)
                    .SetEase(Ease.Linear)
                    .SetLink(_lineRect.gameObject)
                    .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
            }
        }
    }

    /// <summary>
    /// �Ăяo���p�A�j���[�V����
    /// </summary>
    /// <returns></returns>
    private Tweener PressAnyAnimation()
    {
        _pressAny.transform.localScale = Vector3.one;

        return _pressAny.DOScale(_pressAnySize, _pressAnyDuration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true)
            .SetLink(_pressAny.gameObject);
    }

    /// <summary>
    /// �L�����o�X�ړ�
    /// </summary>
    /// <param name="logoPos"></param>
    /// <param name="stagePos"></param>
    /// <returns></returns>
    private async UniTask MoveCanvas(float logoPos, float stagePos, CancellationToken ct)
    {
        var moveTasks = new List<UniTask>()
            {
                MoveY(_logoCanvas,_logoRect, logoPos, _canvasMoveDuration, _canvasMoveEase, ct),
                MoveY(_stageCanvas, _stageRect, stagePos, _canvasMoveDuration, _canvasMoveEase, ct),
            };
        await UniTask.WhenAll(moveTasks);
    }

    /// <summary>
    /// Y���ړ�
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="rect"></param>
    /// <param name="toValue"></param>
    /// <param name="duration"></param>
    /// <param name="ease"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask MoveY
        (GameObject obj
        , RectTransform rect
        , float toValue
        , float duration
        , Ease ease
        , CancellationToken ct)
    {
        await rect.DOAnchorPosY(toValue, duration)
            .SetEase(ease)
            .SetUpdate(true)
            .SetLink(obj)
            .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
    }
}
