using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Brick;

public class BrickLayers : MonoBehaviour
{
    public Material colorMatA;
    public Material colorMatB;
    public GameObject bricks;
    public int brickValue;
    public int roundCount;
    public Material[] mats;
    public int color1, color2;
    public int roundsUntilChange;
    public bool canChangeMaterial;
    public bool materialHasChanged;
    private GameObject[] bricksInRow;

    // Start is called before the first frame update
    private void Start()
    {

        bricksInRow = GetBricksInLayer();
    }

    void Awake()
    {
        //int brickNumber = 1;
        int rowNumber = name.ToCharArray()[name.Length-1] -48; //correcting for weird char shenanigans

        color1 = 0;
        color2 = 1;
        roundsUntilChange = 3;
        canChangeMaterial = false;
        materialHasChanged = false;

        //mats = Resources.LoadAll<Material>("BrickMats/");
        LoadBrickMaterials();

        bricks = (GameObject)Resources.Load("Brick");
        if (rowNumber %2 == 1)
        {
            colorMatA = mats[color1];
            colorMatB = mats[color2];
        }
        else
        {
            colorMatA = mats[color2];
            colorMatB = mats[color1];
        }

        int brickNumber = 1;
        for (float f = -4.5f; f < 5f; f += 1f)
        {
            GameObject i = Instantiate(bricks, gameObject.transform);
            i.transform.localPosition = new Vector3(f, 0, 0);
            SpriteRenderer s = i.GetComponent<SpriteRenderer>();
            if (brickNumber % 2 == 1)
            {
                s.material = colorMatA;
            }
            else
            {
                s.material = colorMatB;
            }
            //s.material = colorMatA;
            Brick b = i.GetComponent<Brick>();
            b.StartForBuild();
            brickNumber++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Material[] mats = Resources.LoadAll<Material>("BrickMats/");
        bricks = (GameObject)Resources.Load("Brick");

        int rowNumber = name.ToCharArray()[name.Length - 1] - 48; //correcting for weird char shenanigans

        roundCount = BaseBuilding.getRoundCount();
        int roundCountLastDigit = roundCount % roundsUntilChange;
        //print(roundCountLastDigit);

        


        // if the round number is the amount of rounds where the change should happen
        // and the material change has not yet happened
        if (roundCountLastDigit == 0 && !materialHasChanged)
        {
            canChangeMaterial = true;
        }

        // the change in material occurs
        if (canChangeMaterial)
        {
            color1 += 2;
            color2 += 2;

            if (color1 >= mats.Length)
                color1 = 0;
            if (color2 >= mats.Length)
                color2 = 1;

            ChangeBrickMaterial();
            materialHasChanged = true;
        }

        // no longer can the material change
        if (materialHasChanged)
        {
            canChangeMaterial = false;
        }


        if(materialHasChanged && roundCountLastDigit != 0) 
        {
            materialHasChanged = false;
        }

        
    }

    public void LoadBrickMaterials()
    {
        mats = new Material[10];
        mats[0] = Resources.Load<Material>("BrickMats/BrickBlue");
        mats[1] = Resources.Load<Material>("BrickMats/BrickRed");
        mats[2] = Resources.Load<Material>("BrickMats/BrickTeal");
        mats[3] = Resources.Load<Material>("BrickMats/BrickMuavePink");
        mats[4] = Resources.Load<Material>("BrickMats/BrickViolet");
        mats[5] = Resources.Load<Material>("BrickMats/BrickYellow");
        mats[6] = Resources.Load<Material>("BrickMats/BrickLightBlue");
        mats[7] = Resources.Load<Material>("BrickMats/BrickOrange");
        mats[8] = Resources.Load<Material>("BrickMats/BrickLightGreen");
        mats[9] = Resources.Load<Material>("BrickMats/BrickPink");
    }

    public void ChangeBrickMaterial()
    {
        int brickNumber = 1;
        int rowNumber = name.ToCharArray()[name.Length - 1] - 48; //correcting for weird char shenanigans

        // determines the color arrangements on the bricks of each row
        if (rowNumber % 2 == 1)
        {
            colorMatA = mats[color1];
            colorMatB = mats[color2];
        }
        else
        {
            colorMatA = mats[color2];
            colorMatB = mats[color1];
        }

        // changes the color of the bricks to the colorMat
        foreach (GameObject b in bricksInRow)
        {
            SpriteRenderer s = b.GetComponent<SpriteRenderer>();
            if (brickNumber % 2 == 1)
            {
                s.material = colorMatA;
            }
            else
            {
                s.material = colorMatB;
            }
            brickNumber++;
        }
    }

    public GameObject[] GetBricksInLayer()
    {
        Transform[] brickTransforms = GameObject.Find(this.gameObject.name).GetComponentsInChildren<Transform>();
        GameObject[] brickArray = new GameObject[10];
        int index = 0;
        foreach(Transform t in brickTransforms)
        {
            print(t.gameObject.name);
            if (t.gameObject.name.Equals("Brick(Clone)"))
            {
                brickArray[index] = t.gameObject;
                index++;
            }
        }
        return brickArray;
    }
}
