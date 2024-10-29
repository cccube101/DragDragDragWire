using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DG.Tweening;
using Alchemy.Inspector;

public class SliderAnimation : UIAnimatorBase, IUIAnimation
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�p�����[�^")] private Image _fillFrameImg;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private Image _fillImg;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private Image _handleFrameImg;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private Image _handleImg;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private float _animeDuration;

    [SerializeField, Required, BoxGroup("�m�[�}��")] private Color _normalColor;
    [SerializeField, Required, BoxGroup("�m�[�}��")] private Color _normalFrameColor;

    [SerializeField, Required, BoxGroup("�n�C���C�g")] private Color _highlightColor;
    [SerializeField, Required, BoxGroup("�n�C���C�g")] private Color _highlightFrameColor;
    [SerializeField, Required, BoxGroup("�n�C���C�g")] private UnityEvent _highlightClip;


    // ---------------------------- Field



    // ---------------------------- UnityMessage



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
