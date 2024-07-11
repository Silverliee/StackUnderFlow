package com.example.stackunderflow.repository

import com.example.stackunderflow.dto.LoginUserDto
import com.example.stackunderflow.dto.RegisterUserDto
import com.example.stackunderflow.models.LoginResponse
import com.example.stackunderflow.models.User
import com.example.stackunderflow.service.StackUnderFlowApiService
import io.reactivex.rxjava3.core.Flowable

class UsersRepository( private val stackUnderFlowApiService: StackUnderFlowApiService){


    fun login(loginUserDto: LoginUserDto): Flowable<LoginResponse> {
        return stackUnderFlowApiService.loginUser(loginUserDto)
    }

    fun register(registerModelDto: RegisterUserDto): Flowable<RegisterUserDto> {
        return stackUnderFlowApiService.registerUser(registerModelDto)
    }

    fun getUserInfo(): Flowable<User> {
        return stackUnderFlowApiService.getUserInfo()
    }
}