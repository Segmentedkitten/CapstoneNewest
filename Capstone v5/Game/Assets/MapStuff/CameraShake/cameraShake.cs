using UnityEngine;
using System.Collections;

public class cameraShake : MonoBehaviour
{
	public float duration = 0.5f;
	public float speed = 3.0f;
	public float magnitude = 0.1f;
    Vector3 originalCamPos;

    public void PlayShake()
    {	
		StopAllCoroutines();
		StartCoroutine("Shake");
	}
    public void PlayShake(float _duration, float _speed, float _magnitude)
    {
        duration = _duration;
        speed = _speed;
        magnitude = _magnitude;
        StopAllCoroutines();
        StartCoroutine("Shake");
    }
	
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayShake();
        }

        originalCamPos = this.transform.position;
    }

	IEnumerator Shake()
    {
		float elapsed = 0.0f;

        //for randomshake
        //float randomStartX = Random.Range(-1000.0f, 1000.0f);
        //float randomStartY = Random.Range(-1000.0f, 1000.0f);

        //for constant cirlce shake
        float randomStartX = 0;
        float randomStartY = 0;

        
		
		while (elapsed < duration)
        {
			elapsed += Time.deltaTime;			
			
			float percentComplete = elapsed / duration;			
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
			
			// map noise to [-1, 1]
			float x = Mathf.Sin(randomStartX + percentComplete * speed);
			float y = Mathf.Cos(randomStartY + percentComplete * speed);
			x *= magnitude * damper;
			y *= magnitude * damper;

            this.transform.position = new Vector3(originalCamPos.x + x, originalCamPos.y + y, originalCamPos.z);

            yield return null;
		}
		
		this.transform.position = originalCamPos;
	}
}
