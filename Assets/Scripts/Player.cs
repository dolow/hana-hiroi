using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int graduateFrames = 60;

    private int currentGraduateFrames = 0;

    private Color lastColor = Leaf.springColor;
    private Color currentColor = Leaf.springColor;

    private void Update()
    {
        if (this.currentGraduateFrames < this.graduateFrames)
        {
            Color colorDiff = this.currentColor - this.lastColor;
            float ratio = ((float)this.currentGraduateFrames / (float)this.graduateFrames);
            SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
            Color newColor = this.lastColor + colorDiff * ratio;
            renderer.color = newColor;

            this.currentGraduateFrames++;
        }
    }

    // Start is called before the first frame update
    public void Follow(Vector3 destination)
    {
        Follow follow = this.GetComponent<Follow>();
        if (follow == null)
        {
            return;
        }

        if (follow.IsFollowing())
        {
            follow.ExtendDestination(destination);
        }
        else
        {
            follow.SetDestination(destination);
        }
    }

    public void Idle()
    {
        Follow follow = this.GetComponent<Follow>();
        follow.enabled = false;
    }

    public void SetSeason(GameController.Seasons season)
    {
        this.currentGraduateFrames = 0;
        this.lastColor = this.currentColor;

        switch (season)
        {
            case GameController.Seasons.Spring: this.currentColor = Leaf.springColor; break;
            case GameController.Seasons.Summer: this.currentColor = Leaf.summerColor; break;
            case GameController.Seasons.Autumn: this.currentColor = Leaf.autumnColor; break;
            default: this.currentColor = Leaf.springColor; break;
        }
    }
}
