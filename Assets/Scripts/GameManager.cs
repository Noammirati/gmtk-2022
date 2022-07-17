using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject board;
    public GameObject level_complete;

    AudioSource level_complete_source;

    private boardBuilder bb;

    private int current_level = 0;
    private static string[] levels = new string[]{
        "Assets/Levels/test.lvl",
        "Assets/Levels/level1.lvl",
        "Assets/Levels/level2.lvl",
        "Assets/Levels/level3.lvl"
    };

    private static Vector3[] pos = new Vector3[]{
        new Vector3(-0.5f, 1f, -0.5f),
        new Vector3(-1.5f, 1f, -1.5f),
        new Vector3(-2.68f, 1f, -3.26f),
        new Vector3(-4.32f, 1f, -4.85f),
    };

    // Start is called before the first frame update
    void Start()
    {
        bb = board.GetComponent<boardBuilder>();
        bb.loadLevel(levels[current_level], pos[current_level]);

        level_complete = GameObject.Find("levelComplete");
        level_complete_source = level_complete.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void NextBoard()
    {
        bb.clearBoard();
        level_complete_source.Play();
        current_level++;
        bb.loadLevel(levels[current_level], pos[current_level]);
    }
}
