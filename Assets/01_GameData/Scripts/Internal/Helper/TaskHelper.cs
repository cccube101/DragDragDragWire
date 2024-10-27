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
        public static TweenCancelBehaviour TCB => TweenCancelBehaviour.KillAndCancelAwait;
        public static bool IsFade => _isFade;

        public static UnityEvent FadeClip { set => _fadeClip = value; }



        // ---------------------------- PublicMethod
        #region ------ Fade
        /// <summary>
        /// �t�F�[�h�C��
        /// </summary>
        /// <param name="canvas">�L�����o�X�O���[�v</param>
        /// <param name="ct">�L�����Z���g�[�N��</param>
        /// <returns>�t�F�[�h�C������</returns>
        public static async UniTask FadeIn(CanvasGroup canvas, CancellationToken ct)
        {
            //  UI����u���b�N
            canvas.blocksRaycasts = false;

            //  �t�F�[�h����
            await FadeTask(0, 1, FadeImage.ImageType.FADEIN, ct);
        }

        /// <summary>
        /// �t�F�[�h�A�E�g
        /// </summary>
        /// <param name="ct">�L�����Z���g�[�N��</param>
        /// <returns>�t�F�[�h�A�E�g����</returns>
        public static async UniTask FadeOut(CancellationToken ct)
        {
            //  �t�F�[�h����
            await FadeTask(1, 0, FadeImage.ImageType.FADEOUT, ct);
        }

        /// <summary>
        /// �t�F�[�h
        /// </summary>
        /// <param name="start">�X�^�[�g���摜���</param>
        /// <param name="end">�I�����摜���</param>
        /// <param name="type">�C���[�W�̎w��</param>
        /// <param name="ct">�L�����Z���g�[�N��</param>
        /// <returns>�t�F�[�h����</returns>
        private static async UniTask FadeTask
            (float start
            , float end
            , FadeImage.ImageType type
            , CancellationToken ct)
        {
            //  ���Ԓ�~
            Time.timeScale = 0.0f;
            _isFade = true;

            //  �C���[�W�ύX
            FadeImage.Instance.UpdateMaskTexture(type);
            //  ���ʉ��Đ�
            _fadeClip?.Invoke();
            //  �t�F�[�h�����Đ�
            await Canceled(Fade.Instance.FadeTask(start, end, FADE_TIME, ct));

            //  ���ԍĐ�
            _isFade = false;
            Time.timeScale = 1.0f;
        }

        #endregion

        #region ------ TransitionEvent
        /// <summary>
        /// �V�[���J��
        /// </summary>
        /// <param name="scene">�J�ڐ�V�[��</param>
        /// <param name="canvas">�L�����o�X�O���[�v</param>
        /// <param name="ct">�L�����Z���g�[�N��</param>
        /// <returns>�V�[���J�ڏ���</returns>
        public static async UniTask SceneChange(int scene, CanvasGroup canvas, CancellationToken ct)
        {
            //  �t�F�[�h����
            await FadeIn(canvas, ct);

            //  �V�[���J��
            SceneManager.LoadScene(scene);
            //  ���ԍĐ�
            Time.timeScale = 1.0f;
        }

        /// <summary>
        /// �I��
        /// </summary>
        /// <param name="canvas">�L�����o�X�O���[�v</param>
        /// <param name="ct">�L�����Z���g�[�N��</param>
        /// <returns>�I������</returns>
        public static async UniTask ApplicationQuit(CanvasGroup canvas, CancellationToken ct)
        {
            //  �t�F�[�h����
            await FadeIn(canvas, ct);

            //  ���ʕۑ�
            Audio.SaveVolume();

            //  �I��
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
        /// <param name="time">�ҋ@����</param>
        /// <param name="ct">�L�����Z���g�[�N��</param>
        /// <returns>�ҋ@����</returns>
        public static async UniTask DelayTime(float time, CancellationToken ct)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(time), true, cancellationToken: ct);
        }

        /// <summary>
        /// UniTask�L�����Z��
        /// </summary>
        /// <param name="task">�L�����Z���������^�X�N</param>
        /// <returns>�L�����Z������</returns>
        public static async UniTask Canceled(UniTask task)
        {
            if (await task.SuppressCancellationThrow()) { return; }
        }
    }
}
