using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public Text dialoguePugText;
    public Text dialogueHumanText;

    public Animator animator;

    private Queue<Sentence> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<Sentence>();
    }

    public void StartDialogue (Dialogue dialogue)
    {  
        //animator.SetBool("isOpen", true);

        sentences.Clear();
        foreach(Sentence s in dialogue.sentences)
        {
            sentences.Enqueue(s);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0){
            EndDialogue();
            return;
        }

        Sentence current = sentences.Dequeue();
        clearTexts();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(current));
        Debug.Log(current.talker == 'P' ? "Sprinkles" : "Human" + " : " + current.sentence);
    }

    void clearTexts()
    {
        dialoguePugText.text = "";
        dialogueHumanText.text = "";
    }

    IEnumerator TypeSentence (Sentence sentence)
    {
        Text d = sentence.talker == 'P' ? dialoguePugText : dialogueHumanText;
        d.text = "";
        foreach (char c in sentence.sentence.ToCharArray())
        {
            d.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }



    void EndDialogue()
    {
        clearTexts();
        Debug.Log("End of Convo");
        //animator.SetBool("isOpen", false);
    }
}
