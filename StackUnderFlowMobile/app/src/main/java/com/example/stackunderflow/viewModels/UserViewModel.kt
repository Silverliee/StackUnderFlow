package com.example.stackunderflow.viewModels

import android.util.Log
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.example.stackunderflow.dto.FriendRequestCreationRequestDto
import com.example.stackunderflow.dto.FriendRequestDto
import com.example.stackunderflow.dto.LoginUserDto
import com.example.stackunderflow.dto.PasswordDto
import com.example.stackunderflow.dto.RegisterUserDto
import com.example.stackunderflow.models.User
import com.example.stackunderflow.repository.UsersRepository
import com.example.stackunderflow.utils.SessionManager
import io.reactivex.rxjava3.disposables.CompositeDisposable
import io.reactivex.rxjava3.disposables.Disposable
import io.reactivex.rxjava3.kotlin.addTo

class UserViewModel
    (private val usersRepository: UsersRepository, private val sessionManager: SessionManager) :
    ViewModel() {

    private val disposeBag = CompositeDisposable()

    val isLogged: MutableLiveData<Boolean> = MutableLiveData<Boolean>(false)
    val isRegistered: MutableLiveData<Boolean> = MutableLiveData<Boolean>(false)
    val loginError: MutableLiveData<String> = MutableLiveData<String>()
    val user: MutableLiveData<User> = MutableLiveData<User>()
    val friendRequest = MutableLiveData<List<FriendRequestDto>>()

    val users = MutableLiveData<List<User>>()
    val friend = MutableLiveData<MutableList<User>>()
    val isAvailable: MutableLiveData<Boolean> = MutableLiveData<Boolean>(false)
    val isCorrect: MutableLiveData<Boolean> = MutableLiveData<Boolean>(false)

    fun getLogin(loginUserDto: LoginUserDto): Disposable {
        return this.usersRepository.login(loginUserDto).subscribe({ result ->
            this.isLogged.postValue(true)
            sessionManager.saveAuthToken(result.authToken)
        }, { error ->
            Log.d("Error in Login", error.message ?: "error")
        }).addTo(disposeBag)
    }

    fun getRegister(registerUserDto: RegisterUserDto): Disposable {
        return this.usersRepository.register(registerUserDto).subscribe({
            this.isRegistered.postValue(true)
        }, { error ->
            Log.d("Error in Register", error.message ?: "error")
        }).addTo(disposeBag)
    }

    fun getUserInfo(): Disposable {
        return this.usersRepository.getUserInfo().subscribe({ userInfo ->
            this.user.postValue(userInfo)
            Log.d("User Info", userInfo.toString())
        }, { error ->
            Log.d("Error in getUserInfo", error.message ?: "error")
        }).addTo(disposeBag)
    }

    fun getFriendRequests(): Disposable {
        return this.usersRepository.getFriendRequests().subscribe({ friendRequests ->
            Log.d("Friend Requests", friendRequests.toString())
            this.friendRequest.postValue(friendRequests)
        }, { error ->
            Log.d("Error in getFriendRequests", error.message ?: "error")
        }).addTo(disposeBag)
    }

    fun getMyFriends(): Disposable {
        return this.usersRepository.getMyFriends().subscribe({ friends ->
            Log.d("Friends", friends.toString())
            this.friend.postValue(friends.toMutableList())
        }, { error ->
            Log.d("Error in getFriends", error.message ?: "error")
        }).addTo(disposeBag)
    }

    fun getUserByKeyword(keyword : String): Disposable {
        return this.usersRepository.getUsersByKeyword(keyword).subscribe({ friends ->
            Log.d("Friends", friends.toString())
            this.users.postValue(friends)
        }, { error ->
            Log.d("Error in getFriends", error.message ?: "error")
        }).addTo(disposeBag)
    }

    fun createFriendRequest(userId: Int, friendRequestCreationRequestDto: FriendRequestCreationRequestDto): Disposable {
        return this.usersRepository.createFriendRequest(userId, friendRequestCreationRequestDto).subscribe({ friendRequest ->
            Log.d("Friend Request", friendRequest.toString())
        }, { error ->
            Log.d("Error in createFriendRequest", error.message ?: "error")
        }).addTo(disposeBag)
    }

    fun acceptFriendRequest(friendRequestId: Int): Disposable {
        return this.usersRepository.acceptFriendRequest(friendRequestId).subscribe({ newFriend ->
            val currentFriends = friend.value ?: mutableListOf()
            currentFriends.add(newFriend)
            friend.postValue(currentFriends)
            Log.d("Friend", friend.toString())
        }, { error ->
            Log.d("Error in acceptFriendRequest", error.message ?: "error")
        }).addTo(disposeBag)
    }

    fun declineFriendRequest(friendRequestId: Int): Disposable {
        return this.usersRepository.declineFriendRequest(friendRequestId).subscribe({
            Log.d("Friend Request Declined", "Friend Request Declined")
        }, { error ->
            Log.d("Error in declineFriendRequest", error.message ?: "error")
        }).addTo(disposeBag)
    }

    fun updateUserInfo(user: RegisterUserDto): Disposable {
        return this.usersRepository.updateUserInfo(user).subscribe({ userInfo ->
            this.user.postValue(userInfo)
            Log.d("User Info", userInfo.toString())
        }, { error ->
            Log.d("Error in updateUserInfo", error.message ?: "error")
        }).addTo(disposeBag)
    }

    fun checkEmailAvailability(email: String): Disposable {
        return this.usersRepository.checkEmailAvailability(email).subscribe({ data ->
            this.isAvailable.postValue(data.IsAvailable)
            Log.d("Email Availability", data.IsAvailable.toString())
        }, { error ->
            Log.d("Error in checkEmailAvailability", error.message ?: "error")
        }).addTo(disposeBag)
    }

    fun checkUsernameAvailability(username: String): Disposable {
        return this.usersRepository.checkUsernameAvailability(username).subscribe({ data ->
            Log.d("Username Availability", data.toString())
            this.isAvailable.postValue(data.isAvailable)
        }, { error ->
            Log.d("Error in checkUsernameAvailability", error.message ?: "error")
        }).addTo(disposeBag)
    }

    fun checkPasswordValidity(password: PasswordDto): Disposable {
        return this.usersRepository.checkPasswordValidity(password).subscribe({ data ->
            Log.d("Password Validity", data.isValid.toString())
            this.isCorrect.postValue(data.isValid)
        }, { error ->
            Log.d("Error in checkPasswordValidity", error.message ?: "error")
        }).addTo(disposeBag)
    }
}