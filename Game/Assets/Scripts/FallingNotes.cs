using UnityEngine;
using Unity.Mathematics;
using UnityEditor;
//using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor.Tilemaps;
public class FallingNotes : MonoBehaviour

{
    [Header("Player1Left")]
    public GameObject Spawn1Left;
    public GameObject Spawn2Left;
    public GameObject Spawn3Left;
    public GameObject Spawn4Left;

    [Header("Player2Right")]
    public GameObject Spawn1Right;
    public GameObject Spawn2Right;
    public GameObject Spawn3Right;
    public GameObject Spawn4Right;

    [Header("Other stuff")]
    public GameObject NormalBurger;
    public GameObject Nothing;
    [SerializeField] GameObject Map;
    int x;
    int y;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // als alle orbs heeft opent passage naar 3d world
    private int[,] mapMaker =
    {
        {0,0,0,0},
        {0,0,0,0},
        {0,2,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,2,0,0},
        {0,0,0,0},
        {0,0,0,0},
        {0,0,0,4},
        {0,0,3,0},
        {0,2,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,2,0,0},
        {0,0,0,4},
        {0,0,3,0},
        {0,0,0,4},
        {0,0,0,0},
        {0,0,0,0},
        {0,2,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,2,0,0},
        {0,0,0,0},
        {0,0,0,0},
        {0,0,0,4},
        {0,0,3,0},
        {0,2,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,2,0,0},
        {0,0,0,4},
        {0,0,3,0},
        {0,0,0,4},

        {0,0,0,0},
        {0,0,0,0},
        {0,2,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,2,0,0},
        {0,0,0,0},
        {0,0,0,0},
        {0,0,0,4},
        {0,0,3,0},
        {0,2,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,2,0,0},
        {0,0,0,4},
        {0,0,3,0},
        {0,0,0,4},
    };

    void Start()
    {
        // .transform.SetParent(Map.transform,false);
        Vector2 TileSize = NormalBurger.GetComponent<SpriteRenderer>().bounds.size;
        //Vector2 TileSize = Note.GetComponent<Renderer>().bounds.size;
        float ox = (mapMaker.GetLength(1) * TileSize.x - 1) / 2;
        float oy = (mapMaker.GetLength(0) * TileSize.y - 1) / 2;

        for (int y = 0; y < mapMaker.GetLength(0); y++)
        {
            for (int x = 0; x < mapMaker.GetLength(1); x++)
            {
                float ux = (x * TileSize.x) - ox;
                float uy = (-y * TileSize.y) + oy + 10f;
                switch (mapMaker[y, x])
                {
                    case 0:
                        GameObject nothing = Instantiate(Nothing, new Vector3(ux, uy), quaternion.identity);
                        nothing.transform.SetParent(Map.transform);
                        //nothing.GetComponent<SpriteRenderer>().material.color = Color.white;
                        break;
                    case 1:
                        GameObject MostLeft1normalBurger = Instantiate(NormalBurger, new Vector3(Spawn1Left.transform.position.x, uy), quaternion.identity);
                        MostLeft1normalBurger.transform.SetParent(Map.transform);
                        //MostLeft1normalBurger.transform.position = Spawn1Left.transform.position;
                        break;
                    case 2:
                        GameObject Left2normalBurger = Instantiate(NormalBurger, new Vector3(Spawn2Left.transform.position.x, uy), quaternion.identity);
                        Left2normalBurger.transform.SetParent(Map.transform);
                        break;
                    case 3:
                        GameObject Right3normalBurger = Instantiate(NormalBurger, new Vector3(Spawn3Left.transform.position.x, uy), quaternion.identity);
                        Right3normalBurger.transform.SetParent(Map.transform);
                        break;
                    case 4:
                        GameObject MostRight4normalBurger = Instantiate(NormalBurger, new Vector3(Spawn4Left.transform.position.x, uy), quaternion.identity);
                        MostRight4normalBurger.transform.SetParent(Map.transform);
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

