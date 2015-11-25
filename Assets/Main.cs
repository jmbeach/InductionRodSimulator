using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Main : MonoBehaviour {

	private const string StrIdPnlFluxDirection = "PnlFluxDirection";
	private const string StrIdTglGroupFluxDirection = "FluxDirectionGroup";
	private const string StrIdArrow = "arrow";
	public Button BtnSimulate;
	public GameObject PnlFluxDirection;
	public GameObject Arrow;
	public ToggleGroup TglGroupFluxDirection;
	public int ArrowCount = 30;

	// Use this for initialization
	void Start () {
		BtnSimulate =  GameObject.Find ("BtnSimulate").GetComponent<Button>();
		PnlFluxDirection = GameObject.Find(StrIdPnlFluxDirection);
		PnlFluxDirection.SetActive(false);
		Arrow = GameObject.Find(StrIdArrow);
		Arrow.SetActive(false);
		BtnSimulate.onClick.AddListener(() => {
			// Show ui for simulation
			Debug.Log("BtnSimulate pressed");
			// Hide this button
			BtnSimulate.gameObject.SetActive(false);
			// Show Flux direction panel
			PnlFluxDirection.SetActive(true);
			TglGroupFluxDirection = GameObject.Find(StrIdTglGroupFluxDirection).GetComponent<ToggleGroup>();
			var TglGroupFluxDirectionListener = TglGroupFluxDirection.GetComponent<BetterToggleGroup>();
			// Show Flux
			ShowFlux();
		});
	}

	private void ShowFlux() {
		// get selected flux direction
		Toggle activeToggle = TglGroupFluxDirection.GetActive();
		Debug.Log(activeToggle.name);
		switch(activeToggle.name) {
			case "CbUp":
				var upRotation = Quaternion.Euler(270,0,0);
				float zInitial = 5.15f;
				var position = new Vector3(zInitial,0,3.15f);
				for (int i = 1; i <= ArrowCount; i++) {
					GameObject arrow = (GameObject)Instantiate(Arrow,position,upRotation);
					arrow.SetActive(true);
					var renderer = arrow.GetComponent<Renderer>();
					// if this is the fifth time
					if (i % 5 == 0) {
						// shift arrows over right and keep going
						position.x-= renderer.bounds.size.z/2;
						position.z = zInitial - renderer.bounds.size.z;
					}
					else {
						position.z -= renderer.bounds.size.z;
					}
				}
				break;
		}
	}


	// Update is called once per frame
	void Update () {
	
	}

}
