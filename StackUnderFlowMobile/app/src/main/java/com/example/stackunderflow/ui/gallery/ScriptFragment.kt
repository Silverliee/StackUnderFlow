package com.example.stackunderflow.ui.gallery

import android.annotation.SuppressLint
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.example.stackunderflow.R
import com.example.stackunderflow.databinding.FragmentScriptBinding
import com.example.stackunderflow.module.injectModuleDependencies
import org.koin.androidx.viewmodel.ext.android.viewModel

class ScriptFragment : Fragment() {

    private val scriptViewModel: ScriptViewModel by viewModel()
    private var _binding: FragmentScriptBinding? = null
      // This property is only valid between onCreateView and
      // onDestroyView.
      private val binding get() = _binding!!


    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentScriptBinding.inflate(inflater, container, false)

        scriptViewModel.GetScriptById(1)
        scriptViewModel.script.observe(viewLifecycleOwner) { script ->
            Log.d("ScriptFragment", "Received script: $script")
            Log.d("ScriptFragment", "Script ID: ${script.scriptId}")
            Log.d("ScriptFragment", "Script Name: ${script.ScriptName}")
            Log.d("ScriptFragment", "Description: ${script.Description}")
            Log.d("ScriptFragment", "Input Script Type: ${script.InputScriptType}")
            Log.d("ScriptFragment", "Output Script Type: ${script.OutputScriptType}")
            Log.d("ScriptFragment", "Programming Language: ${script.ProgrammingLanguage}")
            Log.d("ScriptFragment", "Visibility: ${script.Visibility}")
            Log.d("ScriptFragment", "User ID: ${script.UserId}")
            Log.d("ScriptFragment", "Creator Name: ${script.CreatorName}")
        }




        /*val textView: TextView = binding.textGallery
        galleryViewModel.text.observe(viewLifecycleOwner) {
          textView.text = it
        }*/
        return inflater.inflate(R.layout.fragment_script, container, false)
      }

    override fun onDestroyView() {
            super.onDestroyView()
            _binding = null
        }
}