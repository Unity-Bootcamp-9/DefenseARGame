using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HPBar : MonoBehaviour
{
/* 
    [SerializeField] private Sprite redBar;
    [SerializeField] private Sprite blueBar;
    [SerializeField] private Transform outLine;
    [SerializeField] private Transform backGround;
    [SerializeField] private Transform fill;
    Sprite hpBarSprite;
    int redLayer = 6;
    int blueLayer = 7;
    
    public void Awake()
    {
        if (gameObject.layer == redLayer)
            hpBarSprite = redBar;
        else if (gameObject.layer == blueLayer)
            hpBarSprite = blueBar;
        
        outLine.GetComponent<Image>().sprite = hpBarSprite;
        backGround.GetComponent<Image>().sprite = hpBarSprite;
        fill.GetComponent<Image>().sprite = hpBarSprite;
    }
    
   public void Init(int allyLayer, string tag)
    {
        gameObject.layer = allyLayer;
        if (tag == "Minion")
            GetComponent<Transform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
        else if (tag == "Turret")
            GetComponent<Transform>().localScale = new Vector3(1,1,1);
    }*/

    void Update()
    {
        LookCam();
    }


    public void LookCam()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

}
