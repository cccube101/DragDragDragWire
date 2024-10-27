using Alchemy.Inspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonAnimation : UIAnimatorBase, IUIAnimation
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�p�����[�^")] private TMP_Text _text;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private float _pressedTextPos;

    [SerializeField, Required, BoxGroup("�m�[�}��")] private Sprite _normalImage;

    [SerializeField, Required, BoxGroup("�n�C���C�g")] private Sprite _highlightImage;
    [SerializeField, Required, BoxGroup("�n�C���C�g")] private UnityEvent _highlightClip;

    [SerializeField, Required, BoxGroup("�v���X")] private Sprite _pressedImage;
    [SerializeField, Required, BoxGroup("�v���X")] private UnityEvent _pressedClip;

    // ---------------------------- Field
    //  �A�j���[�V����
    private bool _isPlayPressClip;
    private Vector3 _initPos;


    // ---------------------------- UnityMessage
    public override void Awake()
    {
        StartEvent();

        //  �e�L�X�g�����ʒu�ۑ�
        _initPos = _text.rectTransform.anchoredPosition;
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
        UpdateAnimation
            (Color.black
            , _initPos
            , _highlightClip
            , _highlightImage);
        _isPlayPressClip = false;

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
    /// <param name="textColor">�e�L�X�g�F</param>
    /// <param name="textPos">�e�L�X�g�ʒu</param>
    /// <param name="clip">���ʉ�</param>
    /// <param name="image">�ύX�C���[�W</param>
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
