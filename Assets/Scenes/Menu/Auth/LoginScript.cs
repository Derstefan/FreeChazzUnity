using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoginScript : MonoBehaviour
{

    Label errorMessage;
    TextField passwordField;
    private void OnEnable()
    {

        if (PlayerPrefs.HasKey("username") && PlayerPrefs.HasKey("token"))
        {
            SceneManager.LoadScene("Menu");
            return;
        }

        PlayerPrefs.DeleteAll();
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // Find elements in the visual tree
        var usernameField = root.Q<TextField>("username");
        passwordField = root.Q<TextField>("password");
        var loginButton = root.Q<Button>("login");
        errorMessage = root.Q<Label>("errorMessage");
        var signUpLabel = root.Q<Label>("signUp");
        Button quitButton = root.Q<Button>("quit");

        // Register callback for the login button
        loginButton.clickable.clicked += () => OnLogin(usernameField.value, passwordField.value);

        //onclick on the sign up label
        signUpLabel.RegisterCallback<MouseUpEvent>(ev => showSignUpMenu());
        quitButton.clicked += exitGame;
    }

    private void OnLogin(string username, string password)
    {
        if (username == "" || password == "")
        {
            errorMessage.text = "Username and password are required";
            errorMessage.style.display = DisplayStyle.Flex;
            return;
        }

        password = AuthUtil.HashPassword(password);
        passwordField.value = "";

        StartCoroutine(AuthService.LoginUser(username, password, LoginDone));
    }

    private void LoginDone(UserResponse userResponse)
    {
        if (userResponse.message != "")
        {
            errorMessage.text = "Login failed: " + userResponse.message;
            errorMessage.style.display = DisplayStyle.Flex;
            return;
        }
        errorMessage.style.display = DisplayStyle.None;

        PlayerPrefs.SetString("userId", userResponse.userId);
        PlayerPrefs.SetString("token", userResponse.token);
        PlayerPrefs.SetString("username", userResponse.username);
        PlayerPrefs.SetString("email", userResponse.email);

        SceneManager.LoadScene("Menu");
    }

    private void showSignUpMenu()
    {
        SceneManager.LoadScene("SignUp");
    }

    private void exitGame()
    {
        Application.Quit();
    }

}
