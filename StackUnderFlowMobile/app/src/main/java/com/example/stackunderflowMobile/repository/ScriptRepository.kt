package com.example.stackunderflow.repository

import com.example.stackunderflowMobile.dto.ScriptModelDto
import com.example.stackunderflow.service.StackUnderFlowApiService
import io.reactivex.rxjava3.core.Flowable

class ScriptRepository( private val stackUnderFlowApiService: StackUnderFlowApiService) {

    fun GetScriptById(id: Int): Flowable<ScriptModelDto>
    {
        return stackUnderFlowApiService.getScriptById(id)
    }

    fun GetMyScripts(): Flowable<List<ScriptModelDto>>
    {
        return stackUnderFlowApiService.GetMyScripts()
    }

    fun CreateLike(scriptId : Int) : Flowable<Int>
    {
        return stackUnderFlowApiService.createLike(scriptId)
    }

    fun DeleteLike(scriptId : Int) : Flowable<Void>
    {
        return stackUnderFlowApiService.deleteLike(scriptId)
    }
}