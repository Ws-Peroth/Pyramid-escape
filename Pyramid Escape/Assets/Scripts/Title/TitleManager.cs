using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Scenes
{
    Title,
    Main,
    Game,
    End
}
public class TitleManager : MonoBehaviour
{
    [SerializeField] private Text mainText;

    [SerializeField] private Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(OnButton);
        mainText
            .DOFade(0, 1)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnButton()
    {
        SceneManager.LoadScene((int) Scenes.Main);
    }
}
