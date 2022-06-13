using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class End : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    [SerializeField] private Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(OnButton);
        scoreText.text = $"Score : {GameDataManager.instance.Gold.ToString()}";
    }

    private void OnButton()
    {
        SceneManager.LoadScene((int) Scenes.Main);
    }
}
