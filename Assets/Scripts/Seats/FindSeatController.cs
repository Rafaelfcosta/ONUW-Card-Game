using System.Collections;
using System.Collections.Generic;
using SharpNeat.Phenomes;
using UnityEngine;
using UnitySharpNEAT;

public class FindSeatController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> positions = new List<Vector3>();
        positions.Add(new Vector3(-871.5f, -419f, 0));
        positions.Add(new Vector3(-871.5f, 418.5f, 0));
        positions.Add(new Vector3(871.5f, 418.5f, 0));
        positions.Add(new Vector3(871.5f, -419f, 0));

        TableFillerController tbc = GameObject.Find("TableFiller").GetComponent<TableFillerController>();
        // Transform parent = tbc.getTable();
        SeatController table = tbc.getTable();
        if (table != null)
        {
            transform.SetParent(table.transform);
            // name = "Player" + (transform.GetSiblingIndex() - 1);
            name = "Player" + table.getSLOTS();
            transform.position = positions[table.getSLOTS() - 1];
            GetComponent<PlayerBase>().dialogBox = transform.parent.gameObject.FindComponentInChildWithTag<Transform>("dialog").gameObject.transform.GetChild(table.getSLOTS() - 1).gameObject;

            if (table.getSLOTS() > 3)
            {
                table.GetComponent<GameController>().initialSetup();
            }
        }
    }
}
