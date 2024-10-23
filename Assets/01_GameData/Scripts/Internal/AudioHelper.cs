using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Helper
{
    /// <summary>
    /// �����p�����[�^����
    /// </summary>
    public static class Audio
    {
        /// <summary>
        /// �p�����[�^�ۑ��N���X
        /// </summary>
        public class Param
        {
            public Param(float volume, Slider bar)
            {
                Volume = volume;
                Bar = bar;
            }

            public float Volume { get; set; }
            public Slider Bar { get; set; }
        }


        // ---------------------------- Field
        private static readonly Dictionary<string, Param> _params = new();



        // ---------------------------- Property
        public static readonly string MASTER = "Master", BGM = "BGM", SE = "SE";
        public static Dictionary<string, Param> Params => _params;



        // ---------------------------- PublicMethod
        /// <summary>
        /// �p�����[�^�쐬
        /// </summary>
        public static void CreateParam()
        {
            Create(MASTER);
            Create(BGM);
            Create(SE);

            static void Create(string group)
            {
                _params.Add(group, new Param(PlayerPrefs.GetFloat(group), null));

            }
        }


        /// <summary>
        /// ���ʏ�����
        /// </summary>
        /// <param name="mixer"></param>
        public static void InitParam(AudioMixer mixer)
        {
            foreach (var param in _params)
            {
                mixer.GetFloat(param.Key.ToString(), out float value);
                param.Value.Volume = Mathf.Clamp((float)Math.Pow(10, value / 20), 0f, 1f);
            }
        }

        /// <summary>
        /// ���ʕۑ�
        /// </summary>
        public static void SaveVolume()
        {
            foreach (var param in _params)
            {
                PlayerPrefs.SetFloat(param.Key, param.Value.Volume);
            }
        }

        /// <summary>
        /// ���ʎ擾
        /// </summary>
        public static void LoadVolume()
        {
            foreach (var param in _params)
            {
                param.Value.Volume = PlayerPrefs.GetFloat(param.Key);
            }
        }
    }
}