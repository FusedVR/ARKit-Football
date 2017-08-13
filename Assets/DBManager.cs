using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class DBManager: MonoBehaviour {
	public static DBManager Instance;
	public long topScore = 0;

	public delegate void ScoreAction ();
	public static event ScoreAction TopScoreUpdated;

	private DatabaseReference db;
	private long curScore = 0;


	void Awake() {
		if (Instance == null) Instance = this;

		// Set this before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("use_your_FirebaseDB_URL_here");

		// Get the root reference location of the database, from which we will operate on the database.
		db = FirebaseDatabase.DefaultInstance.GetReference("scores");

		// Get top score, listen for changes.
		GetTopScore ();
		db.ValueChanged += HandleTopScoreChange;
	}

	private void GetTopScore() {
		db.GetValueAsync().ContinueWith(task => {
			if (task.IsFaulted) {
				// ERROR HANDLER
			}
			else if (task.IsCompleted) {
				Dictionary<string, object> results = (Dictionary<string, object>) task.Result.Value;
				topScore = (long) results["topScore"];
			}
		});
	}

	public void LogScore(long s) {
		curScore = s;
		if (curScore > topScore) {
			db.RunTransaction (UpdateTopScore);
		}
	}

	private TransactionResult UpdateTopScore(MutableData md) {
		if (md.Value != null) {
			Dictionary<string,object> updatedScore = md.Value as Dictionary<string,object>;
			topScore = (long) updatedScore ["topScore"];
		}

		// Compare the cur score to the top score.
		if (curScore > topScore) { // Update topScore, triggers other UpdateTopScores to retry
			topScore = curScore;
			md.Value = new Dictionary<string,object>(){{"topScore", curScore}};
			return TransactionResult.Success(md);
		}
		return TransactionResult.Abort (); // Aborts the transaction
	}

	void HandleTopScoreChange(object sender, ValueChangedEventArgs args) {
		Dictionary<string,object> update = (Dictionary<string,object>)args.Snapshot.Value;
		topScore = (long) update["topScore"];
		Debug.Log ("New Top Score: " + topScore);
		if (TopScoreUpdated != null) TopScoreUpdated ();
	}
}
