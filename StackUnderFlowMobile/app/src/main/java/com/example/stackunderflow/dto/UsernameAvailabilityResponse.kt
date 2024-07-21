package com.example.stackunderflow.dto

import com.google.gson.annotations.SerializedName

data class UsernameAvailabilityResponse(
    @SerializedName("isAvailable")
    val isAvailable: Boolean
)
