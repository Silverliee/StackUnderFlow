package com.example.stackunderflow.ui.comment

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.example.stackunderflow.dto.CommentDto
import com.example.stackunderflow.dto.CommentRequestDto
import com.example.stackunderflow.models.User
import com.example.stackunderflow.repository.ScriptRepository
import com.example.stackunderflow.repository.UsersRepository
import com.example.stackunderflow.viewModels.UserViewModel
import io.reactivex.rxjava3.disposables.CompositeDisposable
import io.reactivex.rxjava3.kotlin.addTo

class CommentViewModel(private val scriptRepository: ScriptRepository, private val usersRepository: UsersRepository) : ViewModel() {

    private val comment = MutableLiveData<CommentDto>()
    val user = MutableLiveData<User>()

    val allComments = MutableLiveData<MutableList<CommentDto>>()
    private val disposeBag = CompositeDisposable()

    fun getComments(scriptId: Int) {
        usersRepository.getUserInfo().subscribe({ user ->
            this.user.postValue(user)
        }, { error ->
            error.message?.let {
                println(it)
            }
        }).addTo(disposeBag)
        scriptRepository.getComment(scriptId).subscribe({ post ->
            this.allComments.postValue(post.toMutableList())
        }, { error ->
            error.message?.let {
                println(it)
            }
        }).addTo(disposeBag)
    }

    fun createComment( scriptId: Int, commentDto: CommentRequestDto) {
        scriptRepository.createComment(scriptId, commentDto).subscribe({ post ->
            val currentComments = allComments.value ?: mutableListOf()
            currentComments.add(post)
            allComments.postValue(currentComments)
        }, { error ->
            error.message?.let {
                println(it)
            }
        }).addTo(disposeBag)
    }

    fun deleteComment(commentId: Int) {
        scriptRepository.deleteComment(commentId).subscribe({
            val currentComments = allComments.value ?: mutableListOf()
            val index = currentComments.indexOfFirst { it.commentId == commentId }
            currentComments.removeAt(index)
            allComments.postValue(currentComments)
        }, { error ->
            error.message?.let {
                println(it)
            }
        }).addTo(disposeBag)
    }

    fun updateComment(commentId: Int, commentDto: CommentRequestDto) {
        scriptRepository.updateComment(commentId, commentDto).subscribe({
            val currentComments = allComments.value ?: mutableListOf()
            allComments.postValue(currentComments)
        }, { error ->
            error.message?.let {
                println(it)
            }
        }).addTo(disposeBag)
    }



}