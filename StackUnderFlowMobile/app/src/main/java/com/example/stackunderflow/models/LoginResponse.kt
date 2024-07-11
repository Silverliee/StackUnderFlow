package com.example.stackunderflow.models

import com.google.gson.annotations.SerializedName

data class LoginResponse (

    @SerializedName("token")
    var authToken: String,

    @SerializedName("username")
    var user: String
)
