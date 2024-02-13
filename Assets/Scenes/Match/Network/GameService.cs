using System;
using System.Collections;
using System.Text;
using Assets.Scenes.Match.Interfaces.DTOs.GameParams;
using UnityEngine;
using UnityEngine.Networking;

public class GameService
{

    public static string URL = Config.backendUrl;

    public static IEnumerator createGame(RandomGameParams randomGameParams, Action<string> callback)
    {
        // Convert RandomGameParams to JSON
        string jsonParams = JsonUtility.ToJson(randomGameParams);

        // Define the URL based on the hot seat status
        string url = URL + "/api/match/createrandomgame";

        UnityWebRequest request = new UnityWebRequest(url, "POST");

        // Set the request body content type to JSON
        request.SetRequestHeader("Content-Type", "application/json");

        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

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
        }
        else
        {
            string gameId = (request.downloadHandler.text);
            //remove first and last character from gameId ("...")
            gameId = gameId.Substring(1, gameId.Length - 2);
            Debug.Log(request.downloadHandler.text);
            callback(gameId);
        }
    }

    public static IEnumerator joinMatch(string gameId, Action<string> callback)
    {
        // Create a UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get(URL + "/api/match/join/" + gameId);

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            string responseGameId = (request.downloadHandler.text);
            //remove first and last character from gameId ("...")
            responseGameId = responseGameId.Substring(1, responseGameId.Length - 2);
            callback(responseGameId);
        }
    }



    //create Game
    public static IEnumerator createGame2(bool isHotSeat, Action<UpdateDataDTO> callback)
    {
        // Create a UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get(isHotSeat ? URL + "/api/test/newgame" : URL + "/api/test/newbotgame");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {

            UpdateDataDTO updateDataDTO = JsonUtility.FromJson<UpdateDataDTO>(request.downloadHandler.text);
            Debug.Log(request.downloadHandler.text);
            callback(updateDataDTO);
        }
    }


    //get Update
    public static IEnumerator checkUpdate(string gameId, int turn, Action<UpdateDataDTO> callback)
    {


        // Create a UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get(URL + "/api/match/update/" + gameId + "/" + turn);

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            UpdateDataDTO updateData = JsonUtility.FromJson<UpdateDataDTO>(request.downloadHandler.text);
            if (updateData.drawEvent != null)
            {
                //Debug.Log(JsonUtility.ToJson(updateData.drawEvent));
            }
            callback(updateData);
        }
    }


    public static IEnumerator loadpieceTypes(string gameId, int turn, Action<PieceTypeDTOCollection> callback)
    {
        // Create a UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get(URL + "/api/match/loadAllPieceTypes/" + gameId + "/" + turn);

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            PieceTypeDTOCollection pieceTypeDTOCollection = JsonUtility.FromJson<PieceTypeDTOCollection>(request.downloadHandler.text);
            callback(pieceTypeDTOCollection);
        }
    }




    //play draw
    public static IEnumerator play(string gameId, Pos from, Pos to, Action<UpdateDataDTO> callback)
    {
        // Create a UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get(URL + "/api/match/play/" + gameId + "/" + from.x + "/" + from.y + "/" + to.x + "/" + to.y + "/");

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            UpdateDataDTO updateData = JsonUtility.FromJson<UpdateDataDTO>(request.downloadHandler.text);
            if (updateData.drawEvent != null)
            {
                Debug.Log(JsonUtility.ToJson(updateData.drawEvent));
            }
            callback(updateData);
        }
    }

    //surrender
    public static void surrender()
    {
        // Create a UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get(URL + "/api/match/surrender/" + PlayerPrefs.GetString("gameId"));

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

        // Send the request and wait for a response
        request.SendWebRequest();
    }

    //get all matches of user
    public static IEnumerator getAllMatchesOfUser(Action<MatchDataCollection> callback)
    {
        string url = URL + "/api/match/matches";

        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

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
            MatchDataCollection matchDataCollection = JsonUtility.FromJson<MatchDataCollection>(response);
            callback(matchDataCollection);
        }
    }

}
