using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Events;

using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using Alchemy.Inspector;
using R3;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Helper;

public class UIController : MonoBehaviour
{
    private class LogoParam
    {
        public LogoParam(GameObject obj, RectTransform rect, TMP_Text text, Vector2 scale)
        {
            Obj = obj;
            Rect = rect;
            Text = text;
            Scale = scale;
        }
        public GameObject Obj { get; set; }
        public RectTransform Rect { get; set; }
        public TMP_Text Text { get; set; }

        public Vector2 Scale { get; set; }
    }

    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�{�^��")] private Button _pauseButton = null;
    [SerializeField, Required, BoxGroup("�{�^��")] private Button _clearButton = null;
    [SerializeField, Required, BoxGroup("�{�^��")] private Button _overButton = null;

    [SerializeField, Required, BoxGroup("�L�����o�X")] private CanvasGroup _frame;
    [SerializeField, Required, BoxGroup("�L�����o�X")] private GameObject _defaultFrame;
    [SerializeField, Required, BoxGroup("�L�����o�X")] private GameObject _optionFrame;
    [SerializeField, Required, BoxGroup("�L�����o�X")] private float _optionDuration;

    [SerializeField, Required, BoxGroup("�X�^�[�g")] private TMP_Text _stageNameText;
    [SerializeField, Required, BoxGroup("�X�^�[�g")] private float _waitStageNameFade;
    [SerializeField, Required, BoxGroup("�X�^�[�g")] private float _stageNameFadeDuration;

    [SerializeField, Required, BoxGroup("UI")] private TMP_Text _timeText;
    [SerializeField, Required, BoxGroup("UI")] private GameObject[] _hpObjects;
    [SerializeField, Required, BoxGroup("UI")] private GameObject _guide;
    [SerializeField, Required, BoxGroup("UI")] private Image _guideImage;
    [SerializeField, Required, BoxGroup("UI")] private Sprite[] _guideSprite;

    [SerializeField, Required, BoxGroup("�A���[�g")] private Color _originColor;
    [SerializeField, Required, BoxGroup("�A���[�g")] private Color _heartAlertColor;
    [SerializeField, Required, BoxGroup("�A���[�g")] private Color _alertColor;
    [SerializeField, Required, BoxGroup("�A���[�g")] private Image _alertPanel;
    [SerializeField, Required, BoxGroup("�A���[�g")] private float _alertHpSize;
    [SerializeField, Required, BoxGroup("�A���[�g")] private float _alertHpDuration;
    [SerializeField, Required, BoxGroup("�A���[�g")] private float _alertTextSize;
    [SerializeField, Required, BoxGroup("�A���[�g")] private float _alertEndValue;
    [SerializeField, Required, BoxGroup("�A���[�g")] private float _alertTime;
    [SerializeField, Required, BoxGroup("�A���[�g")] private float _alertDuration;


    [SerializeField, Required, BoxGroup("���U���g")] private GameObject _resultFrame;
    [SerializeField, Required, BoxGroup("���U���g")] private float _initResultUIPos;
    [SerializeField, Required, BoxGroup("���U���g")] private Image _resultPanel;
    [SerializeField, Required, BoxGroup("���U���g")] private float _resultPanelAlpha;
    [SerializeField, Required, BoxGroup("���U���g")] private GameObject _clearLogo;
    [SerializeField, Required, BoxGroup("���U���g")] private GameObject _gameOverLogo;
    [SerializeField, Required, BoxGroup("���U���g")] private float _logoDuration;
    [SerializeField, Required, BoxGroup("���U���g")] private Vector2 _gameClearLogSize;
    [SerializeField, Required, BoxGroup("���U���g")] private Vector2 _gameOverLogSize;
    [SerializeField, Required, BoxGroup("���U���g")] private float _sizeDuration;

    [SerializeField, Required, BoxGroup("���U���g")] private GameObject[] _coinObjects;
    [SerializeField, Required, BoxGroup("���U���g")] private GameObject[] _heartObjects;
    [SerializeField, Required, BoxGroup("���U���g")] private float _fadeBgmVolume;
    [SerializeField, Required, BoxGroup("���U���g")] private Vector3 _timeScoreLimit;
    [SerializeField, Required, BoxGroup("���U���g")] private Vector3 _timeScore;
    [SerializeField, Required, BoxGroup("���U���g")] private float _resultWait;
    [SerializeField, Required, BoxGroup("���U���g")] private RectTransform[] _clearUI;
    [SerializeField, Required, BoxGroup("���U���g")] private RectTransform[] _gameOverUI;
    [SerializeField, Required, BoxGroup("���U���g")] private float _resultUIDuration;
    [SerializeField, Required, BoxGroup("���U���g")] private float _resultUIShifting;
    [SerializeField, Required, BoxGroup("���U���g")] private TMP_Text _timeResultText;
    [SerializeField, Required, BoxGroup("���U���g")] private TMP_Text _coinScoreText;
    [SerializeField, Required, BoxGroup("���U���g")] private TMP_Text _timeScoreText;
    [SerializeField, Required, BoxGroup("���U���g")] private TMP_Text _hpScoreText;
    [SerializeField, Required, BoxGroup("���U���g")] private TMP_Text _scoreText;
    [SerializeField, Required, BoxGroup("���U���g")] private float _scoreDuration;


    [SerializeField, Required, BoxGroup("�I�[�f�B�I")] private AudioSource _bgmSource;
    [SerializeField, Required, BoxGroup("�I�[�f�B�I")] private UnityEvent _resultClips;
    [SerializeField, Required, BoxGroup("�I�[�f�B�I")] private UnityEvent _scoreClip;
    [SerializeField, Required, BoxGroup("�I�[�f�B�I")] private UnityEvent _gameOverClip;
    [SerializeField, Required, BoxGroup("�I�[�f�B�I")] private UnityEvent _alertClip;


    // ---------------------------- Field
    private static UIController _instance;

    //  �{�^��
    private Dictionary<GameState, Button> _selectButtons;

    //  �t���[��
    private RectTransform _optionFrameRect = null;

    //  �V�[���J��
    private bool _isMoveMenu = false;
    private bool _isGameStart = false;

    //  �R�C��
    private readonly int MAX_COIN = 3;
    private readonly int GET_HUNDRED = 100;

    // �A���[�g
    private bool _isAlert = false;

    //  ���U���g
    private LogoParam _gameClearParam = null;
    private LogoParam _gameOverParam = null;
    private readonly List<float> _clearUIPos = new();
    private readonly List<float> _gameOverUIPos = new();

    // ---------------------------- Property
    public static UIController Instance => _instance;
    public bool IsMoveMenu => _isMoveMenu;



    // ---------------------------- UnityMessage
    private void Awake()
    {
        _instance = this;
    }

    private async void Start()
    {
        //  �p�����[�^�ۑ�
        ImplementParam();

        //  �C�x���g�Ď�
        BaseEventObserve();
        PlayerEventObserve();

        //  �X�^�[�g�C�x���g
        await Tasks.Canceled(StartEvent(this.GetCancellationTokenOnDestroy()));
    }

    // ---------------------------- PublicMethod



    // ---------------------------- PrivateMethod
    /// <summary>
    /// �p�����[�^�ۑ�
    /// </summary>
    private void ImplementParam()
    {
        //  �{�^���L���b�V��
        _selectButtons = new()
        {
            {GameState.PAUSE, _pauseButton},
            {GameState.GAMECLEAR ,_clearButton},
            {GameState.GAMEOVER, _overButton},
        };

        //  �t���[�����N�g�L���b�V��
        _optionFrameRect = _optionFrame.GetComponent<RectTransform>();

        //  ���S�p�����[�^�L���b�V��
        _gameClearParam = CreateLogoParam(_clearLogo, _gameClearLogSize);
        _gameOverParam = CreateLogoParam(_gameOverLogo, _gameOverLogSize);
        static LogoParam CreateLogoParam(GameObject obj, Vector2 scale)
        {
            return new LogoParam(obj, obj.GetComponent<RectTransform>(), obj.GetComponent<TMP_Text>(), scale);
        }
    }

    /// <summary>
    /// �x�[�X�C�x���g�Ď�
    /// </summary>
    private void BaseEventObserve()
    {
        var Game = GameManager.Instance;

        //  ------  ���ԃC�x���g
        Game.CurrentTime.Subscribe(time =>
        {
            _timeText.text = $"TIME {time:00}";
        })
        .AddTo(this);
        //  �c���Ԃɂ��A���[�g�J�n
        Game.CurrentTime.Where((time) => !_isAlert && time <= _alertTime)
        .SubscribeAwait(async (_, ct) =>
        {
            _isAlert = true;

            //  �A���[�g�񐔎Z�o
            var loopTime = (int)(_alertTime / _alertDuration);

            var tcb = TweenCancelBehaviour.KillAndCancelAwait;  // UniTask�ւ̕ϊ����ɕK�v�Ȑݒ�
            var tasks = new List<UniTask>()
            {
                FadePanel(),
                TextColor(),
                TextScale(),
                PlayClip(),
            };
            //  �p�l���_��
            async UniTask FadePanel()
            {
                await _alertPanel.DOFade(_alertEndValue, _alertDuration)
                    .SetEase(Ease.Linear)
                    .SetUpdate(true)
                    .SetLink(_alertPanel.gameObject)
                    .SetLoops(loopTime, LoopType.Yoyo)
                    .ToUniTask(tcb, cancellationToken: ct);
            }
            //  �e�L�X�g�_��
            async UniTask TextColor()
            {
                await DOVirtual.Color(_timeText.color, _alertColor, _alertDuration,
                    (color) =>
                    {
                        _timeText.color = color;
                    })
                    .SetEase(Ease.Linear)
                    .SetUpdate(true)
                    .SetLink(_timeText.gameObject)
                    .SetLoops(loopTime, LoopType.Yoyo)
                    .ToUniTask(tcb, cancellationToken: ct);
            }
            //  �e�L�X�g�T�C�Y
            async UniTask TextScale()
            {
                await _timeText.GetComponent<RectTransform>().DOScale(_alertTextSize, _alertDuration)
                    .SetEase(Ease.OutExpo)
                    .SetUpdate(true)
                    .SetLink(_timeText.gameObject)
                    .SetLoops(loopTime, LoopType.Yoyo)
                    .ToUniTask(tcb, cancellationToken: ct);
            }
            //  �I�[�f�B�I�Đ�
            async UniTask PlayClip()
            {
                //  ���[�v�񐔕�����
                for (int i = 0; i < loopTime / 2; i++)
                {
                    //  �X�e�[�g����
                    if (Game.State.CurrentValue == GameState.DEFAULT)
                    {
                        //  �Đ�
                        _alertClip?.Invoke();
                    }
                    //  �ҋ@
                    await Tasks.DelayTime(_alertDuration * 2, ct);
                }
            }
            await UniTask.WhenAll(tasks);

            //  �A���[�g�I�����C���[�W�X�V
            //  ���ʉ�ʕ\���O�ɂO�ɖ߂�
            await Fade_Img(_alertPanel, 0, _alertDuration, Ease.Linear, ct);

        }, AwaitOperation.Drop)
        .RegisterTo(destroyCancellationToken);



        //  ------  �X�e�[�g�C�x���g
        Game.State.Where(_ => _isGameStart)
            .SubscribeAwait(async (state, ct) =>
            {
                switch (state)
                {
                    case GameState.DEFAULT:
                        Implement(true);
                        await MoveOptionFrame(false, Base.WIDTH, ct); //  ���j���[�ړ�

                        break;

                    case GameState.PAUSE:
                        Implement(false);
                        await MoveOptionFrame(true, 0, ct); //  ���j���[�ړ�

                        break;

                    case GameState.GAMECLEAR:
                        Implement(false);
                        await ResultStaging(_gameClearParam, _clearUI, _clearUIPos, ct); //  ���U���g�\��

                        break;

                    case GameState.GAMEOVER:
                        Implement(false);
                        await ResultStaging(_gameOverParam, _gameOverUI, _gameOverUIPos, ct);    //  ���U���g�\��

                        break;
                }

                //  �X�e�[�^�X�ݒ�
                void Implement(bool isDefault)
                {
                    _defaultFrame.SetActive(isDefault); //  �t���[���A�N�e�B�u
                    UISelect(); //  �I��UI
                }

            }, AwaitOperation.Drop)
        .RegisterTo(destroyCancellationToken);

    }

    /// <summary>
    /// �v���C���[�C�x���g�Ď�
    /// </summary>
    private void PlayerEventObserve()
    {
        var Player = PlayerController.Instance;

        //  ------  ���͕��@�ύX�C�x���g
        Player.Scheme.Subscribe(scheme =>
        {
            switch (scheme)
            {
                case Scheme.KeyboardMouse:
                    Cursor.visible = true;  //  �J�[�\���\��
                    EventSystem.current.SetSelectedGameObject(null);    //  UI�̃Z���N�g������
                    _guideImage.sprite = _guideSprite[0];

                    break;

                case Scheme.Gamepad:
                    Cursor.visible = false; //  �J�[�\����\��
                    UISelect();
                    _guideImage.sprite = _guideSprite[1];

                    break;
            }
        })
        .AddTo(this);

        //  ------  HP�C�x���g
        //  HP�I�u�W�F�N�g
        Player.HP.SubscribeAwait(async (hp, ct) =>
        {
            //  HP����
            for (int i = 0; i < _hpObjects.Length; i++)
            {
                _hpObjects[i].SetActive(i < hp && i < Player.MaxHP);
            }

            if (hp > 1)
            {
                //  �C���^�X�N
                var inTask = new List<UniTask>();
                HPEvent(inTask, _originColor, _heartAlertColor, _alertHpSize);
                await UniTask.WhenAll(inTask);

                //  �A�E�g�^�X�N
                var outTask = new List<UniTask>();
                HPEvent(outTask, _heartAlertColor, _originColor, 1);
                await UniTask.WhenAll(outTask);


                //  HP�I�u�W�F�N�g�C�x���g
                void HPEvent(List<UniTask> tasks, Color startColor, Color endColor, float endScale)
                {
                    //  �I�u�W�F�N�g�����X�V
                    foreach (var obj in _hpObjects)
                    {
                        if (obj != null)
                        {
                            //  �C���[�W�̐F�ύX
                            var img = obj.GetComponent<Image>();
                            tasks.Add(ColorChange_Img());
                            async UniTask ColorChange_Img()
                            {
                                await DOVirtual.Color(startColor, endColor, _alertHpDuration,
                                     (color) =>
                                     {
                                         img.color = color;
                                     })
                                     .SetEase(Ease.Linear)
                                     .SetUpdate(true)
                                     .SetLink(img.gameObject)
                                     .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
                            }

                            //  �X�P�[���ύX
                            var rect = obj.GetComponent<RectTransform>();
                            tasks.Add(Scale_Rect(rect, endScale, _alertHpDuration, Ease.Linear, ct));
                        }
                    }
                }
            }
            else
            {
                //  HP�c��ቺ���A���[�g����
                var tasks = new List<UniTask>();

                //  �I�u�W�F�N�g�����X�V
                foreach (var obj in _hpObjects)
                {
                    if (obj != null)
                    {
                        tasks.Add(ColorChange(obj));
                        tasks.Add(ScaleChange(obj));
                    }
                }

                async UniTask ColorChange(GameObject obj)
                {
                    await DOVirtual.Color(_originColor, _heartAlertColor, _alertHpDuration,
                        (color) =>
                        {
                            obj.GetComponent<Image>().color = color;
                        })
                        .SetEase(Ease.Linear)
                        .SetUpdate(true)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetLink(obj)
                        .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
                }
                async UniTask ScaleChange(GameObject obj)
                {
                    await obj.GetComponent<RectTransform>().DOScale(_alertHpSize, _alertHpDuration)
                        .SetEase(Ease.Linear)
                        .SetUpdate(true)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetLink(obj)
                        .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
                }
            }

        }, AwaitOperation.Drop)
        .RegisterTo(destroyCancellationToken);
        //  ��ʑS�̂̃p�l��
        Player.HP.SubscribeAwait(async (hp, ct) =>
        {
            //  �C���^�X�N
            await Fade_Img(_alertPanel, _alertEndValue, _alertDuration, Ease.Linear, ct);

            //  �A�E�g�^�X�N
            await Fade_Img(_alertPanel, 0, _alertDuration, Ease.Linear, ct);

        }, AwaitOperation.Drop)
        .RegisterTo(destroyCancellationToken);
    }

    /// <summary>
    /// �J�n�C�x���g
    /// </summary>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�J�n�C�x���g����</returns>
    private async UniTask StartEvent(CancellationToken ct)
    {
        //  �X�e�[�W��
        var scene = SceneManager.GetActiveScene();
        _stageNameText.text = $"No.{scene.buildIndex} {scene.name}";

        //  ���U���gUI�ʒu�ۑ�
        var initTasks = new List<UniTask>();
        SetPos(_clearUI, _clearUIPos);  //  �N���AUI
        SetPos(_gameOverUI, _gameOverUIPos);    //  �Q�[���I�[�o�[UI
        void SetPos(RectTransform[] setPos, List<float> getPos)
        {
            foreach (var rect in setPos)
            {
                getPos.Add(rect.anchoredPosition.y);    //  �ۑ�
                initTasks.Add(InitPos(rect, _initResultUIPos)); //  ������
            }
        }
        await UniTask.WhenAll(initTasks);

        //  �ʒu������
        await InitPos(_gameClearParam.Rect, 0);   //  �N���A���S
        await InitPos(_gameOverParam.Rect, 0);    //  �Q�[���I�[�o�[���S
        async UniTask InitPos(RectTransform rect, float pos)
        {
            await rect.DOAnchorPosY(pos, 0)
                .SetLink(rect.gameObject)
                .SetUpdate(true)
                .ToUniTask(cancellationToken: ct);  //  ��ʒu�ɏ�����
        }

        //  �t�F�[�h�A�E�g
        var fadeTasks = new List<UniTask>()
        {
            Tasks.FadeOut(ct),    //  �t�F�[�h�A�E�g
            StageNameFade(1,ct),
        };

        await UniTask.WhenAll(fadeTasks);

        _isGameStart = true;
        GameManager.Instance.ChangeState(GameState.DEFAULT);

        //  BGM�Đ�
        if (_bgmSource.isActiveAndEnabled)
        {
            _bgmSource.Play();
        }

        //  �X�e�[�W���t�F�[�h�C��
        await StageNameFade(0, ct);
        async UniTask StageNameFade(float endValue, CancellationToken ct)
        {
            await Tasks.DelayTime(_waitStageNameFade, ct);
            await DOVirtual.Float(_stageNameText.alpha, endValue, _stageNameFadeDuration,
                (value) =>
                {
                    _stageNameText.alpha = value;
                })
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .SetLink(_stageNameText.gameObject)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
        }
        _stageNameText.gameObject.SetActive(false);
    }

    /// <summary>
    /// UI�I��
    /// </summary>
    private void UISelect()
    {
        //  �X�L�[���ƃX�e�[�g�Ŕ���
        var scheme = PlayerController.Instance.Scheme.CurrentValue;
        var state = GameManager.Instance.State.CurrentValue;
        //  Scheme����
        if (scheme == Scheme.Gamepad)
        {
            switch (state)
            {
                case GameState.PAUSE:
                case GameState.GAMECLEAR:
                case GameState.GAMEOVER:

                    _selectButtons[state].Select();

                    break;
            }
        }
    }

    /// <summary>
    /// �I�v�V������ʈړ�
    /// </summary>
    /// <param name="isPause">�|�[�Y���</param>
    /// <param name="target">�ړ��ʒu</param>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�I�v�V������ʈړ�����</returns>
    private async UniTask MoveOptionFrame
        (bool isPause
        , float target
        , CancellationToken ct)
    {
        //  �ݒ菉����
        _isMoveMenu = true;
        if (_optionFrame.activeSelf)    //  �J����
        {
            Helper.Audio.SaveVolume();
            BlocksRayCasts(false);  //  �ڐG��
        }
        else
        {
            _optionFrame.SetActive(true);
        }

        //  �ړ�
        await MoveX_Rect(_optionFrameRect, target, _optionDuration, Ease.OutBack, ct);

        //  �ݒ�ύX
        _optionFrame.SetActive(isPause);
        BlocksRayCasts(isPause);  //  �ڐG��
        _isMoveMenu = false;
    }

    /// <summary>
    /// ���U���g���o
    /// </summary>
    /// <param name="logo"></param>
    /// <param name="ui"></param>
    /// <returns></returns>
    private async UniTask ResultStaging
        (LogoParam logo
        , RectTransform[] ui
        , List<float> uiPos
        , CancellationToken ct)
    {
        //  �p�����[�^�擾
        var Game = GameManager.Instance;
        var TimeValue = Game.CurrentTime.CurrentValue;
        var remainingHour = Game.LimitTime - TimeValue;
        var State = Game.State.CurrentValue;
        var (Points, Count) = Game.Score;
        var hp = PlayerController.Instance.HP.CurrentValue;

        //  ------  �ݒ菉����
        _guide.SetActive(false);
        _resultFrame.SetActive(true);
        //  �N���A��UI�\���ݒ�
        if (State == GameState.GAMECLEAR)
        {
            //  �������\��
            for (int i = 0; i < _coinObjects.Length; i++)
            {
                _coinObjects[i].SetActive(i < Count);
            }
            _timeResultText.text = $"{remainingHour:00.00}";

            //  �����\��
            for (int i = 0; i < _heartObjects.Length; i++)
            {
                _heartObjects[i].SetActive(i < hp);
            }
        }
        BlocksRayCasts(false);  //  �ڐG��

        //  ------  SE�Đ�
        switch (State)
        {
            case GameState.GAMECLEAR:
                _resultClips?.Invoke();
                break;

            case GameState.GAMEOVER:
                _gameOverClip?.Invoke();
                break;
        }

        //  ------  ���S���o
        var logoTasks = new List<UniTask>
        {
            Fade_Img(_resultPanel,_resultPanelAlpha,_logoDuration,Ease.Linear,ct),
            Scale_Rect(logo.Rect,1,_logoDuration,Ease.OutBack,ct),
            AudioFade(_bgmSource, _fadeBgmVolume, _logoDuration,ct),

            logo.Text.DOFade(1, _logoDuration)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .SetLink(logo.Obj)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct),
        };

        await UniTask.WhenAll(logoTasks);

        //  ------  �ҋ@����
        await Tasks.DelayTime(_resultWait, ct);

        //  ------  UI�ړ�����
        var uiTasks = new List<UniTask>();
        //  �ړ����Ԃ����X�ɑ���
        //  UI�̐��������炷
        for (int i = 0; i < ui.Length; i++)
        {
            var duration = _resultUIDuration + _resultUIShifting * i;
            uiTasks.Add(MoveY_Rect(ui[i], uiPos[i], duration, Ease.OutBack, ct));
        }
        if (State == GameState.GAMEOVER)
        {
            uiTasks.Add(Scale_Rect(_gameOverParam.Rect, _gameOverLogSize.y, _resultUIDuration, Ease.OutBack, ct));
        }
        await UniTask.WhenAll(uiTasks);

        BlocksRayCasts(true);   //  �ڐG��

        //  ------  ���S�A�j���[�V����
        LogoAnimation().Forget();
        async UniTask LogoAnimation()
        {
            await Scale_Rect(logo.Rect, logo.Scale.x, _sizeDuration, Ease.Linear, ct);
            await logo.Rect.DOScale(logo.Scale.y, _sizeDuration)
                .SetEase(Ease.OutBack)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true)
                .SetLink(logo.Obj)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
        }

