using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject board;

    private boardBuilder bb;

    private int current_level = 0;
    private static string[] levels = new string[]{
        "Assets/Levels/intro.lvl",
        "Assets/Levels/level1.lvl",
        "Assets/Levels/level2.lvl",
        "Assets/Levels/level3.lvl"
    };

    // Start is called before the first frame update
    void Start()
    {
        bb = board.GetComponent<boardBuilder>();
    }

    // Update is called once per frame
    public void NextBoard()
    {
        bb.clearBoard();
        current_level++;
        bb.loadLevel(levels[current_level]);
    }
}
