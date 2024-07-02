package com.example.stackunderflow.ui.gallery

import android.util.Log
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.example.stackunderflow.dto.ScriptModelDto
import com.example.stackunderflow.repository.ScriptRepository
import io.reactivex.rxjava3.disposables.CompositeDisposable
import io.reactivex.rxjava3.kotlin.addTo

class ScriptViewModel(private val scriptRepository: ScriptRepository): ViewModel() {

    val script = MutableLiveData<ScriptModelDto>()

    val myScripts = MutableLiveData<List<ScriptModelDto>>()
    private val disposeBag = CompositeDisposable()
    private val _text = MutableLiveData<String>().apply {
        value = "This is gallery Fragment"
    }
    val text: LiveData<String> = _text


    fun GetScriptById(id: Int){
        scriptRepository.GetScriptById(id).subscribe({ post ->
            Log.d("GetScriptById", "Received script: $post")
            this.script.postValue(post)
        }, { error ->
            Log.d("Test error in GetScriptById",
                error.message?:"erroer")
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
}