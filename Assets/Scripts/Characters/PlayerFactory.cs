using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{
    public bool botsOnly = false;
    public GameObject playerPrefab;
    public GameObject botPrefab;
    private static List<Vector3> positions = new List<Vector3>(new Vector3[] {
        new Vector3(-871.5f, -419f, 0),
        new Vector3(-871.5f, 418.5f, 0),
        new Vector3(871.5f, 418.5f, 0),
        new Vector3(871.5f, -419f, 0)
    });

    void Start()
    {
        Transform parent = GameObject.Find("MainBoard").transform;

        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                if (botsOnly)
                {
                    createPlayer(botPrefab, parent, PlayersAreasConstants.player1, positions[i], i);
                }
                else
                {
                    createPlayer(playerPrefab, parent, PlayersAreasConstants.player1, positions[i], i);
                }
            }
            else
            {
                string botname = "Player" + (i + 1);
                createPlayer(botPrefab, parent, botname, positions[i], i);
            }
        }
    }

    private void createPlayer(GameObject prefab, Transform parent, string name, Vector3 position, int dialogPos)
    {
        GameObject player = Instantiate(prefab);
        player.transform.SetParent(parent);
        player.name = name;
        player.transform.position = position;
        FindSeatController findSeatController = player.GetComponent<FindSeatController>();
        findSeatController.setTbc(GameObject.Find("TableFiller").GetComponent<TableFillerController>());
        findSeatController.seat(findSeatController.getTbc().getTable());
    }
}
