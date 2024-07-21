package com.example.stackunderflow.repository

import com.example.stackunderflow.dto.CommentDto
import com.example.stackunderflow.dto.CommentRequestDto
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
        return stackUnderFlowApiService.getMyScripts()
    }

    fun getScriptsForFeed(): Flowable<List<ScriptModelDto>>
    {
        return stackUnderFlowApiService.getScriptsForFeed()
    }

    fun CreateLike(scriptId : Int) : Flowable<Int>
    {
        return stackUnderFlowApiService.createLike(scriptId)
    }

    fun DeleteLike(scriptId : Int) : Flowable<Void>
    {
        return stackUnderFlowApiService.deleteLike(scriptId)
    }

    fun getComment(scriptId : Int) : Flowable<List<CommentDto>>
    {
        return stackUnderFlowApiService.getComment(scriptId)
    }

    fun createComment(scriptId : Int,commentRequestDto: CommentRequestDto) : Flowable<CommentDto>
    {
        return stackUnderFlowApiService.createComment(scriptId, commentRequestDto)
    }

    fun deleteComment(commentId : Int) : Flowable<Void>
    {
        return stackUnderFlowApiService.deleteComment(commentId)
    }

    fun updateComment(commentId : Int, commentRequestDto: CommentRequestDto) : Flowable<CommentDto>
    {
        return stackUnderFlowApiService.updateComment(commentId, commentRequestDto)
    }
}