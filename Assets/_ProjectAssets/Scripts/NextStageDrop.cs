using UnityEngine;

public class NextStageDrop : MonoBehaviour
{
    public void OnDrop()
    {
        GameManager.Instance.NextStage();
    }
}
