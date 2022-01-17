using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anhXaCollider_AirplaneMinigame2 : MonoBehaviour
{
    public GameObject colFxPrefab;
    private bool isLock = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {      
        var colFx = Instantiate(colFxPrefab);
        colFx.transform.parent = GameController_AirplaneMinigame2.instance.canvas.transform;
        colFx.transform.localScale = new Vector3(0.25f ,0.25f, 0.25f);
        colFx.GetComponent<RectTransform>().localPosition = gameObject.transform.localPosition;
        Destroy(gameObject);
        Destroy(colFx, 0.3f);
        GameController_AirplaneMinigame2.instance.isLoseGame = true;
    }

    private void Update()
    {
        if(GameController_AirplaneMinigame2.instance.stage == 11 && !isLock)
        {
            isLock = true;
            gameObject.GetComponent<RectTransform>().localPosition = GameController_AirplaneMinigame2.instance.ConvertWorldPossitionToCanvasPossition(new Vector2 (-9.5f,0));
        }
    }
}
