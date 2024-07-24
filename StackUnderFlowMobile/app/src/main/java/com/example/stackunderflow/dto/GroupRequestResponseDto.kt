package com.example.stackunderflow.dto

data class GroupRequestResponseDto(val groupId: Int,
                                   val groupName: String,
                                   val status: String,
                                   val userId : Int,
                                   val username: String)
