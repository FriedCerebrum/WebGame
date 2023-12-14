using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions; // Для FirebaseApp.CheckAndFixDependenciesAsync()
using Firebase.Firestore; // Для работы с Firestore
using System.Collections.Generic; // Для использования Dictionary
using TMPro; // Для работы с текстовыми полями
using System.Threading.Tasks;
using UnityEngine.SceneManagement;


public class AuthManager : MonoBehaviour
{
    //Firebase переменные
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public static string PlayerId { get; private set; }
    [Header("Scenes")]
    public string menuSceneName = "LoadingBeforeLobby";

    //Переменные логина
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Регистрационные переменные
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    [Header("Player Data")]
    public string nickname;
    public string items;
    public int level;
    public int losses;
    public int money;
    public int wins;

    [Header("General Info")]
    public TMP_Text generalInfoText;


    void Awake()
    {
        //Чекаем зависимости FireBase в системе
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
       
        auth = FirebaseAuth.DefaultInstance;
    }

    //Функция для кнопки логина
    public void LoginButton()
    {
        //Вызов корутины входа в систему с передачей email и пароля
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //Функция для кнопки регистрации
    public void RegisterButton()
    {
        //Вызов корутины register с передачей email, пароля и имени пользователя
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Вызоваем функцию входа в систему Firebase auth, передав ей email и пароль
        Task<AuthResult> LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Ждём завершения выполнения задания
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //При наличии ошибок обработаваем их
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //Пользователь вошёл
            //Получаем результат
            User = LoginTask.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            UpdateUIText(generalInfoText, "Logged In");
            PlayerId = User.UserId;
            PlayerPrefs.SetString("PlayerId", PlayerId);
            PlayerPrefs.Save();

            SceneManager.LoadScene(menuSceneName);
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //Если поле имени пользователя пустое, выведим пред.
            warningRegisterText.text = "Missing Username";
        }
        else if(passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //Если пароль не совпадает, пред
            warningRegisterText.text = "Password Does Not Match!";
        }
        else 
        {
            //Вызовем функцию входа в систему Firebase auth, передав ей email и пароль
            Task<AuthResult> RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //ждём выполнения операции
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //При наличии ошибок обработаваем их
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                UpdateUIText(generalInfoText, RegisterTask.Exception.ToString());


                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                //Пользователь создан
                //получаем результат
                User = RegisterTask.Result.User;

                if (User != null)
                {
                    // Создаём профиль пользователя и задаём имя пользователя
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    // Вызовем функцию Firebase auth update user profile, передав ей профиль с именем пользователя
                    Task ProfileTask = User.UpdateUserProfileAsync(profile);
                    // Ждём
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        // Обработка ошибок
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        UpdateUIText(generalInfoText, "Username Set Failed!");
                    }
                    else
                    {
                        // Добавляем данные игрока в Firestore
                        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(dependencyTask =>
                        {
                            if (dependencyTask.Result == DependencyStatus.Available)
                            {
                                FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

                                Dictionary<string, object> playerData = new Dictionary<string, object>
                    {
                        { "Nickname", _username },
                        { "Items", "" }, // Изначально пустой список предметов
                        { "Level", 1 }, // Начальный уровень
                        { "Losses", 0 }, // Начальное количество поражений
                        { "Money", 0 }, // Начальное количество денег
                        { "Wins", 0 } // Начальное количество побед
                    };

                                // Создаем документ в коллекции "Players" с идентификатором, равным UID пользователя
                                db.Collection("Players").Document(User.UserId).SetAsync(playerData).ContinueWith(task =>
                                {
                                    if (task.IsCompleted)
                                    {
                                        Debug.Log("Player data added to Firestore");
                                        UpdateUIText(generalInfoText, "Account was created");
                                        PlayerId = User.UserId;
                                        warningRegisterText.text = "";
                                        SceneManager.LoadScene(menuSceneName);
                                    }
                                    else
                                    {
                                        Debug.LogWarning("Failed to add player data to Firestore");
                                        warningRegisterText.text = "Failed to add player data to Firestore";
                                    }
                                });
                            }
                            else
                            {
                                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyTask.Result);
                                warningRegisterText.text = "Failed to add player data to Firestore (Firebase dependencies)";
                            }
                        });
                    }
                }
            }
        }
    }

    private void UpdateUIText(TMP_Text textComponent, string message)
    {
        if (textComponent != null)
        {
            textComponent.text = message;
        }
        else
        {
            Debug.LogWarning("Text component is not assigned!");
        }
    }




}
