using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
using static MainMenu;
using UnityEditor;

public class HowToPlay : MonoBehaviour
{
    //Strings
    [TextArea(10,10)]
    [SerializeField] private string helpText1 = "";
    [TextArea(10,10)]
    [SerializeField] private string helpText2 = "";
    [TextArea(10,10)]
    [SerializeField] private string helpText3 = "";
    [TextArea(10,10)]
    [SerializeField] private string helpText4 = "";



    private string[] HelpText;

    [SerializeField] int page = 0;

    private static VideoPlayer[] helpVideo1;
    private static VideoPlayer[] helpVideo2;
    private static VideoPlayer[] helpVideo3;
    private static VideoPlayer[] helpVideo4;

    private VideoPlayer[][] HelpVideo;

    [SerializeField] private GameObject videoHolder1;
    [SerializeField] private GameObject videoHolder2;
    [SerializeField] private GameObject videoHolder3;
    [SerializeField] private GameObject videoHolder4;

    [SerializeField] private RenderTexture videoTexture1;
    [SerializeField] private RenderTexture videoTexture2;

    [SerializeField] private TextMeshProUGUI helpTextUI;

    // Start is called before the first frame update
    void Awake()
    {
        HelpText = new string[] {helpText1, helpText2, helpText3, helpText4};
        page = 0;
        helpTextUI.text = HelpText[page];
        helpVideo1 =  new VideoPlayer[] {videoHolder1.transform.GetChild(0).GetComponent<VideoPlayer>(), videoHolder1.transform.GetChild(1).GetComponent<VideoPlayer>()};   
        helpVideo2 =  new VideoPlayer[] {videoHolder2.transform.GetChild(0).GetComponent<VideoPlayer>(), videoHolder2.transform.GetChild(1).GetComponent<VideoPlayer>()};
        //helpVideo3 =  new VideoPlayer[] {videoHolder3.transform.GetChild(0).GetComponent<VideoPlayer>(), videoHolder3.transform.GetChild(1).GetComponent<VideoPlayer>()};
        //helpVideo4 =  new VideoPlayer[] {videoHolder4.transform.GetChild(0).GetComponent<VideoPlayer>(), videoHolder4.transform.GetChild(1).GetComponent<VideoPlayer>()};
        HelpVideo = new VideoPlayer[][] {helpVideo1, helpVideo2};
        foreach(VideoPlayer[] vp in HelpVideo)
        {
            foreach (VideoPlayer v in vp)
            {
                v.targetTexture = null;
            }
        }
        bool first = true;
        foreach(VideoPlayer v in HelpVideo[page])
        {
            if (first)
            {
                v.targetTexture = videoTexture1;
                first = false;
            }
            else
            {
                v.targetTexture = videoTexture2;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapPage(int newPage)
    {
        page += newPage;
        if (page < 0 || page > 3)
        {
            gameObject.GetComponent<MainMenu>().LoadScene(0);
        }
        else
        {
            helpTextUI.text = HelpText[page];
            foreach(VideoPlayer[] vp in HelpVideo)
            {
                foreach (VideoPlayer v in vp)
                {
                    v.targetTexture = null;
                }
            }
            bool first = true;
            foreach(VideoPlayer v in HelpVideo[page])
            {
                if (first)
                {
                    v.targetTexture = videoTexture1;
                    first = false;
                }
                else
                {
                    v.targetTexture = videoTexture2;
                }
            }
        }
    }
}
