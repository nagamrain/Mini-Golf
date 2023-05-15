using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    [SerializeField] BallController ballController;
    [SerializeField] CameraController camController;
    [SerializeField] GameObject finishWidow;
    [SerializeField] TMP_Text finishText;
    [SerializeField] TMP_Text shootCountText;

    bool isBallOutside;
    bool isBallTeleporting;
    bool isGoal;

    Vector3 lastBallPosition;

    private void OnEnable() 
    {
        ballController.onBallShooted.AddListener(UpdateShootCount);
    }

    private void OnDisable() 
    {
        ballController.onBallShooted.RemoveListener(UpdateShootCount);
    }

    private void Update() 
    {
        if (ballController.ShootingMode)
        {
            lastBallPosition = ballController.transform.position;
        }
        var InputActive = Input.GetMouseButton(0) 
            && ballController.IsMove() == false 
            && ballController.ShootingMode == false
            && isBallOutside == false;

        camController.SetInputActive(InputActive);
    }

    public void OnBallGoalEnter()
    {
        isGoal = true;
        ballController.enabled = false;

        finishWidow.gameObject.SetActive(true);
        finishText.text = "Finish!!\n" + "Shoot Count: " + ballController.ShootCount;
    }

    public void OnBallOutside()
    {
        if (isGoal)
        {
            return;
        }

        if (isBallTeleporting == false)
        {
            Invoke("TeleportBallLastPosition", 2);
        }
        ballController.enabled = false;
        isBallOutside = true;
        isBallTeleporting = true;
    }

    public void TeleportBallLastPosition()
    {
        TeleportBall(lastBallPosition);
    }

    public void TeleportBall(Vector3 targetPosition)
    {
        var rb = ballController.GetComponent<Rigidbody>();
        // rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        // rb.isKinematic = true;
        ballController.transform.position = targetPosition;
        // rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        // rb.isKinematic = false;

        ballController.enabled = true;
        isBallOutside = false;
        isBallTeleporting = false;
    }

    public void UpdateShootCount(int shootCount)
    {
        shootCountText.text = shootCount.ToString();
    }

    public void Quit()
    {
        SceneManager.LoadScene("Main");
    }
}
