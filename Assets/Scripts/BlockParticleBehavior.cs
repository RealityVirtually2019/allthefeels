using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class HueColour {

    public enum HueColorNames {
        Lime,
        Green,
        Aqua,
        Blue,
        Navy,
        Purple,
        Pink,
        Red,
        Orange,
        Yellow
    }

    private static Hashtable hueColourValues = new Hashtable{
         { HueColorNames.Lime,     new Color32( 166 , 254 , 0, 1 ) },
         { HueColorNames.Green,     new Color32( 0 , 254 , 111, 1 ) },
         { HueColorNames.Aqua,     new Color32( 0 , 201 , 254, 1 ) },
         { HueColorNames.Blue,     new Color32( 0 , 122 , 254, 1 ) },
         { HueColorNames.Navy,     new Color32( 60 , 0 , 254, 1 ) },
         { HueColorNames.Purple, new Color32( 143 , 0 , 254, 1 ) },
         { HueColorNames.Pink,     new Color32( 232 , 0 , 254, 1 ) },
         { HueColorNames.Red,     new Color32( 254 , 9 , 0, 1 ) },
         { HueColorNames.Orange, new Color32( 254 , 161 , 0, 1 ) },
         { HueColorNames.Yellow, new Color32( 254 , 224 , 0, 1 ) },
     };

    public static Color32 HueColourValue(HueColorNames color) {
        return (Color32)hueColourValues[color];
    }

}

public class BlockParticleBehavior : ParticleBehavior {

    public float launchDelay = 2f;
    float timeSinceLaunch = 0f;


    public override void Start() {
        base.Start();

        GetComponent<MeshRenderer>().material.color = HueColour.HueColourValue((HueColour.HueColorNames)Random.Range(0, 10));

        timeSinceLaunch = Time.time;

    }

    public override void Update() {
        if (Time.time - timeSinceLaunch >= launchDelay) {

            rigidbody.drag = 0.95f;

            if (Time.time - time >= interval + intervalBetween) {
                intervalBetween = Random.Range(intervalBetweenRandomness.x, intervalBetweenRandomness.y);
                interval = Random.Range(intervalRandomness.x, intervalRandomness.y);
                speed = Random.Range(speedRandomness.x, speedRandomness.y);

                time = Time.time;
                intervalTime = Time.time;

                direction = CalculateTrajectory();
            }

            if (Time.time - time <= interval) {

                float delta = movementVelocity.Evaluate((Time.time - intervalTime) / interval);

                if (Time.time - time <= interval / 3) {
                    rigidbody.velocity = new Vector3(direction.x, 0, 0) * delta * speed;
                } else if (Time.time - time > interval / 3 && Time.time - time <= interval / 1.5f) {
                    rigidbody.velocity = new Vector3(0, direction.y, 0) * delta * speed;
                } else if (Time.time - time > interval / 1.5f && Time.time - time <= interval) {
                    rigidbody.velocity = new Vector3(0, 0, direction.z) * delta * speed;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == this.gameObject.tag) {
            if (collision.gameObject.GetComponent<BlockParticleBehavior>() && collision.gameObject.GetComponent<BlockParticleBehavior>().hasBecomeLeader) {
                Destroy(GetComponent<Rigidbody>());
                transform.SetParent(collision.gameObject.transform);
                enabled = false;
                transform.position = collision.contacts[0].point + collision.contacts[0].normal * GetComponent<Collider>().bounds.extents.x;
            }
        }
    }

}