using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button matchmaking = root.Q<Button>("Matchmaking");
        Button training = root.Q<Button>("Training");
        Button quit = root.Q<Button>("Quit");

        training.clicked += () => SceneManager.LoadScene("Training");
        quit.clicked += () => Application.Quit();

        //matchmaking button functionality goes here once multiplayer is implemented

    }
}
