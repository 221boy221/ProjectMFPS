using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public static UIManager Instance;

    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        // entry

    }



}