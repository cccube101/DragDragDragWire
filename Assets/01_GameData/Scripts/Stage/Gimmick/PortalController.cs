using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�p�����[�^")] private Transform _warpPos;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private float _duration;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private SpriteRenderer _fadeRing;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private float _fadeValue;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private float _scaleValue;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private float _fadeDuration;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private AudioSource _audio;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private AudioClip _clip;


    // ---------------------------- Field
    private static bool _isWarping = false;
    private float _ringScaleInit = 0;


    // ---------------------------- UnityMessage
    private void Start()
    {
        _ringScaleInit = _fadeRing.transform.localScale.x;
        _fadeRing.color = new Color(_color.r, _color.g, _color.b, 0);
    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagName.Player))
        {
            FadeColor(destroyCancellationToken).Forget();

            async UniTask FadeColor(CancellationToken ct)
            {
                List<UniTask> toTasks = new()
                {
                    FadeTask(_fadeValue),
                    ScaleTask(_scaleValue),
                };
                await UniTask.WhenAll(toTasks);

                List<UniTask> endTasks = new()
                {
                    FadeTask(0),
                    ScaleTask(_ringScaleInit),
                };
                await UniTask.WhenAll(endTasks);


                async UniTask FadeTask(float value)
                {
                    await _fadeRing.DOFade(value, _fadeDuration)
                    .SetEase(Ease.OutBack)
                    .SetUpdate(true)
                    .SetLink(gameObject)
                    .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);

                }

                async UniTask ScaleTask(float value)
                {
                    await _fadeRing.transform.DOScale(value, _fadeDuration)
                    .SetEase(Ease.OutBack)
                    .SetUpdate(true)
                    .SetLink(gameObject)
                    .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);

                }
            }

            if (!_isWarping)
            {
                collision.transform.position = _warpPos.position;

                _isWarping = true;

                await Helper.Tasks.Canceled(WarpEvent(destroyCancellationToken));

                async UniTask WarpEvent(CancellationToken ct)
                {
                    _audio.PlayOneShot(_clip);

                    await Helper.Tasks.DelayTime(_duration, ct);
                }

                _isWarping = false;
            }
        }
    }
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�M�Y���p�����[�^")] private Helper.Switch _gizmoSwitch;
    [SerializeField, Required, BoxGroup("�M�Y���p�����[�^")] private Color _color;

#if UNITY_EDITOR
    // ---------------------------- UnityMessage
    void OnDrawGizmos()
    {
        if (_gizmoSwitch == Helper.Switch.ON)
        {
            Gizmos.color = _color;
            Gizmos.DrawWireSphere(transform.position, transform.localScale.y);
        }
    }

#endif
}
