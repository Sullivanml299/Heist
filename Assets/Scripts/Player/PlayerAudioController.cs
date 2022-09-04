using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    // [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
    // public AudioClip[] FootstepAudioClips;
    // public AudioClip LandingAudioClip;
    // public CharacterController _controller;
    public AK.Wwise.Event footstepSFX;

    private void OnFootstep(AnimationEvent animationEvent)
    {

        if (animationEvent.animatorClipInfo.weight > 0.5f) //This is to prevent blended animations with low weight from triggering events
        {
            footstepSFX.Post(gameObject);
        }
        // if (animationEvent.animatorClipInfo.weight > 0.5f)
        // {
        //     if (FootstepAudioClips.Length > 0)
        //     {
        //         var index = Random.Range(0, FootstepAudioClips.Length);
        //         AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
        //     }
        // }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        // if (animationEvent.animatorClipInfo.weight > 0.5f)
        // {
        //     AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
        // }
    }
}
