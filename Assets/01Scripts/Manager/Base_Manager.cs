using UnityEngine;
public class Base_Manager : MonoBehaviour
{
    public static Base_Manager instance;

    private static Game_Manager game_Manager = new Game_Manager();
    private static Pool_Manager pool_Manager = new Pool_Manager();
    private static UI_Manager ui_Manager = new UI_Manager();
    private static Data_Manager data_Manager = new Data_Manager();
    private static Inventory_Manager inventory_Manager = new Inventory_Manager();
    public static Game_Manager game_Mng { get { return game_Manager; } }
    public static Pool_Manager pool_Mng { get { return pool_Manager; } }
    public static UI_Manager ui_Mng { get { return ui_Manager; } }
    public static Data_Manager data_Mng { get { return data_Manager; } }
    public static Inventory_Manager inventory_Mng { get {return inventory_Manager; } }

    [SerializeField]
    private Base_Canvas base_Canvas;

    public Player current_Player; // 임시코드 장착중인 캐릭터 저장용

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

        pool_Mng.Init(this.transform);
        ui_Mng.Init(base_Canvas);
        data_Mng.Init();
    }

    void Start()
    {
        // 테스트용 코드
        pool_Mng.Pooling_OBJ("HP_Potion").Get(obj =>
        {
            obj.GetComponent<Item_Base>().Init(new Vector3(2,0.2f,2), data_Mng.d_Item_Data["HP_Potion"]);
        });

        pool_Mng.Pooling_OBJ("HP_Potion").Get(obj =>
        {
            obj.GetComponent<Item_Base>().Init(new Vector3(-2, 0.2f, 2), data_Mng.d_Item_Data["HP_Potion"]);
        });
    }

    void Update()
    {
        
    }

    public GameObject Get_Prefab_OBJ(string path) 
    {
        GameObject go = Instantiate(Resources.Load<GameObject>(path));
        return go;
    }
}
