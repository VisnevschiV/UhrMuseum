using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserDialog : MonoBehaviour
{
    [SerializeField] private Instructions instructions;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image pieceImage;

    private int _index;
    private Coroutine _revealCoroutine;

    [ContextMenu("Show Next Instruction")]
    public void ShowInstruction()
    {
        if (instructions == null || instructions.instructions == null || _index < 0 || _index >= instructions.instructions.Count)
        {
            Debug.LogWarning("Invalid instruction index or instructions not set.");
            return;
        }

        Instruction instruction = instructions.instructions[_index];
        string fullText = $"<b>{instruction.name}</b>\n\n{instruction.instructionText}";
        _index++;

        if (_revealCoroutine != null)
            StopCoroutine(_revealCoroutine);

        _revealCoroutine = StartCoroutine(FadeInOutText(instruction.audioClip, instruction.sprite, fullText, 1f, 1f));
    }

    private IEnumerator FadeInOutText(AudioClip audioClip, Sprite sprite, string fullText, float fadeInTime, float fadeOutTime)
    {
        yield return StartCoroutine(FadeTextAlpha(1f, 0f, fadeOutTime));

        text.text = fullText;
        pieceImage.sprite = sprite;
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
