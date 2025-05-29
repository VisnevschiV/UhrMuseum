using UnityEngine;

public class NextStageOnGrab : MonoBehaviour
{
   
    public void OnGrab()
    {
        GameManager.Instance.NextStage();
    }
}