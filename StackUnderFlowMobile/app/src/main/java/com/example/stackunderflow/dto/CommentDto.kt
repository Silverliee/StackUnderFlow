package com.example.stackunderflow.dto

data class CommentDto(
    val commentId: Int,
    val userId: Int,
    val userName: String,
    val scriptId: Int,
    var description: String
) {
}
