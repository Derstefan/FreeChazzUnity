using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SignUpScript : MonoBehaviour
{
    Label errorMessage;
    TextField passwordField;
    TextField passwordAgainField;

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
        var emailField = root.Q<TextField>("email");
        passwordField = root.Q<TextField>("password");
        passwordAgainField = root.Q<TextField>("passwordAgain");
        var signUpButton = root.Q<Button>("signUp");
        errorMessage = root.Q<Label>("errorMessage");
        var loginLabel = root.Q<Label>("login");
        Button quitButton = root.Q<Button>("quit");

        // Register callback for the signUp button
        signUpButton.clickable.clicked += () => OnSignUp(usernameField.value, emailField.value, passwordField.value, passwordAgainField.value);

        // On click on the login label
        loginLabel.RegisterCallback<MouseUpEvent>(ev => ShowLoginMenu());
        quitButton.clicked += exitGame;
    }

    private void OnSignUp(string username, string email, string password, string passwordAgain)
    {
        if (username == "" || email == "" || password == "" || passwordAgain == "")
        {
            errorMessage.text = "All fields are required";
            errorMessage.style.display = DisplayStyle.Flex;
            return;
        }

        if (password != passwordAgain)
        {
            errorMessage.text = "Passwords do not match";
            errorMessage.style.display = DisplayStyle.Flex;
            return;
        }

        password = AuthUtil.HashPassword(password);
        StartCoroutine(AuthService.RegisterUser(username, password, email, SignUpDone));

        passwordField.value = "";
        passwordAgainField.value = "";
        Debug.Log($"Signing up with username: {username}, email: {email}, password: {password}, passwordAgain: {passwordAgain}");
    }

    private void SignUpDone(UserResponse userResponse)
    {
        if (userResponse.message != "")
        {
            errorMessage.text = "Sign Up failed: " + userResponse.message;
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

    private void ShowLoginMenu()
    {
        SceneManager.LoadScene("Login");
    }


    private void exitGame()
    {
        Application.Quit();
    }
}
