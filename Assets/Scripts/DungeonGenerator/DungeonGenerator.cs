using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    [System.Serializable]
    public class Rule
    {
        public GameObject room;
        public Vector2Int minPosition;
        public Vector2Int maxPosition;

        public bool obligatory;

        public int ProbabilityOfSpawning(int x, int y)
        {
            // 0 - cannot spawn 1 - can spawn 2 - HAS to spawn

            if (x>= minPosition.x && x<=maxPosition.x && y >= minPosition.y && y <= maxPosition.y)
            {
                return obligatory ? 2 : 1;
            }

            return 0;
        }

    }

    public Vector2Int size;
    public int startPos = 0;
    public Rule[] rooms;
    public Vector2 offset;

    [SerializeField]
    private Minimap minimap;

    List<Cell> board;

    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }

    void GenerateDungeon()
    {

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[(i + j * size.x)];
                if (currentCell.visited)
                {
                    int randomRoom = -1;
                    List<int> availableRooms = new List<int>();

                    for (int k = 0; k < rooms.Length; k++)
                    {
                        int p = rooms[k].ProbabilityOfSpawning(i, j);

                        if(p == 2)
                        {
                            randomRoom = k;
                            break;

                        } else if (p == 1)
                        {
                            availableRooms.Add(k);
                        }
                    }

                    if(randomRoom == -1)
                    {
                        if (availableRooms.Count > 0)
                        {
                            randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                        }
                        else
                        {
                            randomRoom = 0;
                        }
                    }


                    var newRoom = Instantiate(rooms[randomRoom].room, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    newRoom.UpdateRoom(currentCell.status);
                    newRoom.name += " " + i + "-" + j;

                }
            }
        }

        ConnectRooms();
        minimap.CreateMinimap(this.transform);
    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos;

        Stack<int> path = new Stack<int>();

        int k = 0;
        // loop exit
        while (k<1000)
        {
            k++;

            board[currentCell].visited = true;

            if(currentCell == board.Count - 1)
            {
                break;
            }

            //Check the cell's neighbors
            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    //down or right
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    //up or left
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }

            }

        }
        GenerateDungeon();
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        //check up neighbor
        if (cell - size.x >= 0 && !board[(cell-size.x)].visited)
        {
            neighbors.Add((cell - size.x));
        }

        //check down neighbor
        if (cell + size.x < board.Count && !board[(cell + size.x)].visited)
        {
            neighbors.Add((cell + size.x));
        }

        //check right neighbor
        if ((cell+1) % size.x != 0 && !board[(cell +1)].visited)
        {
            neighbors.Add((cell +1));
        }

        //check left neighbor
        if (cell % size.x != 0 && !board[(cell - 1)].visited)
        {
            neighbors.Add((cell -1));
        }

        return neighbors;
    }

    const int SPAWNS_OBJECT = 2;
    const int Y_OFFSET = 1;

    void ConnectRooms()
    {
        int n = this.transform.childCount;

        for (int i = 0; i < n; i++)
        {
            GameObject room = this.transform.GetChild(i).gameObject;
            RoomBehaviour roomBehaviour = room.GetComponent<RoomBehaviour>();
            GameObject[] roomDoors = roomBehaviour.doors;
            Vector3[] positionsToTP = new Vector3[4];

            for (int j = 0; j < roomDoors.Length; j++)
            {
                if (roomDoors[j].activeSelf)
                {
                    Vector3[] data = PositionAndDirection(j);
                    Vector3 position = room.transform.position + data[0];
                    Vector3 direction = data[1];

                    RaycastHit hit;
                    if (Physics.Raycast(position, direction, out hit))
                    {
                        if (hit.transform.gameObject != null)
                        {
                            GameObject nextRoom = hit.transform.gameObject;
                            positionsToTP[j] = nextRoom.transform.GetChild(SPAWNS_OBJECT).GetChild(SpawnIndex(j)).position;
                        }
                    }
                }
            }

            roomBehaviour.ConnectDoors(positionsToTP);
        }
    }

    Vector3[] PositionAndDirection(int j)
    {
        Vector3 position = Vector3.zero;
        Vector3 direction = Vector3.zero;

        if (j == 0)
        {
            position = new Vector3(0, Y_OFFSET, 30);
            direction = Vector3.forward;

        }
        else if (j == 1)
        {
            position = new Vector3(0, Y_OFFSET, -30);
            direction = -Vector3.forward;
        }
        else if (j == 2)
        {
            position = new Vector3(30, Y_OFFSET, 0);
            direction = Vector3.right;
        }
        else // j must be 3
        {
            position = new Vector3(-30, Y_OFFSET, 0);
            direction = -Vector3.right;
        }

        return new Vector3[] {position, direction};
    }

    int SpawnIndex(int j)
    {
        if (j == 0) return 1;
        if (j == 1) return 0;
        if (j == 2) return 3;
        return 2;
    }
}
