using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    private int currentRound;
    private int score;
    
    public Text roundText;
    public Text scoreText;
    
    void Start()
    {
        currentRound = DefaultNamespace.GameOverGameData.CurrentRound;
        score = DefaultNamespace.GameOverGameData.Score;
        
        roundText.text = "You survived " + currentRound + " rounds!";
        scoreText.text = "Total score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
