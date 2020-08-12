using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disolve : MonoBehaviour
{
    public int frames = 60;
    private int currentFrame = 0;

    private SpriteRenderer spriteRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        Color baseColor = this.spriteRenderer.color;
        baseColor.a = 0.0f;
        this.spriteRenderer.color = baseColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.currentFrame >= this.frames)
        {
            return;
        }
        this.currentFrame++;

        Color baseColor = this.spriteRenderer.color;
        baseColor.a = ((float)this.currentFrame / (float)this.frames);
        this.spriteRenderer.color = baseColor;
    }
}