        //  ------  �X�R�A�\��
        if (State == GameState.GAMECLEAR)
        {
            //  �X�R�A�Z�o
            var coinScore = Points + BonusPoint(); //  �R�C��
            int BonusPoint()    //  �{�[�i�X
            {
                if (Count == MAX_COIN)
                {
                    return GET_HUNDRED;
                }
                else
                {
                    return 0;
                }
            }
            var timeScore = TimeScore(); //  ����
            int TimeScore()
            {
                //  �w��͈͂ɂ���ăX�R�A�ϓ�
                if (TimeValue > _timeScoreLimit.x)
                {
                    return (int)_timeScore.x;
                }
                else if (TimeValue > _timeScoreLimit.y)
                {
                    return (int)_timeScore.y;
                }
                else if (TimeValue > _timeScoreLimit.z)
                {
                    return (int)_timeScore.z;
                }
                else
                {
                    return 0;
                }
            }

            var hpScore = hp * GET_HUNDRED;  //  HP
            var totalScore = coinScore + timeScore + hpScore;  //  ���v

            //  �X�R�A�ۑ�
            var indexName = ((SceneName)SceneManager.GetActiveScene().buildIndex).ToString();   //  �V�[�����擾
            Data.SaveScore(indexName, Count, hp, remainingHour, totalScore);

            //  �J�E���g
            await CountTask(_coinScoreText, coinScore, ct); //  �R�C��
            await CountTask(_hpScoreText, hpScore, ct);     //  HP
            await CountTask(_timeScoreText, timeScore, ct); //  ����
            await CountTask(_scoreText, totalScore, ct);    //  ���v



            // �J�E���g�^�X�N
            async UniTask CountTask(TMP_Text text, int value, CancellationToken ct)
            {
                _scoreClip?.Invoke();
                await DOVirtual.Int(0, value, _scoreDuration,
                    (value) =>
                    {
                        text.text = value.ToString();
                    })
                    .SetEase(Ease.OutBack)
                    .SetLink(text.gameObject)
                    .SetUpdate(true)
                    .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
            }
        }

