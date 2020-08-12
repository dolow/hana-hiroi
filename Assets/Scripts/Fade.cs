using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public bool animate = true;
    public int frames = 60;

    public float targetVolume = 1.0f;

    public delegate void OnFadeFinished();

    public OnFadeFinished callbacks;

    protected int currentFrames = 0;

    protected AudioSource audioSource = null;
    protected float initialVolume = 0.0f;

    void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
        this.initialVolume = this.audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.animate)
        {
            return;
        }

        if (this.currentFrames == this.frames)
        {
            this.animate = false;
            this.currentFrames = 0;
            if (this.callbacks != null)
            {
                this.callbacks();
            }

            return;
        }

        float ratio = ((float)this.currentFrames / (float)this.frames);
        float diff = this.targetVolume - this.initialVolume;
        float addition = diff * ratio;
        this.audioSource.volume = this.initialVolume + addition;

        this.currentFrames++;
    }

    public void BeginFade()
    {
        if (this.animate)
        {
            return;
        }
        this.animate = true;
        this.initialVolume = this.audioSource.volume;
    }
}
