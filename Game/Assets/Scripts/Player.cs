using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]private int playerId = 1;

    public int PlayerId => playerId;
}