using TMPro;
using UnityEngine;

public class CharacterSpeech : MonoBehaviour
{
    private TextMeshProUGUI speechBubble;

    void Awake()
    {
        GameObject taggedBubble = GameObject.FindWithTag("SpeechBubble");
        if (taggedBubble != null)
        {
            speechBubble = taggedBubble.GetComponent<TextMeshProUGUI>();
        }
    }

    public void Say(string message, float duration = 3f)
    {
        if (speechBubble == null)
        {
            Debug.LogWarning("Speech bubble not found!");
            return;
        }

        speechBubble.text = message;
        CancelInvoke(nameof(ClearSpeech));
        Invoke(nameof(ClearSpeech), duration);
    }

    void ClearSpeech()
    {
        if (speechBubble != null)
            speechBubble.text = "";
    }
}
