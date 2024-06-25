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
            GameObject.Find("Cost").SetActive(false);
            GameObject.Find("Current_Material").SetActive(false);
            GameObject.Find("Current_Material_Label").SetActive(false);
            GameObject.Find("Start Button").SetActive(false);
            GameObject.Find("Resources").SetActive(false);
            GameObject.Find("RoundCounter").SetActive(false);
            GameObject.Find("HighScores").SetActive(false);
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
            GreenGuy.canMove = false;
            step = 1;
        }
        switch (step)
        {
            case 1:
                if (!stepping)
                {
                    stepping = true;
                    StartCoroutine(Step1Text());
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
    private IEnumerator TextOutput(string s)
    {
        for (int n = 0; n < s.Length; n++)
        {
            textBox.text += s[n];
            yield return new WaitForSeconds(0.1f);
        }
    }

    //called when ball has been caught by box, dictates how to progress when ball is caught 
    public void BallCaught()
    {}




    // Step 1: have Blue Guy introduce and run ball WITHOUT bricks ONCE
    void Step1()
    {
        ball.gameObject.SetActive(true);
        ball.LaunchSequence();
        paddle.SetActive(true);
    }

    private IEnumerator Step1Text()
    {
        string[] lines = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "Tutorial Text/Opening.txt"));
        for (int i = 0; i < lines.Length; i++)
        {
            textBox.text = "";
            yield return StartCoroutine(TextOutput(lines[i]));
            yield return new WaitForSeconds(5);
        }
    }
}
