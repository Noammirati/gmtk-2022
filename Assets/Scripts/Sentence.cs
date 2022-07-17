using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{
    public char talker;

    [TextArea(3,10)]
    public string sentence;
    
    public int pic;
}
