using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class History
    {
        private List<UpdateDataDTO> history = new List<UpdateDataDTO>();

     public void Add(UpdateDataDTO updateData)
       {
            history.Add(updateData);

    }


    public UpdateDataDTO get(int i)
        {
        return history[i];
        }

}
