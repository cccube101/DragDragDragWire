using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField, Required, BoxGroup("�I�[�f�B�I")] private AudioSource _source;
    [SerializeField, Required, BoxGroup("�I�[�f�B�I")] private AudioClip[] _clips;

    private GameObject _obj = null;

    private async void Start()
    {
        _obj = gameObject;  //  �L���b�V��

        //  ���ʉ��Đ�
        foreach (var clip in _clips)
        {
            _source.PlayOneShot(clip);
        }

        //  �Đ����ҋ@
        await Helper.Tasks.Canceled(UniTask.WaitUntil(
            () => !_source.isPlaying, cancellationToken: destroyCancellationToken));

        //  �폜
        if (_obj != null)
        {
            Destroy(gameObject);
        }
    }
}
