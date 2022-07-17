using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject board;
    public GameObject level_complete;

    AudioSource level_complete_source;

    private boardBuilder bb;
    private Dictionary <string, DialogueTrigger> dialogues;
    private DialogueManager dm;
    private bool isDialogueActive;

    private int current_level = 0;
    private static string[] levels = new string[]{
        "Assets/Levels/intro.lvl",
        "Assets/Levels/level1.lvl",
        "Assets/Levels/level2.lvl",
        "Assets/Levels/level3.lvl",
        "Assets/Levels/level4.lvl",
        "Assets/Levels/level5.lvl",
        "Assets/Levels/level6.lvl"
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

        dm = FindObjectOfType<DialogueManager>();
        var dtObj = FindObjectsOfType(typeof(DialogueTrigger));
        dialogues = new Dictionary <string, DialogueTrigger>();
        foreach (var o in dtObj)
        {
            dialogues.Add(o.name, (DialogueTrigger) o);
        }
        dialogues["Start"].TriggerDialogue();
        bb.loadLevel(levels[current_level], pos[current_level]);

        level_complete = GameObject.Find("levelComplete");
        level_complete_source = level_complete.GetComponent<AudioSource>();
    }

    public void NextBoard()
    {
        if (current_level == 0){
            dialogues["Reussi1"].TriggerDialogue();
        }
        bb.clearBoard();
        level_complete_source.Play();
        current_level++;
        if (current_level < levels.Length)
        {   
            bb.loadLevel(levels[current_level], pos[current_level]);
        } else 
        {
            dialogues["End"].TriggerDialogue(() => SceneManager.LoadScene("EndScene"));
        }
    }

    public bool canPlay()
    {
        return !isDialogueActive;
    }

    public void setDialogueActive(bool isActive)
    {
        isDialogueActive = isActive;
    }
}
