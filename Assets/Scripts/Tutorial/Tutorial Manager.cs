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
    [SerializeField]
    private TextMeshProUGUI textBox;

    private GameObject score;
    private GameObject head;
    private GameObject resources;
    private GameObject cost;
    private GameObject current_Material;
    private GameObject current_Material_Label;
    // Start is called before the first frame update
    void Start()
    {
        step = 0;
        firstFrame = true;
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
            
            resources = GameObject.Find("Resources");
            resources.SetActive(false);
            cost = GameObject.Find("Cost");
            cost.SetActive(false);
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
            GameObject.Find("Resource Values").SetActive(false);
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
                break;
            default: break;
        }
    }

    public void HeadbuttTrigger()
    {
        if (step == 2)
        {
            StartCoroutine(Step2Half());
        }
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
        private IEnumerator Step2Half()
    {
        yield return new WaitForSeconds(1);
        ball.gameObject.SetActive(false);
        BlueGuyFade(true);
        score.SetActive(true);
        string[] lines = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "Tutorial Text/Explain Score.txt"));
        yield return StartCoroutine(StepText(lines));
    }

    private IEnumerator StepText(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            textBox.text = "";
            yield return StartCoroutine(TextOutput(lines[i]));
            yield return new WaitForSeconds(2);
        }
        textBox.text = "";
    }
        private IEnumerator TextOutput(string s)
    {
        for (int n = 0; n < s.Length; n++)
        {
            textBox.text += s[n];
            yield return new WaitForSeconds(0.1f);
        }
    }
}
