package com.example.stackunderflow.repository

import com.example.stackunderflow.dto.LoginUserDto
import com.example.stackunderflow.models.LoginResponse
import com.example.stackunderflow.service.StackUnderFlowApiService
import io.reactivex.rxjava3.core.Flowable

class UsersRepository( private val stackUnderFlowApiService: StackUnderFlowApiService){


    fun login(userModelDto: LoginUserDto): Flowable<LoginResponse> {
        return stackUnderFlowApiService.loginUser(userModelDto)
    }
}