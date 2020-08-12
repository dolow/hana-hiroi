using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisolveSpriteAnimation : MonoBehaviour
{
    public int baseFramesPerSprite = 120;
    public bool animate = true;
    public GameObject[] gameObjects = new GameObject[] { };

    private int framesPerSprite = 120;

    private int currentIndex = 0;
    private int currentFrame = 0;

    private void Start()
    {
        this.framesPerSprite = this.baseFramesPerSprite + (int)Random.Range(0, (float)baseFramesPerSprite);
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.animate)
        {
            return;
        }
        
        if (this.currentFrame >= this.framesPerSprite)
        {
            this.currentFrame = 0;
            this.currentIndex++;
            if (this.currentIndex >= this.gameObjects.Length)
            {
                this.currentIndex = 0;
            }
        }

        float ratio = (float)this.currentFrame / (float)this.framesPerSprite;
        float threshold = 0.5f;

        for (int i = 0; i < this.gameObjects.Length; i++)
        {
            GameObject go = this.gameObjects[i];
            SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
            Color currentColor = renderer.color;
            if (i == this.currentIndex)
            {
                // appearing
                currentColor.a = Mathf.Max((ratio + threshold), 0.0f);
                renderer.color = currentColor;

            } else
            {
                // disappearing
                currentColor.a = Mathf.Min(0.0f, 1.0f - (ratio - threshold) * 2);
                renderer.color = currentColor;
            }
        }

        // TODO: performance
        // float peak = (float)this.framesPerSprite * 0.5f;
        // float ratio = Mathf.Abs(peak - (float)this.currentFrame) / peak;
       

        this.currentFrame++;
    }

    public Color GetColor()
    {
        SpriteRenderer renderer = this.gameObjects[this.currentIndex].GetComponent<SpriteRenderer>();
        return renderer.color;
    }

    public void SetColor(Color color)
    {
        SpriteRenderer renderer = this.gameObjects[this.currentIndex].GetComponent<SpriteRenderer>();
        renderer.color = color;
    }
    public void SetAlpha(float alpha)
    {
        SpriteRenderer renderer = this.gameObjects[this.currentIndex].GetComponent<SpriteRenderer>();
        Color baseColor = renderer.color;
        baseColor.a = alpha;
        renderer.color = baseColor;
    }
}
