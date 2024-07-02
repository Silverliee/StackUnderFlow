package com.example.stackunderflow.viewModels

import android.util.Log
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.example.stackunderflow.dto.LoginUserDto
import com.example.stackunderflow.models.LoginResponse
import com.example.stackunderflow.repository.UsersRepository
import com.example.stackunderflow.utils.SessionManager
import io.reactivex.rxjava3.disposables.CompositeDisposable
import io.reactivex.rxjava3.disposables.Disposable
import io.reactivex.rxjava3.kotlin.addTo

class UserViewModel
    (private val usersRepository: UsersRepository, private val sessionManager: SessionManager) : ViewModel()
{
    private val disposeBag = CompositeDisposable()

    val isLogged: MutableLiveData<Boolean> = MutableLiveData<Boolean>(false)

    fun getLogin(loginUserDto: LoginUserDto): Disposable {
        return this.usersRepository.login(loginUserDto).subscribe({
                result -> this.isLogged.postValue(true)
                sessionManager.saveAuthToken(result.authToken)
        }, { error ->
            Log.d("Error in Login", error.message ?: "error")
        }).addTo(disposeBag)
    }
}