using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {
    private void Awake() {
        transform.Find("playBtn").GetComponent<Button>().onClick.AddListener(() => {
            Time.timeScale = 1f;
            GameSceneManager.Load(GameSceneManager.Scene.GameScene);
        });
        transform.Find("quitBtn").GetComponent<Button>().onClick.AddListener(() => {
            Application.Quit();
        });

    }
}