package com.example.stackunderflow.dto

data class FriendRequestDto(val userId: Int,
                            val friendId: Int,
                            val friendName: String,
                            val status: String,
                            val message: String)
