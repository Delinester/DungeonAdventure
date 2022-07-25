using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] rooms;
    [SerializeField] GameObject startRoom;

    private int roomsCount = 0;
    [SerializeField]
    private int maxAmountOfRooms = 8;
    private float spawnOffset = 82;
    private Cell rootCell;
    private List<Cell> board = new List<Cell>();
    private class Cell
    {
        public Cell() { }
        public Cell(bool[] enterances)
        {
            enterancesStatus = enterances;
        }
        public Cell(Vector2 pos)
        {
            position = pos;
        }

        public bool isVisited = false;
        public bool[] enterancesStatus = new bool[4];
        public Vector2 position;
        public bool AreThereAnyEnerances()
        {
            int wallsCount = 0;
            foreach (bool enterance in enterancesStatus)
            {
                if (!enterance) wallsCount++;
            }
            return wallsCount == 4 ? false : true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rootCell = GenerateCell();
        rootCell.position = Vector2.zero;
        board.Add(rootCell);
        startRoom.SetActive(false);
        GenerateDungeon(rootCell);
        // Make room count match the desired amount
        for (int i = 0; i < 3; i++)
        {
            if (roomsCount < maxAmountOfRooms) UseUnusedEnterances();
            else break;
        }
        while (roomsCount < maxAmountOfRooms) AddDeficientRooms();
        //
        DeactivateUnusedEnterances();
        startRoom.GetComponent<RoomBehaviour>().UpdateRoom(board[0].enterancesStatus);
        SpawnDungeon();
        Debug.Log($"Rooms: {roomsCount}");
    }

    private bool GetNeighbourUp(Cell cell, out Cell neighbour)
    {
        neighbour = null;
        foreach (Cell room in board)
        {
            if (cell.position.x == room.position.x && cell.position.y + 1 == room.position.y)
            {
                neighbour = room;
                return true;
            }
        }
        return false;
    }
    private bool GetNeighbourUp(Cell cell)
    {        
        foreach (Cell room in board)
        {
            if (cell.position.x == room.position.x && cell.position.y + 1 == room.position.y)
            {               
                return true;
            }
        }
        return false;
    }
    private bool GetNeighbourDown(Cell cell, out Cell neighbour)
    {
        neighbour = null;
        foreach (Cell room in board)
        {
            if (cell.position.x == room.position.x && cell.position.y - 1 == room.position.y)
            {
                neighbour = room;
                return true; ;
            }
        }
        return false;
    }
    private bool GetNeighbourDown(Cell cell)
    {       
        foreach (Cell room in board)
        {
            if (cell.position.x == room.position.x && cell.position.y - 1 == room.position.y)
            {                
                return true; ;
            }
        }
        return false;
    }
    private bool GetNeighbourLeft(Cell cell, out Cell neighbour)
    {
        neighbour = null;
        foreach (Cell room in board)
        {
            if (cell.position.y == room.position.y && cell.position.x - 1 == room.position.x)
            {
                neighbour = room;
                return true;
            }
        }
        return false;
    }
    private bool GetNeighbourLeft(Cell cell)
    {        
        foreach (Cell room in board)
        {
            if (cell.position.y == room.position.y && cell.position.x - 1 == room.position.x)
            {               
                return true;
            }
        }
        return false;
    }
    private bool GetNeighbourRight(Cell cell, out Cell neighbour)
    {
        neighbour = null;
        foreach (Cell room in board)
        {
            if (cell.position.y == room.position.y && cell.position.x + 1 == room.position.x)
            {
                neighbour = room;
                return true;
            }
        }
        return false;
    }
    private bool GetNeighbourRight(Cell cell)
    {       
        foreach (Cell room in board)
        {
            if (cell.position.y == room.position.y && cell.position.x + 1 == room.position.x)
            {                
                return true;
            }
        }
        return false;
    }
    private Cell GenerateCell()
    {
        Cell cell = new Cell();
        for (int i = 0; i < cell.enterancesStatus.Length; i++)
        {
            cell.enterancesStatus[i] = Random.Range(0, 2) == 0 ? false : true;
        }
        return cell;
    }   
    private void GenerateDungeon(Cell parentCell)
    {
        if (parentCell.AreThereAnyEnerances())
        {
            if (parentCell.enterancesStatus[0] && roomsCount < maxAmountOfRooms && !GetNeighbourUp(parentCell))
            {
                roomsCount++;
                Cell newCell = GenerateCell();
                newCell.position = new Vector2(parentCell.position.x, parentCell.position.y + 1);
                newCell.enterancesStatus[1] = true;
                board.Add(newCell);
                GenerateDungeon(newCell);
            }
            if (parentCell.enterancesStatus[1] && roomsCount < maxAmountOfRooms && !GetNeighbourDown(parentCell))
            {
                roomsCount++;
                Cell newCell = GenerateCell();
                newCell.position = new Vector2(parentCell.position.x, parentCell.position.y - 1);
                newCell.enterancesStatus[0] = true;
                board.Add(newCell);
                GenerateDungeon(newCell);
            }
            if (parentCell.enterancesStatus[2] && roomsCount < maxAmountOfRooms && !GetNeighbourLeft(parentCell))
            {
                roomsCount++;
                Cell newCell = GenerateCell();
                newCell.position = new Vector2(parentCell.position.x - 1, parentCell.position.y);
                newCell.enterancesStatus[3] = true;
                board.Add(newCell);
                GenerateDungeon(newCell);
            }
            if (parentCell.enterancesStatus[3] && roomsCount < maxAmountOfRooms && !GetNeighbourRight(parentCell))
            {
                roomsCount++;
                Cell newCell = GenerateCell();
                newCell.position = new Vector2(parentCell.position.x + 1, parentCell.position.y);
                newCell.enterancesStatus[2] = true;
                board.Add(newCell);
                GenerateDungeon(newCell);
            }
        }
    }
    private void UseUnusedEnterances()
    {
        foreach (Cell cell in board)
        {
            if (roomsCount < maxAmountOfRooms) GenerateDungeon(cell);
            else break;
        }
    }
    private void DeactivateUnusedEnterances()
    {
        foreach (Cell cell in board)
        {
            if (!GetNeighbourUp(cell, out Cell neighbourUp)) cell.enterancesStatus[0] = false;
            else if (!neighbourUp.enterancesStatus[1] && cell.enterancesStatus[0]) cell.enterancesStatus[0] = false;
            if (!GetNeighbourDown(cell, out Cell neighbourDown)) cell.enterancesStatus[1] = false;
            else if (!neighbourDown.enterancesStatus[0] && cell.enterancesStatus[1]) cell.enterancesStatus[1] = false;
            if (!GetNeighbourLeft(cell, out Cell neighbourLeft)) cell.enterancesStatus[2] = false;
            else if (!neighbourLeft.enterancesStatus[3] && cell.enterancesStatus[2]) cell.enterancesStatus[2] = false;
            if (!GetNeighbourRight(cell, out Cell neighbourRight)) cell.enterancesStatus[3] = false;
            else if (!neighbourRight.enterancesStatus[2] && cell.enterancesStatus[3]) cell.enterancesStatus[3] = false;
        }
    }
    private void SpawnDungeon()
    {        
        foreach (Cell cell in board)
        {
            int roomIndex = Random.Range(0, rooms.Length);
            Vector3 spawnPos = new Vector3(startRoom.transform.position.x + cell.position.x * spawnOffset, startRoom.transform.position.y, startRoom.transform.position.z + cell.position.y * spawnOffset);
            GameObject room = Instantiate(rooms[roomIndex], spawnPos, rooms[roomIndex].transform.rotation);
            room.GetComponent<RoomBehaviour>().UpdateRoom(cell.enterancesStatus);
            if (cell.enterancesStatus[0]) room.GetComponent<RoomBehaviour>().DeactivateWall(0);
            if (cell.enterancesStatus[3]) room.GetComponent<RoomBehaviour>().DeactivateWall(3);            
        }
    }
    private void AddDeficientRooms()
    {
        for (int i = 0; i < board.Count; i++)
        {
            if (roomsCount < maxAmountOfRooms)
            {                
                for (int j = 0; j < 4; j++) board[i].enterancesStatus[j] = true;
                GenerateDungeon(board[i]);
            }
            else break;
        }
    }
    private void ClearDuplicateWalls()
    {

    }
}
