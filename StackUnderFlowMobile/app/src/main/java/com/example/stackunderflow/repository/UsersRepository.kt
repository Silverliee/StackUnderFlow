package com.example.stackunderflow.repository

import com.example.stackunderflow.dto.CheckPasswordResponseDto
import com.example.stackunderflow.dto.EmailAvailabilityResponse
import com.example.stackunderflow.dto.FriendRequestCreationRequestDto
import com.example.stackunderflow.dto.FriendRequestDto
import com.example.stackunderflow.dto.LoginUserDto
import com.example.stackunderflow.dto.PasswordDto
import com.example.stackunderflow.dto.RegisterUserDto
import com.example.stackunderflow.dto.UsernameAvailabilityResponse
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

    fun getFriendRequests(): Flowable<List<FriendRequestDto>> {
        return stackUnderFlowApiService.getFriendsRequest()
    }

    fun getUsersByKeyword(keyword : String): Flowable<List<User>> {
        return stackUnderFlowApiService.getUserByKeyword(keyword)
    }

    fun getMyFriends(): Flowable<List<User>> {
        return stackUnderFlowApiService.getMyFriends()
    }

    fun createFriendRequest(userId: Int, friendRequestCreationRequestDto: FriendRequestCreationRequestDto): Flowable<FriendRequestDto> {
        return stackUnderFlowApiService.createFriendRequest(userId, friendRequestCreationRequestDto)
    }

    fun acceptFriendRequest(friendRequestId: Int): Flowable<User> {
        return stackUnderFlowApiService.acceptFriendRequest(friendRequestId)
    }

    fun declineFriendRequest(friendRequestId: Int): Flowable<Void> {
        return stackUnderFlowApiService.declineFriendRequest(friendRequestId)
    }

    fun updateUserInfo(user: RegisterUserDto): Flowable<User> {
        return stackUnderFlowApiService.updateUser(user)
    }

    fun checkEmailAvailability(email: String): Flowable<EmailAvailabilityResponse> {
        return stackUnderFlowApiService.checkEmailAvailability(email)
    }

    fun checkUsernameAvailability(username: String): Flowable<UsernameAvailabilityResponse> {
        return stackUnderFlowApiService.checkUsernameAvailability(username)
    }

    fun checkPasswordValidity(password: PasswordDto): Flowable<CheckPasswordResponseDto> {
        return stackUnderFlowApiService.checkPasswordValidity(password)
    }
}