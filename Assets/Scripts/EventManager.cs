using System.Collections;
using System.Collections.Generic;

public class EventManager
{
    public EventManager Instance { get;private set; }

    public EventManager()
    {
        if (Instance != null && Instance != this)
        {
            return;
        }
        Instance = this;
    }

    ~EventManager()
    {

    }
}
