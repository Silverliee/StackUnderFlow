package com.example.stackunderflow.repository

import com.example.stackunderflow.dto.ScriptModelDto
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
}