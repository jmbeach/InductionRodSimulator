using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Main : MonoBehaviour
{

    public const string StrIdPnlFluxDirection = "PnlFluxDirection";
    public const string StrIdTglGroupFluxDirection = "FluxDirectionGroup";
    public const string StrIdArrow = "arrow";
    public const string StrIdCbUp = "CbUp";
    public const string StrIdCbDown = "CbDown";
    public const string StrIdCbLeft = "CbLeft";
    public const string StrIdCbRight = "CbRight";
    public const string StrIdCbIntoPage = "CbIntoPage";
    public const string StrIdCbOutOfPage = "CbOutOfPage";
    public const string StrIdRod = "Rod";
    public Button BtnSimulate;
    public GameObject PnlFluxDirection;
    public GameObject Arrow;
    public GameObject Rod;
    public BetterToggleGroup TglGroupFluxDirection;
    public List<GameObject> Arrows = new List<GameObject>();
    public int ArrowCount = 30;

    // Use this for initialization
    void Start()
    {
        BtnSimulate = GameObject.Find("BtnSimulate").GetComponent<Button>();
        Rod = GameObject.Find(StrIdRod);
        PnlFluxDirection = GameObject.Find(StrIdPnlFluxDirection);
        PnlFluxDirection.SetActive(false);
        Arrow = GameObject.Find(StrIdArrow);
        Arrow.SetActive(false);
        BtnSimulate.onClick.AddListener(() =>
        {
            // Show ui for simulation
            Debug.Log("BtnSimulate pressed");
            // Hide this button
            BtnSimulate.gameObject.SetActive(false);
            // Show Flux direction panel
            PnlFluxDirection.SetActive(true);
            TglGroupFluxDirection = GameObject.Find(StrIdTglGroupFluxDirection).GetComponent<BetterToggleGroup>();
            TglGroupFluxDirection.OnChange += TglGroupFluxDirection_OnChange;
            // Show Flux
            ShowFlux();
        });
    }

    void TglGroupFluxDirection_OnChange(Toggle newActive)
    {
        ShowFlux();
    }

    private void ShowFlux()
    {
        // get selected flux direction
        Toggle activeToggle = TglGroupFluxDirection.GetActive();
        Debug.Log(activeToggle.name);
        Quaternion rotation = new Quaternion();
        Vector3 initialPosition = new Vector3();
        switch (activeToggle.name)
        {
            case StrIdCbUp:
                rotation = Quaternion.Euler(270, 0, 0);
                initialPosition = new Vector3(4.95f,0,3.25f);
                break;
            case StrIdCbDown:
                rotation = Quaternion.Euler(90, 0, 0);
                initialPosition = new Vector3(4.95f,0,5f);
                break;
            case StrIdCbLeft:
                rotation = Quaternion.Euler(90, 90, 0);
                initialPosition = new Vector3(11f,0,4f);
                break;
            case StrIdCbRight:
                rotation = Quaternion.Euler(90, -90, 0);
                initialPosition = new Vector3(9.5f,0,4f);
                break;
            case StrIdCbIntoPage:
                rotation = Quaternion.Euler(0,0,180);
                initialPosition = new Vector3(10f,-1.36f,4f);
                break;
            case StrIdCbOutOfPage:
                rotation = Quaternion.Euler(0,0,0);
                initialPosition = new Vector3(10f,1.36f,3.86f);
                break;
        }
        PositionArrows(rotation,initialPosition);
    }

    private void PositionArrows(Quaternion rotation, Vector3 initialPosition)
    {
        var position = initialPosition;
        DestroyArrows();
        Arrows.Clear();
        for (int i = 1; i <= ArrowCount; i++)
        {
            GameObject arrow = (GameObject)Instantiate(Arrow, position, rotation);
            arrow.SetActive(true);
            var renderer = arrow.GetComponent<Renderer>();
            var isHorizontal = rotation.eulerAngles.y == 90 || rotation.eulerAngles.y == 270;
            var isIntoOrOutOfPage = (rotation.eulerAngles == new Vector3(0, 0, 0) || Math.Abs(rotation.eulerAngles.z - 180) < 1);
            // if this is the fifth time
            if (i % 5 == 0)
            {
                // shift arrows over right and keep going
                position.z = initialPosition.z;
                if (isHorizontal)
                {
                    position.x -= renderer.bounds.size.x;
                }
                else if (isIntoOrOutOfPage)
                {
                    position.x -= renderer.bounds.size.y;
                }
                else
                {
                    position.x -= renderer.bounds.size.z / 2;
                }
            }
            else
            {
                float incrementZ = 0;
                if (isHorizontal)
                {
                    incrementZ = renderer.bounds.size.x;
                }
                else if (isIntoOrOutOfPage)
                {
                    incrementZ = renderer.bounds.size.y;
                }
                else
                {
                    incrementZ = renderer.bounds.size.z;
                }
                position.z -= incrementZ;
            }
            Arrows.Add(arrow);
        }
    }

    private void DestroyArrows()
    {
        foreach (var arrow in Arrows)
        {
            Destroy(arrow);
        }
    }

    

    // Update is called once per frame
    void Update()
    {

    }

}
