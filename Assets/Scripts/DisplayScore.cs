using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Text>().text = "Moves : " + GameManager.score;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Text>().text = "Moves : " + GameManager.score;
    }
}
