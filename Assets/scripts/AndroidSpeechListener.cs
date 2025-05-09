using UnityEngine;

public class AndroidSpeechListener : MonoBehaviour, ISpeechToTextListener
{
    public LilyManager lily;

    void Start()
    {
        SpeechToText.Initialize("en-US");
        SpeechToText.Start(this, true, false);   
    }

    // ---------- required callbacks ----------
    public void OnReadyForSpeech()               { }
    public void OnBeginningOfSpeech()            { }
    public void OnVoiceLevelChanged(float rms)   { }
    public void OnPartialResultReceived(string t){ }   
    public void OnError(string err, int code)
    {
        Debug.LogWarning($"STT error {code}: {err}");
        SpeechToText.Start(this, false, false); // retry
    }

    public void OnResultReceived(string text, int? error)
    {
        if (error != null) { OnError("", error.Value); return; }

        text = text.ToLower();
        Debug.Log("STT heard: " + text);

        if (text.Contains("attack"))
            lily.OnVoiceAttack();

        SpeechToText.Start(this, false, false);
    }
}
