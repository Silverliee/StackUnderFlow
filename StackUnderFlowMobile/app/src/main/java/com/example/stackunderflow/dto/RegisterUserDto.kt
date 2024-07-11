package com.example.stackunderflow.dto

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName
import java.util.regex.Pattern

data class RegisterUserDto(

    @field:SerializedName("UserName")
    @field:Expose
    val userName: String,

    @field:SerializedName("Email")
    @field:Expose
    val email: String,

    @field:SerializedName("Password")
    @field:Expose
    val password: String
) {
    init {
        validate()
    }

    private fun validate() {
        require(userName.isNotBlank()) { "UserName is required" }
        require(email.isNotBlank()) { "Email is required" }
        require(isValidEmail(email)) { "Invalid email address" }
        require(password.isNotBlank()) { "Password is required" }
    }

    private fun isValidEmail(email: String): Boolean {
        val emailRegex = "^[A-Za-z0-9+_.-]+@(.+)$"
        return Pattern.compile(emailRegex).matcher(email).matches()
    }
}
