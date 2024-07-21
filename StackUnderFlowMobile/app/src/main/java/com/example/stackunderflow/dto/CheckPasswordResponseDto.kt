package com.example.stackunderflow.dto

import com.google.gson.annotations.SerializedName

data class CheckPasswordResponseDto(
    @SerializedName("isValid") val isValid: Boolean)
