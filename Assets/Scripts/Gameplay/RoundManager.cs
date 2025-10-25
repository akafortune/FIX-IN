using System;
using System.Collections;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class RoundManager : MonoBehaviour
{

    public static int resources;
    public TextMeshProUGUI resourcesText;
    public Transform resourcesTextTransform;
    public enum Mode { build, defend };
    public enum RoundType { Normal, IronBall, HeadRush, WreckingBall, Portals, BombBlitz}

    // round timer shenanigans
    public RoundTimeManager roundTM;

    //public float roundTime;
    //public TextMeshProUGUI roundText;
    private float roundClock = 0;

    //private static float roundCount = 0;
    public GameObject freakyText;
    public TextMeshProUGUI freakTypeText;
    public static Mode GameMode;
    public static RoundType FreakyType;
    public static bool lastBrickBuilt;
    private bool firstRound, canRebuild, countingDown;

    private AudioSource songSource;
    private AudioClip buildSong, defendSong, winJingle, freakSong, freakyLaugh;
    private Animator brickAnimator;
    public GameObject ball, ironBall, bouncyBall;
    public GameObject paddle;
    private Paddle paddleScript;
    public GameObject[] Bricks;
    public GameObject[] brickTypes;

    public Transform B1Transform, B2Transform;
    public GameObject itemBubble;
    public GameObject BubbleOne, BubbleTwo;
    public int bubble1Item, bubble2Item;

    private GameObject BuildUI;
    private GameObject DefendUI;
    private TextMeshProUGUI Countdown;
    private GameObject StartButton;
    private GameObject RebuildButton;
    private GameObject FloatingText;
    private GreenGuy greenGuy;
    private Transform ggT;
    private ScoreManager scoreManager;
    private FloatingText floatingText;
    public int scoreLast;
    public Sprite[] hatArray;
    public SpriteRenderer hatSprite;
    private int prestigeAmt = 0;
    public SpriteRenderer blueGuy;
    public AudioSource blueGuyVoice;
    public AudioClip[] blueGuyLines;
    public static bool justGot = false;
    private bool firstDef = true;

    // Start is called before the first frame update
    void Start()
    {
        songSource = GameObject.Find("SongSource").GetComponent<AudioSource>();
        buildSong = (AudioClip) Resources.Load("SFX/BuildMusic");
        defendSong = (AudioClip) Resources.Load("SFX/UpdatedDefendSong");
        winJingle = (AudioClip)Resources.Load("SFX/Win");
        freakSong = (AudioClip)Resources.Load("SFX/FreakyFella");
        freakyLaugh = (AudioClip)Resources.Load("SFX/FreakyLaugh");
        songSource.clip = defendSong;
        songSource.Play();
        songSource.clip = freakSong;
        songSource.Play();
        songSource.clip = freakyLaugh;
        songSource.Play();
        songSource.clip = buildSong;
        songSource.Play();
        Countdown = GameObject.Find("Countdown").GetComponent<TextMeshProUGUI>();
        FloatingText = (GameObject)Resources.Load("FloatingTextParent");
        firstRound = true;
        canRebuild = true;
        countingDown = true;
        Bricks = getBrickArray();
        resources = 45;
        GameMode = Mode.build;
        ball = GameObject.Find("Ball");
        ball.gameObject.SetActive(false);
        if (GameObject.Find("Tutorial Manager") == null)
        {    
            ironBall = GameObject.Find("IronBall").transform.GetChild(0).gameObject;
            ironBall.SetActive(false);
            bouncyBall = GameObject.Find("BouncyBall").transform.GetChild(0).gameObject;
            bouncyBall.gameObject.SetActive(false);
        }
        paddle = GameObject.Find("Paddle");
        paddleScript = paddle.GetComponent<Paddle>();
        paddle.SetActive(false);
        BuildUI = GameObject.Find("BuildUI");
        DefendUI = GameObject.Find("DefendUI");
        freakyText = GameObject.Find("FreakyText");
        if (freakyText != null) 
        {
            freakyText.SetActive(false);
            freakTypeText = GameObject.Find("FreakType").GetComponent<TextMeshProUGUI>();
            freakTypeText.gameObject.SetActive(false);
        }
        RebuildButton = GameObject.Find("Rebuild Button");
        DefendUI.SetActive(false);
        lastBrickBuilt = false;
        GreenGuy.buildTimer = 0.65f;
        //roundTime = 63.5f;

        roundTM = GameObject.Find("TimeManager").GetComponent<RoundTimeManager>();
        //roundTM.currentTime = 60;
        //RoundTimeManager.roundCount = 5;
        scoreLast = 0;

        B1Transform = GameObject.Find("Bubble 1").GetComponent<Transform>();
        B2Transform = GameObject.Find("Bubble 2").GetComponent<Transform>();

        greenGuy = GameObject.Find("GreenGuy").GetComponent<GreenGuy>();
        if (!Directory.Exists(Application.persistentDataPath + "/SaveData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveData");
        }
        if (!File.Exists(Application.persistentDataPath + "/SaveData/lastRound1.txt"))
        {
            StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/SaveData/lastRound1.txt");
            sw.WriteLine(resources);
            sw.Close();
        }

        StreamReader sr = new StreamReader(Application.persistentDataPath + "/SaveData/lastRound1.txt");
        int savedResources = Convert.ToInt32(sr.ReadLine());
        sr.Close();
        if (resources == savedResources)
        {
            RebuildButton.SetActive(false);
        }
        //roundText = GameObject.Find("RoundNumberText").GetComponent<TextMeshProUGUI>();

        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        floatingText = GameObject.Find("ScoreManager").GetComponent<FloatingText>();
        //Comment out for build
        //spawnBubble();
        ggT = GameObject.Find("GreenGuy").GetComponent<Transform>();
        hatSprite.sprite = hatArray[prestigeAmt];
        resourcesTextTransform = GameObject.Find("ResourcesGameSpace").GetComponent<Transform>();
    }

    private GameObject[] getBrickArray()
    {
        Transform[] array1 = GameObject.Find("Layer 1").GetComponentsInChildren<Transform>();
        Transform[] array2 = GameObject.Find("Layer 2").GetComponentsInChildren<Transform>();
        Transform[] array3 = GameObject.Find("Layer 3").GetComponentsInChildren<Transform>();
        Transform[] array4 = GameObject.Find("Layer 4").GetComponentsInChildren<Transform>();
        Transform[] array5 = GameObject.Find("Layer 5").GetComponentsInChildren<Transform>();

        Transform[] combinedArray;

        combinedArray = array1.Concat(array2).Concat(array3).Concat(array4).Concat(array5).ToArray();
        int index = 0;
        GameObject[] finalArray = new GameObject[50];
        foreach (Transform t in combinedArray)
        {
            if (t.gameObject.name.Equals("Brick(Clone)"))
            {
                finalArray[index] = t.gameObject;
                index++;
            }
        }

        return finalArray;
    }

    // Update is called once per frame
    void Update()
    {

        if (justGot)
        {
            blueGuyVoice.clip = blueGuyLines[2];
            blueGuyVoice.Play();
            justGot = false;
        }

        resourcesText.text = Convert.ToString(resources);

        //if(GameMode == Mode.defend)
        //{
        //    if(countingDown)
        //        roundClock += Time.deltaTime;

        //    //if(roundClock >= roundTime)
        //    if(roundClock > roundTM.currentTime)
        //    {
        //        BeginBuild();
        //        roundClock = 0;
        //    }

            
        //}
        //roundText.text = roundCount.ToString();
        if ((GameMode == Mode.build) && Time.timeScale != 0 && Input.GetKeyDown(KeyCode.Return)|| GameMode == Mode.build && Input.GetKeyDown("joystick button 6")|| GameMode == Mode.build && Input.GetKeyDown("joystick button 5"))
        {
            BeginRound();
        }
        if(GameMode == Mode.build && firstRound && canRebuild && Time.timeScale != 0 &&  (Input.GetKeyDown("f")||Input.GetKeyDown("joystick button 3")))
        {
            BuildLast();
            canRebuild = false;
        }
    }

    /*
    void CountdownToStart()
    {
        if (ctdTimer > 2)
        {
            countdown.text = "3";
        }
        else if (ctdTimer > 1)
        {
            countdown.text = "2";
        }
        else if (ctdTimer > 0)
        {
            countdown.text = "1";
        }
        else
        {
            countdown.text = "";
            ctd =false;
            StartDefend();
        }
    }*/

    public static void Spend(int value)
    {
        resources -= value;
    }

    public void BeginRound()
    {
        if (GameObject.Find("Tutorial Manager") == null)
        {
            roundTM.roundTime = 60f;
            roundTM.currentTime = 60f;
            if (firstRound)
            {
                StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/SaveData/lastRound1.txt");
                sw.WriteLine(resources);
                foreach (GameObject brick in Bricks)
                {
                    sw.Write(brick.GetComponent<Brick>().isBuilt() + ",");
                }
                sw.Close();
                firstRound = false;
                RebuildButton.SetActive(false);
            }

            if (BubbleOne != null)
                BubbleOne.transform.GetChild(0).SendMessage("StartPop");

            foreach (GameObject brick in Bricks)
            {
                //brick.GetComponent<Animator>().SetFloat("FixMultiplier",.65f);
                if (brick.GetComponent<Collider2D>().isTrigger)
                {
                    brick.SetActive(false);
                }
            }
            StartCoroutine(FadeOut());
            GameMode = Mode.defend;
            DefendUI.SetActive(true);
            BuildUI.SetActive(false);
            if (RoundTimeManager.roundCount % 5 == 0)
            {
                StartCoroutine(GetFreaky());
                return;
            }
            paddle.SetActive(true);
            paddleScript.Target = ball.transform;
            ball.SetActive(true);
            ball.GetComponent<Ball>().LaunchSequence();
            ball.GetComponent<Ball>().NewRound(RoundTimeManager.roundCount);
        }
        else if (GameObject.Find("Tutorial Manager") != null)
            if (RoundTimeManager.roundCount % 5 == 0)
            {
                Debug.Log("Starting tutorial round");
                StartCoroutine(FadeOut());
                GameMode = Mode.defend;
                DefendUI.SetActive(true);
                BuildUI.SetActive(false);
                paddle.SetActive(true);
                paddleScript.Target = ball.transform;
                ball.SetActive(true);
                ball.GetComponent<Ball>().LaunchSequence(60f);
                ball.GetComponent<Ball>().NewRound(RoundTimeManager.roundCount);
            }
    }

    public bool ModeSet;
    public bool CycleDone;
    public IEnumerator GetFreaky()
    {
        countingDown = false;
        yield return new WaitForSeconds(.25f);
        songSource.clip = freakyLaugh;
        songSource.Play();
        yield return new WaitForSeconds(2);
        songSource.clip = freakSong;
        songSource.Play();
        ModeSet = false;
        //cue freak guy entrance
        freakyText.SetActive(true);
        freakyText.GetComponent<TextMeshProUGUI>().color = Color.white;
        StartCoroutine(FreakyTextCycle());
        for (int i = 0; i < 4; i++)
        {
            freakyText.SetActive(true);
            yield return new WaitForSeconds(.5f);
            freakyText.SetActive(false);
            yield return new WaitForSeconds(.5f);
        }
        freakyText.SetActive(true);
        yield return new WaitForSeconds(1);
        FreakyType = (RoundType)Random.Range(1, 3);
        //FreakyType = (RoundType)3;
        ModeSet = true;
        yield return new WaitUntil(() => CycleDone);
        yield return new WaitForSeconds(.75f);
        StartCoroutine(FreakyTextFade());
        yield return new WaitForSeconds(1.75f);
        freakyText.SetActive(false);
        freakTypeText.gameObject.SetActive(false);
        
        switch(FreakyType)
        {
            case RoundType.IronBall:
                IronBallStart();
                break;
            case RoundType.BombBlitz:
                break;
            case RoundType.HeadRush:
                HeadRushStart();
                break;
            case RoundType.Portals:
                break;
            case RoundType.WreckingBall:
                break;
        }    
        countingDown = true;
    }

    private IEnumerator FreakyTextCycle()
    {
        CycleDone = false;
        RoundType type = (RoundType)1;
        freakTypeText.gameObject.SetActive(true);
        freakTypeText.color = Color.white;
        while(!ModeSet || (ModeSet && type != FreakyType))
        {
            freakTypeText.text = type.ToSafeString();
            yield return new WaitForSeconds(.2f);
            type += 1;
            if ((int)type > 5)
                type = (RoundType)1;
        }
        freakTypeText.text = type.ToSafeString();
        freakTypeText.color = Color.red;
        CycleDone = true;
    }

    private IEnumerator FreakyTextFade()
    {
        TextMeshProUGUI freakyTextMesh = freakyText.GetComponent<TextMeshProUGUI>();
        float a = freakyTextMesh.color.a;
        Color temp1 = freakyTextMesh.color;
        Color temp2 = freakTypeText.color;

        while (freakyTextMesh.color.a > 0)
        {
            a -= .01f;
            temp1.a = a;
            temp2.a = a;
            freakyTextMesh.color = temp1;
            freakTypeText.color = temp2;
            yield return new WaitForSeconds(0.01f);
        }
    }
    private void IronBallStart()
    {
        ironBall.SetActive(true);
        ironBall.transform.position = new Vector3(0, ironBall.transform.position.y);
        paddle.SetActive(true);
        paddle.transform.position = new Vector3(ironBall.transform.position.x, paddle.transform.position.y);
        paddleScript.Target = ironBall.transform;
        ironBall.GetComponent<IronBall>().LaunchSequence(roundTM.IronRndTime);
        ironBall.GetComponent<Ball>().NewRound(RoundTimeManager.roundCount);
        //roundTM.roundTime = roundTM.IronRndTime;
    }

    private void HeadRushStart()
    {
        GreenGuy.speedMod = 1.5f;
        GreenGuy.SetSpeeding(true);
        GreenGuy.SetInvulnerable(true);
        foreach(GameObject brick in Bricks)
        {
            brick.SetActive(false);
        }
        bouncyBall.SetActive(true);
        bouncyBall.transform.position = new Vector3(0, bouncyBall.transform.position.y);
        paddle.SetActive(true);
        paddle.transform.position = new Vector3(bouncyBall.transform.position.x, paddle.transform.position.y);
        paddleScript.Target = bouncyBall.transform;
        bouncyBall.GetComponent<Ball>().LaunchSequence(roundTM.HeadRndTime);
        bouncyBall.GetComponent<Ball>().NewRound(RoundTimeManager.roundCount);
        //roundTM.roundTime = roundTM.HeadRndTime;
    }

    public void BeginBuild()
    {
        if (GameObject.Find("Tutorial Manager") == null)
        {
            StartCoroutine(FadeIn());
            Reinforced[] reinforcedTiles = GameObject.FindObjectsByType<Reinforced>(FindObjectsSortMode.None);
            
            foreach(Reinforced tile in reinforcedTiles)
            {
                tile.ResetHits();
            }

            if (Teleporter.brokenPortals %2 == 1)
            {
                greenGuy.specialBrickAmounts[3]++;
            }
            Teleporter.brokenPortals = 0;

            spawnBubble();

            Countdown.text = "";
            foreach(GameObject brick in Bricks)
            {
                if (!brick.activeInHierarchy)
                {
                    brick.SetActive(true);
                    if (brick.GetComponent<Brick>().bc.isTrigger)
                    {
                        Animator brickAnim = brick.GetComponent<Animator>();
                        brickAnim.SetBool("IsBroken", true);
                        brickAnim.Play("BrokenBrick", 0, 0);
                    }
                }
            }
            FreakyType = RoundType.Normal;
            songSource.clip = buildSong;
            songSource.Play();
            resources += 15;
            //floatingText.ShowV2FloatingText("+15", resourcesTextTransform);
            GameMode = Mode.build;
            DefendUI.SetActive(false);
            BuildUI.SetActive(true);
            paddle.SetActive(false);
            ball.GetComponent<Ball>().RestFilters();
            ball.SetActive(false);
            ironBall.SetActive(false);
            bouncyBall.SetActive(false);
            RoundTimeManager.roundCount++;
            scoreManager.GetSpecialBricks();
            
            //Resources for score
            float scoreDiff = scoreManager.currentScore - scoreLast;
            Debug.Log("Score Diff: "+scoreDiff);
            if (scoreDiff > 0) 
            {
                //divide by 100 and round up
                float gainF = scoreDiff / 100f;
                gainF += 0.5f;
                int gainI = Mathf.RoundToInt(gainF);
                //*2
                gainI *= 2;
                resources += gainI;
                floatingText.ShowV2FloatingText("+" + (/*(int)*/(15 + gainI)).ToString(), resourcesTextTransform);
            }
            Debug.Log("Score Last Before:" + scoreLast);
            scoreLast = scoreManager.currentScore;
            Debug.Log("Score Last After:" + scoreLast);
        }
        if (GameObject.Find("Tutorial Manager") != null)
        {
            Debug.Log("Ending tutorial round");
            StartCoroutine(FadeIn());
            songSource.clip = buildSong;
            songSource.Play();
            DefendUI.SetActive(false);
            BuildUI.SetActive(true);
            paddle.SetActive(false);
            ball.GetComponent<Ball>().RestFilters();
            ball.SetActive(false);
        }
    }

    public void SkipBuild()
    {
        Brick[] Layer1 = GameObject.Find("Layer 1").GetComponentsInChildren<Brick>();
        Brick[] Layer2 = GameObject.Find("Layer 2").GetComponentsInChildren<Brick>();

        BuildLayer(Layer1);
        BuildLayer(Layer2);
        BeginRound();
    }

    public void BuildLayer(Brick[] array)
    {
        foreach(Brick brick in array)
        {
            brick.fixBrick();
        }
    }

    public void BuildLast()
    {
        StreamReader sr = new StreamReader(Application.persistentDataPath + "/SaveData/lastRound1.txt");

        int readResources = Convert.ToInt32(sr.ReadLine());
        if(readResources != 45)
        {
            string[] brickArrangement = sr.ReadLine().Split(',');
            int index = 0;
            foreach (GameObject brick in Bricks)
            {
                if (Convert.ToBoolean(brickArrangement[index]))
                    brick.GetComponent<Brick>().fixBrick();
                else
                    brick.GetComponent<Brick>().cancelBrick();
                index++;
            }
            sr.Close();
            RebuildButton.SetActive(false);
        }
    }

    protected void spawnBubble()
    {
        do
        {
            bubble1Item = Random.Range(0, 6);
            Debug.Log(bubble1Item);
            bubble2Item = Random.Range(0, 6);
            Debug.Log(bubble2Item);
        } while (bubble1Item == bubble2Item);

        BubbleOne = Instantiate(itemBubble, B1Transform);
        BubbleOne.GetComponentInChildren<Bubble>().brickInd = bubble1Item;
        BubbleTwo = Instantiate(itemBubble, B2Transform);
        BubbleTwo.GetComponentInChildren<Bubble>().brickInd = bubble2Item;
    }

    public bool checkWin()
    {
        if (GameMode == Mode.build)
        {
            foreach (GameObject brick in Bricks)
            {
                Brick BrickScript = brick.GetComponent<Brick>();
                if (!BrickScript.isBuilt() && !BrickScript.replaced)
                {
                    return false;
                }
            }

            foreach (GameObject brick in Bricks)
            {
                Brick BrickScript = brick.GetComponent<Brick>();
                if (BrickScript.replaced && BrickScript.SpecialBrick != null)
                    BrickScript.SpecialBrick.SendMessage("RemoveBrick");
                else
                    BrickScript.cancelBrick();
            }
            //GameObject flText = Instantiate(FloatingText, Vector3.zero, Quaternion.identity);
            //flText.GetComponentInChildren<TextMesh>().text = "" + 1000;
            

            StartCoroutine(Prestige());
            return true;
        }
        else
            return false;
    }

    public static int getRoundCount()
    {
        return (int)RoundTimeManager.roundCount;
    }

    IEnumerator Prestige()
    {
        yield return new WaitForSeconds(.65f);

        foreach (GameObject brick in Bricks)
        {
            Brick BrickScript = brick.GetComponent<Brick>();
            if (BrickScript.replaced && BrickScript.SpecialBrick != null)
                BrickScript.SpecialBrick.SendMessage("RemoveBrick");
            else
                BrickScript.cancelBrick();
        }
        //GameObject flText = Instantiate(FloatingText, Vector3.zero, Quaternion.identity);
        //flText.GetComponentInChildren<TextMesh>().text = "" + 1000;
        songSource.PlayOneShot(winJingle);
        floatingText.ShowFloatingText("+2500", ggT);
        songSource.PlayOneShot(winJingle);
        resources += 45;
        floatingText.ShowV2FloatingText("+45", resourcesTextTransform);
        //greenGuy.zeroSpecialResources();
        //greenGuy.currentScore += 10000;
        scoreManager.IncreaseScore(2500);
        //ball.GetComponent<Ball>().WinGrace();
        if(prestigeAmt < 10)
        {
            prestigeAmt++;
        }
        hatSprite.sprite = hatArray[prestigeAmt];
    }

    private IEnumerator FadeIn()
    {
        blueGuyVoice.clip = blueGuyLines[0];
        blueGuyVoice.Play();

        float a = blueGuy.color.a;
        Color temp = blueGuy.color;

        while(blueGuy.color.a < 1)
        {
            a += .01f;
            temp.a = a;
            blueGuy.color = temp;

            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator FadeOut()
    {
        if (!firstDef && RoundTimeManager.roundCount % 5 != 0)
        {
            blueGuyVoice.clip = blueGuyLines[1];
            blueGuyVoice.Play();
        } else
        {
            firstDef = false;
        }
        

        float a = blueGuy.color.a;
        Color temp = blueGuy.color;

        while (blueGuy.color.a > 0)
        {
            a -= .01f;
            temp.a = a;
            blueGuy.color = temp;

            yield return new WaitForSeconds(0.01f);
        }
    }

   
}
