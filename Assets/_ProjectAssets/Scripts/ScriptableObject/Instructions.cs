using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NextStageConditions
{
    public float timeToNextStage = 0f;
    public string onGrabObjectName;
    public string onDropObjectName;
    public string onSnapObjectName;
}
[System.Serializable]
public class Instruction
{
    public string name;

    public Texture2D sprite;
    public AudioClip audioClip;
    [TextArea]
    public string instructionText;

    public NextStageConditions nextStageConditions = new NextStageConditions();

}


[CreateAssetMenu(fileName = "Instructions", menuName = "Scriptable Objects/Instructions")]
public class Instructions : ScriptableObject
{
    public List<Instruction> instructions;
}

