using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();

        if(m_audioSource != null )
        {
            m_audioSource.loop = true;
            m_audioSource.Play();
        }
    }
}
