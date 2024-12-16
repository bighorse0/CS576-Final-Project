using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameController : MonoBehaviour
{
    [SerializeField] private int level_number;
    // UI components
    [SerializeField] private GameObject velocity_text;
    [SerializeField] private Button try_again_button;
    [SerializeField] private Button main_menu_button;
    [SerializeField] private Button next_level_button;
    [SerializeField] private GameObject menu_text;
    [SerializeField] private GameObject fail_background;
    [SerializeField] private GameObject win_background;

    private AudioSource source;
    public AudioClip wind;
    public AudioClip crash;
    public AudioClip win_sound;

    private bool is_playing;

    private class SaveObject {
        public int current_level;
    }

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        // Initialize UI element visibilities
        try_again_button.onClick.AddListener(TryAgain);
        try_again_button.gameObject.SetActive(false);
        main_menu_button.onClick.AddListener(GoMainMenu);
        main_menu_button.gameObject.SetActive(false);
        next_level_button.onClick.AddListener(GoNextLevel);
        next_level_button.gameObject.SetActive(false);
        velocity_text.gameObject.SetActive(true);
        menu_text.gameObject.SetActive(false);
        fail_background.gameObject.SetActive(false);
        win_background.gameObject.SetActive(false);

        is_playing = true;
        StartCoroutine(PlayWindSound());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // If a save file exists, updates fields
    // If a not, new save file created with current progress
    private void SaveProgress() {
        if (File.Exists(Application.dataPath + "/save.txt")) {
            Debug.Log("Save file found");

            // load string from save file
            string save_string = File.ReadAllText(Application.dataPath + "/save.txt");
            // deserialize string back into json
            SaveObject save_object = JsonUtility.FromJson<SaveObject>(save_string);
            
            // only update save object's current level if it is 
            // less than or equal to the level just completed
            if (save_object.current_level <= level_number) {
                save_object.current_level = (level_number + 1);
            } 

            // serialize save object back into json and save to file
            string json = JsonUtility.ToJson(save_object);
            File.WriteAllText(Application.dataPath + "/save.txt", json);

            Debug.Log("Updated save file with current progress");
        }
        else {
            Debug.Log("No save file found");

            // create a default save object
            SaveObject save_object = new SaveObject {
                current_level = (level_number + 1)
            };

            // serialize save object into json and save to file
            string json = JsonUtility.ToJson(save_object);
            File.WriteAllText(Application.dataPath + "/save.txt", json);

            Debug.Log("Created new save file with current progress");
        }
    }

    // Called on ground collision by GroundHandler
    public void Fail() {
        Debug.Log("Fail");

        source.Stop();
        is_playing = false;
        source.PlayOneShot(crash);
        // Set visibility of UI elements for failing screen
        velocity_text.gameObject.SetActive(false);
        fail_background.gameObject.SetActive(true);
        menu_text.GetComponent<Text>().text = "You Crashed!";
        menu_text.gameObject.SetActive(true);
        try_again_button.gameObject.SetActive(true);
        main_menu_button.gameObject.SetActive(true);
    }

    // Called on goal collision by GoalHandler
    public void Win() {
        Debug.Log("Win");

        source.Stop();
        is_playing = false;
        source.PlayOneShot(win_sound);
        // Set visibility of UI elements for winning screen
        velocity_text.gameObject.SetActive(false);
        win_background.gameObject.SetActive(true);
        menu_text.GetComponent<Text>().text = "You Won!";
        menu_text.gameObject.SetActive(true);
        try_again_button.gameObject.SetActive(true);
        main_menu_button.gameObject.SetActive(true);
        // Only display next level button if it isn't the last level
        if (level_number < 5) {
            next_level_button.gameObject.SetActive(true);
        }

        // Save progress into save file
        SaveProgress();
    }

    // Reloads current scene
    private void TryAgain() {
        Debug.Log("Reloading Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Loads MainMenu scene
    private void GoMainMenu() {
        Debug.Log("Loading Main Menu");
        SceneManager.LoadScene("MainMenu");
    }

    // Loads scene of next level
    private void GoNextLevel() {
        Debug.Log("Loading Next Level");
        string next_level_string = "Level_" + (level_number + 1);
        SceneManager.LoadScene(next_level_string);
    }

    IEnumerator PlayWindSound() {
        float wind_length = wind.length;
        while (true) {
            if (is_playing) {
                source.PlayOneShot(wind, 2f);
            }
            yield return new WaitForSeconds(wind_length);
        }
    }
}
