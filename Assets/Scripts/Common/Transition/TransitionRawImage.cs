using UnityEngine;
using UnityEngine.UI;

public class TransitionRawImage : TransitionColor
{
    private RawImage image = null;

    protected override void SetColorComponent()
    {
        this.image = this.GetComponent<RawImage>();
    }

    protected override Color GetColor()
    {
        return this.image.color;
    }

    protected override void SetColor(Color color)
    {
        this.image.color = color;
    }
}
