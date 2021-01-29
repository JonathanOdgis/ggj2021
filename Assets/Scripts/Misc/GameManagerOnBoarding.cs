using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerOnBoarding : MonoBehaviour
{

    public static GameManagerOnBoarding Instance;

    [SerializeField]
    TMPro.TMP_Text scoreText;

    int newScore;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateScore(int pointsEarned)
    {
        newScore += pointsEarned;
        scoreText.text = newScore.ToString();
    }

}
