using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Helper
{
    /// <summary>
    /// �^�X�N����
    /// </summary>
    public static class Tasks
    {
        // ---------------------------- Field
        private static UnityEvent _fadeClip = null;

        private static readonly float FADE_TIME = 2;
        private static bool _isFade = false;
        // ---------------------------- Property
        public static bool IsFade => _isFade;

        public static UnityEvent FadeClip { set => _fadeClip = value; }



        // ---------------------------- PublicMethod
        #region ------ Fade
        /// <summary>
        /// �t�F�[�h�C��
        /// </summary>
        /// <returns></returns>
        public static async UniTask FadeIn(CanvasGroup canvas, CancellationToken ct)
        {
            canvas.blocksRaycasts = false;
            await FadeTask(0, 1, FadeImage.ImageType.FADEIN, ct);
        }

        /// <summary>
        /// �t�F�[�h�A�E�g
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async UniTask FadeOut(CancellationToken ct)
        {
            await FadeTask(1, 0, FadeImage.ImageType.FADEOUT, ct);
        }

        /// <summary>
        /// �t�F�[�h����
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="type"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private static async UniTask FadeTask
            (float start
            , float end
            , FadeImage.ImageType type
            , CancellationToken ct)
        {
            Time.timeScale = 0.0f;
            _isFade = true;

            FadeImage.Instance.UpdateMaskTexture(type);
            _fadeClip?.Invoke();
            await Canceled(Fade.Instance.FadeTask(start, end, FADE_TIME, ct));

            _isFade = false;
            Time.timeScale = 1.0f;
        }

        #endregion

        #region ------ TransitionEvent
        /// <summary>
        /// �V�[���ύX
        /// </summary>
        /// <param name="scene"></param>
        public static async UniTask SceneChange(int scene, CanvasGroup canvas, CancellationToken ct)
        {
            await FadeIn(canvas, ct);

            SceneManager.LoadScene(scene);
            Time.timeScale = 1.0f;
        }

        /// <summary>
        /// �I��
        /// </summary>
        /// <returns></returns>
        public static async UniTask ApplicationQuit(CanvasGroup canvas, CancellationToken ct)
        {
            await FadeIn(canvas, ct);

            Audio.SaveVolume();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; //�Q�[���V�[���I��
#else
        Application.Quit(); //build��ɃQ�[���v���C�I�����K�p
#endif
        }

        #endregion

        /// <summary>
        /// �ҋ@
        /// </summary>
        /// <param name="time"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async UniTask DelayTime(float time, CancellationToken ct)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(time), true, cancellationToken: ct);
        }

        /// <summary>
        /// UniTask�L�����Z������
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async UniTask Canceled(UniTask task)
        {
            if (await task.SuppressCancellationThrow()) { return; }
        }
    }
}
