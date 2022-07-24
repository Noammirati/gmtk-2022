using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public Text dialoguePugText;
    public Text dialogueHumanText;
    public GameObject nextButton;
    public Texture t_serious;
    public Texture t_anxious;
    public Texture t_shocked;
    public Texture t_neutralOpen;
    public Texture t_neutralClosed;
    public Texture t_laughing;
    public Texture t_angryClosed;
    public Texture t_angryOpen;

    private Queue<Sentence> sentences;
    private GameManager gm;
    private Dictionary<int, Texture> textures;
    private Dictionary<int, AudioSource> sounds;
    private Action callback;


    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<Sentence>();
        gm = FindObjectOfType<GameManager>();

        textures = new Dictionary<int, Texture>();
        textures.Add(8, t_serious);
        textures.Add(2, t_anxious);
        textures.Add(7, t_shocked);
        textures.Add(6, t_neutralOpen);
        textures.Add(4, t_neutralClosed);
        textures.Add(3, t_laughing);
        textures.Add(5, t_angryClosed);
        textures.Add(1, t_angryOpen);

        sounds = new Dictionary<int, AudioSource>();
        sounds.Add(6, GameObject.Find("rire1").GetComponent<AudioSource>());
        sounds.Add(7, GameObject.Find("rire2").GetComponent<AudioSource>());
        sounds.Add(3, GameObject.Find("rire3").GetComponent<AudioSource>());
    }

    public void StartDialogue (Dialogue dialogue)
    {  
        callback = null;
        startDialogueB(dialogue);
    }

    public void StartDialogue (Dialogue dialogue, Action callback)
    {
        this.callback = callback;
        startDialogueB(dialogue);
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0){           
            nextButton.SetActive(false);
            EndDialogue();
            return;
        }

        Sentence current = sentences.Dequeue();
        clearTexts();
        StopAllCoroutines();
        nextButton.SetActive(false);
        StartCoroutine(TypeSentence(current));

        GameObject.Find("Plane").GetComponent<Renderer>().material.SetTexture("_BaseMap", textures[current.pic]);
        GameObject.Find("Plane").GetComponent<Renderer>().material.SetTexture("_EmissionMap", textures[current.pic]);

        sounds[current.pic].Play();
    }
    
    void startDialogueB(Dialogue dialogue)
    {
        sentences.Clear();
        foreach(Sentence s in dialogue.sentences)
        {
            sentences.Enqueue(s);
        }

        gm.setDialogueActive(true);
        DisplayNextSentence();
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
            yield return new WaitForSeconds(0.03f);
        }
        nextButton.SetActive(true);
    }

    void EndDialogue()
    {
        gm.setDialogueActive(false);
        clearTexts();
        GameObject.Find("Plane").GetComponent<Renderer>().material.SetTexture("_BaseMap", t_neutralOpen);
        GameObject.Find("Plane").GetComponent<Renderer>().material.SetTexture("_EmissionMap", t_neutralOpen);
        if (callback != null)
        {
            callback();
        }
        Debug.Log("End of Convo");
    }
}
