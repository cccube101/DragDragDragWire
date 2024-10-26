using Alchemy.Inspector;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("���X�|�[���n�_")] private Transform _pos;

    // ---------------------------- Property
    public Vector3 Pos => _pos.position;


#if UNITY_EDITOR
    //  �����蔻��̉���

    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�M�Y���p�����[�^")] private Helper.Switch _gizmoSwitch;
    [SerializeField, Required, BoxGroup("�M�Y���p�����[�^")] private Color _color;
    [SerializeField, Required, BoxGroup("�M�Y���p�����[�^")] private float _inLine;

    // ---------------------------- UnityMessage
    void OnDrawGizmos()
    {
        if (_gizmoSwitch == Helper.Switch.ON)
        {
            Gizmos.color = _color;
            var scale = transform.localScale;
            Gizmos.DrawWireCube(transform.position, scale);
            var inLine = new Vector3(scale.x - _inLine, scale.y - _inLine, scale.z - _inLine);
            Gizmos.DrawWireCube(transform.position, inLine);
        }
    }

#endif
}
