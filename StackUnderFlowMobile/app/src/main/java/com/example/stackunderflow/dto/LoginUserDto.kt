package com.example.stackunderflow.dto

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName
import org.jetbrains.annotations.NotNull

data class LoginUserDto(
    @field:SerializedName("Email")
    @field:Expose
    @field:NotNull
    val email: String,

    @field:SerializedName("Password")
    @field:Expose
    @field:NotNull
    val password: String
)

