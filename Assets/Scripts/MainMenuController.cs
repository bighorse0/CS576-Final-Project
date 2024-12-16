using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuController : MonoBehaviour
{
    public Camera main_camera;

    public GameObject tutorial;

    public Button tutorial_button;
    public Button exit_tutorial_button;
    public Button prev_lev_button;
    public Button next_lev_button;
    public Button level_one_button;
    public Button level_two_button;
    public Button level_three_button;
    public Button level_four_button;
    public Button level_five_button;

    // TODO: [low priority]
    // Read these values from a save file
    private int current_level;
    private bool completed_level_one;
    private bool completed_level_two;
    private bool completed_level_three;
    private bool completed_level_four;
    private bool completed_level_five;

    private class SaveObject {
        public int current_level;
    }

    // updates completed levels to true up to current level
    private void UpdateCompletedLevels() {
        if (current_level > 1) {
            completed_level_one = true;
        }
        if (current_level > 2) {
            completed_level_two = true;
        }
        if (current_level > 3) {
            completed_level_three = true;
        }
        if (current_level > 4) {
            completed_level_four = true;
        }
        if (current_level > 5) {
            completed_level_five = true;
        }
    }

    // Sets button corresponding to level number as inactive
    private void deactivate(int level) {
        switch (level) {
            case 1:
                Debug.Log("Deactivating LevelOneButton");
                level_one_button.gameObject.SetActive(false);
                break;
            case 2:
                Debug.Log("Deactivating LevelTwoButton");
                level_two_button.gameObject.SetActive(false);
                break;
            case 3:
                Debug.Log("Deactivating LevelThreeButton");
                level_three_button.gameObject.SetActive(false);
                break;
            case 4:
                Debug.Log("Deactivating LevelFourButton");
                level_four_button.gameObject.SetActive(false);
                break;
            case 5:
                Debug.Log("Deactivating LevelFiveButton");
                level_five_button.gameObject.SetActive(false);
                break;
            default:
                Debug.Log("Invalid Level Nuber Passed");
                break;
        }
    }

    // Sets button corresponding to level number as active
    // Also sets camera rotation accordingly
    private void activate(int level) {
        switch (level) {
            case 1:
                Debug.Log("Activating LevelOneButton");
                level_one_button.gameObject.SetActive(true);
                main_camera.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                break;
            case 2:
                Debug.Log("Activating LevelTwoButton");
                level_two_button.gameObject.SetActive(true);
                main_camera.transform.rotation = Quaternion.Euler(0.0f, 72.0f, 0.0f);
                break;
            case 3:
                Debug.Log("Activating LevelThreeButton");
                level_three_button.gameObject.SetActive(true);
                main_camera.transform.rotation = Quaternion.Euler(0.0f, 144.0f, 0.0f);
                break;
            case 4:
                Debug.Log("Activating LevelFourButton");
                level_four_button.gameObject.SetActive(true);
                main_camera.transform.rotation = Quaternion.Euler(0.0f, 216.0f, 0.0f);

                break;
            case 5:
                Debug.Log("Activating LevelFiveButton");
                level_five_button.gameObject.SetActive(true);
                main_camera.transform.rotation = Quaternion.Euler(0.0f, 288.0f, 0.0f);
                break;
            default:
                Debug.Log("Invalid Level Nuber Passed");
                break;
        }
    }

    // If a save file exists, initializes fields based on save file
    // If a not, a default save file is created and fields initialized to default
    private void LoadSave() {
        if (File.Exists(Application.dataPath + "/save.txt")) {
            Debug.Log("Save file found");

            // load string from save file
            string save_string = File.ReadAllText(Application.dataPath + "/save.txt");
            // deserialize string back into json
            SaveObject save_object = JsonUtility.FromJson<SaveObject>(save_string);

            // initialize fields based on save file fields
            current_level = save_object.current_level;
            completed_level_one = false;
            completed_level_two = false;
            completed_level_three = false;
            completed_level_four = false;
            completed_level_five = false;
            UpdateCompletedLevels();

            Debug.Log("Save loaded from file");
        }
        else {
            Debug.Log("No save file found");

            // initialize new save object fields to default values
            SaveObject default_save_object = new SaveObject {
                current_level = 1,
            };

            // serialize save object as json and save as file 
            string json = JsonUtility.ToJson(default_save_object);
            File.WriteAllText(Application.dataPath + "/save.txt", json);

            // initialize fields to default
            current_level = 1;
            completed_level_one = false;
            completed_level_two = false;
            completed_level_three = false;
            completed_level_four = false;
            completed_level_five = false;

            Debug.Log("Created new save file");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tutorial.gameObject.SetActive(false);

        // open listeners to handle button clicks
        tutorial_button.onClick.AddListener(OpenTutorial);
        exit_tutorial_button.onClick.AddListener(ExitTutorial);
        prev_lev_button.onClick.AddListener(PreviousLevel);
        next_lev_button.onClick.AddListener(NextLevel);
        level_one_button.onClick.AddListener(OpenLevelOne);
        level_two_button.onClick.AddListener(OpenLevelTwo);
        level_three_button.onClick.AddListener(OpenLevelThree);
        level_four_button.onClick.AddListener(OpenLevelFour);
        level_five_button.onClick.AddListener(OpenLevelFive);

        LoadSave();

        // deactivate all level buttons that aren't the current level
        for (int i = 1; i <= 5; i++) {
            if (i != current_level) {
                deactivate(i);
            }
        }

        // set current level button as active
        activate(current_level);
    }

    void OpenTutorial() {
        tutorial.gameObject.SetActive(true);
    }

    void ExitTutorial() {
        tutorial.gameObject.SetActive(false);
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