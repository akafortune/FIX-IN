using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
using static SceneTransition;
using UnityEditor;
//using static UnityEngine.UIElements;

public class HowToPlay : MonoBehaviour
{
    //Strings
    [TextArea(10,10)]
    [SerializeField] private string helpText0;
    [TextArea(10,10)]
    [SerializeField] private string helpText1;
    [TextArea(10,10)]
    [SerializeField] private string helpText2;
    [TextArea(10,10)]
    [SerializeField] private string helpText3;
    [TextArea(10,10)]
    [SerializeField] private string helpText4;
    private string helpText5 = "";



    private string[] HelpText;

    [SerializeField] int page = 0;

    private static VideoPlayer[] helpVideo0;
    private static VideoPlayer[] helpVideo1;
    private static VideoPlayer[] helpVideo2;
    private static VideoPlayer[] helpVideo3;
    private static VideoPlayer[] helpVideo4;

    private VideoPlayer[][] HelpVideo;

    [SerializeField] private GameObject PNG1;
    [SerializeField] private GameObject PNG2;
    [SerializeField] private GameObject RI1;
    [SerializeField] private GameObject RI2;

    [SerializeField] private GameObject videoHolder0;
    [SerializeField] private GameObject videoHolder1;
    [SerializeField] private GameObject videoHolder2;
    [SerializeField] private GameObject videoHolder3;
    [SerializeField] private GameObject videoHolder4;

    [SerializeField] private RenderTexture videoTexture1;
    [SerializeField] private RenderTexture videoTexture2;

    [SerializeField] private TextMeshProUGUI helpTextUI;
    [SerializeField] private TextMeshProUGUI modeUI;

    [SerializeField] private Image brick;

    [SerializeField] private GameObject Controls;

    // Start is called before the first frame update
    void Awake()
    {
        HelpText = new string[] {helpText0, helpText1, helpText2, helpText3, helpText4, helpText5};
        page = 0;
        helpTextUI.text = HelpText[page];
        helpVideo0 =  new VideoPlayer[] {videoHolder0.transform.GetChild(0).GetComponent<VideoPlayer>(), videoHolder0.transform.GetChild(1).GetComponent<VideoPlayer>()};   
        helpVideo1 =  new VideoPlayer[] {videoHolder1.transform.GetChild(0).GetComponent<VideoPlayer>(), videoHolder1.transform.GetChild(1).GetComponent<VideoPlayer>()};
        helpVideo2 =  new VideoPlayer[] {videoHolder2.transform.GetChild(0).GetComponent<VideoPlayer>(), videoHolder2.transform.GetChild(1).GetComponent<VideoPlayer>()};
        helpVideo3 =  new VideoPlayer[] {videoHolder3.transform.GetChild(0).GetComponent<VideoPlayer>(), videoHolder3.transform.GetChild(1).GetComponent<VideoPlayer>()};
        helpVideo4 =  new VideoPlayer[] {videoHolder4.transform.GetChild(0).GetComponent<VideoPlayer>(), videoHolder4.transform.GetChild(1).GetComponent<VideoPlayer>()};

        HelpVideo = new VideoPlayer[][] {helpVideo0, helpVideo1, helpVideo2, helpVideo3, helpVideo4};
        brick.color = new Color(0f, 0f, 255f, 1f);
        Controls.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapPage(int newPage)
    {
        page += newPage;
        if (page < 0 || page > 5)
        {
            GameObject.Find("Crossfade").GetComponent<SceneTransition>().LoadLevelTransition(0, "");
        }
        else
        {
            helpTextUI.text = HelpText[page];
            foreach(VideoPlayer[] vp in HelpVideo)
            {
                foreach (VideoPlayer v in vp)
                {
                    v.targetTexture = null;
                    v.time = 0;
                }
            }
            if (page == 5)
            {
                PNG1.SetActive(false);
                PNG2.SetActive(false);
                Controls.SetActive(true);
                RI1.SetActive(false);
                RI2.SetActive(false);
                modeUI.text = "";
                brick.color = new Color(0.754717f, 0.754717f, 0.754717f, 1f);
            }
            else
            {
                PNG1.SetActive(true);
                PNG2.SetActive(true);
                RI1.SetActive(true);
                RI2.SetActive(true);
                Controls.SetActive(false);
                bool first = true;
                foreach(VideoPlayer v in HelpVideo[page])
                {
                    if (first)
                    {
                        v.time = 0;
                        v.targetTexture = videoTexture1;
                        first = false;
                    }
                    else
                    {
                        v.time = 0;
                        v.targetTexture = videoTexture2;
                    }
                }

                if (page < 3) //if build phase
                {
                    brick.color = new Color(0f, 0f, 1f, 1f);
                    modeUI.text = "Build Phase";
                }
                else if (page == 3 || page == 4) //if defend phase
                {
                    brick.color = new Color(1f, 0f, 0f, 1f);
                    modeUI.text = "Defend Phase";
                }
            }

        }

    }
}
