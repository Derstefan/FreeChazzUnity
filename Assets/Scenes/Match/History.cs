using System.Collections.Generic;
using UnityEngine;

public class History
{
    private Dictionary<int, UpdateDataDTO> history = new Dictionary<int, UpdateDataDTO>();

    public void Add(int turn, UpdateDataDTO updateData)
    {
        history.Add(turn, updateData);
    }

    public void Add(UpdateDataDTO updateData)
    {

        Debug.Log("Adding update data to history " + updateData.turn);

        history.Add(updateData.turn, updateData);
    }


    public UpdateDataDTO get(int i)
    {
        return history[i];
    }

}
