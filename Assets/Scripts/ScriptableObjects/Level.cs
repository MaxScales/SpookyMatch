using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "World", menuName = "Level")]
public class Level : ScriptableObject
{
    //board dims
    public int width;
    public int height;

    public GameObject[] dots;

    public int[] scoreGoals;

    public int currentLevel;



}
