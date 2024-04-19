using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _uiScore;
    [SerializeField] private TextMeshProUGUI _uiTime;
    private float hp = 3;
    private float score = 0;
    public float HP => hp;
    public float Score => score;
    public static GameManager Instance { get; private set; }

    void Start()
    {
        _uiScore.text = Score.ToString("Score: " + Score);
    }

    public void IncreaseScore(float plus)
    {
        score += plus;
        _uiScore.text = Score.ToString("Score: " + Score);
    }

    private void Awake()
    {
        Instance = this;
    }
}
