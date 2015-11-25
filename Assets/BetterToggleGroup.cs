using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;

public class BetterToggleGroup : ToggleGroup {
	public delegate void ChangedEventHandler(Toggle newActive);
	public void Start() {
		foreach (Transform transformToggle in gameObject.transform) {
			var toggle = transformToggle.gameObject.GetComponent<Toggle>();
			Debug.Log(toggle.name);
			toggle.onValueChanged.AddListener((isSelected) => {
				if (!isSelected) {
					return;
				}
				var activeToggle = Active();
				Debug.Log(activeToggle.name);
			});
		}
	}
	public Toggle Active() {
		return ActiveToggles().FirstOrDefault();
	}
}