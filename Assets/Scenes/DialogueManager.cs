using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public GameObject nameText;
    public GameObject dialogueText;

    public Animator animator;

    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue (Dialogue dialogue)
    {  
        animator.SetBool("isOpen", true);
        nameText.GetComponent<TMP_Text>().text = dialogue.name;
        Debug.Log("DEBUT - " + dialogue.name);

        sentences.Clear();
        foreach(string s in dialogue.sentences)
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

        string current = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(current));
        Debug.Log(current);
    }

    IEnumerator TypeSentence (string sentence)
    {
        TMP_Text d = dialogueText.GetComponent<TMP_Text>();
        d.text = "";
        foreach (char c in sentence.ToCharArray())
        {
            d.text += c;
            yield return null;
        }
    }



    void EndDialogue()
    {
        
        animator.SetBool("isOpen", false);
    }

    void Display(GameObject obj, string myText)
    {
        
    }
}
