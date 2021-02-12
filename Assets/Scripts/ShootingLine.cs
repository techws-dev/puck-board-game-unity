using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SystemRandom = System.Random;

public enum Direction
{
    Right, Left
}

public class ShootingLine : MonoBehaviour
{
    private float rotationSpeed = 100f;
    private float scaleSpeed = 1f;
    private Direction direction;
    private float angleY;
    private float scaleZ;
    private bool isMoving;
    private bool isGrowing;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = true;
        isGrowing = false;

        SystemRandom random = new SystemRandom();
        direction = (Direction)random.Next(0, 2);
        angleY = (float)random.Next(-45, 46);
        scaleZ = 0.5f;

        setAngleY();
        setScaleZ();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving) {
            // Calculate new angle Y and update rotation
            if(direction == Direction.Right) {
                angleY += rotationSpeed * Time.deltaTime;
            } else {
                angleY -= rotationSpeed * Time.deltaTime;
            }

            if(Math.Abs(angleY) > 45) {
                angleY = 45 * Math.Sign(angleY);
                int direction_value = (int)direction;
                direction = (Direction)((direction_value+1)%2);
            }

            setAngleY();
        }
        
        if(isGrowing && scaleZ < 1.0f) {
            scaleZ += scaleSpeed * Time.deltaTime;

            if(scaleZ > 1.0f) {
                scaleZ = 1.0f;
            }
            setScaleZ();
        }
    }

    // Set the y angle of the line
    void setAngleY() {
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            angleY,
            transform.eulerAngles.z
        );
    }

    // Set the z scale of the line
    void setScaleZ() {
        transform.localScale = new Vector3(
            1.0f,
            1.0f,
            scaleZ
        );
    }

    // Stop moving and start growing
    void Grow() {
        isMoving = false;
        isGrowing = true;
    }
}
