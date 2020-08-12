using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DisolveText : MonoBehaviour
{
    public int frames = 60;
    public UnityEvent OnFinished = null;
    public bool trigger {
        get
        {
            return this.isTriggered;
        }
        set
        {
            this.isTriggered = value;
        }
    }

    [SerializeField]
    private bool isTriggered = false;
    private int currentFrame = 0;

    private Text text = null;

    // Start is called before the first frame update
    void Start()
    {
        this.text = this.GetComponent<Text>();
        Color baseColor = this.text.color;
        baseColor.a = 0.0f;
        this.text.color = baseColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.isTriggered)
        {
            return;
        }
        if (this.currentFrame == this.frames)
        {
            if (this.OnFinished != null)
            {
                this.OnFinished.Invoke();
            }
        }
        if (this.currentFrame >= this.frames)
        {
            return;
        }

        this.currentFrame++;
        Color baseColor = this.text.color;
        baseColor.a = ((float)this.currentFrame / (float)this.frames);
        this.text.color = baseColor;
    }
}
