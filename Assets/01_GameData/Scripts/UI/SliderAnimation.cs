using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DG.Tweening;
using System.Collections.Generic;
using R3;
using R3.Triggers;
using Alchemy.Inspector;

public class SliderAnimation : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("��b�p�����[�^")] private UnityEvent[] _event;

    [SerializeField, Required, BoxGroup("��b�p�����[�^")] private Image _fillFrameImg;
    [SerializeField, Required, BoxGroup("��b�p�����[�^")] private Image _fillImg;
    [SerializeField, Required, BoxGroup("��b�p�����[�^")] private Image _handleFrameImg;
    [SerializeField, Required, BoxGroup("��b�p�����[�^")] private Image _handleImg;
    [SerializeField, Required, BoxGroup("��b�p�����[�^")] private float _animeDuration;

    [SerializeField, Required, BoxGroup("�m�[�}��")] private Color _normalColor;
    [SerializeField, Required, BoxGroup("�m�[�}��")] private Color _normalFrameColor;

    [SerializeField, Required, BoxGroup("�n�C���C�g")] private Color _highlightColor;
    [SerializeField, Required, BoxGroup("�n�C���C�g")] private Color _highlightFrameColor;
    [SerializeField, Required, BoxGroup("�n�C���C�g")] private UnityEvent _highlightClip;


    // ---------------------------- Field
    //  ���s���\�b�h
    private Dictionary<string, UnityEvent> _actions;

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
    }

    private void OnEnable()
    {
        //  �A�j���[�^�[�X�e�[�g�Ď�
        GetComponent<Animator>().GetBehaviour<ObservableStateMachineTrigger>()
            .OnStateEnterAsObservable()
            .Subscribe(state =>
            {
                //  �A�N�V������������
                foreach (var item in _actions)
                {
                    if (state.StateInfo.IsName(item.Key))   //  �X�e�[�g���Ŕ���
                    {
                        _actions[item.Key].Invoke();    //  ���s
                    }
                }
            })
            .AddTo(this);
    }

    // ---------------------------- PublicMethod
    #region ------ StateAnimation
    /// <summary>
    /// �m�[�}��
    /// </summary>
    public void Normal()
    {
        UpdateAnimation(_normalColor, _normalFrameColor);
    }

    /// <summary>
    /// �n�C���C�g
    /// </summary>
    public void Highlighted()
    {
        UpdateAnimation(_highlightColor, _highlightFrameColor);
        _highlightClip?.Invoke();
        GetComponent<Slider>().Select();
    }

    /// <summary>
    /// �v���X
    /// </summary>
    public void Pressed()
    {

    }

    /// <summary>
    /// �Z���N�g
    /// </summary>
    public void Selected()
    {

    }

    /// <summary>
    /// �f�B�T�u��
    /// </summary>
    public void Disabled()
    {

    }

    #endregion


    // ---------------------------- PrivateMethod

    /// <summary>
    /// �A�j���[�V�����X�V
    /// </summary>
    /// <param name="content">�w�i�F</param>
    /// <param name="frame">�t���[���F</param>
    private void UpdateAnimation(Color content, Color frame)
    {
        ChangeColor(_fillImg, content);
        ChangeColor(_fillFrameImg, frame);
        ChangeColor(_handleImg, content);
        ChangeColor(_handleFrameImg, frame);
    }

    /// <summary>
    /// �F�ύX
    /// </summary>
    /// <param name="img">�ύX��</param>
    /// <param name="toColor">�ύX�F</param>
    private void ChangeColor(Image img, Color toColor)
    {
        DOVirtual.Color
            (img.color, toColor
            , _animeDuration,
            (result) =>
            {
                img.color = result;
            })
            .SetUpdate(true)
            .SetEase(Ease.Linear)
            .SetLink(gameObject);
    }
}
