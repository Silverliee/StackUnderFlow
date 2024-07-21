package com.example.stackunderflow.ui.comment

import android.app.AlertDialog
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.EditText
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.lifecycle.Observer
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.stackunderflow.R
import com.example.stackunderflow.databinding.FragmentCommentBinding
import com.example.stackunderflow.dto.CommentRequestDto
import com.example.stackunderflow.models.User
import com.example.stackunderflow.module.injectModuleDependencies
import com.example.stackunderflow.ui.Scripts.ScriptViewModel
import com.example.stackunderflow.ui.feed.FeedAdapter
import com.example.stackunderflow.viewModels.UserViewModel
import org.koin.androidx.viewmodel.ext.android.viewModel


class CommentFragment : Fragment() {

    companion object {
        private const val SCRIPT_ID = "script_id"

        @JvmStatic
        fun newInstance(scriptId: Int): CommentFragment {
            val fragment = CommentFragment()
            val args = Bundle()
            args.putInt(SCRIPT_ID, scriptId)
            fragment.arguments = args
            return fragment
        }
    }

    private val viewModel: CommentViewModel by viewModel()
    private val scriptViewModel: ScriptViewModel by viewModel()
    private var _binding: FragmentCommentBinding? = null
    private lateinit var adapter: CommentAdapter
    private val userViewModel :UserViewModel by viewModel()
    private var scriptId : Int = -1

    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        injectModuleDependencies(this@CommentFragment.requireContext())
        _binding = FragmentCommentBinding.inflate(inflater, container, false)
        val view = binding.root

        scriptId = arguments?.getInt(SCRIPT_ID, -1) ?: 0
        viewModel.getComments(scriptId)

        Log.d("CommentFragment", "Script id: $scriptId")
        Log.d("CommentFragment", "Script id: ${scriptViewModel.myScripts.value?.size}")
        scriptViewModel.myScripts.observe(viewLifecycleOwner) { scripts ->
            scripts?.let {
                val script = scripts.find { it.scriptId == scriptId }
                binding.scriptNameComment.text = script?.scriptName
                binding.scriptDescriptionComment.text = script?.description
                binding.scriptUserIdComment.text = script?.creatorName
                binding.numberOfLikesComment.text = script?.numberOfLikes.toString()
            }
        }

        viewModel.allComments.observe(viewLifecycleOwner) { comments ->
            comments?.let {
                Log.d("CommentFragment", "Received script: ${comments.size}")
                Log.d("CommentFragment", "Received script: ${userViewModel.user.value?.userId}")
                    adapter = CommentAdapter(requireContext(), comments, viewModel)
                    binding.commentRecyclerView.adapter = adapter
                    binding.commentRecyclerView.layoutManager = LinearLayoutManager(context)
                } ?: run {
                    Log.d("CommentFragment", "User data is null")
                }
        }

        binding.addingBtn.setOnClickListener {
            addInfo()
        }

        binding.backButton.setOnClickListener {
            requireActivity().onBackPressed();
        }

        return view
    }


    private fun addInfo() {
        val inflter = LayoutInflater.from(this.context)
        val v = inflter.inflate(R.layout.create_comment,null)
        /**set view*/
        val commentDescription = v.findViewById<EditText>(R.id.newCommentContent)

        val addDialog = AlertDialog.Builder(this.context)

        addDialog.setView(v)
        addDialog.setPositiveButton("Ok"){
                dialog,_->
            val commentContent = commentDescription.text.toString()
            Log.d("CommentFragment", "Comment: $commentContent")
            var newComment = CommentRequestDto(commentContent)
            viewModel.createComment(scriptId,newComment)
            Toast.makeText(this.context,"Adding User Information Success",Toast.LENGTH_SHORT).show()
            dialog.dismiss()
        }
        addDialog.setNegativeButton("Cancel"){
                dialog,_->
            dialog.dismiss()
            Toast.makeText(this.context,"Cancel",Toast.LENGTH_SHORT).show()

        }
        addDialog.create()
        addDialog.show()
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

}