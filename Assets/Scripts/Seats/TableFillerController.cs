using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TableFillerController : MonoBehaviour
{
    public int TABLES_START_AMOUNT = 100;
    public int ADD_TABLES_AMOUNT = 25;
    public GameObject tablePrefab;
    public List<SeatController> tables = new List<SeatController>();
    Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().name);

        parent = GameObject.Find("Board").transform;
        for (int i = 0; i < TABLES_START_AMOUNT; i++)
        {
            GameObject table = Instantiate(tablePrefab, tablePrefab.transform.position, tablePrefab.transform.rotation);
            table.transform.SetParent(parent);
            table.name = "Table " + (i + 1);
            tables.Add(table.GetComponent<SeatController>());
        }

        InvokeRepeating("addTables", 2.0f * Time.timeScale / 100, 4.0f * Time.timeScale / 100);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public SeatController getTable()
    {
        foreach (var table in tables)
        {
            if (table.hasSlots())
            {
                table.setSLOTS(table.getSLOTS() + 1);
                return table;
            }

        }
        return null;
    }

    public void realocatePlayers(List<PlayerBase> players)
    {
        foreach (var player in players)
        {
            player.GetComponent<FindSeatController>().seat(getTable());
        }
    }

    public void addTable()
    {
        GameObject table = Instantiate(tablePrefab, tablePrefab.transform.position, tablePrefab.transform.rotation);
        table.transform.SetParent(parent);
        tables.Add(table.GetComponent<SeatController>());
        table.name = "Table " + (tables.Count);
    }

    public void addTables()
    {
        for (int i = 0; i < ADD_TABLES_AMOUNT; i++)
        {
            GameObject table = Instantiate(tablePrefab, tablePrefab.transform.position, tablePrefab.transform.rotation);
            table.transform.SetParent(parent);
            tables.Add(table.GetComponent<SeatController>());
            table.name = "Table " + (tables.Count);
        }
    }
}
