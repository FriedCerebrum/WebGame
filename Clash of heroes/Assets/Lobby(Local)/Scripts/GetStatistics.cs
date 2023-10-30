using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;

public class FirestoreManager : MonoBehaviour
{
    private FirebaseFirestore db;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp.Create();
            db = FirebaseFirestore.DefaultInstance;
            GetPlayerData();
        });
    }

    public void GetPlayerData()
    {
        DocumentReference docRef = db.Collection("Players").Document("users");
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    PlayerData playerData = snapshot.ConvertTo<PlayerData>();
                    Debug.Log($"Player Level: {playerData.Level}, Wins: {playerData.Wins}, Losses: {playerData.Losses}, Money: {playerData.Money}");
                }
                else
                {
                    Debug.Log("Document does not exist!");
                }
            }
            else
            {
                Debug.LogError("Error fetching document: " + task.Exception);
            }
        });
    }
}

[System.Serializable]
public class PlayerData
{
    public int Level = 1;
    public int Wins = 0;
    public int Losses = 0;
    public int Money = 0;
}
