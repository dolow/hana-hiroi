using UnityEngine;

public abstract class TransitionColor : MonoBehaviour
{
    public bool animate = true;
    public int frames = 60;
    public bool transitionIn = true;

    public float targetAlpha = 1.0f;

    public delegate void OnTransitionFinished();

    public OnTransitionFinished callbacks;

    protected int currentFrames = 0;

    void Start()
    {
        this.SetColorComponent();
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
        Color color = this.GetColor();
        color.a = (this.transitionIn) ? ratio * this.targetAlpha : this.targetAlpha - ratio;
        this.SetColor(color);

        this.currentFrames++;
    }

    protected virtual void SetColorComponent()
    {

    }

    protected virtual Color GetColor()
    {
        return Color.clear;
    }

    protected virtual void SetColor(Color color)
    {
    }
}
