using UnityEngine;
using System.Collections;

public class RandomShake : MonoBehaviour
{
	public float duration = 0.5f;
	public float magnitude = 0.1f;
	
	public bool test = false;

	public void PlayShake() {
		
		StopAllCoroutines();
		StartCoroutine("Shake");
	}
	
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayShake();
        }
    }
	
	IEnumerator Shake()
    {
		
		float elapsed = 0.0f;
		
		Vector3 originalCamPos = this.transform.position;
		
		while (elapsed < duration)
        {	
			elapsed += Time.deltaTime;			
			
			float percentComplete = elapsed / duration;			
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
			
			// map noise to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;

            this.transform.position = new Vector3(originalCamPos.x + x, originalCamPos.y + y, originalCamPos.z);

            yield return null;
		}
		
		this.transform.position = originalCamPos;
	}
}
