using System.Collections.Generic;

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
