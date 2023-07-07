using System;

[Serializable]

public class ActEventDTO : EventDTO
{
    public String actName;
    public Pos fromPos;
    public Pos toPos;
}
