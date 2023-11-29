using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

public class FirestoreDataFetcher : MonoBehaviour
{
    private FirebaseFirestore db;

    void Start()
    {
        // ��������� ������������� Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Firebase ��������, �������� ������ �� Firestore
                db = FirebaseFirestore.DefaultInstance;
                FetchPlayerData();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    void FetchPlayerData()
    {
        // �������� ID ������ �� PlayerPrefs
        string playerId = PlayerPrefs.GetString("PlayerId");

        // ������ ������ � Firestore ��� ��������� ������ ������
        DocumentReference docRef = db.Collection("Players").Document(playerId);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // ��������� ������
                Debug.LogError("Error fetching player data: " + task.Exception);
                return;
            }

            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Debug.Log("Player data fetched successfully.");

                // ��������� ������ ������ � PlayerPrefs
                PlayerPrefs.SetInt("Level", snapshot.GetValue<int>("Level"));
                PlayerPrefs.SetInt("Losses", snapshot.GetValue<int>("Losses"));
                PlayerPrefs.SetInt("Money", snapshot.GetValue<int>("Money"));
                PlayerPrefs.SetString("Nickname", snapshot.GetValue<string>("Nickname"));
                PlayerPrefs.SetInt("Wins", snapshot.GetValue<int>("Wins"));
                // ��� ���� Items ������������, ��� ��� ������.
                PlayerPrefs.SetString("Items", snapshot.GetValue<string>("Items"));

                // �� �������� ��������� ��������� PlayerPrefs, ���� ��� ����������
                PlayerPrefs.Save();
            }
            else
            {
                Debug.Log("Document does not exist!");
            }
        });
    }
}
