using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class GameService
{


    //create Game
    public static IEnumerator createGame(Action<UpdateDataDTO> callback)
    {
        // Create a UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:8080/api/test/newgame");

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
    public static IEnumerator checkUpdate(int turn,Action<UpdateDataDTO> callback)
    {
        // Create a UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:8080/api/test/update/"+turn);

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
            //Debug.Log(request.downloadHandler.text);
            callback(updateData);
        }
    }




    //play draw
    public static IEnumerator play(Pos from, Pos to)
    {
        // Create a UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:8080/api/test/play/"+from.x+"/"+from.y+"/"+to.x+"/"+to.y+"/");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

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
