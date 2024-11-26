using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Camera main_camera;

    public Button tutorial_button;
    public Button prev_lev_button;
    public Button next_lev_button;
    public Button level_one_button;
    public Button level_two_button;
    public Button level_three_button;
    public Button level_four_button;
    public Button level_five_button;

    // TODO: [low priority]
    // Read these values from a save file
    public int current_level;
    public bool completed_level_one;
    public bool completed_level_two;
    public bool completed_level_three;
    public bool completed_level_four;
    public bool completed_level_five;

    // Sets button corresponding to level number as inactive
    void deactivate(int level) {
        switch (level) {
            case 1:
                Debug.Log("Deactivating LevelOneButton");
                level_one_button.gameObject.SetActive(false);
                break;
            case 2:
                Debug.Log("Deactivating LevelOneButton");
                level_two_button.gameObject.SetActive(false);
                break;
            case 3:
                Debug.Log("Deactivating LevelOneButton");
                level_three_button.gameObject.SetActive(false);
                break;
            case 4:
                Debug.Log("Deactivating LevelOneButton");
                level_four_button.gameObject.SetActive(false);
                break;
            case 5:
                Debug.Log("Deactivating LevelOneButton");
                level_five_button.gameObject.SetActive(false);
                break;
            default:
                Debug.Log("Invalid Level Nuber Passed");
                break;
        }
    }

    // Sets button corresponding to level number as active
    // Also sets camera rotation accordingly
    void activate(int level) {
        switch (level) {
            case 1:
                Debug.Log("Activating LevelOneButton");
                level_one_button.gameObject.SetActive(true);
                main_camera.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                break;
            case 2:
                Debug.Log("Activating LevelOneButton");
                level_two_button.gameObject.SetActive(true);
                main_camera.transform.rotation = Quaternion.Euler(0.0f, 72.0f, 0.0f);
                break;
            case 3:
                Debug.Log("Activating LevelOneButton");
                level_three_button.gameObject.SetActive(true);
                main_camera.transform.rotation = Quaternion.Euler(0.0f, 144.0f, 0.0f);
                break;
            case 4:
                Debug.Log("Activating LevelOneButton");
                level_four_button.gameObject.SetActive(true);
                main_camera.transform.rotation = Quaternion.Euler(0.0f, 216.0f, 0.0f);

                break;
            case 5:
                Debug.Log("Activating LevelOneButton");
                level_five_button.gameObject.SetActive(true);
                main_camera.transform.rotation = Quaternion.Euler(0.0f, 288.0f, 0.0f);
                break;
            default:
                Debug.Log("Invalid Level Nuber Passed");
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tutorial_button.onClick.AddListener(OpenTutorial);
        prev_lev_button.onClick.AddListener(PreviousLevel);
        next_lev_button.onClick.AddListener(NextLevel);
        level_one_button.onClick.AddListener(OpenLevelOne);
        level_two_button.onClick.AddListener(OpenLevelTwo);
        level_three_button.onClick.AddListener(OpenLevelThree);
        level_four_button.onClick.AddListener(OpenLevelFour);
        level_five_button.onClick.AddListener(OpenLevelFive);

        for (int i = 1; i <= 5; i++) {
            if (i != current_level) {
                deactivate(i);
            }
        }
    }

    void OpenTutorial() {
        // TODO: Figure out what to do here
    }

    void PreviousLevel() {
        if (current_level == 1) {
            Debug.Log("Cannot go to previous level");
        }
        else {
            deactivate(current_level);
            current_level--;
            activate(current_level);
        }
    }

    void NextLevel() {
        if (current_level == 5) {
            Debug.Log("No more levels");
        }
        else {
            deactivate(current_level);
            current_level++;
            activate(current_level);
        }
    }

    void OpenLevelOne() {
        Debug.Log("Switching to Level_1");
        SceneManager.LoadScene("Level_1");
    }

    void OpenLevelTwo() {
        if (completed_level_one) {
            Debug.Log("Switching to Level_2");
            SceneManager.LoadScene("Level_2");
        }
        else {
            Debug.Log("Cannot load Level_2: Has not completed Level_1");
        }
    }

    void OpenLevelThree() {
        if (completed_level_two) {
            Debug.Log("Switching to Level_3");
            SceneManager.LoadScene("Level_3");
        }
        else {
            Debug.Log("Cannot load Level_3: Has not completed Level_2");
        }
    }

    void OpenLevelFour() {
        if (completed_level_three) {
            Debug.Log("Switching to Level_4");
            SceneManager.LoadScene("Level_4");
        }
        else {
            Debug.Log("Cannot load Level_4: Has not completed Level_3");
        }
    }

    void OpenLevelFive() {
        if (completed_level_four) {
            Debug.Log("Switching to Level_5");
            SceneManager.LoadScene("Level_5");
        }
        else {
            Debug.Log("Cannot load Level_5: Has not completed Level_4");
        }
    }
}
