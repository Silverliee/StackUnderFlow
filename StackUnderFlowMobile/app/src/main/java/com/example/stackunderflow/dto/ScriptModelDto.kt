package com.example.stackunderflow.dto

import com.google.gson.annotations.SerializedName

data class ScriptModelDto(
    @SerializedName("scriptId") val scriptId: Int,
    @SerializedName("scriptName") val scriptName: String?,
    @SerializedName("description") val description: String?,
    @SerializedName("inputScriptType") val inputScriptType: String?,
    @SerializedName("outputScriptType") val outputScriptType: String?,
    @SerializedName("programmingLanguage") val programmingLanguage: String?,
    @SerializedName("visibility") val visibility: String?,
    @SerializedName("userId") val userId: Int,
    @SerializedName("creatorName") val creatorName: String?,
    @SerializedName("numberOfLikes") val numberOfLikes: Int,
    @SerializedName("isLiked") val isLiked: Boolean
)

