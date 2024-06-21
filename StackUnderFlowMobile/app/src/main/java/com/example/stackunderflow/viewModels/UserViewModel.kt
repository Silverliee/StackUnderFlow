package com.example.stackunderflow.viewModels

import android.util.Log
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.example.stackunderflow.dto.UserModelDto
import com.example.stackunderflow.models.UserModel
import com.example.stackunderflow.repository.UsersRepository
import io.reactivex.rxjava3.disposables.CompositeDisposable
import io.reactivex.rxjava3.disposables.Disposable
import io.reactivex.rxjava3.kotlin.addTo

class UserViewModel
    (private val usersRepository: UsersRepository) : ViewModel()
{

    private val disposeBag = CompositeDisposable()

    val userConnected: MutableLiveData<UserModel> = MutableLiveData()



    fun getLogin(userModeldto: UserModelDto): Disposable {
        return this.usersRepository.login(userModeldto).subscribe({
                result -> this.userConnected.postValue(result)
        }, { error ->
            Log.d("Error in function getRecordingsByArtist", error.message ?: "error")
        }).addTo(disposeBag)
    }
}