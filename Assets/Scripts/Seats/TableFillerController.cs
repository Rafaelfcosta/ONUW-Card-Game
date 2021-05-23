using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TableFillerController : MonoBehaviour
{
    public int TABLES_START_AMOUNT = 100;
    public int ADD_TABLES_AMOUNT = 25;
    public int minTableAmount = 50;
    public int currentTables = 0;
    public int tableCounter = 0;
    public GameObject tablePrefab;
    public List<SeatController> tables = new List<SeatController>();
    private Transform parent;
    private string sceneName;

    public List<SeatController> getTables()
    {
        return this.tables;
    }

    public void setTables(List<SeatController> tables)
    {
        this.tables = tables;
    }


    public int getCurrentTables()
    {
        return this.currentTables;
    }

    public void setCurrentTables(int currentTables)
    {
        this.currentTables = currentTables;
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        parent = GameObject.Find("MainBoard").transform;
        for (int i = 0; i < TABLES_START_AMOUNT; i++)
        {
            setCurrentTables(getCurrentTables() + 1);
            tableCounter++;
            GameObject table = Instantiate(tablePrefab, tablePrefab.transform.position, tablePrefab.transform.rotation);
            table.transform.SetParent(parent);
            table.name = "Table " + tableCounter;
            tables.Add(table.GetComponent<SeatController>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneName.Equals("NeatScene"))
        {
            if (getCurrentTables() < minTableAmount)
            {
                addTables();
            }
        }
        // InvokeRepeating("addTables", 2.0f * Time.timeScale / 100, 4.0f * Time.timeScale / 100);
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
        setCurrentTables(getCurrentTables() + 1);
        tableCounter++;
        GameObject table = Instantiate(tablePrefab, tablePrefab.transform.position, tablePrefab.transform.rotation);
        table.transform.SetParent(parent);
        tables.Add(table.GetComponent<SeatController>());
        table.name = "Table " + tableCounter;
    }

    public void addTables()
    {
        for (int i = 0; i < ADD_TABLES_AMOUNT; i++)
        {
            setCurrentTables(getCurrentTables() + 1);
            tableCounter++;
            GameObject table = Instantiate(tablePrefab, tablePrefab.transform.position, tablePrefab.transform.rotation);
            table.transform.SetParent(parent);
            tables.Add(table.GetComponent<SeatController>());
            table.name = "Table " + tableCounter;
        }
    }
}
