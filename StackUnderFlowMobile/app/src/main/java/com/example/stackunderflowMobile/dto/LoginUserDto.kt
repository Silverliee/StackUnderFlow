package com.example.stackunderflowMobile.dto

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

data class LoginUserDto(
    @field:SerializedName("Email")
    @field:Expose
    val email: String,

    @field:SerializedName("Password")
    @field:Expose
    val password: String
)

