using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class TutorialManager : MonoBehaviour
{
    
    private bool firstFrame; //used to set the state after frame
    [SerializeField]
    private GreenGuy GreenGuy;
    [SerializeField]
    private GameObject BlueGuy;
    [SerializeField]
    private SpriteRenderer blueGuy;
    [SerializeField]
    private Ball ball;
    [SerializeField]
    private GameObject paddle;

    private int step;
    private bool stepping;
    private bool survived;
    [SerializeField]
    private TextMeshProUGUI textBox;

    private GameObject score;
    private GameObject head;
    private GameObject resourcesBox;
    private GameObject cost;
    private GameObject costAmounts;
    private GameObject current_Material;
    private GameObject current_Material_Label;

    private AudioSource songSource;
    private AudioClip buildSong;
    private Paddle paddleScript;

    

    [SerializeField]
    private GameObject[] Layers;

    [SerializeField]
    private SceneTransition ST; // to exit tutorial at end
    [SerializeField]
    private RoundManager RM; // to exit tutorial at end

    // Start is called before the first frame update
    void Start()
    {
        songSource = GameObject.Find("SongSource").GetComponent<AudioSource>();
        buildSong = (AudioClip) Resources.Load("SFX/BuildMusic");
        paddleScript = paddle.GetComponent<Paddle>();
        step = 0;
        firstFrame = true;
        survived = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (firstFrame == true)
        {
            BlueGuy.SetActive(true);
            BlueGuyFade(true);
            firstFrame = false;
            
            GameObject.Find("Start Button").SetActive(false);
            GameObject.Find("RoundCounter").SetActive(false);
            GameObject.Find("HighScores").SetActive(false);
            
            resourcesBox = GameObject.Find("Resources");
            resourcesBox.SetActive(false);
            cost = GameObject.Find("Cost");
            cost.SetActive(false);
            costAmounts = GameObject.Find("Resource Values");
            costAmounts.SetActive(false);
            current_Material = GameObject.Find("Current_Material");
            current_Material.SetActive(false);
            current_Material_Label = GameObject.Find("Current_Material_Label");
            current_Material_Label.SetActive(false);
            score = GameObject.Find("ScoreText");
            score.SetActive(false);
            head = GameObject.Find("HeadBox");
            head.SetActive(false);

            GameObject rebuild = GameObject.Find("Rebuild Button");
            if (rebuild != null)
            {
                rebuild.SetActive(false);
            }
            GameObject.Find("SelectedIcon").SetActive(false);
            GameObject.Find("Layer 1").SetActive(false);
            GameObject.Find("Layer 2").SetActive(false);
            GameObject.Find("Layer 3").SetActive(false);
            GameObject.Find("Layer 4").SetActive(false);
            GameObject.Find("Layer 5").SetActive(false);
            step = 1;
        }
        switch (step)
        {
            case 1:
                if (!stepping)
                {
                    songSource.clip = buildSong;
                    songSource.Play();
                    stepping = true;
                    StartCoroutine(Step1());
                }
                break;
            case 2:
                if (!stepping)
                {
                    stepping = true;
                    StartCoroutine(Step2());
                }
                break;
            case 3:
                if (!stepping)
                {
                    stepping = true;
                    StartCoroutine(Step3());
                }
                break;
            case 4:
                if (!stepping)
                {
                    BoxCollider2D[] bricks =  Layers[0].GetComponentsInChildren<BoxCollider2D>();
                    int i = 0;
                    foreach(BoxCollider2D b in bricks)
                    {
                        if (b.isTrigger == false)
                        {
                            i++;
                        }
                    } 
                    if (i >= 10)
                    {
                        Debug.Log("Step 4");
                        stepping = true;
                        StartCoroutine(Step4());
                    }
                }
                break;
            case 5:     
                if (RoundManager.GameMode == RoundManager.Mode.build)  
                {
                    step = 6;
                }
                break;
            case 6:
                if (!stepping)
                {
                    stepping = true;
                    StartCoroutine(Step6());
                }
                break;
            default: break;
        }
    }

    void TextPopulate(string text)
    {

    }

    void BlueGuyFade(bool inOut) //true = in, false = out;
    {
        if (inOut == true)
        {
             StartCoroutine(FadeIn());
        }
        else
        {
             StartCoroutine(FadeOut());
        }
            
    }

    void ShowOnlyBottomRow()
    {
        Layers[0].SetActive(true);
        Transform[] Bricks = Layers[0].GetComponentsInChildren<Transform>();
        for (int i = 0; i < Bricks.Length; i++)
        {
            if (Bricks[i].name == "Brick(Clone)")
            {
                Animator b = Bricks[i].gameObject.GetComponent<Animator>();
                b.SetBool("IsBroken", true);
                b.Play("BrokenBrick", 0, 0);
            }
        }
        for (int i = 1; i < Layers.Length; i++)
        {
            Layers[i].SetActive(true);
            Transform[] HiddenBricks = Layers[i].GetComponentsInChildren<Transform>();
            for (int j = 0; j < HiddenBricks.Length; j++)
            {
                if (HiddenBricks[j].name == "Brick(Clone)")
                {
                    Animator b = HiddenBricks[i].gameObject.GetComponent<Animator>();
                    b.SetBool("IsBroken", true);
                    b.Play("BrokenBrick", 0, 0);
                    HiddenBricks[j].GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
    }

    private IEnumerator FadeIn()
    {

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



    //called when ball has been caught by box, dictates how to progress when ball is caught 
    public void BallCaught()
    {
        switch (step)
        {
            case 1:
                    step = 2;
                    ball.gameObject.SetActive(false);
                    stepping = false;
                break;
            case 2:
                    ball.ResetBall();
                    ball.LaunchSequence();
                break;
            default: break;
            case 5:
                RoundManager.GameMode = RoundManager.Mode.build;
                RM.BeginBuild();
                survived = false;
                step = 6;
                break;
        }
    }

    public void HeadbuttTrigger()
    {
        if (step == 2)
        {
            StartCoroutine(Step2Half());
        }
    }

    public void SimulatedRound()
    {
       RoundManager.GameMode = RoundManager.Mode.defend;
       RM.BeginRound();
    }






    // Step 1: have Blue Guy introduce and run ball WITHOUT bricks ONCE
    private IEnumerator Step1()
    {
        string[] lines = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "Tutorial Text/Opening.txt"));
        yield return StartCoroutine(StepText(lines));
        BlueGuyFade(false);
        ball.gameObject.SetActive(true);
        ball.LaunchSequence();
        paddle.SetActive(true);
        paddleScript.Target = ball.transform;
    }
    //Step 2: have player hit ball 
    private IEnumerator Step2()
    {
        BlueGuyFade(true);
        head.SetActive(true);
        string[] lines = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "Tutorial Text/Explain Headbutt.txt"));
        yield return StartCoroutine(StepText(lines));
        ball.gameObject.SetActive(true);
        ball.LaunchSequence();
        BlueGuyFade(false);
    }
    //Step 2.5, explain after having player hit ball
    private IEnumerator Step2Half()
    {
        yield return new WaitForSeconds(1);
        ball.gameObject.SetActive(false);
        BlueGuyFade(true);
        score.SetActive(true);
        ScoreManager sm = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        sm.currentScore = 5;
        sm.scoreText.text = sm.currentScore.ToString();
        string[] lines = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "Tutorial Text/Explain Score.txt"));
        yield return StartCoroutine(StepText(lines));
        yield return new WaitForSeconds(1);
        step = 3;
        stepping = false;
    }
    // Step 3, let player build
    private IEnumerator Step3()
    {
        ShowOnlyBottomRow();
        resourcesBox.SetActive(true);
        cost.SetActive(true);
        costAmounts.SetActive(true);
        RoundManager bb = GameObject.Find("Bricks").GetComponent<RoundManager>();
        RoundManager.resources = 0;
        bb.resourcesText.text = "0";
        string[] lines = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "Tutorial Text/Explain Building.txt"));
        yield return StartCoroutine(StepText(lines));
        RoundManager.resources = 50;
        bb.resourcesText.text = "50";
        step = 4;
        stepping = false;
    }
    //Simulate a round
    private IEnumerator Step4()
    {
        string[] lines = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "Tutorial Text/Bricks Part 2.txt"));
        yield return StartCoroutine(StepText(lines));
        SimulatedRound();
        step = 5;
        stepping = false;
        
    }
    //finish up
    private IEnumerator Step6()
    {
        BlueGuyFade(true);
        ball.gameObject.SetActive(false);
        string[] lines;
        if (!survived)
        {
            lines = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "Tutorial Text/Test Round Loss.txt"));
            yield return StartCoroutine(StepText(lines));
        }
        else
        {
            lines = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "Tutorial Text/Test Round Win.txt"));
            yield return StartCoroutine(StepText(lines));
        }
        yield return new WaitForSeconds(2);
        lines = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "Tutorial Text/Final.txt"));
        yield return StartCoroutine(StepText(lines));
        yield return new WaitForSeconds(2);
        
        ST.LoadLevelTransition(0, "");
    }




    private IEnumerator StepText(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            textBox.text = "";
            yield return StartCoroutine(TextOutput(lines[i]));
            yield return new WaitForSeconds(1);
        }
        textBox.text = "";
    }
        private IEnumerator TextOutput(string s)
    {
        for (int n = 0; n < s.Length; n++)
        {
            textBox.text += s[n];
            if (s[n] == '.') {yield return new WaitForSeconds(0.08f);}
            else {yield return new WaitForSeconds(0.05f);}
        }
    }
}
