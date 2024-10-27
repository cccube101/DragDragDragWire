using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using R3;
using R3.Triggers;
using Alchemy.Inspector;

public class UIAnimatorBase : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Required, BoxGroup("��b�p�����[�^")] protected UnityEvent[] _event;


    // ---------------------------- Field
    protected Dictionary<string, UnityEvent> _actions;
    protected Animator _animator = null;


    // ---------------------------- UnityMessage
    public virtual void Awake()
    {
        StartEvent();
    }

    public virtual void OnEnable()
    {
        AnimatorStateObserve();
    }

    // ---------------------------- PublicMethod
    /// <summary>
    /// �J�n�C�x���g
    /// </summary>
    public void StartEvent()
    {
        //  �L���b�V��
        _animator = GetComponent<Animator>();

        //  ���C���[���擾
        var layer = _animator.GetLayerName(0);
        var clips = _animator.runtimeAnimatorController.animationClips;

        //  ���\�b�h�i�[
        _actions = new Dictionary<string, UnityEvent>(clips.Length);
        for (int i = 0; i < clips.Length; i++)
        {
            //  "���C���[.�X�e�[�g��"
            _actions.Add($"{layer}.{clips[i].name}", _event[i]);
        }
    }

    /// <summary>
    /// �A�j���[�^�[�X�e�[�g�̊Ď�
    /// </summary>
    public void AnimatorStateObserve()
    {
        //  �A�j���[�^�[�X�e�[�g�Ď�
        if (_animator == null) return;
        _animator.GetBehaviour<ObservableStateMachineTrigger>()
            .OnStateEnterAsObservable()
            .Subscribe(state =>
            {
                //  �A�N�V������������
                foreach (var item in _actions)
                {
                    if (state.StateInfo.IsName(item.Key))   //  �X�e�[�g���Ŕ���
                    {
                        _actions[item.Key]?.Invoke();    //  ���s
                    }
                }
            })
            .AddTo(this);
    }


    // ---------------------------- PrivateMethod
}
