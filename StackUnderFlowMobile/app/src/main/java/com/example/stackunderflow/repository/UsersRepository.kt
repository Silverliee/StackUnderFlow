package com.example.stackunderflow.repository

import com.example.stackunderflow.dto.UserModelDto
import com.example.stackunderflow.models.UserModel
import com.example.stackunderflow.service.StackUnderFlowApiService
import io.reactivex.rxjava3.core.Flowable

class UsersRepository( private val stackUnderFlowApiService: StackUnderFlowApiService){


    fun login(userModelDto: UserModelDto): Flowable<UserModel> {
        return stackUnderFlowApiService.loginUser(userModelDto)
    }
}