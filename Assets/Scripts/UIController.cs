using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject velocity_text;
    [SerializeField] private Button try_again_button;
    [SerializeField] private Button next_level_button;
    [SerializeField] private GameObject menu_text;
    [SerializeField] private GameObject fail_background;
    [SerializeField] private GameObject win_background;

    // Start is called before the first frame update
    void Start()
    {
        try_again_button.onClick.AddListener(TryAgain);
        try_again_button.gameObject.SetActive(false);
        next_level_button.onClick.AddListener(NextLevel);
        next_level_button.gameObject.SetActive(false);
        velocity_text.gameObject.SetActive(true);
        menu_text.gameObject.SetActive(false);
        fail_background.gameObject.SetActive(false);
        win_background.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FailMenu() {
        Debug.Log("Fail Menu");
        fail_background.gameObject.SetActive(true);
        menu_text.GetComponent<Text>().text = "You Crashed!";
        menu_text.gameObject.SetActive(true);
        try_again_button.gameObject.SetActive(true);
    }

    public void WinMenu() {
        win_background.gameObject.SetActive(true);
        menu_text.GetComponent<Text>().text = "You Won!";
        menu_text.gameObject.SetActive(true);
        try_again_button.gameObject.SetActive(true);
        next_level_button.gameObject.SetActive(true);
    }

    private void TryAgain() {
        Debug.Log("trying again");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void NextLevel() {
        Debug.Log("next level");
        SceneManager.LoadScene("MainMenu");
    }
}
