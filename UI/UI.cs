using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UI : VisualElement
{
    public new class UxmlFactory : UxmlFactory<UI, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits { }

    public UI()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private VisualElement mainMenu;
    private Button play;
    private Button training;
    private Button quit;
    private VisualElement multiplayerMenu;
    private Button host;
    private Button join;
    private Button back;
    private VisualElement joinGameMenu;
    private Button joinConfirm;
    private Button backToMPH;
    private Button backToMPJ;
    private TextField joinCode;
    private VisualElement hostSettings;
    private TextField spin;
    private TextField bSpeed;
    private TextField pSpeed;
    private Button startHost;
    public static Label code;
    private VisualElement score;
    public static Label player1;
    public static Label player2;
    private VisualElement pauseMenu;
    public static Slider musicSlider;
    private Button resume;
    private Button exit;
    private VisualElement root;

    private bool paused = true;
    private bool inGame = false;

    private void OnGeometryChange(GeometryChangedEvent evt)
    {
        mainMenu = this.Q<VisualElement>("MainMenu");
        play = this.Q<Button>("Play");
        training = this.Q<Button>("Training");
        quit = this.Q<Button>("Quit");
        multiplayerMenu = this.Q<VisualElement>("MultiplayerMenu");
        host = this.Q<Button>("Host");
        join = this.Q<Button>("Join");
        back = this.Q<Button>("Back");
        joinGameMenu = this.Q<VisualElement>("JoinGameMenu");
        joinConfirm = this.Q<Button>("JoinConfirm");
        backToMPH = this.Q<Button>("BackToMPH");
        backToMPJ = this.Q<Button>("BackToMPJ");
        joinCode = this.Q<TextField>("JoinCode");
        hostSettings = this.Q<VisualElement>("HostSettings");
        spin = this.Q<TextField>("Spin");
        bSpeed = this.Q<TextField>("BallSpeed");
        pSpeed = this.Q<TextField>("PlayerSpeed");
        startHost = this.Q<Button>("StartHost");
        code = this.Q<Label>("Code");
        score = this.Q<VisualElement>("Score");
        player1 = this.Q<Label>("Player1");
        player2 = this.Q<Label>("Player2");
        pauseMenu = this.Q<VisualElement>("PauseMenu");
        musicSlider = this.Q<Slider>("MusicSlider");
        resume = this.Q<Button>("Resume");
        exit = this.Q<Button>("Exit");
        root = this.Q<VisualElement>("Root");
        root.focusable = true;

        play?.RegisterCallback<ClickEvent>(evt => ChooseMultiplayer());
        training?.RegisterCallback<ClickEvent>(evt => LoadTraining());
        quit?.RegisterCallback<ClickEvent>(evt => Application.Quit());
        host?.RegisterCallback<ClickEvent>(evt => Host());
        join?.RegisterCallback<ClickEvent>(evt => Join());
        back?.RegisterCallback<ClickEvent>(evt => ReturnToMainMenu());
        joinConfirm?.RegisterCallback<ClickEvent>(evt => JoinGame());
        backToMPH?.RegisterCallback<ClickEvent>(evt => BackToMultiplayer());
        backToMPJ?.RegisterCallback<ClickEvent>(evt => BackToMultiplayer());
        startHost?.RegisterCallback<ClickEvent>(evt => StartHost());
        resume?.RegisterCallback<ClickEvent>(evt => Resume());
        exit?.RegisterCallback<ClickEvent>(evt => ReturnToMainMenu());

        root?.RegisterCallback<MouseMoveEvent>(evt => root.Focus());
        root?.RegisterCallback<KeyDownEvent>(evt => Pause(evt.keyCode));
    }

    private void LoadTraining()
    {
        SceneManager.LoadScene("Training");

        DisableAllScreens();
        score.style.display = DisplayStyle.Flex;

        paused = false;
        inGame = true;
    }

    private void ChooseMultiplayer()
    {
        DisableAllScreens();
        multiplayerMenu.style.display = DisplayStyle.Flex;
    }

    private void BackToMultiplayer()
    {
        SceneManager.LoadScene("Main Menu");

        DisableAllScreens();
        ChooseMultiplayer();
    }

    private void Host()
    {
        SceneManager.LoadScene("Match");

        DisableAllScreens();
        hostSettings.style.display = DisplayStyle.Flex;
    }

    private void StartHost()
    {
        DisableAllScreens();
        //score.style.display = DisplayStyle.Flex;

        if (int.TryParse(spin.value, out int s))
            Values.spinCoefficient = s;
        else
            Values.spinCoefficient = 1;

        if (int.TryParse(bSpeed.value, out int b))
            Values.speedModifier = b;
        else
            Values.speedModifier = 1;

        if (int.TryParse(pSpeed.value, out int p))
            Values.playerSpeed = p;
        else
            Values.playerSpeed = 5;

        GameObject.FindGameObjectWithTag("Relay").GetComponent<Matchmaking>().HostGame();

        paused = false;
        inGame = true;
    }

    private void Join()
    {
        SceneManager.LoadScene("Match");

        DisableAllScreens();
        joinGameMenu.style.display = DisplayStyle.Flex;
    }

    private void JoinGame()
    {
        DisableAllScreens();
        //score.style.display = DisplayStyle.Flex;

        GameObject.FindGameObjectWithTag("Relay").GetComponent<Matchmaking>().JoinGame(joinCode.text);

        paused = false;
        inGame = true;
    }

    private void ReturnToMainMenu()
    {
        if (SceneManager.GetActiveScene().name == "Match")
            GameObject.FindGameObjectWithTag("Relay").GetComponent<Matchmaking>().ExitMatch();

        if (SceneManager.GetActiveScene().name != "Main Menu")
            SceneManager.LoadScene("Main Menu");

        DisableAllScreens();
        mainMenu.style.display = DisplayStyle.Flex;

        inGame = false;
    }

    public void Pause(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Escape && inGame)
        {
            if (paused)
                Resume();
            else
            {
                score.style.display = DisplayStyle.None;

                if (SceneManager.GetActiveScene().name == "Training")
                    Time.timeScale = 0;

                pauseMenu.style.display = DisplayStyle.Flex;

                paused = true;
            }
        }
    }

    private void Resume()
    {
        DisableAllScreens();

        if (SceneManager.GetActiveScene().name == "Training")
            score.style.display = DisplayStyle.Flex;

        paused = false;
    }

    private void DisableAllScreens()
    {
        multiplayerMenu.style.display = DisplayStyle.None;
        mainMenu.style.display = DisplayStyle.None;
        joinGameMenu.style.display = DisplayStyle.None;
        hostSettings.style.display = DisplayStyle.None;
        score.style.display = DisplayStyle.None;
        pauseMenu.style.display = DisplayStyle.None;
        code.text = null;
        Time.timeScale = 1;
    }
}
