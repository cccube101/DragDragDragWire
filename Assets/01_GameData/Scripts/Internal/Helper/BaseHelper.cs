using UnityEngine;

namespace Helper
{
    // ---------------------------- Enum
    public enum Switch
    {
        ON, OFF
    }
    public enum Scheme
    {
        KeyboardMouse, Gamepad
    }
    public enum GameState
    {
        DEFAULT, PAUSE, GAMECLEAR, GAMEOVER
    }



    /// <summary>
    /// ��b����
    /// </summary>
    public static class Base
    {
        // ---------------------------- Field
        private static (Rect[], GUIStyle) _logParam = GetLogParam();

        // ---------------------------- Property
        public static readonly float HEIGHT = 1080;
        public static readonly float WIDTH = 1920;
        public static (Rect[] pos, GUIStyle style) LogParam => _logParam;



        // ---------------------------- PublicMethod
        /// <summary>
        /// ���O�p�����[�^�擾
        /// </summary>
        /// <returns>���O�p�p�����[�^</returns>
        private static (Rect[], GUIStyle) GetLogParam()
        {
            //  �p�����[�^����
            var pos = new Rect[30];

            //  �ʒu�ۑ�
            for (int i = 0; i < pos.Length; i++)
            {
                pos[i] = new Rect(10, 1080 - i * 30, 300, 30);
            }

            //  �o�̓X�^�C���ۑ�
            var style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 25;


            return (pos, style);

        }
    }
}
