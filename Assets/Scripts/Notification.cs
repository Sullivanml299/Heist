using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class Notification : MonoBehaviour
{
    public float holdTime;
    public float scrollSpeed;
    public UnityEngine.UI.Image lootIcon;
    public TextMeshProUGUI lootName;
    public UIController mainUI;

    RectTransform rectTransform;
    float yHeight;


    // Start is called before the first frame update
    void Start()
    {
        setup();
    }

    // Update is called once per frame
    void Update()
    {
        if(holdTime > 0) holdTime -= Time.deltaTime;
        else if(holdTime<=0 && !isOnScreen()) Destroy(this.gameObject);
        else rectTransform.position += Vector3.up * Time.deltaTime * scrollSpeed;
        
    }

    private bool isOnScreen(){
        if(rectTransform.anchoredPosition.y < yHeight) return true;
        return false;
    }

    public void setValues(string newName, Sprite newIcon){
        lootName.text = newName;
        lootIcon.sprite = newIcon;
    }

    public void setStackPosition(int index){
        setup();
        rectTransform.position -= Vector3.up * yHeight * 2 * index;
    }

    private void setup(){
        if(rectTransform == null) {
            rectTransform = GetComponent<RectTransform>();
            yHeight = Mathf.Abs(rectTransform.anchoredPosition.y);
        }
    }

    void OnDestroy(){
        // print("Destroy");
        mainUI.NotificationDestroyed();
    }
}
