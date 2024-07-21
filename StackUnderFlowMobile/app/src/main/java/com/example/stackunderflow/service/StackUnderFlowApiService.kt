package com.example.stackunderflow.service

import androidx.compose.ui.geometry.Offset
import com.example.stackunderflow.dto.CheckPasswordResponseDto
import com.example.stackunderflow.dto.CommentDto
import com.example.stackunderflow.dto.CommentRequestDto
import com.example.stackunderflow.dto.EmailAvailabilityResponse
import com.example.stackunderflow.dto.FriendRequestCreationRequestDto
import com.example.stackunderflow.dto.FriendRequestDto
import com.example.stackunderflow.dto.LoginUserDto
import com.example.stackunderflow.dto.PasswordDto
import com.example.stackunderflow.dto.RegisterUserDto
import com.example.stackunderflow.dto.ScriptModelDto
import com.example.stackunderflow.dto.UsernameAvailabilityResponse
import com.example.stackunderflow.models.LoginResponse
import com.example.stackunderflow.models.User
import io.reactivex.rxjava3.core.Flowable
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.PATCH
import retrofit2.http.POST
import retrofit2.http.Path
import retrofit2.http.Query

interface StackUnderFlowApiService {

    @POST("User/login")
    fun loginUser(@Body loginUserDto: LoginUserDto): Flowable<LoginResponse>

    @POST("User/register")
    fun registerUser(@Body loginUserDto: RegisterUserDto): Flowable<RegisterUserDto>

    @GET("User/search/{keyword}")
    fun getUserByKeyword(@Path("keyword") keyword: String): Flowable<List<User>>

    @GET("User/")
    fun getUserInfo(): Flowable<User>

    @GET("Script/{id}")
    fun getScriptById(@Path("id") id: Int): Flowable<ScriptModelDto>

    @GET("Script?offset=0&records=40")
    fun getScriptsForFeed(): Flowable<List<ScriptModelDto>>

    @GET("Script/user")
    fun getMyScripts(): Flowable<List<ScriptModelDto>>


    @POST("SocialInteraction/likes/{id}")
    fun createLike(@Path("id") id: Int) : Flowable<Int>

    @DELETE("SocialInteraction/likes/{id}")
    fun deleteLike(@Path("id") id: Int) : Flowable<Void>

    @GET("SocialInteraction/comments/{id}")
    fun getComment(@Path("id") id: Int) : Flowable<List<CommentDto>>

    @POST("SocialInteraction/comments/{id}")
    fun createComment(@Path("id") scriptId : Int, @Body commentRequestDto: CommentRequestDto) : Flowable<CommentDto>

    @DELETE("SocialInteraction/comments/{id}")
    fun deleteComment(@Path("id") commentId: Int) : Flowable<Void>

    @PATCH("SocialInteraction/comments/{id}")
    fun updateComment(@Path("id") commentId: Int, @Body commentRequestDto: CommentRequestDto) : Flowable<CommentDto>

    @GET("SocialInteraction/friends/requests")
    fun getFriendsRequest() : Flowable<List<FriendRequestDto>>

    @GET("SocialInteraction/friends")
    fun getMyFriends(): Flowable<List<User>>

    @POST("SocialInteraction/friends/{id}")
    fun createFriendRequest(@Path("id") id: Int, @Body friendRequestCreationRequestDto: FriendRequestCreationRequestDto) : Flowable<FriendRequestDto>

    @PATCH("SocialInteraction/friends/{id}")
    fun acceptFriendRequest(@Path("id") id: Int) : Flowable<User>

    @DELETE("SocialInteraction/friends/{id}")
    fun declineFriendRequest(@Path("id") id: Int) : Flowable<Void>

    @PATCH("User/update")
    fun updateUser(@Body registerUserDto: RegisterUserDto) : Flowable<User>

    @GET("User/checkEmailAvailability")
    fun checkEmailAvailability(@Query("email") email: String) : Flowable<EmailAvailabilityResponse>

    @GET("User/checkUsernameAvailability")
    fun checkUsernameAvailability(@Query("username") username: String) : Flowable<UsernameAvailabilityResponse>

    @POST("User/checkPassword")
    fun checkPasswordValidity(@Body password: PasswordDto): Flowable<CheckPasswordResponseDto>
}