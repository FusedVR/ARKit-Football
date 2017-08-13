using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShotManager : MonoBehaviour {
	public GameObject cam;
	public GameObject shotPrefab;

	// Shoot a ball: on Space in the Unity editor, on Tap in an iPhone build.
	void Update () {
		#if UNITY_EDITOR
		if ( Input.GetKeyDown(KeyCode.Space) ) {
			Shoot ();
		}
		#elif UNITY_IPHONE
		if ( Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
			if (!EventSystem.current.IsPointerOverGameObject (Input.GetTouch(0).fingerId)) {
				// Did not tap a UI button, shoot
				Shoot ();
			}
		}
		#endif
	}

	private void Shoot() {
		GameObject go = Instantiate (shotPrefab, cam.transform.position, cam.transform.rotation);
	}
}
