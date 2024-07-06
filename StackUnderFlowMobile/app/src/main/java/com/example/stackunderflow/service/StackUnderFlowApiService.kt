package com.example.stackunderflow.service

import com.example.stackunderflow.dto.LoginUserDto
import com.example.stackunderflow.dto.RegisterUserDto
import com.example.stackunderflow.dto.ScriptModelDto
import com.example.stackunderflow.models.LoginResponse
import com.example.stackunderflow.models.User
import io.reactivex.rxjava3.core.Flowable
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.Path

interface StackUnderFlowApiService {

    @POST("User/login")
    fun loginUser(@Body loginUserDto: LoginUserDto): Flowable<LoginResponse>

    @POST("User/register")
    fun registerUser(@Body loginUserDto: RegisterUserDto): Flowable<RegisterUserDto>


    @GET("Script/{id}")
    fun getScriptById(@Path("id") id: Int): Flowable<ScriptModelDto>

    @GET("Script/user")
    fun GetMyScripts(): Flowable<List<ScriptModelDto>>

    @GET("User/")
    fun getUserInfo(): Flowable<User>

}