using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Alchemy.Inspector;
using Helper;


public class AudioManager : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�p�����[�^")] private AudioMixer _mixer;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private Slider _masterSlider;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private Slider _bgmSlider;
    [SerializeField, Required, BoxGroup("�p�����[�^")] private Slider _seSlider;

    // ---------------------------- UnityMessage
    private void Start()
    {
        if (!Audio.Params.TryGetValue(Audio.MASTER, out var value))
        {
#if UNITY_EDITOR
            Audio.CreateParam();   //  �f�o�b�O�p�Ƀp�����[�^��������
            Debug.Log("AudioParam����");
#else
            //  �f�[�^��������Ȃ��ꍇ�͏������V�[���Ɉړ�
            SceneManager.LoadScene((int)SceneName.BaseInit);
#endif
        }

        //  �p�����[�^�ɃX���C�_�[��ۑ�
        SetParam(Audio.MASTER, _masterSlider);
        SetParam(Audio.BGM, _bgmSlider);
        SetParam(Audio.SE, _seSlider);

        void SetParam(string group, Slider slider)
        {
            var volume = Audio.Params[group].Volume;

            slider.value = volume;  //  �X���C�_�[
            Audio.Params[group].Bar = slider;
            _mixer.SetFloat(group, ConvertVolumeToDb(volume));  //  �~�L�T�[
        }
    }


    // ---------------------------- PublicMethod
    #region ------ ChangeSliderVolume
    /// <summary>
    /// �}�X�^�[�{�����[���ύX
    /// </summary>
    /// <param name="value"></param>
    public void ChangeValue_Master(float value)
    {
        ChangeValue(Audio.MASTER, value);
    }

    /// <summary>
    /// BGM�{�����[���ύX
    /// </summary>
    /// <param name="value"></param>
    public void ChangeValue_BGM(float value)
    {
        ChangeValue(Audio.BGM, value);
    }

    /// <summary>
    /// SE�{�����[���ύX
    /// </summary>
    /// <param name="value"></param>
    public void ChangeValue_SE(float value)
    {
        ChangeValue(Audio.SE, value);
    }

    #endregion ---




    // ---------------------------- PrivateMethod
    /// <summary>
    /// �{�����[���ύX
    /// </summary>
    /// <param name="group"></param>
    /// <param name="value"></param>
    private void ChangeValue(string group, float value)
    {
        Audio.Params[group].Volume = value;
        _mixer.SetFloat(group, ConvertVolumeToDb(value));
    }

    /// <summary>
    /// �X���C�_�[�̒l���~�L�T�[�p�ɒ���(Volume -> Db)
    /// </summary>
    /// <param name="volume"></param>
    /// <returns></returns>
    private float ConvertVolumeToDb(float volume)
    {
        return Mathf.Clamp(Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)) * 20f, -80f, 20f);
    }
}
