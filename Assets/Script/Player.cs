using UnityEngine;

public class Player : MonoBehaviour {

	public GameObject Planet;
	public GameObject placeHolder;

	public float speed = 4;
	public float jumpHeight = 10;


	float gravity=100;
	bool OnGround=false;

	private Rigidbody rb;

	float distanceToGround;
	Vector3 Groundnormal;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
	}
	
	// Update is called once per frame
	void Update () {

		Movement ();
		LocalRotation ();
		GroundControl ();
		GravityNRotation ();

	}

	void Movement(){
		//Movement
		float x = Input.GetAxis ("Horizontal") * Time.deltaTime * speed;
		float z = Input.GetAxis ("Vertical") * Time.deltaTime * speed;

		transform.Translate(x,0,z);
	}

	void LocalRotation(){
		//Local Rotation
		if (Input.GetKey (KeyCode.E)) {
			transform.Rotate (0, 150 * Time.deltaTime, 0);
		}

		if (Input.GetKey (KeyCode.Q)) {
			transform.Rotate (0, -150 * Time.deltaTime, 0);
		}

		if (Input.GetKeyDown(KeyCode.Space)) {	//Jump
			rb.AddForce (transform.up * 4000 * jumpHeight * Time.deltaTime);
		}

	}

	void GroundControl(){
		//Ground Control
		RaycastHit hit = new RaycastHit ();
		if (Physics.Raycast (transform.position, -transform.up, out hit, 10)) {

			distanceToGround = hit.distance;
			Groundnormal = hit.normal;

			if (distanceToGround <= 0.2f) {
				OnGround = true;
			} else {
				OnGround = false;
			}
		}
	}

	void GravityNRotation(){
		//gravity & rotation
		Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

		if (OnGround == false) {
			rb.AddForce (gravDirection * -gravity);
		}

		Quaternion toRotation = Quaternion.FromToRotation (transform.up, Groundnormal) * transform.rotation;
		transform.rotation = toRotation;
	}

	private void OnTriggerEnter(Collider col){	//Change new planet
		
		if (col.transform != Planet.transform) {
			Planet = col.transform.gameObject;

			Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

			Quaternion toRotation = Quaternion.FromToRotation (transform.up, gravDirection) * transform.rotation;
			transform.rotation = toRotation;

			rb.velocity =Vector3.zero;
			rb.AddForce (gravDirection * gravity);

			placeHolder.GetComponent<PlayerPlaceHolder> ().NewPlanet (Planet);
		}
	}
}
