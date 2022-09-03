using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject lootNotification;
    public GameObject Timer;
    public TMP_Text minutes;
    public TMP_Text seconds;
    private int activeNotificationCount = 0;

    // void Start(){
    //     newNotification(testLoot);
    //     newNotification(testLoot,1);
    // }

    public void newNotification(Loot loot){
        var index = activeNotificationCount;
        Notification newNotification = GameObject.Instantiate(lootNotification).GetComponent<Notification>();
        newNotification.setValues(loot.lootName, loot.itemIcon);
        newNotification.transform.SetParent(gameObject.transform, false);
        newNotification.setStackPosition(index);
        newNotification.mainUI = this;
        activeNotificationCount++;
    }

    public void NotificationDestroyed(){
        activeNotificationCount --;
    }

    public void updateTimeRemaining(int minutes, int seconds){
        if(minutes < 10) this.minutes.text = "0" + minutes.ToString();
        else this.minutes.text = minutes.ToString();
        if(seconds < 10) this.seconds.text = "0" + seconds.ToString();
        else this.seconds.text = seconds.ToString();
    }

    public void enableTimer(bool value){
        Timer.SetActive(value);
    }
}
