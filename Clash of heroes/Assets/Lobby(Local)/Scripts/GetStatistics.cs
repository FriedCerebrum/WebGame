using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

public class FirestoreDataFetcher : MonoBehaviour
{
    private FirebaseFirestore db;

    void Start()
    {
        // Проверяем инициализацию Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Firebase доступен, получаем ссылку на Firestore
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
        // Получаем ID игрока из PlayerPrefs
        string playerId = PlayerPrefs.GetString("PlayerId");

        // Делаем запрос к Firestore для получения данных игрока
        DocumentReference docRef = db.Collection("Players").Document(playerId);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Обработка ошибок
                Debug.LogError("Error fetching player data: " + task.Exception);
                return;
            }

            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Debug.Log("Player data fetched successfully.");

                // Сохраняем данные игрока в PlayerPrefs
                PlayerPrefs.SetInt("Level", snapshot.GetValue<int>("Level"));
                PlayerPrefs.SetInt("Losses", snapshot.GetValue<int>("Losses"));
                PlayerPrefs.SetInt("Money", snapshot.GetValue<int>("Money"));
                PlayerPrefs.SetString("Nickname", snapshot.GetValue<string>("Nickname"));
                PlayerPrefs.SetInt("Wins", snapshot.GetValue<int>("Wins"));
                // Для поля Items предполагаем, что это строка.
                PlayerPrefs.SetString("Items", snapshot.GetValue<string>("Items"));

                // Не забудьте сохранить изменения PlayerPrefs, если это необходимо
                PlayerPrefs.Save();
            }
            else
            {
                Debug.Log("Document does not exist!");
            }
        });
    }
}
