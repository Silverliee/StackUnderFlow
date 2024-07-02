package com.example.stackunderflow.ui.feed

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import com.example.stackunderflow.R
import com.example.stackunderflow.databinding.FragmentFeedBinding
import com.example.stackunderflow.viewModels.UserViewModel
import org.koin.androidx.viewmodel.ext.android.viewModel

class FeedFragment : Fragment() {

    private val feedViewModel: FeedViewModel by viewModel()
    private val userViewModel : UserViewModel by viewModel()
    private var _binding: FragmentFeedBinding? = null
    // This property is only valid between onCreateView andN
    // onDestroyView.
    private val binding get() = _binding!!


    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentFeedBinding.inflate(inflater, container, false)

        /*scriptViewModel.GetScriptById(1)
        scriptViewModel.script.observe(viewLifecycleOwner) { script ->
            Log.d("ScriptFragment", "Received script: $script")
            Log.d("ScriptFragment", "Script ID: ${script.scriptId}")
            Log.d("ScriptFragment", "Script Name: ${script.scriptName}")
            Log.d("ScriptFragment", "Description: ${script.description}")
            Log.d("ScriptFragment", "Input Script Type: ${script.inputScriptType}")
            Log.d("ScriptFragment", "Output Script Type: ${script.outputScriptType}")
            Log.d("ScriptFragment", "Programming Language: ${script.programmingLanguage}")
            Log.d("ScriptFragment", "Visibility: ${script.visibility}")
            Log.d("ScriptFragment", "User ID: ${script.userId}")
            Log.d("ScriptFragment", "Creator Name: ${script.creatorName}")
        }*/


        return inflater.inflate(R.layout.fragment_feed, container, false)
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}