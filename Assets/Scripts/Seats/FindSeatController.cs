using System.Collections;
using System.Collections.Generic;
using SharpNeat.Phenomes;
using UnityEngine;
using UnitySharpNEAT;

public class FindSeatController : UnitController
{
    // Start is called before the first frame update
    void Start()
    {
        TableFillerController tbc = GameObject.Find("TableFiller").GetComponent<TableFillerController>();
        Transform parent = tbc.getTable();
        if (parent != null)
        {
            transform.SetParent(parent);
            name = "Player" + (transform.GetSiblingIndex() + 1);
        }
    }

    // public new void FixedUpdate()
    // {

    // }

    public override float GetFitness()
    {
        // throw new System.NotImplementedException();
        return Random.Range(0, 5);
    }

    protected override void HandleIsActiveChanged(bool newIsActive)
    {
        // throw new System.NotImplementedException();
        // Debug.Log("trid to change active");
        gameObject.SetActive(newIsActive);
    }

    protected override void UpdateBlackBoxInputs(ISignalArray inputSignalArray)
    {
        for (int i = 0; i < 90; i++)
        {
            inputSignalArray[i] = Random.Range(0, 2);
        }
    }

    protected override void UseBlackBoxOutpts(ISignalArray outputSignalArray)
    {
        // Debug.Log(outputSignalArray[0]);
        // Debug.Log(outputSignalArray[1]);
        // Debug.Log(outputSignalArray[2]);
        // Debug.Log(outputSignalArray[3]);
    }

}
