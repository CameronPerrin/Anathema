using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class NetworkBoss : MonoBehaviourPunCallbacks, IPunObservable
{

    private PhotonView PV;
    private Vector3 realPos = Vector3.zero;
    private Quaternion realRot = Quaternion.identity;
    private Vector3 realScale = Vector3.zero;

    private Vector3 lastPos;
    private Vector3 lastScale;
    private Vector3 velocity;

    [Range(0.0f, 1.0f)]
    public float predictionCoeff = 1.0f; //How much the game should predict an observed object's velocity: between 0 and 1

    public bool isAuthoritative = false; //Only the master client can send this object's data

    void Start()
    {
        PV = GetComponent<PhotonView>();
        realPos = this.transform.position;
        realRot = this.transform.rotation;
        realScale = this.transform.localScale;
        predictionCoeff = Mathf.Clamp(predictionCoeff, 0.0f, 1.0f);  //Uncomment this to ensure the prediction is clamped
    }

    public void Reset()
    {
        realPos = this.transform.position;
        realRot = this.transform.rotation;
        lastPos = realPos;
        velocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        lastPos = realPos;
        lastScale = realScale;

        if (!PV.IsMine)
        {
            //Set the position & rotation based on the data that was received
            //transform.position = realPos;
            //transform.position = Vector3.Lerp(transform.position, realPos + (predictionCoeff*velocity*Time.deltaTime),  7.0f * Time.deltaTime);
            //transform.position = Vector3.Lerp(transform.position, realPos, 1);
            transform.position = realPos;
            transform.rotation = Quaternion.Lerp(transform.rotation, realRot, 7.0f * Time.deltaTime);
            //transform.localScale = Vector3.Lerp(transform.localScale, realScale + (predictionCoeff * velocity * Time.deltaTime), 1);;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PV.IsMine)
        {
            //Send position over network
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            //Send velocity over network
            stream.SendNext((realPos - lastPos) / Time.deltaTime);
            //stream.SendNext((realScale - lastScale) / Time.deltaTime);
        }
        else if (stream.IsReading)
        {
            //Receive positions
            realPos = (Vector3)(stream.ReceiveNext());
            realRot = (Quaternion)(stream.ReceiveNext());
            //Receive velocity
            velocity = (Vector3)(stream.ReceiveNext());
        }
    }
}