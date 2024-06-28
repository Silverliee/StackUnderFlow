package com.example.stackunderflow.service

import com.example.stackunderflow.dto.UserModelDto
import com.example.stackunderflow.dto.ScriptModelDto
import com.example.stackunderflow.models.UserModel
import io.reactivex.rxjava3.core.Flowable
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.Path
import retrofit2.http.Query

interface StackUnderFlowApiService {

    @POST("User/login")
    fun loginUser(@Query("query") userModelDto: UserModelDto): Flowable<UserModel>


    @GET("Script/{id}")
    fun getScriptById(@Path("id") id: Int): Flowable<ScriptModelDto>

}