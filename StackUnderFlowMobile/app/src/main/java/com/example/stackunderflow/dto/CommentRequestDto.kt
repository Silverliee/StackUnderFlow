package com.example.stackunderflow.dto

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

data class CommentRequestDto(
    @field:SerializedName("Description")
    @field:Expose
    val description: String
)
