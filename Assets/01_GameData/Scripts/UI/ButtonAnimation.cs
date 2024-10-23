using Alchemy.Inspector;
using R3;
using R3.Triggers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("��b�p�����[�^")] private UnityEvent[] _event;
    [SerializeField, Required, BoxGroup("��b�p�����[�^")] private TMP_Text _text;
    [SerializeField, Required, BoxGroup("��b�p�����[�^")] private float _pressedTextPos;

    [SerializeField, Required, BoxGroup("�m�[�}��")] private Sprite _normalImage;

    [SerializeField, Required, BoxGroup("�n�C���C�g")] private Sprite _highlightImage;
    [SerializeField, Required, BoxGroup("�n�C���C�g")] private UnityEvent _highlightClip;

    [SerializeField, Required, BoxGroup("�v���X")] private Sprite _pressedImage;
    [SerializeField, Required, BoxGroup("�v���X")] private UnityEvent _pressedClip;

    // ---------------------------- Field
    //  ���s���\�b�h
    private Dictionary<string, UnityEvent> _actions;

    //  �A�j���[�V����
    private bool _isPlayPressClip;
    private Vector3 _initPos;


    // ---------------------------- UnityMessage
    private void Start()
    {
        //  ���C���[���擾
        var layer = GetComponent<Animator>().GetLayerName(0);
        var clips = GetComponent<Animator>().runtimeAnimatorController.animationClips;

        //  ���\�b�h�i�[
        _actions = new Dictionary<string, UnityEvent>(clips.Length);
        for (int i = 0; i < clips.Length; i++)
        {
            //  "���C���[.�X�e�[�g��"
            _actions.Add($"{layer}.{clips[i].name}", _event[i]);
        }

        //  �e�L�X�g�����ʒu�ۑ�
        _initPos = _text.rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        //  �A�j���[�^�[�X�e�[�g�Ď�
        GetComponent<Animator>().GetBehaviour<ObservableStateMachineTrigger>()
            .OnStateEnterAsObservable()
            .Subscribe(state =>
            {
                foreach (var item in _actions)
                {
                    if (state.StateInfo.IsName(item.Key))   //  �X�e�[�g���Ŕ���
                    {
                        _actions[item.Key]?.Invoke();
                    }
                }
            })
            .AddTo(this);
    }

    // ---------------------------- PublicMethod
    #region ------ StateAnimation
    /// <summary>
    /// �m�[�}���C�x���g
    /// </summary>
    public void Normal()
    {
        UpdateAnimation
            (Color.white
            , _initPos
            , null
            , _normalImage);

        _isPlayPressClip = false;
    }

    /// <summary>
    /// �n�C���C�g�C�x���g
    /// </summary>
    public void Highlighted()
    {
        GetComponent<Button>().Select();
        UpdateAnimation
            (Color.black
            , _initPos
            , _highlightClip
            , _highlightImage);
    }

    /// <summary>
    /// �v���X�C�x���g
    /// </summary>
    public void Pressed()
    {
        UpdateAnimation
            (Color.black
            , new Vector3(_initPos.x, _pressedTextPos, _initPos.z)
            , _pressedClip
            , _pressedImage);
        _isPlayPressClip = true;
    }

    /// <summary>
    /// �Z���N�g�C�x���g
    /// </summary>
    public void Selected()
    {

    }

    /// <summary>
    /// �f�B�T�u���C�x���g
    /// </summary>
    public void Disabled()
    {

    }

    #endregion

    // ---------------------------- PrivateMethod
    /// <summary>
    /// �A�j���[�V�����X�V
    /// </summary>
    /// <param name="textColor"></param>
    /// <param name="textPos"></param>
    /// <param name="clip"></param>
    /// <param name="image"></param>
    private void UpdateAnimation
        (Color textColor
        , Vector3 textPos
        , UnityEvent clip
        , Sprite image)
    {
        _text.color = textColor;
        _text.rectTransform.anchoredPosition = textPos;
        if (!_isPlayPressClip)
        {
            clip?.Invoke();
        }
        GetComponent<Image>().sprite = image;
    }
}