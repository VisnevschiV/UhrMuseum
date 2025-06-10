using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private Instructions instructions;
    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private List<GameObject> watchElelments;

    [SerializeField]
    private UserDialog userDialog;

    private int _stage = -1;

    void Start()
    {
        foreach (GameObject element in watchElelments)
        {
            element.SetActive(false);
        }
        NextStage();
    }

    public void NextStage()
    {
        PreviousStageListeners();
        _stage++;
        userDialog.ShowInstruction(instructions.instructions[_stage]);
        LevelChanges();
        NextStageListeners();
       
        
    }

    private void NextStageListeners()
    {
        if (instructions.instructions[_stage].nextStageConditions.timeToNextStage > 0)
        {
            StartCoroutine(NextStageInTime(instructions.instructions[_stage].nextStageConditions.timeToNextStage));
        }
        else if (instructions.instructions[_stage].nextStageConditions.onGrabObjectName != "")
        {
            var obj = GameObject.Find(instructions.instructions[_stage].nextStageConditions.onGrabObjectName);
            obj.AddComponent<NextStageOnGrab>();
        }
        else if (instructions.instructions[_stage].nextStageConditions.onDropObjectName != "")
        {
            var obj = GameObject.Find(instructions.instructions[_stage].nextStageConditions.onDropObjectName);
            obj.AddComponent<NextStageDrop>();
        }
        else if (instructions.instructions[_stage].nextStageConditions.onSnapObjectName != "")
        {
            var obj = GameObject.Find(instructions.instructions[_stage].nextStageConditions.onSnapObjectName);
            obj.GetComponent<SnapBehaviour>().onSnapComplete += NextStage;
        }
       
    }

    private void PreviousStageListeners()
    {
        if (_stage < 0) return;
        Debug.Log("Previous Stage Listeners for stage: " + _stage);
        var conditions = instructions.instructions[_stage].nextStageConditions;

        // Undo time-based coroutine (if any)
        StopAllCoroutines();

        // Undo onGrabObjectName
        if (!string.IsNullOrEmpty(conditions.onGrabObjectName))
        {
            var obj = GameObject.Find(conditions.onGrabObjectName);
            if (obj != null)
            {
                var grabComp = obj.GetComponent<NextStageOnGrab>();
                if (grabComp != null)
                    Destroy(grabComp);
            }
        }

        // Undo onDropObjectName
        if (!string.IsNullOrEmpty(conditions.onDropObjectName))
        {
            var obj = GameObject.Find(conditions.onDropObjectName);
            if (obj != null)
            {
                var dropComp = obj.GetComponent<NextStageDrop>();
                if (dropComp != null)
                    Destroy(dropComp);
            }
        }

        // Undo onSnapObjectName
        if (!string.IsNullOrEmpty(conditions.onSnapObjectName))
        {
            var obj = GameObject.Find(conditions.onSnapObjectName);
            if (obj != null)
            {
                var snap = obj.GetComponent<SnapBehaviour>();
                if (snap != null)
                    snap.onSnapComplete -= NextStage;
            }
        }
    }

    private void LevelChanges()
    {
        if (_stage == 5)
        {
            ball.SetActive(false);
            foreach (GameObject element in watchElelments)
            {
                element.SetActive(true);
            }
        }

    }

    
    private IEnumerator NextStageInTime(float time)
    {
        yield return new WaitForSeconds(time);
        NextStage();
    }
}
