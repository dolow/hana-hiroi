using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum Seasons
    {
        Spring = 1,
        Summer = 2,
        Autumn = 3
    }

    public enum State
    {
        Initiate = 1,
        InGame = 2,
        Finishing = 3,
        ShowingResult = 4
    }

    [SerializeField]
    private int gameFrames = 60 * 60;

    [SerializeField]
    private Camera mainCamera = null;

    [SerializeField]
    private Player player = null;

    [SerializeField]
    private GameObject kago = null;

    [SerializeField]
    private GameObject kagoCover = null;

    [SerializeField]
    private GameObject pattern = null;

    [SerializeField]
    private TransitionColor resultBg = null;

    [SerializeField]
    private TransitionColor resultText = null;

    [SerializeField]
    private TransitionColor transition = null;

    [SerializeField]
    private int maxLeafs = 60;

    private State state = State.Initiate;

    private int elapsedFrames = 0;

    private int enabledLeafs = 0;
    private readonly int leafProduceFrames = 30;

    GameObject boidPrefab = null;
    GameObject staticFlowerPrefab = null;

    public int score = 0;

    private bool touchBeganInsideUI = false;
    private GameController.Seasons lastSeason = GameController.Seasons.Spring;

    private bool canGoTitle = false;

    void Start()
    {
        UserInteractionController uic = this.GetComponent<UserInteractionController>();
        uic.pointerDownCallbackMap.Add("game_pointer_down", this.OnPointerDown);
        uic.pointerHoldCallbackMap.Add("game_pointer_hold", this.OnPointerHold);

        this.boidPrefab = Resources.Load<GameObject>("Prefabs/BoidsEntity");
        this.staticFlowerPrefab = Resources.Load<GameObject>("Prefabs/StaticFlower");

        this.transition.animate = true;
        this.transition.callbacks += this.OnTransitionFinished;
    }

    void Update()
    {
        if (this.elapsedFrames > this.gameFrames)
        {
            return;
        }

        this.elapsedFrames++;

        if (this.elapsedFrames == this.gameFrames)
        {
            this.Finish();
            return;
        }

        GameController.Seasons currentSeason = this.GetCurrentSeason();
        if (this.lastSeason != currentSeason)
        {
            this.OnSeasonChanged(currentSeason);
            this.lastSeason = currentSeason;
        }

        if ((this.elapsedFrames % this.leafProduceFrames) == 0)
        {
            if (Random.Range(0.0f, 1.0f) > (float)this.enabledLeafs / (float)this.maxLeafs)
            {
                this.AddLeaf();
            }
        }
    }

    void OnPointerDown(EventData data)
    {
        if (this.canGoTitle) {
            this.GoToTitle();
            return;
        }
        if (data.tag == "AddBoidButton")
        {
            this.touchBeganInsideUI = true;
            this.AddLeaf();
        } else
        {
            this.touchBeganInsideUI = false;
            this.MovePlayer(data);
        }
    }

    void OnPointerHold(EventData data)
    {
        this.MovePlayer(data);
    }

    void MovePlayer(EventData data)
    {
        if (this.player == null)
        {
            return;
        }
        if (this.mainCamera == null)
        {
            return;
        }
        if (this.touchBeganInsideUI)
        {
            return;
        }

        Vector3 worldPosition = this.mainCamera.ScreenToWorldPoint(data.position);

        this.player.Follow(worldPosition);
    }

    void AddLeaf()
    {
        if (this.enabledLeafs >= this.maxLeafs)
        {
            return;
        }

        Vector3 maxScreenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        Vector3 pos = new Vector3(
            Random.Range(-maxScreenPos.x, maxScreenPos.x),
            Random.Range(-maxScreenPos.y, maxScreenPos.y),
            0.0f
        );
        GameObject boid = Instantiate<GameObject>(this.boidPrefab, pos, Quaternion.identity);
        boid.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        float scale = Random.Range(0.2f, 0.3f);
        boid.transform.localScale = new Vector3(scale, scale, scale);

        BoidsConfig config = this.GetComponent<BoidsConfig>();
        GameObject container = config.GetBoidsContainer();
        boid.transform.parent = container.transform;

        BoidsEntity entity = boid.GetComponent<BoidsEntity>();
        entity.config = config;

        Leaf leaf = boid.GetComponent<Leaf>();
        leaf.gameController = this;
        leaf.SetSeason(this.GetCurrentSeason());

        this.enabledLeafs++;
    }

    public void AddFlower()
    {
        this.score++;
        this.enabledLeafs--;

        Vector3 maxScreenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        Vector3 pos = new Vector3(
            Random.Range(-maxScreenPos.x, maxScreenPos.x),
            Random.Range(-maxScreenPos.y, maxScreenPos.y),
            this.kago.transform.position.z
        );
        GameObject flowerObj = Instantiate<GameObject>(this.staticFlowerPrefab, pos, Quaternion.identity);
        flowerObj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        float scale = Random.Range(0.5f, 0.6f);
        flowerObj.transform.localScale = new Vector3(scale, scale, scale);
        flowerObj.transform.parent = this.kago.transform;

        Flower flower = flowerObj.GetComponent<Flower>();
        flower.SetSeason(this.GetCurrentSeason());
        flower.GrayShader();
    }

    public void OnSeasonChanged(GameController.Seasons season)
    {
        BoidsConfig config = this.GetComponent<BoidsConfig>();
        GameObject container = config.GetBoidsContainer();
        Leaf[] leafs = container.GetComponentsInChildren<Leaf>();
        for (int i = 0; i < leafs.Length; i++)
        {
            leafs[i].SetSeason(season);
        }

        this.player.SetSeason(season);
    }

    public void OnLeafLeaving(Leaf leaf)
    {
        this.AddFlower();
        GameObject.Destroy(leaf.gameObject);
    }

    public GameController.Seasons GetCurrentSeason()
    {
        float seasonUnit = (float)this.gameFrames / 3.0f;
        if (this.elapsedFrames <= seasonUnit * 1)
        {
            return GameController.Seasons.Spring;
        }
        else if (this.elapsedFrames <= seasonUnit * 2)
        {
            return GameController.Seasons.Summer;
        }
        return GameController.Seasons.Autumn;
    }

    private void Finish()
    {
        BoidsConfig config = this.GetComponent<BoidsConfig>();
        GameObject container = config.GetBoidsContainer();
        Leaf[] leafs = container.GetComponentsInChildren<Leaf>();
        for (int i = 0; i < leafs.Length; i++)
        {
            leafs[i].Idle();
        }

        this.player.Idle();

        this.transition.frames = 120;
        this.transition.transitionIn = true;
        this.transition.animate = true;
    }

    private void OnTransitionFinished()
    {
        switch (this.state)
        {
            case State.Initiate: this.state = State.InGame; break;
            case State.InGame:
                this.ShowResult();
                this.state = State.Finishing;
                break;
            case State.Finishing:
                this.ShowRank();
                this.resultBg.callbacks += this.SetCanGoTitle;
                this.state = State.ShowingResult;
                break;
            case State.ShowingResult:
                SceneManager.LoadScene("TitleScene");
                break;
            default: break;
        }
    }

    private void ShowResult()
    {
        this.pattern.SetActive(false);

        Flower[] flowers = this.kago.GetComponentsInChildren<Flower>();
        for (int i = 0; i < flowers.Length; i++)
        {
            Flower flower = flowers[i];
            flower.DefaultShader();
            flower.SeasonColor();
        }

        BoidsConfig config = this.GetComponent<BoidsConfig>();
        GameObject container = config.GetBoidsContainer();
        Leaf[] leafs = container.GetComponentsInChildren<Leaf>();
        for (int i = 0; i < leafs.Length; i++)
        {
            GameObject.Destroy(leafs[i].gameObject);
        }

        GameObject.Destroy(this.player.gameObject);

        this.kagoCover.SetActive(false);

        this.transition.frames = 120;
        this.transition.transitionIn = false;
        this.transition.animate = true;
    }

    private void ShowRank()
    {
        this.resultBg.animate = true;
        this.resultText.animate = true;

        Color color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        string text = "";

        if (this.score < 20)
        {
            color = new Color(130.0f / 255.0f, 70.0f / 255.0f, 30.0f / 255.0f, 0.0f);
            text = "Hana Tsubomi";
        }
        else if (this.score < 60)
        {
            color = new Color(40.0f / 255.0f, 140.0f / 255.0f, 50.0f / 255.0f, 0.0f);
            text = "Hana Hiraki";
        }
        else if (this.score < 80)
        {
            color = new Color(50.0f / 255.0f, 140.0f / 255.0f, 150.0f / 255.0f, 0.0f);
            text = "Hana Shigure";
        }
        else
        {
            color = new Color(200.0f / 255.0f, 80.0f / 255.0f, 140.0f / 255.0f, 0.0f);
            text = "Hana Sakari";
        }

        RawImage image = this.resultBg.GetComponent<RawImage>();
        Color baseColor = image.color;
        color.a = baseColor.a;
        image.color = color;

        this.resultText.GetComponent<Text>().text = text;
    }

    private void SetCanGoTitle()
    {
        this.canGoTitle = true;
    }

    private void GoToTitle()
    {
        this.transition.transitionIn = true;
        this.transition.frames = 60;
        this.transition.animate = true;

        this.resultBg.animate = true;
        this.resultBg.transitionIn = false;
        this.resultBg.frames = 60;
        this.resultText.animate = true;
        this.resultText.transitionIn = false;
        this.resultText.frames = 60;
    }
}
