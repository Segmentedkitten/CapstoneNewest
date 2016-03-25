using UnityEngine;
using System.Collections;

public class DDOL : MonoBehaviour {
	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
}