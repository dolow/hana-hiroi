using UnityEngine;
using UnityEngine.UI;

public class TransitionText : TransitionColor
{
    private Text text = null;

    protected override void SetColorComponent()
    {
        this.text = this.GetComponent<Text>();
    }

    protected override Color GetColor()
    {
        return this.text.color;
    }

    protected override void SetColor(Color color)
    {
        this.text.color = color;
    }
}
