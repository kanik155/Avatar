using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public List<GameObject> IconList;
    private ColorControl CCScript;
    private Color CurrentColor;
    public float Animspeed;
    //generates a list at start of all icons
    void Start()
    {
        CCScript = GetComponentInParent<ColorControl>();
        {
            foreach (Transform icon in transform)
            {
                IconList.Add(icon.gameObject);
            }
        }
    }

    // Update is called once per frame
    public void UpdateColor()
    {
        //changes the color to what ever the master color variable is set to
        CurrentColor = CCScript.MasterColor;

        foreach (GameObject go in IconList)
        {
            if (go.GetComponent<Image>())
            {
                go.GetComponent<Image>().color = new Color(CurrentColor.r, CurrentColor.g, CurrentColor.b, go.GetComponent<Image>().color.a);
            }
            if (go.GetComponent<TrailRenderer>())
            {
                go.GetComponent<TrailRenderer>().startColor = CurrentColor;
                go.GetComponent<TrailRenderer>().endColor = CurrentColor;

            }
        }
    }

    public void UpdateAnimSpeed()
    {
        //changes the speed to what ever the master speed variable is set to
        Animspeed = CCScript.AnimationSpeed;
        GetComponent<Animator>().speed = Animspeed;
      /*  foreach (GameObject go in IconList)
        {
            go.GetComponent<Animator>().speed = Animspeed;
        }*/
    }
}
