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
            transform.eulerAngles += new Vector3(0, 0, _turnSpeed * Time.deltaTime);
        }
    }




    // ---------------------------- PublicMethod
    /// <summary>
    /// �v���C���[�ւ̃_���[�W
    /// </summary>
    /// <param name="player">�v���C���[�I�u�W�F�N�g</param>
    /// <returns>�_���[�W��</returns>
    public override int Damage(GameObject player)
    {
        //  �G�t�F�N�g
        Instantiate(_knockEffect, transform.position, Quaternion.identity);

        //  �m�b�N�o�b�N
        var dir = (player.transform.position - transform.position).normalized;
        player.GetComponent<Rigidbody2D>().AddForce(dir * _knockBackForce);

        return _damage;
    }
}
