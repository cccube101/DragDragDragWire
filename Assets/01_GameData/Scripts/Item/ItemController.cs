using UnityEngine;
using DG.Tweening;
using Alchemy.Inspector;

public class ItemController : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�X�R�A")] private int _point;

    [SerializeField, Required, BoxGroup("�f�t�H���g�G�t�F�N�g")] private Transform _circle;
    [SerializeField, Required, BoxGroup("�f�t�H���g�G�t�F�N�g")] private Vector3 _afterPos;
    [SerializeField, Required, BoxGroup("�f�t�H���g�G�t�F�N�g")] private Vector3 _afterTransform;
    [SerializeField, Required, BoxGroup("�f�t�H���g�G�t�F�N�g")] private float _duration;
    [SerializeField, Required, BoxGroup("�f�t�H���g�G�t�F�N�g")] private float _turnDuration;

    [SerializeField, Required, BoxGroup("�f�X�g���C�G�t�F�N�g")] private GameObject _destroyEffect;
    [SerializeField, Required, BoxGroup("�f�X�g���C�G�t�F�N�g")] private GameObject _audioPlayer;


    // ---------------------------- Property
    public int Point => _point;


    // ---------------------------- UnityMessage

    private void Start()
    {
        Animation();
    }


    // ---------------------------- PublicMethod
    public void Destroy()
    {
        //  �G�t�F�N�g
        Instantiate(_destroyEffect, transform.position, Quaternion.identity);
        Instantiate(_audioPlayer, transform.position, Quaternion.identity);

        //  �폜
        Destroy(gameObject);
    }




    // ---------------------------- PrivateMethod
    /// <summary>
    /// �A�j���[�V����
    /// </summary>
    private void Animation()
    {
        transform.DOMove(transform.position + _afterPos, _duration)
            .SetEase(Ease.OutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(gameObject);

        transform.DOScale(_afterTransform, _duration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(gameObject);

        _circle.DORotate(new Vector3(0, 360, 0), _turnDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental)
            .SetLink(_circle.gameObject);
    }
}
