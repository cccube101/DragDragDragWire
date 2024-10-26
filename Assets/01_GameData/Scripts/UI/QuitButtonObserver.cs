using R3;
using UnityEngine;
using UnityEngine.UI;
using Helper;
using Alchemy.Inspector;

public class QuitButtonObserver : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�}�l�[�W���[")] private TitleManager _manager;
    [SerializeField, Required, BoxGroup("�}�l�[�W���[")] private CanvasGroup _baseCanvas;
    [SerializeField, Required, BoxGroup("�{�^��")] private Button _quitYesButton;
    [SerializeField, Required, BoxGroup("�{�^��")] private Button _quitNoButton;

    // ---------------------------- Field



    // ---------------------------- UnityMessage
    private void OnEnable()
    {
        _quitYesButton.OnClickAsObservable()
            .SubscribeAwait(async (_, ct) =>
            {
                //  �Q�[���I��
                await Tasks.ApplicationQuit(_baseCanvas, destroyCancellationToken);

            }, AwaitOperation.Drop)
            .RegisterTo(destroyCancellationToken);

        _quitNoButton.OnClickAsObservable()
            .SubscribeAwait(async (_, ct) =>
            {
                //  �I����ʂ����
                var value = _manager.QuitFadeValue;
                await _manager.FadeQuitCanvas(false, value.y, value.x, ct);

            }, AwaitOperation.Drop)
            .RegisterTo(destroyCancellationToken);
    }
}
