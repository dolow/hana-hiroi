using UnityEngine;

public class TransitionSpriteRenderer : TransitionColor
{
    private SpriteRenderer spriteRenderer = null;

    protected override void SetColorComponent()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    protected override Color GetColor()
    {
        return this.spriteRenderer.color;
    }

    protected override void SetColor(Color color)
    {
        this.spriteRenderer.color = color;
    }
}
