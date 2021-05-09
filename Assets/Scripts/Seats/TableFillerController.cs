using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableFillerController : MonoBehaviour
{
    public const int TABLES_AMOUNT = 25;
    public GameObject tablePrefab;
    public List<SeatController> tables = new List<SeatController>();
    // Start is called before the first frame update
    void Start()
    {
        Transform parent = GameObject.Find("Board").transform;
        for (int i = 0; i < TABLES_AMOUNT; i++)
        {
            GameObject table = Instantiate(tablePrefab, tablePrefab.transform.position, tablePrefab.transform.rotation);
            table.transform.SetParent(parent);
            table.name = "BotsTable " + (i + 1);
            tables.Add(table.GetComponent<SeatController>());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Transform getTable()
    {
        foreach (var table in tables)
        {
            Debug.Log(table);
            if (table.hasSlots())
            {
                table.setSLOTS(table.getSLOTS() - 1);
                return table.gameObject.transform;
            }

        }
        return null;
    }
}
