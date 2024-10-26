using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Helper;

public class GameManager : MonoBehaviour
{

    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�t�F�[�hSE")] private UnityEvent _fadeClip;

    [SerializeField, Required, BoxGroup("�x�[�X")] private CanvasGroup _baseCanvas;

    [SerializeField, Required, BoxGroup("�{�^��/�I�v�V����")] private Button _btn_optionBack;
    [SerializeField, Required, BoxGroup("�{�^��/�I�v�V����")] private Button _btn_optionRetry;
    [SerializeField, Required, BoxGroup("�{�^��/�I�v�V����")] private Button _btn_optionTitle;

    [SerializeField, Required, BoxGroup("�{�^��/�N���A")] private Button _btn_clearRetry;
    [SerializeField, Required, BoxGroup("�{�^��/�N���A")] private Button _btn_clearTitle;
    [SerializeField, Required, BoxGroup("�{�^��/�N���A")] private Button _btn_clearNext;

    [SerializeField, Required, BoxGroup("�{�^��/�Q�[���I�[�o�[")] private Button _btn_overRetry;
    [SerializeField, Required, BoxGroup("�{�^��/�Q�[���I�[�o�[")] private Button _btn_overTitle;


    // ---------------------------- Field
    private static GameManager _instance;
    private int _points, _coinCount;    //  �X�R�A

    // ---------------------------- ReactiveProperty
    //  �Q�[���X�e�[�g
    private readonly ReactiveProperty<GameState> _state = new();
    public ReadOnlyReactiveProperty<GameState> State => _state;

    //  ����
    private readonly ReactiveProperty<float> _currentTime = new();
    public ReadOnlyReactiveProperty<float> CurrentTime => _currentTime;

    // ---------------------------- Property
    public static GameManager Instance => _instance;

    public float LimitTime => Helper.Data.LimitTime;
    public (int Points, int Count) Score => (_points, _coinCount);





    // ---------------------------- UnityMessage
    private void Awake()
    {
        //  �V���O���g���L���b�V��
        _instance = this;

#if UNITY_EDITOR
        if (Data.ScoreList.Count == 0)
        {
            Data.ScoreInit();
        }
#endif

        //  DOTween�����ݒ�
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 20000, sequencesCapacity: 200);

        //  �t���[�����[�g�Œ�
        Application.targetFrameRate = 60;

        //  ������
        _state.Value = GameState.PAUSE;
        _currentTime.Value = Helper.Data.LimitTime;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 == (int)SceneName.Title)
        {
            _btn_clearNext.gameObject.SetActive(false);
        }

        Tasks.FadeClip = _fadeClip;   //  �t�F�[�hSE�ݒ�

        EventObserver();    //  �C�x���g�Ď�
    }

    private void Update()
    {
        //  �f�t�H���g�X�e�[�^�X���X�V
        if (_state.Value == GameState.DEFAULT)
        {
            _currentTime.Value -= Time.deltaTime;   //  ���ԍX�V
        }
    }



    // ---------------------------- PublicMethod
    /// <summary>
    /// �X�e�[�g�ύX
    /// </summary>
    /// <param name="state">�ύX�������X�e�[�g��</param>
    public void ChangeState(GameState state)
    {
        SetState(state);
    }

    /// <summary>
    /// �X�R�A����
    /// </summary>
    /// <param name="item">�X�R�A</param>
    public void SetScore(int point)
    {
        //  �X�R�A�X�V
        _points += point;
        _coinCount++;
    }


    // ---------------------------- PrivateMethod
    /// <summary>
    /// �C�x���g�Ď�
    /// </summary>
    private void EventObserver()
    {
        //  ------  ���ԃC�x���g
        _currentTime.Subscribe(time =>
        {
            //  �Q�[���I�[�o�[����
            if (time < 0)
            {
                SetState(GameState.GAMEOVER);
            }
        })
        .AddTo(this);

        //  ------  HP
        PlayerController.Instance.HP.Subscribe(hp =>
        {
            //  �Q�[���I�[�o�[����
            if (hp <= 0)
            {
                SetState(GameState.GAMEOVER);
            }
        })
        .AddTo(this);

        //  ------  �{�^���C�x���g
        var current = SceneManager.GetActiveScene().buildIndex;

        //  �f�t�H���g
        _btn_optionBack.OnClickAsObservable()
        .Subscribe(_ =>
        {
            //  �f�t�H���g�ɕύX
            SetState(GameState.DEFAULT);
        })
        .AddTo(this);

        _btn_optionRetry.OnClickAsObservable()
        .SubscribeAwait(async (_, ct) =>
        {
            //  �V�[���J��
            await Tasks.SceneChange(current, _baseCanvas, ct);
        })
        .AddTo(this);

        _btn_optionTitle.OnClickAsObservable()
        .SubscribeAwait(async (_, ct) =>
        {
            //  �V�[���J��
            await Tasks.SceneChange((int)SceneName.Title, _baseCanvas, ct);
        })
        .AddTo(this);


        //  �N���A
        _btn_clearRetry.OnClickAsObservable()
        .SubscribeAwait(async (_, ct) =>
        {
            //  �V�[���J��
            await Tasks.SceneChange(current, _baseCanvas, ct);
        })
        .AddTo(this);

        _btn_clearTitle.OnClickAsObservable()
        .SubscribeAwait(async (_, ct) =>
        {
            //  �V�[���J��
            await Tasks.SceneChange((int)SceneName.Title, _baseCanvas, ct);
        })
        .AddTo(this);

        _btn_clearNext.OnClickAsObservable()
        .SubscribeAwait(async (_, ct) =>
        {
            //  �V�[���J��
            await Tasks.SceneChange(current + 1, _baseCanvas, ct);
        })
        .AddTo(this);


        //  �Q�[���I�[�o�[
        _btn_overRetry.OnClickAsObservable()
        .SubscribeAwait(async (_, ct) =>
        {
            //  �V�[���J��
            await Tasks.SceneChange(current, _baseCanvas, ct);
        })
        .AddTo(this);

        _btn_overTitle.OnClickAsObservable()
        .SubscribeAwait(async (_, ct) =>
        {
            //  �V�[���J��
            await Tasks.SceneChange((int)SceneName.Title, _baseCanvas, ct);
        })
        .AddTo(this);
    }

    /// <summary>
    /// �X�e�[�g�̕ύX
    /// </summary>
    /// <param name="state">�ύX�������X�e�[�g��</param>
    /// <returns>�X�e�[�g�ύX����</returns>
    private void SetState(GameState state)
    {
        //  ����
        switch (state)
        {
            case GameState.DEFAULT:
                Decision(GameState.PAUSE);
                break;

            case GameState.PAUSE:
            case GameState.GAMEOVER:
            case GameState.GAMECLEAR:
                Decision(GameState.DEFAULT);
                break;
        }

        //  ����
        void Decision(GameState decisionValue)
        {
            //  �J�ڒ�������
            //  UI���ړ����Ă��邩�ǂ���
            //  �X�e�[�g���ύX�\�ȃX�e�[�g���ǂ���
            if (!Tasks.IsFade
                && !UIController.Instance.IsMoveMenu
                && _state.Value == decisionValue)
            {
                _state.Value = state;
            }
        }
    }
}
