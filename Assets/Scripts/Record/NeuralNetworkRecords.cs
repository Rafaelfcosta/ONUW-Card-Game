using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class NeuralNetworkRecords : MonoBehaviour
{
    public OrderedDictionary playerRecords = new OrderedDictionary();

    void Start()
    {
        playerRecords.Add("test", "aaa");
    }

}
