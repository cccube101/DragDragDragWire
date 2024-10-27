using Alchemy.Inspector;
using UnityEngine;

public class StuckEnemyController : EnemyBase
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�p�����[�^")] private float _turnSpeed;

    // ---------------------------- UnityMessage
    private void Update()
    {
        //  �����]��
        //  ��ʓ��ɃI�u�W�F�N�g������ۂɏ���
        if (_sr.isVisible)
        {
            _tr.eulerAngles += new Vector3(0, 0, _turnSpeed * Time.deltaTime);
        }
    }




    // ---------------------------- PublicMethod
    /// <summary>
    /// �v���C���[�ւ̃_���[�W
    /// </summary>
    /// <returns>�_���[�W��</returns>
    public override int Damage()
    {
        //  �G�t�F�N�g
        Instantiate(_knockEffect, _tr.position, Quaternion.identity);

        //  �m�b�N�o�b�N
        KnockBackPlayer();

        return _damage;
    }
}
