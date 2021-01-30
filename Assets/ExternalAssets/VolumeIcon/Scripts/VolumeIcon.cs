using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class VolumeIcon : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private List<Sprite> volumeSprite = null;
    private int preVomule;
    [Range(0, 3)]
    public int Volume = 0;

    private void Start()
    {
        preVomule = Volume;
    }

    private void Update()
    {
        if (Volume != preVomule)
        {
            ChangeVolume(Volume);
            preVomule = Volume;
        }
    }

    private void ChangeVolume(int volume)
    {
        if (volume > 0 && volume < 3)
        {
            if (volumeSprite[1] != null)
            {
                image.sprite = volumeSprite[1];
            }
        }
        else if(volume >= 3 && volume < 6)
        {
            if (volumeSprite[2] != null)
            {
                image.sprite = volumeSprite[2];
            }
        }
        else if (volume >= 6)
        {
            if (volumeSprite[3] != null)
            {
                image.sprite = volumeSprite[3];
            }
        }
        else
        {
            image.sprite = volumeSprite[0];
        }
    }
}
