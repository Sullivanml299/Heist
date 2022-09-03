using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject lootNotification;
    public Loot testLoot;
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
}