        //  ------  �����t�F�[�h�C��
        await AudioFade(_bgmSource, 1, _logoDuration * 2, ct);
    }

    /// <summary>
    /// UI�ڐG����ύX
    /// </summary>
    /// <param name="rayCast"></param>
    private void BlocksRayCasts(bool rayCast)
    {
        if (_frame != null)
        {
            _frame.blocksRaycasts = rayCast;
        }
    }

    #region ------ TweenTask
    /// <summary>
    /// ���N�g�ړ�X
    /// </summary>
    /// <param name="rect">���N�g�g�����X�t�H�[��</param>
    /// <param name="endValue">�ړ��ʒu</param>
    /// <param name="duration">�ړ�����</param>
    /// <param name="ease">�C�[�X</param>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�ړ�����</returns>
    private async UniTask MoveX_Rect
        (RectTransform rect
        , float endValue
        , float duration
        , Ease ease
        , CancellationToken ct)
    {
        await rect.DOAnchorPosX(endValue, duration)
            .SetEase(ease)
            .SetUpdate(true)
            .SetLink(rect.gameObject)
            .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
    }

    /// <summary>
    /// ���N�g�ړ�Y
    /// </summary>
    /// <param name="rect">���N�g�g�����X�t�H�[��</param>
    /// <param name="endValue">�ړ��ʒu</param>
    /// <param name="duration">�ړ�����</param>
    /// <param name="ease">�C�[�X</param>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�ړ�����</returns>
    private async UniTask MoveY_Rect
        (RectTransform rect
        , float endValue
        , float duration
        , Ease ease
        , CancellationToken ct)
    {
        await rect.DOAnchorPosY(endValue, duration)
            .SetEase(ease)
            .SetUpdate(true)
            .SetLink(rect.gameObject)
            .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
    }

    /// <summary>
    /// ���ʃt�F�[�h
    /// </summary>
    /// <param name="source">�ύX�I�[�f�B�I�\�[�X</param>
    /// <param name="endValue">�ύX�l</param>
    /// <param name="duration">�ύX����</param>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>���ʃt�F�[�h����</returns>
    private async UniTask AudioFade
        (AudioSource source
        , float endValue
        , float duration
        , CancellationToken ct)
    {
        await DOVirtual.Float
            (source.volume, endValue, duration
            , (value) =>
            {
                source.volume = value;
            })
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .SetLink(source.gameObject)
            .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
    }

    /// <summary>
    /// �g��k��
    /// </summary>
    /// <param name="rect">���N�g�g�����X�t�H�[��</param>
    /// <param name="endValue">�ύX�l</param>
    /// <param name="duration">�ύX����</param>
    /// <param name="ease">�C�[�X</param>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns>�g�k����</returns>
    private async UniTask Scale_Rect
        (RectTransform rect
        , float endValue
        , float duration
        , Ease ease
        , CancellationToken ct)
    {
        await rect.DOScale(endValue, duration)
                .SetEase(ease)
                .SetUpdate(true)
                .SetLink(rect.gameObject)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
    }

    /// <summary>
    /// �t�F�[�h_Image
    /// </summary>
    /// <param name="img">�ύX�C���[�W</param>
    /// <param name="endValue">�ύX�l</param>
    /// <param name="duration">�ύX����</param>
    /// <param name="ease">�C�[�X</param>
    /// <param name="ct">�L�����Z���g�[�N��</param>
    /// <returns></returns>
    private async UniTask Fade_Img
        (Image img
        , float endValue
        , float duration
        , Ease ease
        , CancellationToken ct)
    {
        await img.DOFade(endValue, duration)
            .SetEase(ease)
            .SetUpdate(true)
            .SetLink(img.gameObject)
            .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
    }

    #endregion
}
