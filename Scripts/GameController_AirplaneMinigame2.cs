using DG.Tweening;
using GestureRecognizer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_AirplaneMinigame2 : MonoBehaviour
{
    public static GameController_AirplaneMinigame2 instance;

    public Recognizer recognizer;
    public Transform UFOGroupPosDream;
    public List<Transform> UFOsDream;
    public LineObject_AirplaneMinigame2 dreamUIPrefab;
    public Canvas canvas;
    public Camera mainCamera;
    public GameObject Airplane;
    public List<LineObject_AirplaneMinigame2> dreamObj;
    public LineObject_AirplaneMinigame2 Boss;
    public GameObject currBackGround, nextBackGround;
    public int spawnBG;
    public int bossLife;
    public GameObject anhXa;

    public Transform referenceRoot;

    public GesturePatternDraw[] references;
    public DrawDetector drawDetector;

    public Color32[] colors;

    public int stage;
    public bool isLock;
    public int[] count;
    public bool isWinGame;
    public bool isLoseGame;
    public bool isIntro;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);
    }

    void Start()
    {
        isIntro = true;
        isLock = false;
        isWinGame = false;
        stage = 0;
        bossLife = -1;
        count = new int[] { 1, 1, 2, 3, 3, 3, 4, 5, 6, 8, 1 };
        UFOsDream = new List<Transform>();
        foreach (Transform child in UFOGroupPosDream)
        {
            UFOsDream.Add(child);
        }
        spawnBG = 0;
        currBackGround = Instantiate(currBackGround, new Vector3(12.5f, -0.6f, 0), Quaternion.identity);
        MoveBG();
        SetSizeCamera();
        StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        yield return new WaitForSeconds(0.1f);
        Airplane.transform.DOMoveX(0, 5f);
        yield return new WaitForSeconds(1);
        mainCamera.transform.DOMoveX(0, 4f).OnComplete(() => { mainCamera.DOOrthoSize(7, 2f); });
        yield return new WaitForSeconds(6);
        stage = 1;
        isIntro = false;
    }

    void SetSizeCamera()
    {
        float f1 = 16.0f / 9;
        float f2 = Screen.width * 1.0f / Screen.height;

        mainCamera.orthographicSize *= f1 / f2;
    }

    void DestroyBG()
    {
        Destroy(currBackGround);
        spawnBG = 0;
        currBackGround = nextBackGround;
    }

    void MoveNextBG()
    {
        nextBackGround.transform.DOMoveX(-60, 45f).SetEase(Ease.Flash);
    }

    void MoveBG()
    {
        currBackGround.transform.DOMoveX(-60f, 40f).SetEase(Ease.Flash);
    }


    LineObject_AirplaneMinigame2 SpawnUFO(int posIndex, int dreamIndex)
    {
        LineObject_AirplaneMinigame2 dream = Instantiate(dreamUIPrefab);
        dream.transform.parent = referenceRoot.transform;
        dream.transform.localScale = Vector3.one;

        if (dreamIndex == 1)
        {
            dream.transform.GetChild(0).GetChild(0).localPosition = new Vector2(dream.transform.GetChild(0).GetChild(0).localPosition.x + 30,
                dream.transform.GetChild(0).GetChild(0).localPosition.y);
            dream.line.RemoveAt(1);
            dream.line.RemoveAt(1);
        }
        if (dreamIndex == 2)
        {
            dream.transform.GetChild(0).GetChild(0).localPosition = new Vector2(dream.transform.GetChild(0).GetChild(0).localPosition.x + 20,
                dream.transform.GetChild(0).GetChild(0).localPosition.y);
            dream.line.RemoveAt(2);
        }

        List<GesturePattern> listCheckSame = new List<GesturePattern>();
        for (int i = 0; i < recognizer.patterns.Count; i++)
        {
            listCheckSame.Add(recognizer.patterns[i]);
        }
        for (int i = 0; i < dreamIndex; i++)
        {
            int ran = Random.Range(0, listCheckSame.Count);
            dream.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<GesturePatternDraw>().pattern = listCheckSame[ran];
            listCheckSame.RemoveAt(ran);
            dream.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<GesturePatternDraw>().color = colors[Random.Range(0, colors.Length)];
            dream.transform.GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(true);
        }

        dream.GetComponent<RectTransform>().localPosition = ConvertWorldPossitionToCanvasPossition(UFOsDream[posIndex].transform.position);
        dream.GetComponent<RectTransform>().transform.DOLocalMove(ConvertWorldPossitionToCanvasPossition(Airplane.transform.position), 20f);


        //references = referenceRoot.GetComponentsInChildren<GesturePatternDraw>();

        return dream;
    }

    LineObject_AirplaneMinigame2 SpawnBoss()
    {
        LineObject_AirplaneMinigame2 boss = Instantiate(Boss);
        boss.transform.parent = referenceRoot.transform;
        boss.transform.localScale = Vector3.one;
        List<GesturePattern> listCheckSame = new List<GesturePattern>();
        for (int i = 0; i < recognizer.patterns.Count; i++)
        {
            listCheckSame.Add(recognizer.patterns[i]);
        }
        for (int i = 0; i < boss.line.Count; i++)
        {
            int ran = Random.Range(0, listCheckSame.Count);
            boss.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<GesturePatternDraw>().pattern = listCheckSame[ran];
            listCheckSame.RemoveAt(ran);
            boss.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<GesturePatternDraw>().color = colors[Random.Range(0, colors.Length)];
            boss.transform.GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(true);
        }
        boss.GetComponent<RectTransform>().localPosition = ConvertWorldPossitionToCanvasPossition(UFOsDream[8].transform.position);
        boss.GetComponent<RectTransform>().transform.DOLocalMove(ConvertWorldPossitionToCanvasPossition(new Vector2(-9.5f, Airplane.transform.position.y)), 25f);

        return boss;
    }

    IEnumerator AddUFOForStage()
    {
        if (stage == 1)
        {
            dreamObj.Add(SpawnUFO(7, 1));
        }
        if (stage == 2)
        {
            dreamObj.Add(SpawnUFO(5, 2));
        }
        if (stage == 3)
        {
            dreamObj.Add(SpawnUFO(2, 2));
            dreamObj.Add(SpawnUFO(5, 2));
        }
        if (stage == 4)
        {
            dreamObj.Add(SpawnUFO(2, 2));
            dreamObj.Add(SpawnUFO(3, 2));
            dreamObj.Add(SpawnUFO(6, 2));
        }
        if (stage == 5)
        {
            dreamObj.Add(SpawnUFO(0, 2));
            dreamObj.Add(SpawnUFO(3, 2));
            yield return new WaitForSeconds(2);
            dreamObj.Add(SpawnUFO(7, 3));
        }
        if (stage == 6)
        {
            dreamObj.Add(SpawnUFO(4, 2));
            dreamObj.Add(SpawnUFO(5, 2));
            dreamObj.Add(SpawnUFO(7, 2));
        }
        if (stage == 7)
        {
            dreamObj.Add(SpawnUFO(0, 2));
            dreamObj.Add(SpawnUFO(2, 2));
            yield return new WaitForSeconds(3);
            dreamObj.Add(SpawnUFO(3, 3));
            dreamObj.Add(SpawnUFO(5, 3));
        }
        if (stage == 8)
        {
            dreamObj.Add(SpawnUFO(0, 3));
            dreamObj.Add(SpawnUFO(2, 3));
            yield return new WaitForSeconds(3);
            dreamObj.Add(SpawnUFO(3, 3));
            dreamObj.Add(SpawnUFO(5, 3));
            yield return new WaitForSeconds(2);
            dreamObj.Add(SpawnUFO(7, 3));
        }
        if (stage == 9)
        {
            dreamObj.Add(SpawnUFO(1, 2));
            dreamObj.Add(SpawnUFO(2, 2));
            dreamObj.Add(SpawnUFO(3, 3));
            yield return new WaitForSeconds(4);
            dreamObj.Add(SpawnUFO(4, 3));
            dreamObj.Add(SpawnUFO(6, 2));
            dreamObj.Add(SpawnUFO(7, 2));
        }
        if (stage == 10)
        {
            dreamObj.Add(SpawnUFO(0, 3));
            dreamObj.Add(SpawnUFO(1, 3));
            yield return new WaitForSeconds(4);
            dreamObj.Add(SpawnUFO(2, 3));
            dreamObj.Add(SpawnUFO(3, 3));
            yield return new WaitForSeconds(4);
            dreamObj.Add(SpawnUFO(4, 3));
            dreamObj.Add(SpawnUFO(5, 3));
            yield return new WaitForSeconds(4);
            dreamObj.Add(SpawnUFO(6, 3));
            dreamObj.Add(SpawnUFO(7, 3));
        }
        if (stage == 11)
        {
            Airplane.transform.DOMoveX(-9.5f, 2f);
            dreamObj.Add(SpawnBoss());
            bossLife = 5;
        }
    }

    public Vector2 ConvertWorldPossitionToCanvasPossition(Vector2 worldPos)
    {
        RectTransform CanvasRect = canvas.GetComponent<RectTransform>();
        Vector2 ViewportPosition = mainCamera.WorldToViewportPoint(worldPos);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
        return WorldObject_ScreenPosition;
    }

    //public Vector2 ConvertCanvasPosToWorlPos(Vector2 canvasPos)
    //{
    //    Vector2 myPositionOnScreen = Camera.main.ScreenToWorldPoint(canvasPos);
    //    return myPositionOnScreen;
    //}

    public void OnRecognize(RecognitionResult result)
    {
        if (result != RecognitionResult.Empty)
        {
            Blink(result.gesture.id);
        }
    }

    void Blink(string id)
    {
        for (int i = 0; i < dreamObj.Count; i++)
        {
            LineObject_AirplaneMinigame2 dream = dreamObj[i];
            int j = 0;
            while (!dreamObj[i].line[j].IsActive())
            {
                j++;
            }
            if (dreamObj[i].line[j].pattern.id == id)
            {
                StartCoroutine(DrawSuccessOneDream(dreamObj[i]));
            }
        }
    }

    IEnumerator DrawSuccessOneDream(LineObject_AirplaneMinigame2 dream)
    {
        dream.Open();
        dream.gameObject.transform.DOShakeScale(0.1f, 1, 2, 10);
        yield return new WaitForSeconds(0.1f);
        dream.gameObject.transform.DOPlayBackwards();
        yield return new WaitForSeconds(0.3f);
        dream.gameObject.transform.DOPlayForward();
        if (dream.line.Count - 1 == 0)
        {
            if (stage <= 10)
            {
                UpdateStage(dream, stage);
            }
        }
        yield return new WaitForSeconds(0.1f);
        if (stage == 11 && !dream.line[dream.line.Count - 1].IsActive())
        {
            if (bossLife > 0)
            {
                RefreshDreamBoss(dream);
            }
            if (bossLife == 0)
            {
                isWinGame = true;
                Destroy(dream.gameObject);
            }
        }
    }

    void RefreshDreamBoss(LineObject_AirplaneMinigame2 boss)
    {
        bossLife--;
        List<GesturePattern> listCheckSame = new List<GesturePattern>();
        for (int i = 0; i < recognizer.patterns.Count; i++)
        {
            listCheckSame.Add(recognizer.patterns[i]);
        }
        for (int i = 0; i < boss.line.Count; i++)
        {
            int ran = Random.Range(0, listCheckSame.Count);
            boss.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<GesturePatternDraw>().pattern = listCheckSame[ran];
            listCheckSame.RemoveAt(ran);
            boss.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<GesturePatternDraw>().color = colors[Random.Range(0, colors.Length)];
            boss.Sleep();
        }
    }

    void UpdateStage(LineObject_AirplaneMinigame2 dream, int stageIndex)
    {
        dreamObj.Remove(dream);
        dream.gameObject.SetActive(false);
        Destroy(dream.gameObject, 0.1f);
        count[stageIndex - 1]--;

        if (count[stageIndex - 1] == 0)
        {
            stage++;
            isLock = false;
        }
    }

    void WinGame()
    {
        isWinGame = false;
        dreamObj.Clear();
        drawDetector.gameObject.SetActive(false);
        StopAllCoroutines();
        Airplane.transform.DOMoveX(15, 5);
        mainCamera.transform.DOMoveX(7, 5);
        mainCamera.DOOrthoSize(3, 2);
    }

    void LoseGame()
    {
        isLoseGame = false;
        Invoke(nameof(ForDelayClearUFO), 2f);
        drawDetector.gameObject.SetActive(false);
        StopAllCoroutines();
        Airplane.transform.DORotate(new Vector3(0,0, -80), 2f);
        Airplane.transform.DOMoveY(-9, 5).OnComplete(() => { Destroy(Airplane.gameObject); });
    }

    void ForDelayClearUFO()
    {
        dreamObj.Clear();
        //Destroy(referenceRoot.gameObject);
    }


    void Update()
    {
        if (!isIntro)
        {
            if (!isLock)
            {
                StartCoroutine(AddUFOForStage());
                isLock = true;
            }

            if (isWinGame)
            {
                WinGame();
            }

            if (isLoseGame)
            {
                LoseGame();
            }
        }

        if (currBackGround.transform.position.x <= -15f)
        {
            spawnBG++;
            if (spawnBG > 2)
                spawnBG = 2;
        }

        if (spawnBG == 1)
        {
            nextBackGround = Instantiate(currBackGround, new Vector3((currBackGround.transform.position.x + 55f), -0.6f, 0), Quaternion.identity);
            MoveNextBG();
            Invoke("DestroyBG", 8f);
            spawnBG++;
            if (spawnBG > 2)
                spawnBG = 2;
        }
    }
}
