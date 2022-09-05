using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioController : MonoBehaviour
{
    public AK.Wwise.Event gotItemSFX;

    public void GotItem(){
        gotItemSFX.Post(gameObject);
    }
}
