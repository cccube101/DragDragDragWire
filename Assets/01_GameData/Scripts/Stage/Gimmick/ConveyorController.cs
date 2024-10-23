using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.UIElements;

public class ConveyorController : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�p�����[�^")] private float _addSpeed;

    // ---------------------------- Property
    public float GetSpeed() => _addSpeed;




#if UNITY_EDITOR
    // ---------------------------- SerializeField
    [Title("Transform ����� scale �ύX�s�A�ȉ� Inspector ��ŕҏW")]
    [SerializeField, Required, BoxGroup("�X�P�[���p�����[�^")] private Transform _navScale;
    [SerializeField, Required, BoxGroup("�X�P�[���p�����[�^")] private Vector2 _size;

    // ---------------------------- UnityMessage
    private void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall += IsOnValidate;
    }

    // ---------------------------- PrivateMethod
    /// <summary>
    /// �C���X�y�N�^�[���@�X�V
    /// </summary>
    private void IsOnValidate()
    {
        UnityEditor.EditorApplication.delayCall -= IsOnValidate;
        if (this == null) return;
        GetComponent<SpriteRenderer>().size = _size;
        _navScale.localScale = _size;
    }
#endif
}
