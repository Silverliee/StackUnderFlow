package com.example.stackunderflow.ui.Scripts

import android.util.Log
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.example.stackunderflow.dto.ScriptModelDto
import com.example.stackunderflow.repository.ScriptRepository
import io.reactivex.rxjava3.disposables.CompositeDisposable
import io.reactivex.rxjava3.kotlin.addTo

class ScriptViewModel(private val scriptRepository: ScriptRepository): ViewModel() {

    private val script = MutableLiveData<ScriptModelDto>()

    val myScripts = MutableLiveData<List<ScriptModelDto>>()

    val scriptsForFeed = MutableLiveData<List<ScriptModelDto>>()
    private val disposeBag = CompositeDisposable()


    fun GetScriptById(id: Int){
        scriptRepository.GetScriptById(id).subscribe({ post ->
            Log.d("GetScriptById", "Received script: $post")
            this.script.postValue(post)
        }, { error ->
            Log.d("Test error in GetScriptById",
                error.message?:"error")
        }).addTo(disposeBag)
    }

    fun GetScriptsForFeed(){
        scriptRepository.getScriptsForFeed().subscribe({ post ->
            Log.d("GetScriptsForFeed", "Received all scripts: $post")
            this.scriptsForFeed.postValue(post)
        }, { error ->
            Log.d("Test error in GetScriptsForFeed",
                error.message?:"error")
        }).addTo(disposeBag)
    }

    fun GetMyScript(){
        scriptRepository.GetMyScripts().subscribe({ post ->
            Log.d("GetMyScript", "Received all my scripts: $post")
            this.myScripts.postValue(post)
        }, { error ->
            Log.d("Test error in GetMyScript",
                error.message?:"error")
        }).addTo(disposeBag)
    }

    fun CreateLike(scriptId: Int){
        scriptRepository.CreateLike(scriptId).subscribe({ post ->
            Log.d("Like A Script", "Like: $post")
        }, { error ->
            Log.d("Test error in CreateLike",
                error.message?:"error")
        }).addTo(disposeBag)
    }

    fun DeleteLike(scriptId: Int){
        scriptRepository.DeleteLike(scriptId).subscribe({ post ->
            Log.d("UnLike A Script", "Unlike : $post")
        }, { error ->
            Log.d("Test error in DeleteLike",
                error.message?:"error")
        }).addTo(disposeBag)
    }
}