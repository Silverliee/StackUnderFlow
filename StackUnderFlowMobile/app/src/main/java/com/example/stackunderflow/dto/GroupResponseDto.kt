package com.example.stackunderflow.dto

data class GroupResponseDto(
    val groupId: Int,
    val groupName: String,
    val description: String,
    val creatorUserID : Int
)
