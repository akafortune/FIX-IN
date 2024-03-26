using System;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseBuilding : MonoBehaviour
{

    public static int resources;
    public TextMeshProUGUI resourcesText;
    public enum Mode { build, defend };
    public float roundTime;
    private float roundCount = 0;
    public TextMeshProUGUI roundText;
    public static Mode GameMode;
    public static bool lastBrickBuilt;
    private bool firstRound;
    private float roundClock = 0;

    private AudioSource songSource;
    private AudioClip buildSong, defendSong;
    private Animator brickAnimator;
    public GameObject ball;
    public GameObject paddle;
    public GameObject[] Bricks;
    public GameObject[] brickTypes;

    public Transform bubbleOne, bubbleTwo;
    public GameObject itemBubble;
    public GameObject b1, b2;
    public int bubble1Item, bubble2Item;

    private GameObject BuildUI;
    private GameObject DefendUI;
    private TextMeshProUGUI Countdown;
    private GameObject StartButton;
    private GameObject RebuildButton;

    // Start is called before the first frame update
    void Start()
    {
        songSource = GameObject.Find("SongSource").GetComponent<AudioSource>();
        buildSong = (AudioClip) Resources.Load("SFX/BuildMusic");
        defendSong = (AudioClip) Resources.Load("SFX/UpdatedDefendSong");
        songSource.clip = defendSong;
        songSource.Play();
        songSource.clip = buildSong;
        songSource.Play();
        Countdown = GameObject.Find("Countdown").GetComponent<TextMeshProUGUI>();
        firstRound = true;
        Bricks = getBrickArray();
        resources = 45;
        GameMode = Mode.build;
        ball = GameObject.Find("Ball");
        ball.SetActive(false);
        paddle = GameObject.Find("Paddle");
        paddle.SetActive(false);
        BuildUI = GameObject.Find("BuildUI");
        DefendUI = GameObject.Find("DefendUI");
        RebuildButton = GameObject.Find("Rebuild Button");
        DefendUI.SetActive(false);
        lastBrickBuilt = false;
        GreenGuy.buildTimer = 0.65f;
        roundTime = 63.5f;
        roundCount++;

        bubbleOne = GameObject.Find("Bubble 1").GetComponent<Transform>();
        bubbleTwo = GameObject.Find("Bubble 2").GetComponent<Transform>();

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
        roundText = GameObject.Find("RoundNumberText").GetComponent<TextMeshProUGUI>();

        
        do
        {
            bubble1Item = Random.Range(0, 4);
            Debug.Log(bubble1Item);
            bubble2Item = Random.Range(0, 4);
            Debug.Log(bubble2Item);
        } while (bubble1Item == bubble2Item);

        b1 = Instantiate(itemBubble, bubbleOne);
        b1.GetComponentInChildren<Bubble>().brickInd = bubble1Item;
        b2 = Instantiate(itemBubble, bubbleTwo);
        b2.GetComponentInChildren<Bubble>().brickInd = bubble2Item;
        
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
        resourcesText.text = Convert.ToString(resources);

        if(GameMode == Mode.defend)
        {
            roundClock += Time.deltaTime;

            if(roundClock >= roundTime)
            {
                BeginBuild();
                roundClock = 0;
            }

            
        }
        roundText.text = roundCount.ToString();
        
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
        Destroy(b1);
        Destroy(b2);
        {
            StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/SaveData/lastRound1.txt");
            sw.WriteLine(resources);
            foreach(GameObject brick in Bricks)
            {
                sw.Write(brick.GetComponent<Brick>().isBuilt() + ",");
            }
            sw.Close();
            firstRound = false;
            RebuildButton.SetActive(false);
        }
        foreach (GameObject brick in Bricks)
        {
            //brick.GetComponent<Animator>().SetFloat("FixMultiplier",.65f);
            if (brick.GetComponent<Collider2D>().isTrigger)
            {
                brick.SetActive(false);
            }
        }
        GameMode = Mode.defend;
        DefendUI.SetActive(true);
        BuildUI.SetActive(false);
        paddle.SetActive(true);
        ball.SetActive(true);
        ball.GetComponent<Ball>().LaunchSequence();
        
        ball.GetComponent<Ball>().NewRound(roundCount);
    }

    public void BeginBuild()
    {
        do
        {
            bubble1Item = Random.Range(0, 4);
            Debug.Log(bubble1Item);
            bubble2Item = Random.Range(0, 4);
            Debug.Log(bubble2Item);
        } while (bubble1Item == bubble2Item);

        b1 = Instantiate(itemBubble, bubbleOne);
        b1.GetComponentInChildren<Bubble>().brickInd = bubble1Item;
        b2 = Instantiate(itemBubble, bubbleTwo);
        b2.GetComponentInChildren<Bubble>().brickInd = bubble2Item;

        Countdown.text = "";
        foreach(GameObject brick in Bricks)
        {
            if (!brick.activeInHierarchy)
            {
                brick.SetActive(true);
                Animator brickAnim = brick.GetComponent<Animator>();
                brickAnim.SetBool("IsBroken", true);
                brickAnim.Play("BrokenBrick", 0, 0);
            }
        }
        songSource.clip = buildSong;
        songSource.UnPause();
        resources += 15;
        GameMode = Mode.build;
        DefendUI.SetActive(false);
        BuildUI.SetActive(true);
        paddle.SetActive(false);
        ball.SetActive(false);
        roundCount++;
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
        resources = Convert.ToInt32(sr.ReadLine());
        string[] brickArrangement = sr.ReadLine().Split(',');
        int index = 0;
        foreach(GameObject brick in Bricks)
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
