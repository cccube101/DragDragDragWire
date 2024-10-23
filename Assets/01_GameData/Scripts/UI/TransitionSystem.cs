using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionSystem : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("�J�ڐ�V�[��")] private SceneName _toScene;



    // ---------------------------- PublicMethod
    /// <summary>
    /// �V�[���ύX
    /// </summary>
    public async void SceneChange()
    {
        await Helper.Tasks.Canceled(Helper.Tasks.SceneChange((int)_toScene, destroyCancellationToken));
    }

    /// <summary>
    /// ���X�e�[�W�֑J��
    /// </summary>
    public async void SceneChange_Next()
    {
        var next = SceneManager.GetActiveScene().buildIndex + 1;
        await Helper.Tasks.Canceled(Helper.Tasks.SceneChange(next, destroyCancellationToken));
    }
}
