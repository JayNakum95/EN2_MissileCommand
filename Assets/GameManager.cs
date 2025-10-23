using NUnit.Framework;
using UnityEngine;
//using UnityEngine.Assertions;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private Camera mainCamera_;
    [SerializeField, Header("Prefab")]
    private Explosion explosionPrefab_;
    [SerializeField]
    private Meteor meteorPrefab_;
    [SerializeField]
    private GameObject reticlePrefab_;
    [SerializeField]
    private Missile missilePrefab_;
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
    [SerializeField] private float maxLife_ = 10.0f;
    private float life_;
    public void AddScore(int point) {
        score_ += point;
        scoreText_.SetScore(score_);


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject mainCameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        bool isGetComponent = mainCameraObject.TryGetComponent(out mainCamera_);
        Assert.IsTrue(isGetComponent, "MainCameraにcamereコンポーネントがありません");
        Assert.IsTrue(spawnPositions_.Count > 0, "spawnPositionsに1つ以上のTransformをアタッチしてください");
        foreach (Transform t in spawnPositions_)
        {
            Assert.IsNotNull(t, "spawnPositionsにnullが含まれています");
        }
        ResetLife();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            GenerateMissile();
        }
        UpdateMeteorTimer();
    }


    //private void GenerateExplosion() {
    //    Vector3 clickPosition = mainCamera_.ScreenToWorldPoint(Input.mousePosition);
    //    clickPosition.z = 0.0f;
    //    Explosion explosion = Instantiate(explosionPrefab_, clickPosition, Quaternion.identity);


    //}
    private void GenerateMissile() { 
    
    Vector3 ClickPosition= mainCamera_.ScreenToWorldPoint(Input.mousePosition);
        ClickPosition.z = 0.0f;
        GameObject reticle = Instantiate(reticlePrefab_, ClickPosition, Quaternion.identity);
        Vector3 LaunchPosition = new Vector3(0,-3,0);
        Missile missile = Instantiate(missilePrefab_, LaunchPosition, Quaternion.identity);
        missile.SetUp(reticle);

    }

        private void UpdateMeteorTimer() {
        meteorTimer_ -= Time.deltaTime;
        if (meteorTimer_ > 0) { return; }
        meteorTimer_ += meteorInterval_;
        GenerateMeteor();

    }
    private void GenerateMeteor() {
        int max= spawnPositions_.Count;
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
    private void UpdateLifeBar() {
        float lifeRatio = Mathf.Clamp01(life_ / maxLife_);
        lifeBar_.SetGaugeRatio(lifeRatio);

    }
    public void Damage(float point) {
        life_ -= point;
        UpdateLifeBar();
    }
}
