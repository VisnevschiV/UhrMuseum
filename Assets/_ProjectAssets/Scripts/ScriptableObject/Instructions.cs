using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Instruction
{
    public string name;

    [TextArea]
    public string instructionText;

    public Sprite sprite;

    public AudioClip audioClip;
}


[CreateAssetMenu(fileName = "Instructions", menuName = "Scriptable Objects/Instructions")]
public class Instructions : ScriptableObject
{
    public List<Instruction> instructions;
}

