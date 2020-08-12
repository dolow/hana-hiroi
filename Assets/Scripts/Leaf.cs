using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    enum State
    {
        Leaf = 0,
        Sublimating = 1,
        Leaving = 2
    }

    public static readonly Color defaultColor = new Color(1.0f, 100.0f / 255.0f, 40.0f / 255.0f, 1.0f);
    public static readonly Color springColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    public static readonly Color summerColor = new Color(130.0f / 255.0f, 1.0f, 140.0f / 255.0f, 1.0f);
    public static readonly Color autumnColor = new Color(1.0f, 240.0f / 255.0f, 120.0f / 255.0f, 1.0f);

    public GameController gameController = null;
    public int framesToFlower = 120;
    private int currentFramesWithLeader = 0;

    private int sublimateFrames = 60;
    private int currentFramesSublimating = 0;

    private BoidsImpl boidImpl = null;
    private State state = State.Leaf;

    private Color initialColor;
    private Color sublimateColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    // Start is called before the first frame update
    void Start()
    {
        this.boidImpl = this.gameObject.GetComponent<BoidsImpl>();

        DisolveSpriteAnimation animation = this.GetComponent<DisolveSpriteAnimation>();
        this.initialColor = animation.GetColor();
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.state)
        {
            case State.Leaf:
                {
                    if (this.boidImpl.hasLeader)
                    {
                        this.currentFramesWithLeader++;
                    }
                    else
                    {
                        this.currentFramesWithLeader--;
                        if (this.currentFramesWithLeader < 0)
                        {
                            this.currentFramesWithLeader = 0;
                        }
                    }

                    float ratio = (float)this.currentFramesWithLeader / (float)this.framesToFlower;

                    DisolveSpriteAnimation animation = this.GetComponent<DisolveSpriteAnimation>();
                    Color color = animation.GetColor();
                    float baseAlpha = color.a;
                    color = this.initialColor + (this.sublimateColor - this.initialColor) * ratio;
                    color.a = baseAlpha;
                    animation.SetColor(color);


                    if (this.currentFramesWithLeader >= this.framesToFlower)
                    {
                        this.state = State.Sublimating;
                        AudioSource audioSource = this.GetComponent<AudioSource>();
                        audioSource.pitch  = Random.Range(0.8f, 1.0f);
                        audioSource.Play();
                    }
                    break;
                }
            case State.Sublimating:
                {
                    if (this.currentFramesSublimating >= this.sublimateFrames)
                    {
                        this.state = State.Leaving;
                        if (this.gameController != null)
                        {
                            this.gameController.OnLeafLeaving(this);
                        }
                        break;
                    }
                    DisolveSpriteAnimation animation = this.GetComponent<DisolveSpriteAnimation>();
                    animation.animate = false;
                    animation.SetAlpha(((float)this.currentFramesSublimating / (float)this.sublimateFrames));

                    this.currentFramesSublimating++;
                    break;
                }
            case State.Leaving: break;
        }
    }

    public void Idle()
    {
        this.state = State.Leaving;
    }

    public void SetSeason(GameController.Seasons season)
    {
        switch (season)
        {
            case GameController.Seasons.Spring: this.sublimateColor = Leaf.springColor; break;
            case GameController.Seasons.Summer: this.sublimateColor = Leaf.summerColor; break;
            case GameController.Seasons.Autumn: this.sublimateColor = Leaf.autumnColor; break;
            default: this.sublimateColor = Leaf.springColor; break;
        }
    }
}
