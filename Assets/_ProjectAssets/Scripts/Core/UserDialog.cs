using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserDialog : MonoBehaviour
{
    [SerializeField] private Instructions instructions;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RawImage pieceImage;
    private Coroutine _revealCoroutine;

    [ContextMenu("Show Next Instruction")]
    public void ShowInstruction(Instruction instruction)
    {
        string fullText = $"<b>{instruction.name}</b>\n\n{instruction.instructionText}";

        if (_revealCoroutine != null)
            StopCoroutine(_revealCoroutine);

        _revealCoroutine = StartCoroutine(FadeInOutText(instruction.audioClip, instruction.sprite, fullText, 1f, 1f));
    }

    private IEnumerator FadeInOutText(AudioClip audioClip, Texture2D texture, string fullText, float fadeInTime, float fadeOutTime)
    {
        yield return StartCoroutine(FadeTextAlpha(1f, 0f, fadeOutTime));

        text.text = fullText;
        if(texture == null)
        {
            pieceImage.texture = null;
        }
        else
        {
            pieceImage.texture = texture;
        }
        AIVoice.PlayAudioOneShoot(audioClip);

        yield return StartCoroutine(FadeTextAlpha(0f, 1f, fadeInTime));
    }

    private IEnumerator FadeTextAlpha(float from, float to, float duration)
    {
        float elapsed = 0f;
        Color c = text.color;
        while (elapsed < duration)
        {
            pieceImage.color = new Color(c.r, c.g, c.b, Mathf.Lerp(from, to, elapsed / duration));
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            text.color = new Color(c.r, c.g, c.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        text.color = new Color(c.r, c.g, c.b, to);
    }
}
