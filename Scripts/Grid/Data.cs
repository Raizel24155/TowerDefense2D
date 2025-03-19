using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Data
{
    public Dictionary<Vector2Int, bool> canPutBuilding = new Dictionary<Vector2Int, bool>();
    private static Data instance;
    public static Data Instance
    {
        get 
        { 
            if(instance == null)
            {
                instance = new Data();
            }
            return instance;
        }
    }
    public void BudynekZniszczony(Vector3 point)
    {
        if (Data.Instance.canPutBuilding.TryGetValue(Vector2Int.FloorToInt(point), out bool value))
        {
            if (value == true)
            {
                // Zmienic bool na to ze miejsce jest puste
                // Change bool to false to show that this place is empty
                Data.Instance.canPutBuilding[Vector2Int.FloorToInt(point)] = false;
            }
        }
    }

    private Data()
    {
    }

}  
