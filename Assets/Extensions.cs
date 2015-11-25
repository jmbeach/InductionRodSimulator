using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public static class Extensions {
	public static Toggle GetActive(this ToggleGroup group) {
		return group.ActiveToggles().FirstOrDefault();
	}
}
