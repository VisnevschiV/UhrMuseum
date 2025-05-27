using UnityEngine;
using UnityEngine.Rendering;

public enum FeedbackType
{
    IncorrectSnap,
    IncorrectPiece,
    CorrectPiece
}

public class AIVoice : MonoBehaviour
{
    public static AIVoice Instance { get; private set; }
    public static AudioSource audioSource;

    [SerializeField] private AudioClip incorrectSnap;
    [SerializeField] private AudioClip incorrectPiece;
    [SerializeField] private AudioClip correctPiece;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }


    public static void PlayFeedback(FeedbackType feedbackType)
    {
        if (Instance == null || audioSource == null)
        {
            Debug.LogError("AIVoice instance or AudioSource is not initialized.");
            return;
        }

        AudioClip clipToPlay = null;
        switch (feedbackType)
        {
            case FeedbackType.IncorrectSnap:
                clipToPlay = Instance.incorrectSnap;
                break;
            case FeedbackType.IncorrectPiece:
                clipToPlay = Instance.incorrectPiece;
                break;
            case FeedbackType.CorrectPiece:
                clipToPlay = Instance.correctPiece;
                break;
        }

        if (clipToPlay != null)
        {
            PlayAudio(clipToPlay);
        }
        else
        {
            Debug.LogWarning("No audio clip assigned for this feedback type.");
        }
    }

    public static void PlayAudioOneShoot(AudioClip audioClip)
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not initialized.");
            return;
        }

        if (audioClip == null)
        {
            Debug.LogWarning("AudioClip is null, nothing to play.");
            return;
        }

        audioSource.PlayOneShot(audioClip);
    }

    public static void PlayAudio(AudioClip audioClip)
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not initialized.");
            return;
        }

        if (audioClip == null)
        {
            Debug.LogWarning("AudioClip is null, nothing to play.");
            return;
        }

        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public static void StopAudio()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not initialized.");
            return;
        }

        audioSource.Stop();
    }

    public static void PauseAudio()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not initialized.");
            return;
        }

        audioSource.Pause();
    }

    public static void UnPauseAudio()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not initialized.");
            return;
        }

        audioSource.UnPause();
    }

}
