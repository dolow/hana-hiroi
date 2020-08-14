using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleController : UserInteractionReceiver
{
    public TransitionColor whiteout = null;

    public TransitionText title = null;
    public TransitionText tapToStart = null;

    public AudioSource sound = null;

    private bool canGoGame = false;

    void Start()
    {
        this.title.callbacks += this.ShowTapToTitle;
        this.tapToStart.callbacks += () => {
            this.canGoGame = true;
        };
    }

    private void ShowTapToTitle()
    {
        this.tapToStart.animate = true;
        this.title.callbacks -= this.ShowTapToTitle;
    }

    public override void OnPointerDown(PointerEventData data, string tag)
    {
        if (!this.canGoGame)
        {
            return;
        }

        this.whiteout.animate = true;
        this.whiteout.transitionIn = true;
        this.whiteout.callbacks += this.GoToGame;

        this.title.transitionIn = false;
        this.title.animate = true;
        this.title.frames = 60;
        this.tapToStart.transitionIn = false;
        this.tapToStart.animate = true;
        this.tapToStart.frames = 60;

        this.sound.GetComponent<Fade>().BeginFade();

        this.canGoGame = false; 
    }

    private void GoToGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
