using InputController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMouvement : MonoBehaviour
{
    private CustomCotroller m_controls;
    private Animator m_animator;
    private float velocity;

    public Camera m_camera;
    public float m_turnSpeed = 10f;

    private Vector3 m_directionOfMove;


    void Awake()
    {
        m_controls = new CustomCotroller();
        m_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        m_controls.Enable();
    }

    private void OnDisable()
    {
        m_controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        var value = m_controls.Mouvement.Move.ReadValue<Vector2>();

        var speed = Mathf.Abs(value.x) + Mathf.Abs(value.y);
        speed = Mathf.Clamp(speed, 0f, 1f);
        speed = Mathf.SmoothDamp(m_animator.GetFloat("Speed"), speed, ref velocity, .1f);
        m_animator.SetFloat("Speed", speed);

        m_directionOfMove = ExtactDirectionFromCamera();

        if (!(value.magnitude > .1f)) return;

        var moveVector = new Vector3(value.x, 0, value.y);
        var rotation = Quaternion.LookRotation(m_directionOfMove, Vector3.up) * Quaternion.LookRotation(moveVector, Vector3.up);
        var lerpRotation = Quaternion.Lerp(transform.rotation, rotation, m_turnSpeed * Time.deltaTime);

        transform.rotation = lerpRotation;

    }

    private Vector3 ExtactDirectionFromCamera()
    {
        return Vector3.ProjectOnPlane(m_camera.transform.forward, Vector3.up);
    }


}
