using UnityEngine;
using System.Collections;
using DropItemSystem.UnityStuff;

public class Room : MonoBehaviour {

    private enum TileType { WHITE_SPACE, TL_CORNER, L_SIDE, BL_CORNER, B_SIDE, BR_CORNER, R_SIDE, TR_CORNER, T_SIDE };
    private TileType[] TileTypes;
    private GameObject[][] Tiles;

    private int width;
    private int length;
    private int totalSquares;

    private double[] _position;

    private int sizeOfSquare = 128;

    [Header("References - DON'T TOUCH")]
    public GameObject[] TilePrefabs;

    public Room()
    {

    }

    /// <summary>
    /// Generates a new room
    /// </summary>
    /// <param name="Wid">Number of Tiles in the X Axis</param>
    /// <param name="Len">Number of Tiles in the Y Axis</param>
    public Room(int Wid, int Len, double radOfSpawnCircle)
    {
        width = Wid;
        length = Len;

        double[] Position = new double[2];
        Position = DiceRoll.PointInCir(radOfSpawnCircle);
        Position[0] = roundToGrid(Position[0], sizeOfSquare);
        Position[1] = roundToGrid(Position[1], sizeOfSquare);

        _position = Position;

        totalSquares = width * length;

        TileTypes = new TileType[totalSquares];
        Tiles = new GameObject[length][];

        //Goes from top left to bottom right
        for (int i = 0; i < length; i++)
        {
            Tiles[i] = new GameObject[width];

            for (int j = 0; j < width; j++)
            {
                if (i == 0 || i == (length - 1))
                {
                    #region Corners
                    if (i == 0 && j == 0)
                    {
                        TileTypes[i] = TileType.TL_CORNER;
                        Tiles[i][j] = TilePrefabs[1];
                        break;
                    }
                    else if (i == 0 && j == (width - 1))
                    {
                        TileTypes[j] = TileType.TR_CORNER;
                        Tiles[i][width - 1] = TilePrefabs[7];
                        break;
                    }
                    else if (i == (length-1) && j == 0)
                    {
                        TileTypes[i * width] = TileType.BL_CORNER;
                        Tiles[length - 1][0] = TilePrefabs[3];
                        break;
                    }
                    else if (i == (length-1) && j == (width-1))
                    {
                        TileTypes[totalSquares - 1] = TileType.BR_CORNER;
                        Tiles[length - 1][width - 1] = TilePrefabs[5];
                        break;
                    }
                    #endregion

                    #region Top and Bottom Rows
                    //Top row and Bottom Row
                    else if (i == 0 && (j > 0 || j < (width-1)))
                    {
                        TileTypes[j] = TileType.T_SIDE;
                        Tiles[i][j] = TilePrefabs[8];
                        break;
                    }
                    else if (i == (length-1) && (j > 0 || j < (width-1)))
                    {
                        TileTypes[(i * width) + j] = TileType.B_SIDE;
                        Tiles[length - 1][j] = TilePrefabs[4];
                        break;
                    }
                    #endregion
                }

                else
                {
                    #region Sides
                    if (j == 0 && (i > 0 || i < (length-1)))
                    {
                        TileTypes[i*width] = TileType.L_SIDE;
                        Tiles[i][i] = TilePrefabs[2];
                    }
                    else if (j == (width-1) && (i > 0 || i < (length - 1)))
                    {
                        TileTypes[(i * width)+j] = TileType.R_SIDE;
                        Tiles[i][i] = TilePrefabs[6];
                    }
                    #endregion

                    #region Walkable space
                    else if ((i > 0 && i < length-1) && (j > 0 && j < width-1))
                    {
                        TileTypes[i] = TileType.WHITE_SPACE;
                        Tiles[i][i] = TilePrefabs[0];
                    }
                    #endregion
                }
            }
        }

        spawnRoom();
    }

    public int roundToGrid(double randPoint, int tileSize)
    {
        return (int)(Mathf.Floor((float)((randPoint + tileSize - 1) / tileSize) * tileSize));
    }

    public void spawnRoom()
    {
        for (int i = 0; i < totalSquares; i++)
        {
            
        }
    }
}
