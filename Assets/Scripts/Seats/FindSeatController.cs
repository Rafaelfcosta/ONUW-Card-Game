using System.Collections;
using System.Collections.Generic;
using SharpNeat.Phenomes;
using UnityEngine;
using UnitySharpNEAT;

public class FindSeatController : MonoBehaviour
{
    private List<Vector3> positions = new List<Vector3>();
    void Start()
    {
        positions.Add(new Vector3(-871.5f, -419f, 0));
        positions.Add(new Vector3(-871.5f, 418.5f, 0));
        positions.Add(new Vector3(871.5f, 418.5f, 0));
        positions.Add(new Vector3(871.5f, -419f, 0));

        TableFillerController tbc = GameObject.Find("TableFiller").GetComponent<TableFillerController>();
        SeatController table = tbc.getTable();
        seat(table);
    }

    public void seat(SeatController table)
    {
        if (table != null)
        {
            transform.SetParent(table.transform);
            name = "Player" + table.getSLOTS();
            transform.localPosition = positions[table.getSLOTS() - 1];
            GetComponent<PlayerBase>().dialogBox = transform.parent.gameObject.FindComponentInChildWithTag<Transform>("dialog").gameObject.transform.GetChild(table.getSLOTS() - 1).gameObject;

            if (table.getSLOTS() > 3)
            {
                table.GetComponent<GameController>().initialSetup();
            }
        }
        // else
        // {
        //     transform.SetParent(null);
        // }
    }
}
