package com.example.stackunderflow.dto

data class ScriptModelDto(
    val scriptId: Int,
    val ScriptName: String,
    val Description: String,
    val InputScriptType: String,
    val OutputScriptType: String,
    val ProgrammingLanguage: String,
    val Visibility: String,
    val UserId: Int,
    val CreatorName: String,

) {


}