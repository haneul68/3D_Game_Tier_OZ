using UnityEngine;

public class Base_Manager : MonoBehaviour
{
    public static Base_Manager instance;

    private static Game_Manager game_Manager = new Game_Manager();

    public static Game_Manager game_Mng { get { return game_Manager; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
