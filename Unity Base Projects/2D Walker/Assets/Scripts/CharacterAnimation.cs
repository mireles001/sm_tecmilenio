﻿using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private int _isMovingId = Animator.StringToHash("isMoving");
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;
    private PlayerMovement _movement;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (_anim) _anim.SetBool(_isMovingId, _movement.Direction != 0);

        if (_movement.Direction == 0) return;

        if (_spriteRenderer) _spriteRenderer.flipX = (_movement.Direction < 0);
    }
}
