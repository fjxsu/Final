using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public GameObject[] asteroids;
	public Vector3 spawnValues;
	public int asteroidCount;
	public float startWait;
	public float spawnWait;
	public float waveWait;
	public Text scoreText;
	public Text restartText;
	public Text gameOverText;

	private int score;
	private bool gameOver;
	private bool restart;
    void Start()
	{
		score = 0;
		UpdateScore();

		gameOver = false;
		gameOverText.text = "";

		restart = false;
		restartText.text = "";
        //Button btn = GetComponent<Button>();
        //btn.onClick.AddListener(HardModeOnClick);
        StartCoroutine(SpawnWaves());
	}

	void Update()
	{
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            BackgroundScroller bs = GameObject.FindObjectOfType<BackgroundScroller>();
            bs.scrollSpeed = -4.5f;
        }

        if (restart)
        {
			if (Input.GetKeyDown(KeyCode.Q))
            {
                UpdateBackground(0);
                Application.LoadLevel(Application.loadedLevel);
			}
        }
	}

	IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds(startWait);

        while (true)
        {

            for (int i = 0; i < asteroidCount; i++)
            {
                if (score >= 100)
                {
                    gameOver = true;
                    gameOverText.text = "GAME CREATED BY Franz Badias";
                    break;
                }

				GameObject asteroid = asteroids[Random.Range(0, asteroids.Length)];

				Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;

				Instantiate(asteroid, spawnPosition, spawnRotation);

				yield return new WaitForSeconds(spawnWait);
			}

            if (gameOver)
            {
                restart = true;
                restartText.text = "Press 'Q' to Restart";
                break;
            }
            else
            {
                yield return new WaitForSeconds(waveWait);
            }
        }
    }

    void UpdateScore()
	{
		scoreText.text = "Points: " + score.ToString() + "\nPress H for Hard Mode";
	}

	public void AddScore(int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore();
	}

	public void GameOver()
	{
		gameOver = true;
		gameOverText.text = "GAME OVER";
        UpdateBackground(1);
    }

    void UpdateBackground(int flip)
    {
        MeshRenderer[] renderers = UnityEngine.Object.FindObjectsOfType<MeshRenderer>();
        Material startMat;
        Material endMat;

        startMat = (Material)Resources.Load("Assets/Textures/tile_nebula_green_dff");
        endMat = (Material)Resources.Load("Assets/Textures/tile_nebula_updated_dff");
        BackgroundScroller bs = GameObject.FindObjectOfType<BackgroundScroller>();

        if (flip == 1)
        {
            MeshRenderer aren = null;
            MeshRenderer bren = null;

            foreach (MeshRenderer renderer in renderers)
            {
                if (renderer.name == "Background")
                {
                    aren = renderer;
                }

                if (renderer.name == "BackgroundChild")
                {
                    bren = renderer;
                }
            }

            bs.scrollSpeed = -4.5f;

            aren.material = startMat;
            bren.material = endMat;
            bren.material.Lerp(startMat, startMat, 0.5f);

        }
        else
        {
            bs.scrollSpeed = -0.25f;
            foreach (MeshRenderer renderer in renderers)
            {
                if (renderer.name == "Background")
                {
                    renderer.material = startMat;
                }

                if (renderer.name == "BackgroundChild")
                {
                    renderer.material = startMat;
                }
            }
        }

    }

    void HardModeOnClick()
    {
        BackgroundScroller bs = GameObject.FindObjectOfType<BackgroundScroller>();
        bs.scrollSpeed = -4.5f;
    }
}
