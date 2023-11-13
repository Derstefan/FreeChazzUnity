using System;
using System.Collections;
using System.Text;
using Assets.Scenes.Match.Interfaces.DTOs.GameParams;
using UnityEngine;
using UnityEngine.Networking;

public class GameService
{




    public static IEnumerator createGame2(RandomGameParams randomGameParams, Action<UpdateDataDTO> callback)
    {
        // Convert RandomGameParams to JSON
        string jsonParams = JsonUtility.ToJson(randomGameParams);

        Debug.Log(jsonParams);
        // Define the URL based on the hot seat status
        string url = "http://127.0.0.1:8080/api/test/newgame2";

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
        }
        else
        {
            UpdateDataDTO updateDataDTO = JsonUtility.FromJson<UpdateDataDTO>(request.downloadHandler.text);
            Debug.Log(request.downloadHandler.text);
            callback(updateDataDTO);
        }
    }



    //create Game
    public static IEnumerator createGame(bool isHotSeat, Action<UpdateDataDTO> callback)
    {
        // Create a UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get(isHotSeat ? "http://127.0.0.1:8080/api/test/newgame" : "http://127.0.0.1:8080/api/test/newbotgame");

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
    public static IEnumerator checkUpdate(int turn, Action<UpdateDataDTO> callback)
    {
        // Create a UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:8080/api/test/update/" + turn);

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


    public static IEnumerator loadpieceTypes(int turn, Action<PieceTypeDTOCollection> callback)
    {
        // Create a UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:8080/api/test/loadAllPieceTypes/" + turn);

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
    public static IEnumerator play(Pos from, Pos to, Action<UpdateDataDTO> callback)
    {
        // Create a UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:8080/api/test/play/" + from.x + "/" + from.y + "/" + to.x + "/" + to.y + "/");

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
        UnityWebRequest request = UnityWebRequest.Get("https://jsonplaceholder.typicode.com/todos/1");

        // Send the request and wait for a response
        //yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            // Print the response body
            Debug.Log(request.downloadHandler.text);
        }
    }

}
