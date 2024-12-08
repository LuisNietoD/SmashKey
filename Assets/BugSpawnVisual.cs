using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BugSpawnVisual : MonoBehaviour
{
    public Image bg;
    public Image icon;
    public List<Color> colors = new List<Color>();
    public List<Sprite> icons = new List<Sprite>();

    private void Start()
    {
        int r = Random.Range(0, colors.Count);
        bg.color = colors[r];
        icon.sprite = icons[r];
    }
}
