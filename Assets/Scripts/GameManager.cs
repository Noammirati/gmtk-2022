using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject board;
    public GameObject level_complete;

    public static int score;

    AudioSource level_complete_source;

    private boardBuilder bb;
    private Dictionary <string, DialogueTrigger> dialogues;
    private DialogueManager dm;
    private bool isDialogueActive;

    private int current_level = 0;
    private static string[] levels = new string[]{
        "intro", "level1", "level2", "level3", "level4", "level5", "level6", "level7"
    };

    void LoadLevel1() {
        bb.loadLevel(levels[current_level]);
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        bb = board.GetComponent<boardBuilder>();
        level_complete = GameObject.Find("levelComplete");
        level_complete_source = level_complete.GetComponent<AudioSource>();

        dm = FindObjectOfType<DialogueManager>();
        var dtObj = FindObjectsOfType(typeof(DialogueTrigger));
        dialogues = new Dictionary <string, DialogueTrigger>();
        foreach (var o in dtObj)
        {
            dialogues.Add(o.name, (DialogueTrigger) o);
        }

        dialogues["Start"].TriggerDialogue(LoadLevel1);

        //Debug.Log("ici");

        //bb.loadLevel(levels[current_level]);
    }

    public void NextBoard()
    {
        level_complete_source.Play();

        void afterDialogue(){
            bb.clearBoard();
            current_level++;
            if (current_level < levels.Length)
            {
                bb.loadLevel(levels[current_level]);
            }
            else
            {
                dialogues["End"].TriggerDialogue(() => SceneManager.LoadScene("EndScene"));
            }
        }

        if (current_level == 0){
            dialogues["ReussiIntro"].TriggerDialogue(afterDialogue);
        } else {
            string k = "Reussi" + (current_level);
            if (dialogues.ContainsKey(k)){
                dialogues[k].TriggerDialogue(afterDialogue);
            } else {
                afterDialogue();
            }
        }
    }

    public void restartLevel(){
        bb.clearBoard();
        bb.resetBoard();

        var resetDialogues = new List<string> {"Echec1", "Echec2", "Echec3"};

        int n = Random.Range(0, resetDialogues.Count);
        dialogues[resetDialogues[n]].TriggerDialogue();
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
