using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Camera mainCamera_;

    [SerializeField, Header("Prefab")]
    private Explosion explosionPrefab_;
    [SerializeField]
    private Meteor meteorPrefab_;
    [SerializeField]
    private GameObject reticlePrefab_;
    [SerializeField]
    private Missile missilePrefab_;
    [SerializeField]
    private List<ItemBase> items_;

    [SerializeField, Header("ItemSetting")]
    private Transform itemSpawnPosition_;
    [SerializeField]
    private float itemSpawnInterval_ = 10.0f;
    private float itemTimer_ = 0.0f;

    [SerializeField, Header("Launcher")]
    private GameObject launcherPrefab_;

    [SerializeField, Header("MeteorSpawner")]
    private BoxCollider2D ground_;
    [SerializeField]
    private float meteorInterval_ = 1.0f;
    private float meteorTimer_ = 0.0f;
    [SerializeField]
    private List<Transform> spawnPositions_;

    [SerializeField, Header("ScoreUISetting")]
    private ScoreText scoreText_;
    private int score_;

    [SerializeField, Header("LifeUISettings")]
    private LifeBar lifeBar_;
    [SerializeField]
    private float maxLife_ = 10.0f;
    private float life_;

    [SerializeField, Header("LaunchPositions")]
    private List<Transform> launchPositions_;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject); // if needed across scenes
    }

    public void AddScore(int point)
    {
        score_ += point;
        scoreText_.SetScore(score_);
    }

    void Start()
    {
        GameObject mainCameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        bool isGetComponent = mainCameraObject.TryGetComponent(out mainCamera_);
        Assert.IsTrue(isGetComponent, "MainCameraにcameraコンポーネントが見つかりません");

        Assert.IsTrue(spawnPositions_.Count > 0, "spawnPositionsに1つ以上のTransformを追加してください");
        foreach (Transform t in spawnPositions_)
        {
            Assert.IsNotNull(t, "spawnPositionsにnullが含まれています");
        }

        foreach (Transform t in launchPositions_)
        {
            Assert.IsNotNull(t, "launchPositionsにnullが含まれています");
        }

        if (launcherPrefab_ != null && launchPositions_ != null)
        {
            foreach (Transform launchT in launchPositions_)
            {
                if (launchT == null) continue;
                GameObject launcherInstance = Instantiate(launcherPrefab_, launchT.position, Quaternion.identity, launchT);
                launcherInstance.transform.localPosition = Vector3.zero;
            }
        }

        ResetLife();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMissile();
        }

        UpdateMeteorTimer();
        UpDdateItemTimer();
    }

    private void GenerateMissile()
    {
        Vector3 clickPosition = mainCamera_.ScreenToWorldPoint(Input.mousePosition);
        clickPosition.z = 0.0f;

        GameObject reticle = Instantiate(reticlePrefab_, clickPosition, Quaternion.identity);

        Transform nearest = null;
        float minSqrDist = float.MaxValue;

        if (launchPositions_ != null && launchPositions_.Count > 0)
        {
            foreach (Transform launchT in launchPositions_)
            {
                if (launchT == null) { continue; }
                float sqr = (launchT.position - clickPosition).sqrMagnitude;
                if (sqr < minSqrDist)
                {
                    minSqrDist = sqr;
                    nearest = launchT;
                }
            }
        }

        Vector3 launchPosition = nearest != null ? nearest.position : new Vector3(0f, -3f, 0f);

        Missile missile = Instantiate(missilePrefab_, launchPosition, Quaternion.identity);
        missile.SetUp(reticle);
    }

    private void UpdateMeteorTimer()
    {
        meteorTimer_ -= Time.deltaTime;
        if (meteorTimer_ > 0) { return; }
        meteorTimer_ += meteorInterval_;
        GenerateMeteor();
    }

    private void GenerateMeteor()
    {
        int max = spawnPositions_.Count;
        int posIndex = Random.Range(0, max);
        Vector3 spawnPosition = spawnPositions_[posIndex].position;
        Meteor meteor = Instantiate(meteorPrefab_, spawnPosition, Quaternion.identity);
        meteor.Setup(ground_, this, explosionPrefab_);
    }

    private void ResetLife()
    {
        life_ = maxLife_;
        UpdateLifeBar();
    }

    private void UpdateLifeBar()
    {
        float lifeRatio = Mathf.Clamp01(life_ / maxLife_);
        lifeBar_.SetGaugeRatio(lifeRatio);
    }

    public void Damage(float point)
    {
        life_ -= point;
        UpdateLifeBar();
    }

    private ItemBase PickUpItem()
    {
        int itemprefabNum = items_.Count;
        Assert.IsTrue(itemprefabNum > 0);
        int pickUpIndex = Random.Range(0, itemprefabNum);
        ItemBase pickUpItem = items_[pickUpIndex];
        return pickUpItem;
    }

    private void UpDdateItemTimer()
    {
        itemTimer_ -= Time.deltaTime;
        if (itemTimer_ > 0) { return; }
        itemTimer_ += itemSpawnInterval_;
        ItemBase pickedUpItem = PickUpItem();
        Instantiate(pickedUpItem, itemSpawnPosition_.position, Quaternion.identity);
    }

    public void OnPlayerDeath()
    {
        Debug.Log("Player died!");

        PlayerPrefs.SetInt("LastScore", score_);
        PlayerPrefs.Save();
        // High score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score_ > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score_);
        }
        PlayerPrefs.Save();

        LevelLoaderScript loader = Object.FindFirstObjectByType<LevelLoaderScript>();

        if (loader != null)
        {
            loader.LoadGameOver();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }

        
    }

}
