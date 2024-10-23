using Alchemy.Inspector;
using DG.Tweening;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    [SerializeField, Required, BoxGroup("�G�t�F�N�g")] private float _scale;
    [SerializeField, Required, BoxGroup("�G�t�F�N�g")] private float _duration;

    private void Start()
    {
        transform.DOScale
            (_scale, _duration)
            .SetEase(Ease.OutBack)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(gameObject);
    }
}
