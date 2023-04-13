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
    }

    private void LoadTraining()
    {
        SceneManager.LoadScene("Training");
        DisableAllScreens();
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
        GameObject.FindGameObjectWithTag("Relay").GetComponent<Matchmaking>().JoinGame(joinCode.text);
    }

    private void ReturnToMainMenu()
    {
        DisableAllScreens();
        mainMenu.style.display = DisplayStyle.Flex;
    }

    private void DisableAllScreens()
    {
        multiplayerMenu.style.display = DisplayStyle.None;
        mainMenu.style.display = DisplayStyle.None;
        joinGameMenu.style.display = DisplayStyle.None;
        hostSettings.style.display = DisplayStyle.None;
        code.text = null;
    }
}
