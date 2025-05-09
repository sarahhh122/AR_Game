using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;   
public class QuizManager : MonoBehaviour
{
    public GameObject quizPanel;
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public GameObject winPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI correctText;
    public TextMeshProUGUI wrongText;


    //celebrationnn
    public AudioSource audioSource;
    public AudioClip victorySound;
    public AudioClip voiceLineForLevel;
    public AudioClip voiceLine;
    public ParticleSystem confettiEffect;
    public GameObject bonusCharacter;

    [System.Serializable]
    public class QuizQuestion
    {
        public string question;
        public string[] answers;
        public int correctIndex;
    }

    public QuizQuestion[] questions;
    private int currentQuestionIndex = 0;
    // private int questionsAnswered = 0;
    private int score = 0;

    void Start()
    {
        quizPanel.SetActive(false);
        winPanel.SetActive(false);
        if (correctText != null) correctText.text = "";
        if (wrongText != null) wrongText.text = "";
    }

    public void ShowQuestion()
    {
        if (currentQuestionIndex >= questions.Length) return;

        quizPanel.SetActive(true);
        correctText.text = "";
        wrongText.text = "";
        var q = questions[currentQuestionIndex];
        questionText.text = q.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; 
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }
    }

    public void OnAnswerSelected(int selectedIndex)
    {
        var q = questions[currentQuestionIndex];
        if (selectedIndex == q.correctIndex)
        {
            score += 5;
            if (correctText != null)
               correctText.text = "Correct Answer :)) Your score is: " + score;

        }
        else
        {
            if (wrongText != null)
               wrongText.text = "Wrong Answer :((( Your score is " + score;
        }

        currentQuestionIndex++;
        quizPanel.SetActive(false);
        Invoke(nameof(NextStep), 1f);

    }
void NextStep()
{
    if (currentQuestionIndex >= questions.Length)
    {
        // winPanel.SetActive(true);
        if (correctText != null) correctText.text = "";
        if (wrongText != null) wrongText.text = "";

        if (finalScoreText != null)
        {
            if (score == questions.Length * 5)
            {
                finalScoreText.text = "CONGRATULATIONS! You got all answers correct!\nFinal Score: " + score;
                if (audioSource != null && victorySound != null)
                StartCoroutine(PlaySequentialSounds());

                if (confettiEffect != null)
                        confettiEffect.Play();

                if (bonusCharacter != null)
                {
                        Vector3 spawnPos = Camera.main.transform.position + Camera.main.transform.forward * 1.5f - new Vector3(0, 0.5f, 0);
                        Quaternion spawnRot = new Quaternion(0.04252093f, 0.9915443f, -0.00263807f, 0.12257615f);
                        Instantiate(bonusCharacter, spawnPos, spawnRot);
                }           
            }
            else
            {
                finalScoreText.text = "You finished the quiz!\nYour final score is: " + score;
            }

        }
    }
    else
    {
        ShowQuestion();
    }
}
IEnumerator PlaySequentialSounds()
{
    if (audioSource != null && voiceLine != null)
    {
        audioSource.PlayOneShot(voiceLine);
        yield return new WaitForSeconds(voiceLine.length);
    }

    if (audioSource != null && victorySound != null)
    {
        audioSource.PlayOneShot(victorySound);
        yield return new WaitForSeconds(victorySound.length);

    }

        if (audioSource != null && voiceLineForLevel != null)
    {
        audioSource.PlayOneShot(voiceLineForLevel);
        yield return new WaitForSeconds(voiceLineForLevel.length);
    }

        SceneManager.LoadScene(4); 
}

}
