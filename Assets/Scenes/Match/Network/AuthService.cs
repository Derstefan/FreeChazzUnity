using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AuthService
{
    public static string URL = Config.backendUrl;

    public static IEnumerator RegisterUser(string username, string password, string email, Action<UserResponse> callback)
    {
        string url = URL + "/api/auth/register";

        // Create JSON payload
        string jsonParams = $"{{\"username\":\"{username}\", \"password\":\"{password}\", \"email\":\"{email}\"}}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");

        // Set the request body content type to JSON
        request.SetRequestHeader("Content-Type", "application/json");

        // Convert the JSON payload to bytes and attach it to the request
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonParams);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        // Set the download handler to handle the response
        request.downloadHandler = new DownloadHandlerBuffer();

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            callback(null); // Assuming you handle null response in the callback
        }
        else
        {
            string response = request.downloadHandler.text;
            UserResponse userResponse = JsonUtility.FromJson<UserResponse>(response);
            callback(userResponse);
        }
    }


    public static IEnumerator LoginUser(string username, string password, Action<UserResponse> callback)
    {
        string url = URL + "/api/auth/login";

        // Create JSON payload
        string jsonParams = $"{{\"username\":\"{username}\", \"password\":\"{password}\"}}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");

        // Set the request body content type to JSON
        request.SetRequestHeader("Content-Type", "application/json");

        // Convert the JSON payload to bytes and attach it to the request
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonParams);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        // Set the download handler to handle the response
        request.downloadHandler = new DownloadHandlerBuffer();

        Debug.Log("Sending request " + username + " " + password);
        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            callback(null);
        }
        else
        {
            string response = request.downloadHandler.text;
            UserResponse userResponse = JsonUtility.FromJson<UserResponse>(response);
            callback(userResponse);
        }
    }



}
