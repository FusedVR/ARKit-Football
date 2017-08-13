using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Target : MonoBehaviour {
	public float range;
	public Transform camTransform;
	public TextMeshPro score;
	public TextMeshPro topScore;

	private int currScore = 0;
	private float timeOffset;
	private float originOffset = 5f;
	private Vector3 origin;

	void OnEnable() {
		DBManager.TopScoreUpdated += UpdateTopScore;
	}

	void OnDisable() {
		DBManager.TopScoreUpdated -= UpdateTopScore;
	}

	void Start() {
		timeOffset = 0f;
		SetScore ();
		SetTarget ();
	}

	void Update () {
		// sinFactor oscillates between -2f and 2f
		float sinFactor = Mathf.Sin (Time.time - timeOffset) * range;
		transform.localPosition = origin + (transform.right * sinFactor); 
	}

	private void UpdateTopScore() {
		topScore.text = DBManager.Instance.topScore.ToString ();
	}

	// Reset the target position to be 5 meters in front of the camera
	public void SetTarget() {
		origin = camTransform.position + (camTransform.forward * originOffset);
		transform.position = origin;
		timeOffset = Time.time;
		transform.LookAt (camTransform);
	}

	// Detect scores
	private void OnTriggerEnter(Collider col) {
		currScore++;
		DBManager.Instance.LogScore ((long)currScore);
		SetScore ();
	}

	private void SetScore() {
		score.text = currScore.ToString();	
	}
}
