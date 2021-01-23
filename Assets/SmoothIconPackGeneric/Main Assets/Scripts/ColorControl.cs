using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorControl : MonoBehaviour
{
    public List<GameObject> IconList;
    public Color MasterColor;
    public float AnimationSpeed;
    public float DefaultAnimationSpeed;
    private Color defaultColor;
    void Start()
    {
        defaultColor = MasterColor;
       // Creates a list of the parent Icon gameobjects
        foreach(Transform icon in transform)
        {
            IconList.Add(icon.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //When the color is changed from the initial one grab the icon images and update their color
//        if (MasterColor != defaultColor)
  //      {
            foreach(GameObject LI in IconList)
            {
                LI.GetComponent<ColorManager>().UpdateColor();
            }
//        }
        //When the speed is changed from the initial one grab the icon animators and update their speed

        if (AnimationSpeed != DefaultAnimationSpeed)
        {
            foreach (GameObject LI in IconList)
            {
                LI.GetComponent<ColorManager>().UpdateAnimSpeed();
            }
        }
    }
}
