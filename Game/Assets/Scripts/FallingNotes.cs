using UnityEngine;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Collections;

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

    [Header("BPM")]
    public float BPM = 120f;

    [Header("Map Settings")]
    public bool randomMap = false;
    [Range(0f, 1f)]
    public float randomNoteChance = 0.7f;

    int x;
    int y;

    private int[,] mapMaker =
    {
        {0,0,3,0},
        {0,0,3,0},
        {0,0,0,4},
        {1,0,0,0},
        {0,0,0,0},
        {0,2,0,0},
        {1,0,0,0},
        {0,0,0,4},
        {0,0,3,0},
        {0,0,0,0},
        {0,0,0,4},
        {0,2,0,0},
        {0,0,3,0},
        {1,0,0,0},
        {0,0,0,0},
        {0,0,3,0},
        {0,0,0,4},
        {0,0,0,0},
        {1,0,0,0},
        {0,2,0,0},
        {0,0,0,4},
        {0,0,3,0},
        {0,0,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,0,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,2,0,0},
        {0,0,3,0},
        {0,0,0,4},
        {0,0,0,0},
        {1,0,0,0},
        {0,0,0,4},
        {0,0,3,0},
        {0,0,0,0},
        {0,2,0,0},
        {0,0,0,0},
        {0,0,3,0},
        {1,0,0,0},
        {0,0,0,4},
        {0,0,0,0},
        {0,0,0,0},
        {0,2,0,0},
        {0,0,3,0},
        {0,0,0,4},
        {1,0,0,0},
        {0,0,0,0},
        {0,2,0,0},
        {0,0,3,0},
        {0,0,0,4},
        {0,0,0,0},
        {1,0,0,0},
        {0,2,0,0},
        {0,0,3,0},
        {0,0,0,0},
        {0,0,0,4},
        {0,0,3,0},
        {0,2,0,0},
        {1,0,0,0},
        {0,0,0,0},
        {0,0,0,0},
        {0,0,0,4},
        {0,0,3,0},
        {0,2,0,0},
        {1,0,0,0},
        {0,0,0,4},
        {0,0,0,0},
        {0,0,3,0},
        {0,2,0,0},
        {0,0,0,4},
        {1,0,0,0},
        {0,0,3,0},
        {0,0,0,0},
        {0,2,0,0},
        {0,0,3,0},
        {0,0,0,4},
        {0,0,0,0},
        {1,0,0,0},
        {0,2,0,0},
        {0,0,0,4},
        {0,0,3,0},
        {0,0,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,2,0,0},
        {0,0,3,0},
        {0,0,0,4},
        {0,0,0,0},
        {1,0,0,0},
        {0,0,3,0},
        {0,2,0,0},
        {0,0,0,4},
        {0,0,0,0},
        {0,0,3,0},
        {1,0,0,0},
        {0,2,0,0},
        {0,0,0,0},
        {0,0,0,4},
        {0,0,3,0},
        {0,0,3,0},
        {0,0,0,4},
        {0,2,0,0},
        {1,0,0,0},
        {0,0,0,0},
        {0,0,0,0},
        {0,2,0,0},
        {1,0,0,0},
        {0,0,3,0},
        {0,0,0,4},
        {0,0,0,0},
        {0,0,3,0},
        {0,0,0,4},
        {0,2,0,0},
        {1,0,0,0},
        {0,0,0,0},
        {0,2,0,0},
        {0,0,3,0},
        {0,0,0,4},
        {0,0,0,0},
        {1,0,0,0},
        {0,0,0,4},
        {0,0,3,0},
        {0,2,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,0,3,0},
        {0,2,0,0},
        {0,0,0,4},
        {0,0,0,0},
        {0,0,0,0},
        {0,0,3,0},
        {0,2,0,0},
        {1,0,0,0},
        {0,0,0,4},
        {0,0,0,0},
        {0,2,0,0},
        {0,0,0,4},
        {0,0,3,0},
        {1,0,0,0},
        {0,0,0,0},
        {0,0,3,0},
        {0,0,0,4},
        {0,2,0,0},
        {1,0,0,0},
        {0,0,0,0},
        {0,0,0,0},
        {0,0,3,0},
        {0,2,0,0},
        {1,0,0,0},
        {0,0,0,4},
        {0,0,0,0},
        {0,0,3,0},
        {0,0,0,4},
        {0,2,0,0},
        {0,0,0,0},
        {1,0,0,0},
        {0,2,0,0},
        {0,0,3,0},
        {0,0,0,4},
        {0,0,0,0},
        {0,0,3,0},
        {0,2,0,0},
        {1,0,0,0},
        {0,0,0,4},
        {0,0,0,0},
        {0,2,0,0},
        {0,0,3,0},
        {0,0,3,0},
        {0,0,0,4},
        {1,0,0,0},
        {0,0,0,0},
        {0,2,0,0},
        {1,0,0,0},
        {0,0,0,4},
        {0,0,3,0},
        {0,0,0,0},
        {0,0,0,4},
        {0,2,0,0},
        {0,0,3,0},
        {1,0,0,0},
        {0,0,0,0},
        {0,0,3,0},
        {0,0,0,4},
        {0,2,0,0},
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
        if (randomMap)
        {
            GenerateRandomMap();
        }

        StartCoroutine(SpawnMap());
    }

    void GenerateRandomMap()
    {
        for (int y = 0; y < mapMaker.GetLength(0); y++)
        {
            for (int x = 0; x < mapMaker.GetLength(1); x++)
            {
                mapMaker[y, x] = 0;
            }

            if (UnityEngine.Random.value <= randomNoteChance)
            {
                int randomLane = UnityEngine.Random.Range(0, 4);

                mapMaker[y, randomLane] = randomLane + 1;
            }
        }

        mapMaker[0, 0] = 1;

        mapMaker[0, 1] = 0;
        mapMaker[0, 2] = 0;
        mapMaker[0, 3] = 0;
    }

    IEnumerator SpawnMap()
    {
        float beatDelay = 60f / BPM;

        for (int y = 0; y < mapMaker.GetLength(0); y++)
        {
            for (int x = 0; x < mapMaker.GetLength(1); x++)
            {
                int noteType = mapMaker[y, x];

                switch (noteType)
                {
                    case 0:
                        break;

                    case 1:
                        SpawnBurger(
                            Spawn1Left,
                            Spawn1Right
                        );
                        break;

                    case 2:
                        SpawnBurger(
                            Spawn2Left,
                            Spawn2Right
                        );
                        break;

                    case 3:
                        SpawnBurger(
                            Spawn3Left,
                            Spawn3Right
                        );
                        break;

                    case 4:
                        SpawnBurger(
                            Spawn4Left,
                            Spawn4Right
                        );
                        break;
                }
            }

            beatDelay = 60f / BPM;
            yield return new WaitForSeconds(beatDelay);
        }
    }

    void SpawnBurger(GameObject leftSpawn, GameObject rightSpawn)
    {
        GameObject leftBurger = Instantiate(
            NormalBurger,
            leftSpawn.transform.position,
            quaternion.identity
        );

        leftBurger.transform.SetParent(Map.transform);

        GameObject rightBurger = Instantiate(
            NormalBurger,
            rightSpawn.transform.position,
            quaternion.identity
        );

        rightBurger.transform.SetParent(Map.transform);
    }

    public void ChangeBPM(float newBPM)
    {
        BPM = newBPM;

        FallingNotes.OnBPMChanged?.Invoke(BPM);
    }

    public static event System.Action<float> OnBPMChanged;
}