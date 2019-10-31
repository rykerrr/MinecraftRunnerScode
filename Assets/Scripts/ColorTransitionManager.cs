using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
public class ColorTransitionManager : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> platforms;
    [SerializeField] private Camera camBackground;

    [SerializeField] private Color[] colors;
    [SerializeField] private float timeBeforeNewColor;

    private int brickColorIterator = 5;
    private int backgroundIterator = 0;

    private float colorTimer;

    private void Update()
    {
        if (Time.time > colorTimer)
        {
            brickColorIterator++; // iteration + checks
            backgroundIterator++;

            if (brickColorIterator >= 13)
            {
                brickColorIterator = 0;
            }
            if (backgroundIterator >= 13)
            {
                backgroundIterator = 0;
            }

            colorTimer = timeBeforeNewColor + Time.time;
        }

        camBackground.backgroundColor = Color.Lerp(camBackground.backgroundColor, new Color(colors[backgroundIterator].r, colors[backgroundIterator].g, colors[backgroundIterator].b, 0.6f), 0.007f);

        foreach (SpriteRenderer plat in platforms)
        {
            plat.color = Color.Lerp(plat.color, new Color(colors[brickColorIterator].r, colors[brickColorIterator].g, colors[brickColorIterator].b, 1), 0.007f);
        }

    }

    public bool AddPlatform(SpriteRenderer newPlat)
    {
        platforms.Add(newPlat);
        return true;
    }
}
#pragma warning restore 0649