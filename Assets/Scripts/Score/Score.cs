using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score
{
    public int points;
    public string name;
    public Score(int points, string name)
    {
        this.points = points;
        this.name = name;
    }

    public string toString()
    {
        return name + " " + points;
    }
}
