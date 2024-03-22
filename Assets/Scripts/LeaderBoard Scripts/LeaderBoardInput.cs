using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardInput : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI score;
    
    public void SetInput(string username, int score)
    {
        this.score.text = score.ToString();
    }
}
