using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class GuardAnimationHelper : MonoBehaviour
{
    public NavMeshAgent agent;
    private int _animIDSpeed;
    private int _animIDMotionSpeed;
    private Animator _animator;
    private bool _hasAnimator;
    NetworkIdentity identity;

    // Start is called before the first frame update
    void Start()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _hasAnimator = TryGetComponent(out _animator);
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        identity = GetComponent<NetworkIdentity>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(identity.hasAuthority) Animate();
    }

    void Animate(){
        float _speed = agent.desiredVelocity.magnitude > 0.1f ? agent.desiredVelocity.magnitude : 0f;
        if(_hasAnimator) _animator.SetFloat(_animIDSpeed, _speed);
        if(_hasAnimator && _speed > 0f) _animator.SetFloat(_animIDMotionSpeed, 1f);
        // print(_speed);
    }
}
